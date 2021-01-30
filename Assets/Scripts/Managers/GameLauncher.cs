using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class GameLauncher : MonoBehaviourPunCallbacks
{
    /// <summary>
	/// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
	/// </summary>
    private const string GameVersion = "1";

	[Tooltip("The Ui Panel to let the user enter name, connect and play")]
	[SerializeField]
	private GameObject _controlPanel;
	[Tooltip("The Ui Text to inform the user about the connection progress")]
	[SerializeField]
	private Text _feedbackText;
	[SerializeField]
	private byte _maxPlayersPerRoom = 4;
	[SerializeField]
    private string _mainSceneName;
	[SerializeField]
	private TMPro.TMP_InputField _nameInputField;

	/// <summary>
	/// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
	/// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
	/// Typically this is used for the OnConnectedToMaster() callback.
	/// </summary>
	private bool _isConnecting;
    
	void Awake()
	{
		// #Critical
		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.AutomaticallySyncScene = true;
	}

    public void Connect()
	{
		// we want to make sure the log is clear everytime we connect, we might have several failed attempted if connection failed.
		_feedbackText.text = "";
		// keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
		_isConnecting = true;
		// hide the Play button for visual consistency
		_controlPanel.SetActive(false);
		SetName();
        
		// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
		if (PhotonNetwork.IsConnected)
		{
			LogFeedback("Joining Room...");
			// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
			PhotonNetwork.JoinRandomRoom();
		}
        else
        {
			LogFeedback("Connecting...");
			
			// #Critical, we must first and foremost connect to Photon Online Server.
			PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = GameVersion;
		}
	}

	public void SetName()
    {
		var nickName = _nameInputField.text;
		PhotonNetwork.NickName = string.IsNullOrEmpty(nickName) ? "Guest" : nickName;
	}

	void LogFeedback(string message)
	{
		// we do not assume there is a feedbackText defined.
		if (_feedbackText == null) {
			return;
		}
		// add new messages as a new line and at the bottom of the log.
		_feedbackText.text += System.Environment.NewLine+message;
	}
    
    public override void OnConnectedToMaster()
	{
        // we don't want to do anything if we are not attempting to join a room. 
		// this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
		// we don't want to do anything.
		if (_isConnecting)
		{
			LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
			Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
	
			// #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
			PhotonNetwork.JoinRandomRoom();
		}
	}
    
	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
		Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
		// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this._maxPlayersPerRoom});
	}
    
	public override void OnDisconnected(DisconnectCause cause)
	{
		LogFeedback("<Color=Red>OnDisconnected</Color> "+cause);
		Debug.LogError("Launcher:Disconnected");
		_isConnecting = false;
		_controlPanel.SetActive(true);
	}
    
	public override void OnJoinedRoom()
	{
		LogFeedback("<Color=Green>OnJoinedRoom</Color> with "+PhotonNetwork.CurrentRoom.PlayerCount+" Player(s)");
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.");
	
		// #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
		if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
		{
			Debug.Log("We load the main scnee");
			// #Critical
			// Load the Room Level. 
			PhotonNetwork.LoadLevel(_mainSceneName);
		}
	}
}