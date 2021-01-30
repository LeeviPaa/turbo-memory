using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct PlayerData : IElementData
{
    public Player Player;

    public bool IsEqual(IElementData data)
    {
        var comparison = data.GetType()?.GetProperty("Player")?.GetValue(data);
        return Player != comparison;
    }
}
