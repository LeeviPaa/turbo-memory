using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionScript : MonoBehaviour
{
    private float _lerpDuration = 1f;

    private bool _isActivated = true;

    public AnimationCurve lerpCurve;
    

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && _isActivated)
        {
            Debug.Log("Uh oh!");

            Vector3 startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 endPos = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
            
            StartCoroutine(Move(startPos, endPos));
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && _isActivated)
        {
            // yes
        }
    }

    IEnumerator Move(Vector3 startPosition, Vector3 endPosition)
    {
        float timeElapsed = 0;

        while (timeElapsed < _lerpDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpCurve.Evaluate(timeElapsed / _lerpDuration));
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = endPosition;
    }
    
    
}
