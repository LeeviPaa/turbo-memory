using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using UI;

public class HUDMaster : MonoBehaviourPunCallbacks
{
    #region Rooms

    private Room _currentRoom;
    public Room GetCurrentRoom() => _currentRoom == null ? PhotonNetwork.CurrentRoom : _currentRoom;

    public UnityEvent<Room> OnRoomUpdated = new UnityEvent<Room>();
    public UnityEvent<Player> OnPlayerJoined = new UnityEvent<Player>();
    public UnityEvent<Player> OnPlayerDisconnected = new UnityEvent<Player>();

    public void Start()
    {
        OnRoomUpdated.Invoke(GetCurrentRoom());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            var player = GetCurrentRoom().GetPlayer(1);
            ShowKillFeedMessage(player, KillType.Default, player);
        }
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

    #endregion

    #region KillFeed

    [SerializeField]
    private MessageFeed _killFeed;
    public MessageFeed KillFeed => _killFeed;
    public void ShowKillFeedMessage(Player user, KillType type, Player target)
    {
        _killFeed.ShowFeedMessage(new KillFeedData() { User = user, Type = type, Target = target, Timestamp = Time.unscaledTime });
    }

    #endregion
}
