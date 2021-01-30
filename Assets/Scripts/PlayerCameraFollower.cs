using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollower : MonoBehaviour
{
    [SerializeField]
    private float _followSpeed = 10;
    [SerializeField]
    private float _rotationSpeed = 10;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera _followCamera;

    private Transform _followTarget;

    public void SetFollowTarget(Transform followTarget)
    {
        _followTarget = followTarget;
    }

    public void Update()
    {
        if(_followTarget == null)
            return;
            
        transform.position = Vector3.Lerp(transform.position,  _followTarget.position, _followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _followTarget.rotation, _rotationSpeed * Time.deltaTime);
    }

    public void SetFollowCameraActive(bool isActive)
    {
        _followCamera.enabled = isActive;
    }

    public void OnRoleChanged(Player player, PlayerRole role)
    {
        if (!player.IsLocal) return;
        _followCamera.enabled = role != PlayerRole.Human;
    }
}
