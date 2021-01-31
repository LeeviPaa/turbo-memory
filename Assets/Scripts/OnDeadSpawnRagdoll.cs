using Photon.Pun;
using UnityEngine;

public class OnDeadSpawnRagdoll : MonoBehaviourPunCallbacks, IDied
{
    [SerializeField]
    private GameObject _ragdollPrefab;

    private GameObject _currentRagdoll;

    private bool _dead = false;

    public void OnDeath()
    {
        if(_dead || !photonView.IsMine || !PhotonNetwork.IsConnected)
            return;

        _dead = true;
        _currentRagdoll = PhotonNetwork.Instantiate(_ragdollPrefab.name, transform.position, transform.rotation, 0);
    }

    public void OnRespawn()
    {
        _dead = false;
        if (_currentRagdoll != null)
        {
            PhotonNetwork.Destroy(_currentRagdoll);
        }
    }
}