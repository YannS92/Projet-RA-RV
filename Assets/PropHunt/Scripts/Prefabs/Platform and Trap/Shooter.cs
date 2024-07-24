using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class Shooter : NetworkBehaviour
{
    public Transform target;
    public float rotationSpeed = 2.0f;
    public GameObject bulletPrefab;
    public float fireRate = 1.5f;

    private bool isShooting = false;
    public Transform firePoint;


    void Update()
    {
        if (IsServer && target != null)
        {

            RotateTowardsTarget();
            TryShoot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        Transform playerTransform = other.gameObject.transform.root.gameObject.transform;
        PlayerManager playerColManager = other.gameObject.transform.root.gameObject.GetComponent<PlayerManager>();

        if (playerColManager != null)
        {
            target = playerTransform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;

        PlayerManager playerColManager = other.gameObject.transform.root.gameObject.GetComponent<PlayerManager>();

        if (playerColManager != null)
        {
            target = null;
        }
    }

    private void RotateTowardsTarget()
    {
        Vector3 targetDirection = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void TryShoot()
    {
        if (isShooting) return;

        Vector3 targetDirection = target.position - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, targetDirection);

        if (angleToTarget < 30.0f)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        isShooting = true;

        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        NetworkObject bulletNetObj = bulletInstance.GetComponent<NetworkObject>();

        if (bulletNetObj != null)
        {
            bulletNetObj.Spawn();
        }

        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            Vector3 shootingDirection = (target.position - firePoint.position).normalized;
            bulletScript.Initialize(shootingDirection);
        }

        yield return new WaitForSeconds(fireRate);

        isShooting = false;
    }
}
