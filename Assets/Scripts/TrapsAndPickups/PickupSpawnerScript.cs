using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class PickupSpawnerScript : MonoBehaviourPunCallbacks
{
    private bool _treasureCollected;

    private float _timeBetweenSpawns = 10f;
    private float _nextSpawnTime;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _nextSpawnTime = Time.time + _timeBetweenSpawns;
        _treasureCollected = false;
        
        Spawn();
    }

    void Update()
    {
        if (!_treasureCollected)
        {
            if (Time.time > _nextSpawnTime)
            {
                Spawn();
            
                _nextSpawnTime = Time.time + _timeBetweenSpawns;
            }
        }
    }
    
    [PunRPC]
    void Spawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject treasure = PhotonNetwork.Instantiate("Treasure", transform.position, Quaternion.identity);
            _treasureCollected = true;
        }
    }
}
