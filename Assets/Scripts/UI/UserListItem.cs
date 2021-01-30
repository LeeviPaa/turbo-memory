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

    [SerializeField]
    private TMP_Text _score;

    public override void UpdateVisuals()
    {
        var data = (PlayerData)_data;
        if (GameManager.Instance.PlayerRoles.ContainsKey(data.Player))
        {
            _role.text = GameManager.Instance.PlayerRoles[data.Player].ToString();
        }
        else
        {
            _role.text = PlayerRole.Human.ToString();
        }
        if (GameManager.Instance.PlayerScores.ContainsKey(data.Player))
        {
            _score.text = GameManager.Instance.PlayerScores[data.Player].ToString();
        }
        else
        {
            _score.text = "0";
        }
        _userName.text = data.Player.NickName;
    }

    protected override void BindData()
    {
        GameManager.Instance.PlayerRoleChanged.AddListener(OnPlayerRoleChanged);
        GameManager.Instance.PlayerScoreChanged.AddListener(OnPlayerScoreChanged);
    }

    protected override void UnbindData()
    {
        GameManager.Instance.PlayerRoleChanged.RemoveListener(OnPlayerRoleChanged);
        GameManager.Instance.PlayerScoreChanged.RemoveListener(OnPlayerScoreChanged);
    }

    public void OnPlayerRoleChanged(Player player, PlayerRole role)
    {
        _role.text = role.ToString();
    }

    public void OnPlayerScoreChanged(Player player, int score)
    {
        _score.text = score.ToString();
    }
}
