using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SupprimerObject : MonoBehaviour
{
    private XRRayInteractor xrRayInteractor;

    private void Start()
    {
        // Tentative de récupération de l'interacteur XR Ray attaché au GameObject
        xrRayInteractor = GetComponent<XRRayInteractor>();

        // Vérifie si l'interacteur a été attaché correctement
        if (xrRayInteractor == null)
        {
            Debug.LogError("L'interacteur XR Ray n'est pas attaché au GameObject.");
        }
        else
        {
            Debug.Log("Interacteur XR Ray attaché avec succès.");

            // Ajoutez des listeners pour les événements d'interaction
            xrRayInteractor.onSelectEntered.AddListener(OnSelectEntered);
            Debug.Log("Listener d'événement ajouté.");
        }
    }

    private void OnSelectEntered(XRBaseInteractable interactable)
    {
        // Appelé lorsque le bouton de déclenchement est pressé
        SupprimerObjet(interactable.gameObject);
    }

    private void SupprimerObjet(GameObject objetASupprimer)
    {
        Destroy(objetASupprimer);
    }
}
