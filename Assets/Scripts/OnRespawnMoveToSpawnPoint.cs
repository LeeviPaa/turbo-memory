using UnityEngine;

public class OnRespawnMoveToSpawnPoint : MonoBehaviour, IDied
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void OnDeath()
    {
        
    }

    public void OnRespawn()
    {
        transform.position = _gameManager.SpawnPoint.position;
        transform.rotation = Quaternion.identity;
    }
}