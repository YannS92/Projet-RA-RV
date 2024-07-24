using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LaunchGameButton : MonoBehaviour
{
    // Référence au NetworkManager (assurez-vous de l'assigner dans l'éditeur Unity)
    public NetworkManager networkManager;
    public GameOnlineManager gameOnlineManager;
    public MapManager _mapManager;
    //Button LaunchGameButton;

    // Nom de la scène à charger
    public string GameSceneName = "Game";

    private void Start()
    {

        // Initialise le NetworkManager s'il n'est pas déjà assigné
        if (networkManager == null)
        {
            networkManager = FindObjectOfType<NetworkManager>(); // Trouve l'instance de NetworkManager dans la scène
        }


        if (_mapManager == null)
        {
            _mapManager = FindObjectOfType<MapManager>(); // Trouve l'instance de NetworkManager dans la scène
            _mapManager.setButton(this);
        }
        if (gameOnlineManager == null)
        {
            gameOnlineManager = FindObjectOfType<GameOnlineManager>();
        }

        // Exemple : désactive le bouton si ce n'est pas l'host
        //if (networkManager != null && !networkManager.IsHost)  // Correction ici
        //{
        //    GetComponent<Button>().interactable = false;
        //}
        GetComponent<Button>().interactable = false;


    }

    public void CheckIfInteractable()
    {
        // Exemple : désactive le bouton si ce n'est pas l'host
        if (networkManager.IsHost && _mapManager._textmapSelected.text != null && _mapManager._textmapSelected.text != "none")  // Correction ici
        {
            GetComponent<Button>().interactable = true;
        }
    }

    // Fonction appelée lors du clic sur le bouton
    public void LaunchGame()
    {
        // Assurez-vous que l'application est en mode serveur ou host
        if (networkManager != null && (networkManager.IsServer || networkManager.IsHost))  // Correction ici
        {
            // Chargez la scène de jeu
            NetworkManager.Singleton.LocalClient.PlayerObject?.GetComponent<PlayerManager>().ToggleCursorLock();
            NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
            //gameOnlineManager.GameStart();
            //Cursor.lockState = CursorLockMode.None;

        }
    }
}