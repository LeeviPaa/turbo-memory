using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionAltar : MonoBehaviour
{
    public void Interacted(double timeStamp, Player player)
    {
        if (!player.IsLocal) return;
        GameManager.Instance.BroadcastClientRoleChanged(PlayerRole.Human);
    }
}
