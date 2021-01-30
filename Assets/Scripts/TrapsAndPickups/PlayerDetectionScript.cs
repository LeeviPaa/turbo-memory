using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class PlayerDetectionScript : MonoBehaviour
{
    private float _lerpDuration = 1f;

    private int _rand;

    private bool _isActivated = true;

    public AnimationCurve lerpCurve;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && _isActivated)
        {
            Vector3 startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 endPos = new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z);
            
            StartCoroutine(Move(startPos, endPos));

            _rand = UnityEngine.Random.Range(1, 100);
            
            collision.gameObject.SendMessage("ChangeRole", _rand, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && _isActivated)
        {
            // Wait a second and then return up
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
