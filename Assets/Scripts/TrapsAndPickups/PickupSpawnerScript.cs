using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PickupSpawnerScript : MonoBehaviourPunCallbacks
{
    private bool _collected;

    private float _secBetweenSpawns = 2f;
    private float _nextSpawnTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _nextSpawnTime = Time.time + _secBetweenSpawns;
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Time.time > _nextSpawnTime)
            {
                // GetComponent<PhotonView>().RPC("Spawn", PhotonTargets.All);
            }
        }
    }
    
    
    [PunRPC]
    void Spawn()
    {
        Debug.LogWarning("Spawning treasure");
        PhotonNetwork.Instantiate("Treasure", transform.position, Quaternion.identity);
    }
}
