using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class PickupSpawnerScript : MonoBehaviourPunCallbacks
{
    private bool _treasureCollected;
    private bool _canSpawnNew;

    private float _secBetweenSpawns = 10f;
    private float _nextSpawnTime;
    private BoxCollider _treasureCheck;

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
            GameObject treasure = PhotonNetwork.InstantiateRoomObject("Treasure", transform.position, Quaternion.identity);
            _treasureCollected = true;
            _canSpawnNew = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Treasure"))
        {
            Debug.LogWarning("Spawned new treasure");
        }
    }


    void OnTriggerExit(Collider collider)
    {
        var controller = collider.gameObject.GetComponent<PlayerController>();
        
        if (collider.gameObject.CompareTag("Player") && controller.Role == PlayerRole.Human && _canSpawnNew)
        {
            Debug.LogWarning("Ready to spawn a new treasure");
            _canSpawnNew = false;
            _treasureCollected = false;
        }
    }
}
