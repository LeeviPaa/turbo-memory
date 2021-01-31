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
    public UnityEvent<Room> OnEnterVictoryScreen = new UnityEvent<Room>();

    public void Start()
    {
        OnRoomUpdated.Invoke(GetCurrentRoom());
    }

    public void Awake()
    {
        GameManager.Instance.PlayerRoleChanged.AddListener(OnPlayerRoleChanged);
    }

    public void OnPlayerRoleChanged(Player player, PlayerRole role)
    {
        if (!player.IsLocal) return;
        SetState(role == PlayerRole.Human ? HUDState.Default : HUDState.Death);
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
        if (_state == HUDState.Death)
        {
            _respawnText.text = $"Respawn in { Mathf.CeilToInt(Mathf.Max(0, _timeOfDeath + _deathDuration - Time.time))} seconds.";
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
        photonView.RPC("ShowKillFeedMessage", RpcTarget.All, user != null ? user.ActorNumber : -99999, type, target.ActorNumber);
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

    #region PlayerDeath

    [SerializeField]
    private GameObject _baseHUD;
    [SerializeField]
    private GameObject _deathScreen;
    [SerializeField]
    private GameObject _victoryScreen;

    private float _timeOfDeath;
    private float _deathDuration;
    [SerializeField]
    private TMPro.TMP_Text _respawnText;
    public void SetDeathProperties(float timeOfDeath, float deathDuration)
    {
        _timeOfDeath = timeOfDeath;
        _deathDuration = deathDuration;
    }

    public enum HUDState
    {
        Default = 0,
        Death = 1,
        Victory = 2
    }

    private HUDState _state;

    public void SetState(HUDState state)
    {
        _baseHUD.GameObjectSetActive(state == HUDState.Default || state == HUDState.Death);
        _deathScreen.GameObjectSetActive(state == HUDState.Death);
        _victoryScreen.GameObjectSetActive(state == HUDState.Victory);
        _state = state;
        if (state == HUDState.Victory)
        {
            OnEnterVictoryScreen.Invoke(GetCurrentRoom());
        }
    }

    #endregion
}
