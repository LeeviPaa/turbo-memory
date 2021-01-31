using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PickupScript : MonoBehaviourPunCallbacks
{
    public AnimationCurve _hoverCurve;

    private float _yPosition;
    
    private int _pointValue = 100;
    private bool _canInteract = true;

    void Start()
    {
        _yPosition = transform.position.y;
    }
    
    void Update()
    {
        transform.position = new Vector3(transform.position.x, _hoverCurve.Evaluate
                            (Time.time % _hoverCurve.length) + _yPosition, transform.position.z);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;
        var controller = collider.gameObject.GetComponent<PlayerController>();
        if (controller.photonView.Controller.IsLocal && _canInteract)
        {
            controller.AddPoints(_pointValue);
            _canInteract = false;
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            _canInteract = false;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;
        _canInteract = true;
    }
}
