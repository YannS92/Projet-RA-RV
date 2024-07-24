using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SupprimerObject : MonoBehaviour
{
    private XRRayInteractor xrRayInteractor;

    private void Start()
    {
        // Tentative de r�cup�ration de l'interacteur XR Ray attach� au GameObject
        xrRayInteractor = GetComponent<XRRayInteractor>();

        // V�rifie si l'interacteur a �t� attach� correctement
        if (xrRayInteractor == null)
        {
            Debug.LogError("L'interacteur XR Ray n'est pas attach� au GameObject.");
        }
        else
        {
            Debug.Log("Interacteur XR Ray attach� avec succ�s.");

            // Ajoutez des listeners pour les �v�nements d'interaction
            xrRayInteractor.onSelectEntered.AddListener(OnSelectEntered);
            Debug.Log("Listener d'�v�nement ajout�.");
        }
    }

    private void OnSelectEntered(XRBaseInteractable interactable)
    {
        // Appel� lorsque le bouton de d�clenchement est press�
        SupprimerObjet(interactable.gameObject);
    }

    private void SupprimerObjet(GameObject objetASupprimer)
    {
        Destroy(objetASupprimer);
    }
}
