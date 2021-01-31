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
            BroadCastKillFeedMessage(player, KillType.Default, player);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleScoreboardVisible();
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

    public void BroadCastKillFeedMessage(Player user, KillType type, Player target)
    {
        photonView.RPC("ShowKillFeedMesage", RpcTarget.All, user.UserId, type, target.UserId);
    }

    [PunRPC]
    public void ShowKillFeedMessage(int user, KillType type, int target)
    {
        var playerUser = GetCurrentRoom().GetPlayer(user);
        var playerTarget = GetCurrentRoom().GetPlayer(target);
        if (playerTarget == null) return;
        _killFeed.ShowFeedMessage(new KillFeedData() { User = playerUser, Type = type, Target = playerTarget, Timestamp = Time.unscaledTime });
    }

    #endregion

    #region Scoreboard

    [SerializeField]
    private GameObject _scorePage;
    public void ToggleScoreboardVisible()
    {
        _scorePage.GameObjectSetActive(!_scorePage.activeSelf);
    }

    #endregion
}
