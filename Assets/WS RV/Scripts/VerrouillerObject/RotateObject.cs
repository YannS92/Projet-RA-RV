using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private XRRayInteractor lockRay;

    [SerializeField]
    private InputActionReference lockAction;

    // Start is called before the first frame update
    void Start()
    {
        lockRay.enabled = false;

        lockAction.action.Enable();
        lockAction.action.performed += OnRotateActivate;
        lockAction.action.canceled += OnRotateCancel;
    }

    private void OnRotateActivate(InputAction.CallbackContext context)
    {
        // Activer la suppression d'objet
        lockRay.enabled = true;
    }

    private void OnRotateCancel(InputAction.CallbackContext context)
    {
        // Désactiver la suppression d'objet
        lockRay.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Vérifier si le rayon est actif pour la suppression d'objet
        if (lockRay.enabled)
        {
            RaycastHit hit;
            if (lockRay.TryGetCurrent3DRaycastHit(out hit))
            {
                Debug.Log("rotate");

                GameObject objectToRotate = hit.collider.gameObject;
                // Faire tourner l'objet de 10 degrés autour de l'axe Y
                objectToRotate.transform.Rotate(0, 10, 0, Space.World);
            }
        }
    }
}
