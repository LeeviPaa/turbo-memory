using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class E_RoleChanged : UnityEvent<PlayerRole>{}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviourPunCallbacks
{
    public PlayerRole Role => _role;
    public E_RoleChanged RoleChanged => _roleChanged;

    [SerializeField]
    private float _speed = 10;
    [SerializeField]
    private float _rotationSpeed = 10;

    private CharacterController _characterController;
    private GameManager _gameManager;
    private PlayerRole _role;
    [SerializeField]
    private E_RoleChanged _roleChanged = new E_RoleChanged();

    public void Start()
    {
        _gameManager = GameManager.Instance;
        _characterController = GetComponent<CharacterController>();
        CameraWork cameraWork = GetComponent<CameraWork>();

        if (cameraWork != null)
        {
            if (photonView.IsMine)
            {
                cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><b>Missing</b></Color> CameraWork Component on player Prefab.", this);
        }
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

    private void UpdatePlayerRole(Player targetPlayer, PlayerRole value)
    {
        if(photonView.Owner == targetPlayer)
        {
            _role = value;
            _roleChanged.Invoke(_role);
        }
    }

    private void Update()
    {
        if( photonView.IsMine == false && PhotonNetwork.IsConnected == true )
	    {
	        return;
	    }

        Vector3 movement = transform.forward * Input.GetAxis("Vertical");

        _characterController.SimpleMove(movement * _speed * Time.deltaTime);
        transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal"), 0) * _rotationSpeed * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.H))
            _gameManager.BroadcastClientRoleChanged(PlayerRole.Human);
        else if(Input.GetKeyDown(KeyCode.G))
            _gameManager.BroadcastClientRoleChanged(PlayerRole.GoodGhost);
        else if(Input.GetKeyDown(KeyCode.J))
            _gameManager.BroadcastClientRoleChanged(PlayerRole.EvilGhost);
    }

    public void ChangeRole()
    {
        Debug.Log("Changed players role");
        
        if( photonView.IsMine == false && PhotonNetwork.IsConnected == true )
        {
            return;
        }

        _gameManager.BroadcastClientRoleChanged(PlayerRole.EvilGhost);
    }
}
