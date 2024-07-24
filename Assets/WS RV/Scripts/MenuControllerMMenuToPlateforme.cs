using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControllerToPlateforme : MonoBehaviour
{
    public GameObject panelMainMenu;
    public GameObject panelSubMenuPlateforme;

    void Start()
    {
        // Désactiver le sous-menu au début
        panelSubMenuPlateforme.SetActive(false);
    }

    public void FirstButtonClicked()
    {
        // Désactiver le menu principal
        panelMainMenu.SetActive(false);

        // Désactiver tous les sous-menus sauf le premier
        DisableAllSubMenus();

        // Activer le sous-menu lié au premier bouton
        panelSubMenuPlateforme.SetActive(true);
    }

    // Ajoutez des méthodes similaires pour gérer les autres boutons du menu principal

    void DisableAllSubMenus()
    {
        // Désactiver tous les sous-menus ici
        panelSubMenuPlateforme.SetActive(false);
        // Ajoutez d'autres lignes pour désactiver d'autres sous-menus si nécessaire
    }
}
