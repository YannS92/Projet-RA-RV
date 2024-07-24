using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrampoline : MonoBehaviour
{
    public float bounceStrength = 10f; // La force de rebond

    //ceci ne s'applique sur les rigidBodies donc pas notre personnage qui utilise un character controller (gere la méthode direct de bounce dans son script a lui)
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(gameObject.name + " a collisionné avec " + collision.gameObject.name);
        var rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 bounce = Vector3.up * bounceStrength;
            rb.AddForce(bounce, ForceMode.Impulse);
        }
    }
}
