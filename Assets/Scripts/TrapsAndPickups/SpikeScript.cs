using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpikeScript : MonoBehaviourPunCallbacks
{
    private GameObject _activator;
    
    private Animator _spikeTrapAnimator;

    private bool _isActivated;
    
    private int _anmIsActive = Animator.StringToHash("IsActive");
    
    // Start is called before the first frame update
    void Start()
    {
        _spikeTrapAnimator = GetComponent<Animator>();
    }

    void Activate()
    {
        photonView.RPC("ActivateSync", RpcTarget.All);
    }

    [PunRPC]
    void ActivateSync()
    {
        StartCoroutine(Activation());
    }

    void Deactivate()
    {
        photonView.RPC("DeactivateSync", RpcTarget.All);
    }

    [PunRPC]
    void DeactivateSync()
    {
        _spikeTrapAnimator.SetBool(_anmIsActive, false);

        _isActivated = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && _isActivated && photonView.IsMine)
        {
            collider.gameObject.SendMessage("KillPlayer", _activator, SendMessageOptions.DontRequireReceiver);
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
