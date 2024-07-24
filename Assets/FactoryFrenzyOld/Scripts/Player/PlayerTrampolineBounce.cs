using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrampolineBounce : MonoBehaviour
{
    [Tooltip("The force of the bounce")]
    public float bounceForce = 15f;

    private ThirdPersonController playerController;

    private void Start()
    {
        playerController = GetComponent<ThirdPersonController>();

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Trampoline"))
        {
            // Apply the bounce force
            //playerController.ApplyVerticalForce(bounceForce);
        }
    }
}