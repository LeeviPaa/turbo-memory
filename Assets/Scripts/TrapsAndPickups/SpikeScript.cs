using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpikeScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Vector3 _boxCenter;
    [SerializeField]
    private float _boxSize;

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

    IEnumerator Activation()
    {
        _isActivated = true;
        _spikeTrapAnimator.SetBool(_anmIsActive, true);

        CheckCollisions();
        yield return new WaitForSeconds(1);
        CheckCollisions();
        
        _spikeTrapAnimator.SetBool(_anmIsActive, false);
        _isActivated = false;
    }

    private void CheckCollisions()
    {
        // changed OnTriggerEnter to a more manual method since OnTriggerEnter is pretty unreliable it seems
        foreach(var collider in Physics.OverlapBox(transform.position + _boxCenter, Vector3.one * _boxSize / 2))
        {
            if(!collider.gameObject.CompareTag("Player"))
                continue;

            PlayerController player = collider.gameObject.GetComponent<PlayerController>();
            if (player != null && _isActivated)
            {
                player.KillPlayer(_activator);
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        // draw red gizmo that shows the killbox
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + _boxCenter, Vector3.one * _boxSize);
    }
}
