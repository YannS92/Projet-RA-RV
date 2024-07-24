using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrapBumper : MonoBehaviour
{
    public float forceMagnitude = 1000f; // Ajustez cette valeur en fonction de la force souhaitée

    void OnTriggerEnter(Collider collision)
    {
        Rigidbody rb = collision.gameObject.transform.root.gameObject.GetComponent<Rigidbody>();

        if (rb)
        {

            Vector3 direction = collision.transform.position - transform.position;
            direction.y = 0; // Élimine la composante verticale

            rb.AddForce(direction * forceMagnitude, ForceMode.Impulse);
        }
    }
}