using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using Unity.Netcode;
using Unity.UI;

public class HunterController : ClassController
{
    public GameObject bulletPrefab; // Prefab du projectile
    public float fireRate = 1.5f; // Taux de tir (temps en secondes entre les tirs)
    private bool isShooting = false;
    public Transform firePoint; // Point de feu pour instancier les balles
    private float nextFireTime = 0f;

    void Update()
    {
        // Vérifier si le joueur peut tirer
        if (isShooting && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            TryShoot();
        }
    }

    // Appelée pour commencer ou arrêter de tirer
    public void SetShooting(bool shooting)
    {
        isShooting = shooting;
    }

    private void TryShoot()
    {
        if (IsClient) // Assurer que le tir est géré par le client
        {
            ShootServerRpc();
        }
    }

    [ServerRpc]
    private void ShootServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        NetworkObject bulletNetObj = bullet.GetComponent<NetworkObject>();
        bulletNetObj.Spawn();

        BulletPlayer bulletComponent = bullet.GetComponent<BulletPlayer>();
        if (bulletComponent != null)
        {
            Vector3 shootDirection = firePoint.forward;
            //bulletComponent.Initialize(shootDirection, transform.parent.gameObject);
        }
    }
public override void Activate()
    {
        gameObject.SetActive(true);
        _camera.transform.SetParent(transform);

        _camera.transform.localPosition = new Vector3(-0.4f, 0.85f, -1.4f);
        ResetAnimator();
    }

    public override void Deactivate()
    {
        gameObject.SetActive(false);
    }


}




