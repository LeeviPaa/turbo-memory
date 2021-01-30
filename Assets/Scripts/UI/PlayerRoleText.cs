using TMPro;
using UnityEngine;

public class PlayerRoleText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private PlayerController _player;

    private void Update()
    {
        if(_player != null)
            _text.text = _player.Role.ToString();
    }
}