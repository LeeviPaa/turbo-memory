using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class HUDMaster : MonoBehaviourPunCallbacks
{
    private Room _currentRoom;
    public Room GetCurrentRoom() => _currentRoom == null ? PhotonNetwork.CurrentRoom : _currentRoom;

    public UnityEvent<Room> OnRoomUpdated = new UnityEvent<Room>();
    public UnityEvent<Player> OnPlayerJoined = new UnityEvent<Player>();
    public UnityEvent<Player> OnPlayerDisconnected = new UnityEvent<Player>();

    public void Start()
    {
        OnRoomUpdated.Invoke(GetCurrentRoom());
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        OnRoomUpdated.Invoke(GetCurrentRoom());
        OnPlayerJoined.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        OnRoomUpdated.Invoke(GetCurrentRoom());
        OnPlayerDisconnected.Invoke(otherPlayer);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        OnRoomUpdated.Invoke(GetCurrentRoom());
    }

}
