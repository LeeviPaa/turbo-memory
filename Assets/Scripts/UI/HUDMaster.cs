using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class HUDMaster : MonoBehaviourPunCallbacks
{
    private Photon.Realtime.Room _currentRoom;
    public Photon.Realtime.Room GetCurrentRoom() => _currentRoom == null ? PhotonNetwork.CurrentRoom : _currentRoom;

    public UnityEvent<Photon.Realtime.Room> OnRoomUpdated = new UnityEvent<Photon.Realtime.Room>();
    public UnityEvent<Player> OnPlayerJoined = new UnityEvent<Player>();
    public UnityEvent<Player> OnPlayerLeft = new UnityEvent<Player>();

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

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        OnRoomUpdated.Invoke(GetCurrentRoom());
    }
}
