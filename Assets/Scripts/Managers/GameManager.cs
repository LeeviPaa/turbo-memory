using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class E_PlayerRoleChanged : UnityEvent<Player, PlayerRole>{}

public class GameManager : MonoBehaviourPunCallbacks
{
	public static GameManager Instance => FindObjectOfType<GameManager>(); 
	public IReadOnlyDictionary<Player, PlayerRole> PlayerRoles => _playerRoles;
	public E_PlayerRoleChanged PlayerRoleChanged => _playerRoleChanged;

    [SerializeField]
    private PlayerCameraFollower _playerCamera;
    [SerializeField]
    private PlayerController _playerPrefab;
    [SerializeField]
	private Transform _spawnPoint;

    private PlayerController _localPlayerInstance;
	private Dictionary<Player, PlayerRole> _playerRoles = new Dictionary<Player, PlayerRole>();
	[SerializeField]
	private E_PlayerRoleChanged _playerRoleChanged = new E_PlayerRoleChanged();

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	public void QuitApplication()
	{
		Application.Quit();
	}

    /// <summary>
	/// Called when the local player left the room. We need to load the launcher scene.
	/// </summary>
	public override void OnLeftRoom()
	{
		SceneManager.LoadScene("PunBasics-Launcher");
	}

	public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{

	}
	
	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		foreach(var kvp in changedProps)
		{
			switch(kvp.Key)
			{
				case "Player_Role":
					UpdatePlayerRoleLocal(targetPlayer, (PlayerRole)kvp.Value);
					break;
				default:
					break;
			}
		}
	}

	public void BroadcastClientRoleChanged(PlayerRole role)
	{
		var properties = new Hashtable();
		properties.Add("Player_Role", role);

		PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
	}

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (!PhotonNetwork.IsConnected)
		{
			SceneManager.LoadScene("Launcher");

			return;
		}

		if (_localPlayerInstance == null)
		{
			Player player = PhotonNetwork.LocalPlayer;
		    Debug.Log("We are Instantiating LocalPlayer from");

			// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
			var playerObject = PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPoint.position, Quaternion.identity, 0);
            _localPlayerInstance = playerObject.GetComponent<PlayerController>();
			BroadcastClientRoleChanged(PlayerRole.Human);
		}
        else
        {
			Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
		}

        _playerCamera.SetFollowTarget(_localPlayerInstance.transform);
    }
	
	private void UpdatePlayerRoleLocal(Player player, PlayerRole role)
	{
		if(_playerRoles.ContainsKey(player))
			_playerRoles[player] = role;
		else
			_playerRoles.Add(player, role);
		
		Debug.Log($"{player.NickName} role is now {role.ToString()}");
		_playerRoleChanged.Invoke(player, role);
	}

	/// <summary>
	/// MonoBehaviour method called on GameObject by Unity on every frame.
	/// </summary>
	private void Update()
	{
		// "back" button of phone equals "Escape". quit app if that's pressed
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			QuitApplication();
		}
	}
}
