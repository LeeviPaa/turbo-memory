using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingTrap : MonoBehaviour
{

    [SerializeField]
    private Transform _swingingRoot;

    [SerializeField]
    private float _maxRotation = 80f;
    [SerializeField]
    private float _speed = 1.5f;

    private float _currentAngle = 0f;
    
    // Update is called once per frame
    void Update()
    {
        var angle = Mathf.Lerp(_maxRotation, -_maxRotation, (Mathf.Sin((float)PhotonNetwork.Time * _speed) + 1) / 2);
        _currentAngle = Mathf.Lerp(_currentAngle, angle, 0.25f);
        _swingingRoot.localEulerAngles = new Vector3(_currentAngle, 0f, 0f);
    }
}
