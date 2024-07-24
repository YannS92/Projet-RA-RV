using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenuClose : MonoBehaviour
{
    public GameObject panelMainMenu;
    private HandMenuManager handMenuManager;
    public HandMenu handMenu;  // Assurez-vous de lier cet objet dans l'inspecteur Unity.

    // Ajoutez une liste d'objets � d�sactiver lors de la fermeture du menu
    public List<GameObject> objectsToDisable;

    void Start()
    {
        handMenuManager = FindObjectOfType<HandMenuManager>();

        if (handMenuManager == null)
        {
            Debug.LogError("HandMenuManager non trouv�.");
        }
    }

    public void FirstButtonClicked()
    {
        // D�sactiver le menu principal
        panelMainMenu.SetActive(false);

        // R�activer les objets sp�cifi�s lors de la fermeture du menu
        handMenuManager.CloseMenu();
    }

    public void OpenMainMenu()
    {
        // V�rifier si le PanelMainMenu est d�j� activ�
        if (!panelMainMenu.activeSelf)
        {
            // Activer � nouveau le menu principal
            panelMainMenu.SetActive(true);
        }
    }

    public void CloseMenu()
    {
        // Appeler la m�thode CloseMenu de HandMenu pour d�sactiver le menu
        handMenu.CloseMenu();
    }
}
