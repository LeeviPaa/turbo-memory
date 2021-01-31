using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class VictoryScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject _victoryRoot;
        [SerializeField]
        private GameObject _defeatRoot;

        public void OnVictoryScreenEnter()
        {
            var localPlayer = PhotonNetwork.LocalPlayer;
            var ownScore = 0;
            var highestScore = 0;
            foreach (var value in GameManager.Instance.PlayerScores)
            {
                if (value.Value > highestScore) highestScore = value.Value;
                if (value.Key == localPlayer) ownScore = value.Value;
            }
            var victory = ownScore >= highestScore;
            _victoryRoot.GameObjectSetActive(victory);
            _defeatRoot.GameObjectSetActive(!victory);
        }

        public void Return()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

    }
}
