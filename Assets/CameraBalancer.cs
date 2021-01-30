using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBalancer : MonoBehaviour
{
    [SerializeField]
    private Transform _transform;

    public void Update()
    {
        var rotation = _transform.eulerAngles;
        var newRotation = Vector3.Lerp(rotation, new Vector3(rotation.x, 360f, 0f), 0.75f);
        _transform.rotation = Quaternion.Euler(newRotation);
    }
}
