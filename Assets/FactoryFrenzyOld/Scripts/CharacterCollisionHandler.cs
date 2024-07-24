using UnityEngine;

public class CharacterCollisionHandler : MonoBehaviour
{
    public CharacterController characterController;
    public Rigidbody rb;
    public MeshCollider meshCollider;

    public float collisionDuration = 1f; // Durée avant de repasser en mode CharacterController
    private float collisionTimer = 0f;

    private bool isRigidbodyMode = false;

    private void Update()
    {
        if (isRigidbodyMode)
        {
            collisionTimer += Time.deltaTime;
            if (collisionTimer >= collisionDuration)
            {
                SwitchToCharacterControllerMode();
            }
        }
    }

    public void StartCollision()
    {
        isRigidbodyMode = true;
        collisionTimer = 0f;

        characterController.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        meshCollider.enabled = true;
    }

    public void SwitchToCharacterControllerMode()
    {
        isRigidbodyMode = false;
        collisionTimer = 0f;

        rb.isKinematic = true;
        rb.useGravity = false;
        meshCollider.enabled = false;
        characterController.enabled = true;
    }

    public void SwitchToRigidbodyMode()
    {
        StartCollision();
    }
}
