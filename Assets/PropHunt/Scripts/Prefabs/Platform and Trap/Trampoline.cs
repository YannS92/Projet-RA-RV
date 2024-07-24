using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float forceMagnitude = 380f; // Ajustez cette valeur en fonction de la force souhaitée
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            ActionInput actionInput = collision.gameObject.GetComponent<ActionInput>();

            if (playerRigidbody != null)
            {
                // Appliquer une force verticale
                playerRigidbody.AddForce(Vector3.up * forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}
