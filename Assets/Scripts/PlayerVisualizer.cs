using System.Reflection.Emit;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerVisualizer : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;
    [SerializeField]
    private List<RoleVisuals> _roleVisuals;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera _closeUpCamera;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _player.RoleChanged.AddListener(OnRoleChanged);
        _closeUpCamera.enabled = _player.photonView.IsMine;
    }

    private bool ThisComponentIsMine()
    {
        return _player.photonView.IsMine;
    }

    private bool ThisComponentIsTargets(Player targetPlayer)
    {
        return _player.photonView.Owner == targetPlayer;
    }

    private bool TargetPlayerIsMe(Player targetPlayer)
    {
        return targetPlayer == PhotonNetwork.LocalPlayer;
    }

    private void OnRoleChanged(Player targetPlayer, PlayerRole newRole)
    {
        if(ThisComponentIsMine() && !TargetPlayerIsMe(targetPlayer))
            return;

        if(TargetPlayerIsMe(targetPlayer) && ThisComponentIsMine())
        {
            // my role changed
            SetVisualsForRole(newRole.ToString());
            return;
        }

        if(!TargetPlayerIsMe(targetPlayer) && ThisComponentIsTargets(targetPlayer))
        {
            PlayerRole clientRole = _gameManager.PlayerRoles[PhotonNetwork.LocalPlayer];

            if(clientRole == PlayerRole.Human && newRole != PlayerRole.Human)
                SetVisualsForRole("Invisible");
            else
            {
                SetVisualsForRole(newRole.ToString());
            }

            return;
        }

        if(TargetPlayerIsMe(targetPlayer) && !ThisComponentIsTargets(targetPlayer))
        {
            PlayerRole clientRole = _gameManager.PlayerRoles[PhotonNetwork.LocalPlayer];

            if(clientRole == PlayerRole.Human && newRole != PlayerRole.Human)
                SetVisualsForRole("Invisible");
            else
            {
                SetVisualsForRole(newRole.ToString());
            }

            return;
        }

        //when I turn into a ghost, other ghosts should turn visible
    }

    private void SetVisualsForRole(string role)
    {
        RoleVisuals targetRole = null;

        foreach (RoleVisuals roleVisuals in _roleVisuals)
        {
            if(roleVisuals.Role == role)
                targetRole = roleVisuals;

            roleVisuals.ToggleVisuals(false);
        }

        targetRole.ToggleVisuals(true);
    }
}

[System.Serializable]
public class RoleVisuals
{
    public List<GameObject> Visuals;
    public string Role;

    public void ToggleVisuals(bool activeStatus)
    {
        foreach (var visual in Visuals)
        {
            visual.SetActive(activeStatus);
        }
    }
}
