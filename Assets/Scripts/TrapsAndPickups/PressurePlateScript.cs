using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{
    public GameObject[] TrapList;
    
    private Animator _pressurePlateAnimator;

    private BoxCollider _plateDetection;

    private bool _activated;

    private int _anmIsActive = Animator.StringToHash("IsActive");
    
    // Start is called before the first frame update
    void Start()
    {
        _pressurePlateAnimator = GetComponent<Animator>();
        _plateDetection = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && !_activated)
        {
            _activated = true;
            _pressurePlateAnimator.SetBool(_anmIsActive, true);
            TrapList[0].gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && _activated)
        {
            _activated = false;
            _pressurePlateAnimator.SetBool(_anmIsActive, false);
            TrapList[0].gameObject.SendMessage("Deactivate", SendMessageOptions.DontRequireReceiver);
        }
    }
}
