using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private GameObject _activator;
    
    private Animator _spikeTrapAnimator;

    private bool _activated;
    
    private int _anmIsActive = Animator.StringToHash("IsActive");
    
    // Start is called before the first frame update
    void Start()
    {
        _spikeTrapAnimator = GetComponent<Animator>();
    }

    void Activate(GameObject player)
    {
        _spikeTrapAnimator.SetBool(_anmIsActive, true);
        
        _activator = player;
        _activated = true;
    }

    void Deactivate()
    {
        _spikeTrapAnimator.SetBool(_anmIsActive, false);
        
        _activated = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && _activated)
        {
            collider.gameObject.SendMessage("KillPlayer", _activator, SendMessageOptions.DontRequireReceiver);
        }
    }
}
