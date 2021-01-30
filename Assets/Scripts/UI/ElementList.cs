using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ElementList : MonoBehaviour
    {
        [SerializeField]
        private Element _prefab;
        [SerializeField]
        private List<Element> _list = new List<Element>();

        public void UpdateList(List<IElementData> data)
        {
            var count = data.Count;
            var loopCount = Mathf.Max(count, _list.Count);
            for (var i = 0; i < loopCount; ++i)
            {
                if (i >= _list.Count)
                {
                    var newItem = Instantiate(_prefab, transform);
                    _list.Add(newItem);
                }
                var element = _list[i];
                element.gameObject.GameObjectSetActive(i < count);
                if (i < count)
                {
                    element.SetData(data[i]);
                }
                else
                {
                    element.ClearData();
                }
            }
        }
    }
}
