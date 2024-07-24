using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class DeleteObjectManager : MonoBehaviour
{
    [SerializeField]
    private XRRayInteractor deleteRay;

    [SerializeField]
    private InputActionReference deleteAction;

    // Start is called before the first frame update
    void Start()
    {
        deleteRay.enabled = false;

        deleteAction.action.Enable();
        deleteAction.action.performed += OnDeleteActivate;
        deleteAction.action.canceled += OnDeleteCancel;
    }

    private void OnDeleteActivate(InputAction.CallbackContext context)
    {
        // Activer la suppression d'objet
        deleteRay.enabled = true;
    }

    private void OnDeleteCancel(InputAction.CallbackContext context)
    {
        // Désactiver la suppression d'objet
        deleteRay.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Vérifier si le rayon est actif pour la suppression d'objet
        if (deleteRay.enabled)
        {
            RaycastHit hit;
            if (deleteRay.TryGetCurrent3DRaycastHit(out hit))
            {
                GameObject objectToDelete = hit.collider.gameObject;
                Destroy(objectToDelete);
            }
        }
    }
}
