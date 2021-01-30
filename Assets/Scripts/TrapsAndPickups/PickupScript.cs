using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public AnimationCurve _hoverCurve;

    private float _hoverDuration = 1f;
    private float _yPosition;
    
    private int _pointValue = 10;

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
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.SendMessage("AddPoints", _pointValue, SendMessageOptions.DontRequireReceiver);
            Debug.Log("Player got " + _pointValue + " points!");
            Destroy(gameObject);
        }
    }
}
