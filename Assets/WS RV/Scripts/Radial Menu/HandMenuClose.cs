using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenuClose : MonoBehaviour
{
    public GameObject panelMainMenu;
    private HandMenuManager handMenuManager;
    public HandMenu handMenu;  // Assurez-vous de lier cet objet dans l'inspecteur Unity.

    // Ajoutez une liste d'objets à désactiver lors de la fermeture du menu
    public List<GameObject> objectsToDisable;

    void Start()
    {
        handMenuManager = FindObjectOfType<HandMenuManager>();

        if (handMenuManager == null)
        {
            Debug.LogError("HandMenuManager non trouvé.");
        }
    }

    public void FirstButtonClicked()
    {
        // Désactiver le menu principal
        panelMainMenu.SetActive(false);

        // Réactiver les objets spécifiés lors de la fermeture du menu
        handMenuManager.CloseMenu();
    }

    public void OpenMainMenu()
    {
        // Vérifier si le PanelMainMenu est déjà activé
        if (!panelMainMenu.activeSelf)
        {
            // Activer à nouveau le menu principal
            panelMainMenu.SetActive(true);
        }
    }

    public void CloseMenu()
    {
        // Appeler la méthode CloseMenu de HandMenu pour désactiver le menu
        handMenu.CloseMenu();
    }
}
