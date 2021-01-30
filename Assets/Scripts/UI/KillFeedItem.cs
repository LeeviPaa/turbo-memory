using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class KillFeedItem : Element
    {
        [System.Serializable]
        public struct KillFeedIcon
        {
            public KillType Type;
            public Sprite Icon;
        }

        [SerializeField]
        private List<KillFeedIcon> _icons;
        [SerializeField]
        private TMP_Text _user;
        [SerializeField]
        private TMP_Text _target;
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private Animation _anim;

        public void Start()
        {
            if (_anim == null) return;
            _anim.clip.SampleAnimation(_anim.gameObject, 0f);
        }

        public override void UpdateVisuals()
        {
            _anim?.Stop();
            var data = (KillFeedData)_data;
            if (data == null) return;
            var hasUser = data.User != null;
            var IsLocalContext = hasUser && data.User.IsLocal || data.Target.IsLocal;
            _user.gameObject.GameObjectSetActive(hasUser);
            if (hasUser)
            {
                _user.text = data.User.NickName;
            }
            _target.text = data.Target.NickName;
            _anim?.Play();
        }

        public Sprite GetKillTypeSprite(KillType type)
        {
            foreach (var icon in _icons)
            {
                if (icon.Type == type) return icon.Icon;
            }
            if (type != KillType.Default) return GetKillTypeSprite(KillType.Default);
            return null;
        }

        protected override void BindData()
        {

        }

        protected override void UnbindData()
        {

        }
    }
}