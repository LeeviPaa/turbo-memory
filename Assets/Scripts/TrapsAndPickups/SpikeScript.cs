using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private Animator _spikeTrapAnimator;

    private bool _activated;
    
    private int _anmIsActive = Animator.StringToHash("IsActive");
    
    // Start is called before the first frame update
    void Start()
    {
        _spikeTrapAnimator = GetComponent<Animator>();
    }

    void Activate()
    {
        _spikeTrapAnimator.SetBool(_anmIsActive, true);
    }

    void Deactivate()
    {
        _spikeTrapAnimator.SetBool(_anmIsActive, false);
    }
}
