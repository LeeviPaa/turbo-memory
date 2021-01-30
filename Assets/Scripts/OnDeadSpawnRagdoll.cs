using Photon.Pun;
using UnityEngine;

public class OnDeadSpawnRagdoll : MonoBehaviourPunCallbacks, IDied
{
    [SerializeField]
    private GameObject _ragdollPrefab;

    private bool _dead = false;

    public void OnDeath()
    {
        if(_dead || !photonView.IsMine || !PhotonNetwork.IsConnected)
            return;

        _dead = true;
        var playerObject = PhotonNetwork.Instantiate(_ragdollPrefab.name, transform.position, transform.rotation, 0);
    }
}