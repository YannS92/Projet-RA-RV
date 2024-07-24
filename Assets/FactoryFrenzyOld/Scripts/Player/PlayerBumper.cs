using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBumper : MonoBehaviour
{
    [Tooltip("The force applied when the player hits the bumper")]
    public float BumpForce = 90.0f;

    [Tooltip("The minimum upward angle when applying bump force")]
    [Range(0, 90)]
    public float MinUpwardAngle = 15.0f;

    public float bounceForce = 3f;

    private ThirdPersonController playerController;

    private void Start()
    {
        playerController = GetComponent<ThirdPersonController>();

    }

    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.gameObject.CompareTag("Bumper"))
    //    {
    //        Vector3 directionDePropulsion = -hit.normal; // Direction opposée au point de collision
    //        float forceDePropulsion = 10f; // Ajustez cette valeur selon vos besoins

    //        // Appliquer la force de propulsion
    //        playerController.ApplyHorizontalForce(directionDePropulsion * forceDePropulsion);
    //    }
    //}


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Bumper"))
        {
            // Calculer la direction horizontale de la force à appliquer
            Vector3 bumpDirection = hit.transform.position - hit.controller.transform.position;
            bumpDirection.y = 0; // Enlever la composante verticale
            bumpDirection = -bumpDirection.normalized;

            // Ajouter un angle vers le haut pour éviter de pousser le joueur vers le bas
            bumpDirection = Quaternion.Euler(MinUpwardAngle, 0, 0) * bumpDirection;

            // Appliquer la force du bumper
            hit.controller.Move(bumpDirection * BumpForce * Time.deltaTime);
            //playerController.ApplyVerticalForce(bounceForce); // prob 

        }
    }
}


