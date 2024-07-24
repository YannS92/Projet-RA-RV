// MINI Projet XR RAY ne fonctionne pas

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandMenuManager : MonoBehaviour
{
    public InputActionReference activateRef;
    private InputAction activate;

    public HandMenu HandMenu;
    public GameObject panelMainMenu;  // Assurez-vous de lier cet objet dans l'inspecteur Unity.

    [SerializeField]
    private List<GameObject> objectToDisable;

    private void Awake()
    {
        activate = activateRef.action;
        activate.Enable();
        activate.performed += Activate;
    }

    private void OnDestroy()
    {
        activate.performed -= Activate;
    }

    private void Activate(InputAction.CallbackContext obj)
    {
        // Activer le menu principal s'il doit être affiché
        if (obj.ReadValueAsButton())
        {
            panelMainMenu.SetActive(true);
        }

        // Afficher ou masquer le menu en fonction du bouton appuyé
        HandMenu.Show(obj.ReadValueAsButton());

        // Désactiver les objets spécifiés lors de l'activation du menu
        foreach (GameObject go in objectToDisable)
        {
            go.SetActive(false);
        }
    }

    public void CloseMenu()
    {
        HandMenu.CloseMenu();  // Appeler la méthode de fermeture de HandMenu
        foreach (GameObject go in objectToDisable)
        {
            go.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
