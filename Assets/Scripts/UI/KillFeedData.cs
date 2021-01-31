using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KillType
{
    Default = 0,
    SpikeTrap = 1,
    Axe = 2,
    Lava = 3,
    None = 100
}

public class KillFeedData : IElementData
{
    public Player User;
    public KillType Type;
    public Player Target;
    public float Timestamp;

    public bool IsEqual(IElementData data)
    {
        return this != data;
    }
}
