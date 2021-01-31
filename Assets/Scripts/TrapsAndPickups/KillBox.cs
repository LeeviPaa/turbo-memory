using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    [SerializeField]
    KillType TrapType;

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player"))
            return;

        PlayerController player = collider.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.KillPlayer(null, TrapType);
            return;
        }
    }
}
