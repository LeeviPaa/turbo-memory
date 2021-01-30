using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class PlayerLayerManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;
    [SerializeField]
    private int _playerLayer;
    [SerializeField]
    private int _ghostLayer;

    private void Start()
    {
        _player.RoleChanged.AddListener(OnRoleChanged);
    }

    private void OnRoleChanged(Player arg0, PlayerRole arg1)
    {
        if(arg0 == _player.photonView.Owner)
        {
            if(arg1 == PlayerRole.Human)
                gameObject.layer = _playerLayer;
            else
                gameObject.layer = _ghostLayer;
        }
    }
}
