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

    void Start()
    {
        _yPosition = transform.position.y;
    }
    
    void Update()
    {
        transform.position = new Vector3(transform.position.x, _hoverCurve.Evaluate
                            (Time.time % _hoverCurve.length) + _yPosition, transform.position.z);
    }

    
    [PunRPC]
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.SendMessage("AddPoints", _pointValue, SendMessageOptions.DontRequireReceiver);

            PhotonNetwork.Destroy(gameObject);
        }
    }
}
