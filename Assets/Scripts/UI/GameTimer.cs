using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private const string TimeTemplate = "Time left: {0}s";

    [SerializeField]
    private TextMeshProUGUI _text;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        _text.text = string.Format(TimeTemplate, (int)_gameManager.GameTimeLeft);
    }
}
