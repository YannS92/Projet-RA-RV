using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
            playerManager.Respawn();

        }
    }
}
