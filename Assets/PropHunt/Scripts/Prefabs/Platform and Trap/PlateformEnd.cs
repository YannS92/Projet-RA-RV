using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateformEnd : MonoBehaviour
{

    GameOnlineManager _gameOnlineManager;
    bool localPlayerRaceFinish = false;
    void Start()
    {
            _gameOnlineManager = FindObjectOfType<GameOnlineManager>();

    }

    void OnTriggerEnter(Collider collision)
    {

        if (_gameOnlineManager.IsGameStart == false || localPlayerRaceFinish == true) return;

        PlayerManager playerColManager = collision.gameObject.transform.root.gameObject.GetComponent<PlayerManager>();


        if (NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>() == playerColManager)
        {
            Debug.Log("Arrivé franchi");


            playerColManager.PlayerCrossedFinishLineServerRpc(_gameOnlineManager._pseudo);
            localPlayerRaceFinish = true;
        }

    }
}
