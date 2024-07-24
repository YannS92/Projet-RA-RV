using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

//public class VerrouillerObjectManager : MonoBehaviour
//{
//    [SerializeField]
//    private XRRayInteractor lockRay;

//    [SerializeField]
//    private InputActionReference lockAction;

//    private XRGrabInteractable grabInteractable;  // Utilisez XRGrabInteractable au lieu de XRGrabInteractor

//    // Start is called before the first frame update
//    void Start()
//    {
//        lockRay.enabled = false;

//        lockAction.action.Enable();
//        lockAction.action.performed += OnLockToggle;

//        // Récupérer le composant XR Grab Interactable sur cet objet
//        grabInteractable = GetComponent<XRGrabInteractable>();
//    }

//    private void OnLockToggle(InputAction.CallbackContext context)
//    {
//        // Inverser l'état de verrouillage à chaque pression du bouton
//        bool isLocked = !lockRay.enabled;

//        // Activer ou désactiver le rayon en fonction de l'état de verrouillage
//        lockRay.enabled = isLocked;

//        // Appeler la fonction de verrouillage/déverrouillage de l'objet ici
//        ChangeObjectState(isLocked);
//    }

//    private void ChangeObjectState(bool isLocked)
//    {
//        Debug.Log(isLocked ? "Object Locked" : "Object Unlocked");

//        // Désactiver le composant XR Grab Interactable
//        if (grabInteractable != null)
//        {
//            grabInteractable.enabled = !isLocked;
//        }
//    }
//}


public class VerrouillerObjectManager : MonoBehaviour
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
        lockAction.action.performed += OnLockActivate;
        lockAction.action.canceled += OnLockCancel;
    }

    private void OnLockActivate(InputAction.CallbackContext context)
    {
        // Activer la suppression d'objet
        lockRay.enabled = true;
    }

    private void OnLockCancel(InputAction.CallbackContext context)
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
               Debug.Log("hit");

                GameObject objectToLock = hit.collider.gameObject;
                XRGrabInteractable grabInteractable = objectToLock.GetComponent<XRGrabInteractable>();
                if (grabInteractable != null)
                {
                    Debug.Log("grabInteractable.enabled");
                    grabInteractable.enabled = !grabInteractable.enabled;
                }

            }
        }
    }
}
