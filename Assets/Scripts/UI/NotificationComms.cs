using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

namespace UI
{
    public class NotificationComms : MonoBehaviour
    {
        [SerializeField]
        private GameObject _notificationRoot;
        [SerializeField]
        private Animation _anim;
        [SerializeField]
        public TMP_Text _text;

        public Stack<string> _stack = new Stack<string>();
        private float _startTime = -1f;
        private bool IsPlaying() => _startTime + 2f > Time.unscaledTime;

        public void Awake()
        {
            _notificationRoot.gameObject.GameObjectSetActive(false);
        }

        public void Update()
        {
            if (IsPlaying()) return;
            if (_stack.Count > 0)
            {
                DisplayNotification(_stack.Pop());
            }
            else
            {
                _notificationRoot.gameObject.GameObjectSetActive(false);
            }
        }

        public void ShowNotification(string text)
        {
            _notificationRoot.gameObject.GameObjectSetActive(true);
            if (!IsPlaying())
            {
                DisplayNotification(text);
                return;
            }
            _stack.Push(text);
        }

        public void DisplayNotification(string text)
        {
            _startTime = Time.unscaledTime;
            _text.text = text;
            _anim.Play();
        }

        public void OnPlayerJoined(Player player)
        {
            if (player == null) return;
            ShowNotification($"{player.NickName} has joined the game!");
        }

        public void OnPlayerDisconnect(Player player)
        {
            if (player == null) return;
            ShowNotification($"{player.NickName} has left the game!");
        }
    }
}
