using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public Vector3 direction;
    public float pushForce = 35000f; // La force de propulsion appliquée au joueur

    void Start()
    {
        if (IsServer)
        {
            Invoke(nameof(DestroySelf), lifeTime);
        }
    }

    public void Initialize(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        if (IsServer)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        PlayerManager playerColManager = other.gameObject.transform.root.gameObject.GetComponent<PlayerManager>();

        if (playerColManager)
        {



            var rpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { playerColManager._idClient }
                }
            };
            BulletImpactClientRpc(direction, pushForce, rpcParams);

        }
    }

    [ClientRpc]
    public void BulletImpactClientRpc(Vector3 direction, float pushForce,  ClientRpcParams rpcParams)
    {
        Rigidbody rb = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Rigidbody>();
        rb.AddForce(direction * pushForce, ForceMode.Impulse);
        destroyAfterImpactServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void destroyAfterImpactServerRpc()
    {
        DestroySelf();

    }

    private void DestroySelf()
    {
        NetworkObject.Despawn();
    }
}