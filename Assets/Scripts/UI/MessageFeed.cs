using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MessageFeed : MonoBehaviour
    {
        public enum MessageFeedDirection
        {
            Up = 0,
            Down = 1
        }

        [SerializeField]
        private List<Element> _items;
        [SerializeField]
        private Transform _parent;
        [SerializeField]
        private MessageFeedDirection _direction = MessageFeedDirection.Down;

        public void ShowFeedMessage(IElementData data)
        {
            var index = _items.Count - 1;
            var item = _items[index];
            _items.RemoveAt(index);
            item.ClearData();
            switch (_direction)
            {
                case MessageFeedDirection.Up:
                    item.transform.SetAsLastSibling();
                    break;
                case MessageFeedDirection.Down:
                    item.transform.SetAsFirstSibling();
                    break;
            }
            item.SetData(data);
            _items.Insert(0, item);
        }
    }
}