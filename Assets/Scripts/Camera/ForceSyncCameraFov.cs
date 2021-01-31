using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSyncCameraFov : MonoBehaviour
{
    [SerializeField]
    private Camera _cameraToSync;
    [SerializeField]
    private Camera _target;


    // Update is called once per frame
    void Update()
    {
        _cameraToSync.fieldOfView = _target.fieldOfView;
    }
}
