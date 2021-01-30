using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviourPunCallbacks
{
    public PlayerRole Role => _role;
    public E_PlayerRoleChanged RoleChanged => _roleChanged;
    public UnityEvent Jumped => _jumped;
    public PhotonView PhotonView => photonView;
    public int Points => _points;

    [SerializeField]
    private float _speed = 10;
    [SerializeField]
    private float _rotationSpeed = 10;
    [SerializeField]
    private float _jumpSpeed = 10;
    [SerializeField]
    private float _jumpTime = 0.5f;
    [SerializeField]
    private AnimationCurve _jumpSpeedCurve;
    [SerializeField]
    private float _groundedGraceDelay = 0.1f;
    [SerializeField]
    private float _gravitySpeed = 98.1f;

    private CharacterController _characterController;
    private GameManager _gameManager;
    private PlayerRole _role;
    private bool _jumping = false;
    [SerializeField]
    private bool _groundedGrace;
    private float _groundedGraceTime = 0;
    private Vector3 _jumpVelocity;
    private Vector3 _moveVelocity;
    private int _points;
    [SerializeField]
    private E_PlayerRoleChanged _roleChanged = new E_PlayerRoleChanged();
    private UnityEvent _jumped = new UnityEvent();

    public void Start()
    {
        _gameManager = GameManager.Instance;
        _characterController = GetComponent<CharacterController>();
        _groundedGraceTime = 0;
    }

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		foreach(var kvp in changedProps)
		{
			switch(kvp.Key)
			{
				case "Player_Role":
					UpdatePlayerRole(targetPlayer, (PlayerRole)kvp.Value);
					break;
				default:
					break;
			}
		}
	}

    public void KillPlayer(GameObject killer)
    {
        if(_role == PlayerRole.Human)
            Die();
    }

    private void Die()
    {
        if(photonView.Owner == PhotonNetwork.LocalPlayer)
            _gameManager.BroadcastClientRoleChanged(PlayerRole.GoodGhost);

        foreach (var mb in transform.GetComponents<MonoBehaviour>())
        {
            if(mb is IDied died)
            {
                died.OnDeath();
            }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
        }
    }

    private void UpdatePlayerRole(Player targetPlayer, PlayerRole value)
    {
        if(photonView.Owner == targetPlayer)
        {
            _role = value;
        }
        
        _roleChanged.Invoke(targetPlayer, _role);
    }

    private void Update()
    {
        if( photonView.IsMine == false && PhotonNetwork.IsConnected == true )
	    {
	        return;
	    }

        UpdateGroundedGrace();

        Vector3 movement = transform.forward * Input.GetAxis("Vertical") * _speed;
        Vector3 gravity = -Vector3.up * _gravitySpeed * (_jumping ? 0 : 1);
        _moveVelocity = movement + _jumpVelocity + gravity;

        _characterController.Move(_moveVelocity * Time.deltaTime);
        transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal"), 0) * _rotationSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && !_jumping && _groundedGrace)
            StartCoroutine(Jump());

        if(Input.GetKeyDown(KeyCode.H))
            _gameManager.BroadcastClientRoleChanged(PlayerRole.Human);
        else if(Input.GetKeyDown(KeyCode.G))
            _gameManager.BroadcastClientRoleChanged(PlayerRole.GoodGhost);
        else if(Input.GetKeyDown(KeyCode.J))
            _gameManager.BroadcastClientRoleChanged(PlayerRole.EvilGhost);
        else if(Input.GetKeyDown(KeyCode.K))
            KillPlayer(gameObject);
    }

    private void UpdateGroundedGrace()
    {
        if(!_characterController.isGrounded && _groundedGrace)
        {
            _groundedGraceTime += Time.deltaTime;

            if(_groundedGraceTime >= _groundedGraceDelay)
            {
                _groundedGrace = false;
                _groundedGraceTime = 0;
            }
        }
        else if (_characterController.isGrounded)
        {
            _groundedGrace = true;
        }
    }

    public void ChangeRole(int randomNumber)
    {
        if( photonView.IsMine == false && PhotonNetwork.IsConnected == true )
        {
            return;
        }
        
        Debug.Log("Rolled a " + randomNumber);

        if (randomNumber > 50)
        {
            _gameManager.BroadcastClientRoleChanged(PlayerRole.EvilGhost);
        }
        else
        {  
            _gameManager.BroadcastClientRoleChanged(PlayerRole.GoodGhost);
        }
    }

    public void AddPoints(int pointValue)
    {
        _points += pointValue;
    }

    private IEnumerator Jump()
    {
        if(_jumping)
            yield break;
        
        _jumped.Invoke();
        _jumping = true;
        float time = 0;
        while(time < _jumpTime)
        {
            time += Time.deltaTime;
            _jumpVelocity = _jumpSpeedCurve.Evaluate(time) * transform.up * _jumpSpeed;
            yield return null;
        }
        _jumpVelocity = Vector3.zero;
        _jumping = false;
    }
}
