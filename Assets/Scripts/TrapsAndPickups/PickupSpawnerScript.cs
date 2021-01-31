using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class PickupSpawnerScript : MonoBehaviourPunCallbacks
{
    private bool _treasureCollected;

    private float _secBetweenSpawns = 10f;
    private float _nextSpawnTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _nextSpawnTime = Time.time + _secBetweenSpawns;
        _treasureCollected = false;
        
        Spawn();
    }

    void Update()
    {
        if (Time.time > _nextSpawnTime)
        {
            _treasureCollected = false;
            
            Spawn();
            
            _nextSpawnTime = Time.time + _secBetweenSpawns;
        }
    }
    
    [PunRPC]
    void Spawn()
    {
        if (PhotonNetwork.IsMasterClient && !_treasureCollected)
        {
            GameObject treasure = PhotonNetwork.Instantiate("Treasure", transform.position, Quaternion.identity);
            _treasureCollected = true;
        }
    }
}
