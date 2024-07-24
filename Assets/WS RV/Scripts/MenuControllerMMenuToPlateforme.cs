using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControllerToPlateforme : MonoBehaviour
{
    public GameObject panelMainMenu;
    public GameObject panelSubMenuPlateforme;

    void Start()
    {
        // D�sactiver le sous-menu au d�but
        panelSubMenuPlateforme.SetActive(false);
    }

    public void FirstButtonClicked()
    {
        // D�sactiver le menu principal
        panelMainMenu.SetActive(false);

        // D�sactiver tous les sous-menus sauf le premier
        DisableAllSubMenus();

        // Activer le sous-menu li� au premier bouton
        panelSubMenuPlateforme.SetActive(true);
    }

    // Ajoutez des m�thodes similaires pour g�rer les autres boutons du menu principal

    void DisableAllSubMenus()
    {
        // D�sactiver tous les sous-menus ici
        panelSubMenuPlateforme.SetActive(false);
        // Ajoutez d'autres lignes pour d�sactiver d'autres sous-menus si n�cessaire
    }
}
