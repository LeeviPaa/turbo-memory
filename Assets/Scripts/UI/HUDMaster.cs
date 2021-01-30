using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class HUDMaster : MonoBehaviourPunCallbacks
{
    private Room _currentRoom;
    public Room GetCurrentRoom() => _currentRoom == null ? PhotonNetwork.CurrentRoom : _currentRoom;

    public UnityEvent<Room> OnRoomUpdated = new UnityEvent<Room>();

    public void Start()
    {
        OnRoomUpdated.Invoke(GetCurrentRoom());
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        OnRoomUpdated.Invoke(GetCurrentRoom());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        OnRoomUpdated.Invoke(GetCurrentRoom());
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        OnRoomUpdated.Invoke(GetCurrentRoom());
    }

}
