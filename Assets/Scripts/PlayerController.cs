using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float _speed = 10;
    [SerializeField]
    private float _rotationSpeed = 10;

    private CharacterController _characterController;

    public void Start()
    {
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

    public void Initialize(GameManager gameManager)
    {

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
    }
}
