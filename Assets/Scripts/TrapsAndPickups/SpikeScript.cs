using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpikeScript : MonoBehaviourPunCallbacks
{
    PhotonView photonView;
    
    private GameObject _activator;
    
    private Animator _spikeTrapAnimator;

    private bool _isActivated;
    
    private int _anmIsActive = Animator.StringToHash("IsActive");
    
    // Start is called before the first frame update
    void Start()
    {
        photonView = PhotonView.Get(this);
        _spikeTrapAnimator = GetComponent<Animator>();
    }

    void Activate()
    {
        StartCoroutine(Activation());
    }

    void Deactivate()
    {
        _spikeTrapAnimator.SetBool(_anmIsActive, false);

        _isActivated = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && _isActivated)
        {
            // collider.gameObject.SendMessage("KillPlayer", _activator, SendMessageOptions.DontRequireReceiver);
            photonView.RPC("KillPlayer", RpcTarget.All);
        }
    }

    IEnumerator Activation()
    {
        _spikeTrapAnimator.SetBool(_anmIsActive, true);

        yield return new WaitForSeconds(1);
        
        _spikeTrapAnimator.SetBool(_anmIsActive, false);
    }

    [PunRPC]
    void ChangeState(bool newState)
    {
        _isActivated = newState;
    }
}
