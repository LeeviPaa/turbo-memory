using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerCameraFollower _playerCamera;
    [SerializeField]
    private PlayerController _playerPrefab;

    private PlayerController _localPlayerInstance;

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	public void QuitApplication()
	{
		Application.Quit();
	}

    private void Start()
    {
        SpawnPlayer();
    }

    /// <summary>
	/// Called when the local player left the room. We need to load the launcher scene.
	/// </summary>
	public override void OnLeftRoom()
	{
		SceneManager.LoadScene("PunBasics-Launcher");
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
		    Debug.Log("We are Instantiating LocalPlayer from");

			// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
			var playerObject = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
            _localPlayerInstance = playerObject.GetComponent<PlayerController>();
            _localPlayerInstance.Initialize(this);
		}
        else
        {
			Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
		}

        _playerCamera.SetFollowTarget(_localPlayerInstance.transform);
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
