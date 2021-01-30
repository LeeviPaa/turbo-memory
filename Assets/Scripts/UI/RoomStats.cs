using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class RoomStats : MonoBehaviour
    {
        [SerializeField]
        private ElementList _users;
        private List<IElementData> data = new List<IElementData>(4);

        public void OnUsersUpdate(Room room)
        {
            if (room == null) return;
            data.Clear();
            foreach (Player player in room.Players.Values)
            {
                data.Add(new PlayerData{ Player = player });
            }
            _users.UpdateList(data);
        }
    }
}