using System.Collections;
using UnityEngine;
using Unity.Netcode;
using System;

public class BulletPlayer : NetworkBehaviour
{
    //public float speed = 10f;
    //public float lifeTime = 5f;
    //private Vector3 direction;
    //public float pushForce = 150f; // La force de propulsion appliquée au joueur
    //public GameObject owner; // Ajout pour stocker le propriétaire de la balle
    //PlayerManager ownerPlayerManager;
    //// Cette méthode peut être appelée sur le client, mais l'exécution se fait sur le serveur
    //public void Initialize(Vector3 dir, GameObject bulletOwner)
    //{
    //    owner = bulletOwner;
    //    ownerPlayerManager = owner.GetComponent<PlayerManager>();
    //    if (IsServer)
    //    {
    //        direction = dir.normalized;
    //        StartCoroutine(DestroyAfterTime(lifeTime));
    //    }
    //}

    //IEnumerator DestroyAfterTime(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    DestroyBullet();

    //    if(IsServer)
    //    {
    //        if(owner != null)
    //        {
    //            Debug.Log("dommage -1");
    //            ownerPlayerManager.TakeDamage(1);
    //        }
    //    }
    //}

    //void Update()
    //{
    //    if (IsServer)
    //    {
    //        transform.position += direction * speed * Time.deltaTime;
    //        //Debug.Log(transform.position);
    //    }
    //}

    ////void OnTriggerEnter(Collider other)
    ////{

    ////}

    //void OnCollisionEnter(Collision other)
    //{
    //    // Log ou actions quand la sphère entre en collision avec un autre objet
    //    //Debug.Log("Collision détectée avec " + other.gameObject.name);
    //    //Debug.Log("owner " + owner);
    //    //Debug.Log("Collision " + other.gameObject);

    //    if (other.gameObject != owner) // Vérifie que l'objet de collision n'est pas le propriétaire
    //    {


    //        if (IsServer)
    //        {
    //            Debug.Log("Touché");
    //            Debug.Log(other.gameObject);
    //            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
    //            {
    //                var playerManager = other.gameObject.GetComponent<PlayerManager>();
    //                if (playerManager != null && !playerManager.IsHunter)
    //                {
    //                    Debug.Log("dommage -1");
    //                    playerManager.TakeDamage(1);
    //                }
    //            }
    //            else
    //            {
    //                ownerPlayerManager.TakeDamage(1);
    //            }

    //            DestroyBullet();
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("owner touché");

    //    }
    //}

    //private void DestroyBullet()
    //{
    //    // D'abord, désapparaître l'objet du réseau
    //    NetworkObject.Despawn();
    //    // Ensuite, détruire l'instance de l'objet
    //    Destroy(gameObject);
    //}
}
