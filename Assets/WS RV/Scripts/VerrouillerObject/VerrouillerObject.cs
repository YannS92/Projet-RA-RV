using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VerrouillerObject : MonoBehaviour
{
    private XRRayInteractor xrRayInteractor;
    private int propsLayer;

    private void Start()
    {
        xrRayInteractor = GetComponent<XRRayInteractor>();

        if (xrRayInteractor == null)
        {
            Debug.LogError("L'interacteur XR Ray n'est pas attaché au GameObject.");
        }
        else
        {
            Debug.Log("Interacteur XR Ray attaché avec succès.");

            xrRayInteractor.onSelectEntered.AddListener(OnSelectEntered);
            Debug.Log("Listener d'événement ajouté.");
        }

        propsLayer = LayerMask.NameToLayer("Props");
    }

    private void OnSelectEntered(XRBaseInteractable interactable)
    {
        // Vérifie si l'objet est déjà verrouillé
        bool estVerrouille = EstObjetVerrouille(interactable.gameObject);

        // Appelle la fonction appropriée en fonction de l'état actuel
        if (estVerrouille)
        {
            DeverrouillerObjet(interactable.gameObject);
            RetirerLayerProps(interactable.gameObject);
        }
        else
        {
            VerrouillerObjet(interactable.gameObject);
            AjouterLayerProps(interactable.gameObject);
        }
    }

    private void VerrouillerObjet(GameObject objetAVerrouiller)
    {
        // Désactiver le composant XRGrabInteractable pour verrouiller l'objet
        XRGrabInteractable grabInteractable = objetAVerrouiller.GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
            Debug.Log("Objet verrouillé : " + objetAVerrouiller.name);
        }
        else
        {
            Debug.LogWarning("Aucun composant XRGrabInteractable trouvé sur l'objet.");
        }
    }

    private void DeverrouillerObjet(GameObject objetADeverrouiller)
    {
        // Activer le composant XRGrabInteractable pour déverrouiller l'objet
        XRGrabInteractable grabInteractable = objetADeverrouiller.GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
            Debug.Log("Objet déverrouillé : " + objetADeverrouiller.name);
        }
        else
        {
            Debug.LogWarning("Aucun composant XRGrabInteractable trouvé sur l'objet.");
        }
    }

    private bool EstObjetVerrouille(GameObject objet)
    {
        // Vérifie si l'objet est verrouillé en regardant l'état du composant XRGrabInteractable
        XRGrabInteractable grabInteractable = objet.GetComponent<XRGrabInteractable>();
        return grabInteractable != null && !grabInteractable.enabled;
    }

    private void AjouterLayerProps(GameObject objet)
    {
        // Ajouter le layer "Props" à l'objet
        objet.layer |= (1 << propsLayer);
    }

    private void RetirerLayerProps(GameObject objet)
    {
        // Retirer le layer "Props" de l'objet
        objet.layer &= ~(1 << propsLayer);
    }
}
