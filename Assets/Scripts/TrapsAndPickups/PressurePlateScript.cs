using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{
    public GameObject[] TrapList;

    private GameObject _buttonPresser;
    
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
            _buttonPresser = collider.gameObject;
            
            _activated = true;
            _pressurePlateAnimator.SetBool(_anmIsActive, true);

            foreach (GameObject trap in TrapList)
            {
                trap.gameObject.SendMessage("Activate", collider.gameObject, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && _activated)
        {
            _buttonPresser = null;
            
            _activated = false;
            _pressurePlateAnimator.SetBool(_anmIsActive, false);
            
            foreach (GameObject trap in TrapList)
            {
                trap.gameObject.SendMessage("Deactivate", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
