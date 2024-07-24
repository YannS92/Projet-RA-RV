using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlateformeMove : MonoBehaviour
{
    private CharacterController characterController;
    private Transform platformCurrentlyOn;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (platformCurrentlyOn != null)
        {
            // Déplace le personnage avec la plateforme
            Vector3 newGlobalPosition = platformCurrentlyOn.TransformPoint(characterController.transform.localPosition);
            characterController.transform.position = new Vector3(newGlobalPosition.x, characterController.transform.position.y, newGlobalPosition.z);
        }

        // Gérer les mouvements normaux ici
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovingPlatform")) // Assurez-vous que la plateforme a le tag "MovingPlatform"
        {

            platformCurrentlyOn = other.transform;
            characterController.transform.parent = platformCurrentlyOn; // Optionnel
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            platformCurrentlyOn = null;
            characterController.transform.parent = null; // Optionnel
        }
    }
}
