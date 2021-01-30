using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UserListItem : Element
{

    [SerializeField]
    private TMP_Text _role;

    [SerializeField]
    private TMP_Text _userName;

    public override void UpdateVisuals()
    {
        var data = (PlayerData)_data;
        if (GameManager.Instance.PlayerRoles.ContainsKey(data.Player))
        {
            _role.text = GameManager.Instance.PlayerRoles[data.Player].ToString();
        }
        else
        {
            _role.text = string.Empty;
        }
        _userName.text = data.Player.NickName;
    }

    protected override void BindData()
    {
        GameManager.Instance.PlayerRoleChanged.AddListener(OnPlayerRoleChanged);
    }

    protected override void UnbindData()
    {
        GameManager.Instance.PlayerRoleChanged.RemoveListener(OnPlayerRoleChanged);
    }

    public void OnPlayerRoleChanged(Player player, PlayerRole role)
    {
        UpdateVisuals();
    }
}
