using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameOnlineManager : NetworkBehaviour
{

    private List<PlayerManager> playerManagers = new List<PlayerManager>();
    private List<ulong> ResultPlayersList = new List<ulong>();

    private float _gameStartTime = 10.0f; // Dur�e du d�compte en secondes
    private float _gameEndTime = 10.0f; // Dur�e du d�compte en secondes
    PlayerManager _playerManagerClient = null;
    public MapSelect _mapSelect;


    private NetworkVariable<bool> _gameStart = new NetworkVariable<bool>(false);
    private string _result = "";
    //private bool _raceFinished = false;
    GameObject spawnPosPlayers;

    public String _pseudo;

    private void Awake()
    {
        _mapSelect = FindObjectOfType<MapSelect>();

        DontDestroyOnLoad(gameObject);
    }



    public override void OnNetworkSpawn() //OnNetworkSpawn
    {

        //_playerManagerClient = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
        //_gameStart.OnValueChanged += TpPlayerStart;


    }
    public bool IsGameStart
    {
        get { return _gameStart.Value; }

    }

    public PlayerManager getLocalPlayerManager()
    { return _playerManagerClient; }


    private void OnClientConnected(ulong clientId)
    {
        var playerManager = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerManager>();
        //Debug.Log(clientId);
        if (playerManager != null)
        {
            playerManager._idClient = clientId;
            playerManagers.Add(playerManager);
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        var playerManager = playerManagers.FirstOrDefault(p => p.OwnerClientId == clientId);
        if (playerManager != null)
        {
            playerManagers.Remove(playerManager);
        }
    }





    private void TpPlayersStart()
    {
        SortPlayerManagers();

        foreach (PlayerManager playerManager in playerManagers)
        {

            var rpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { playerManager._idClient }
                }
            };

            if (_gameStart.Value == true && spawnPosPlayers != null)
            {
                //Debug.Log(spawnPosPlayers);
                //Debug.Log(playerManagers);
                //Debug.Log(playerManagers.IndexOf(_playerManagerClient));


                var spawnPosition = spawnPosPlayers.transform.GetChild(playerManagers.IndexOf(playerManager)).position;
                spawnPosition.y += 10;
                TeleportClientRpc(spawnPosition, rpcParams );


            }
            if(_gameStart.Value == false)
            {
                UnityEngine.Vector3 spawnLobby = new UnityEngine.Vector3(0, 5, 0);
                TeleportClientRpc(spawnLobby, rpcParams);

            }
        }

    }

    [ClientRpc]
    public void TeleportClientRpc(Vector3 Pos, ClientRpcParams rpcParams )
    {
        var playerManager = getLocalPlayerManager();

        playerManager.TeleportToPos(Pos);
        playerManager.SetSpawn(Pos);
    }



    public void setPlayerManagerLocal(PlayerManager playermanager)
    {
        if(_playerManagerClient == null) _playerManagerClient = playermanager;


    }

    public void GameStart()
    {
        if (IsServer)
        {
            spawnPosPlayers = GameObject.FindGameObjectWithTag("SpawnPlatform");
            Debug.Log(spawnPosPlayers);
            Debug.Log("game start");
            _mapSelect.ToggleMapSelectVisibility(false);
            _gameStart.Value = true;
            StartCompteurRaceClientRpc();
            StartCoroutine(StartGameTimer());
            TpPlayersStart();



        }
    }

    private IEnumerator StartGameTimer()
    {
        float remainingTime = _gameStartTime;
        while (remainingTime > 0)
        {
            UpdateCountdownClientRpc("La partie commence dans : \n" + remainingTime); // Mettre � jour le d�compte sur les clients
            yield return new WaitForSeconds(1f);
            remainingTime--;
            //Debug.Log(remainingTime);
        }

        StartRaceClientRpc(); 
    }

    [ClientRpc]
    private void UpdateCountdownClientRpc(string str)
    {
        
        _playerManagerClient.UpdateCompteurUI(str);
    }

    [ClientRpc]
    private void StartCompteurRaceClientRpc()
    {
        _result = "";
        _playerManagerClient.UpdateScoreUI(_result);
        _playerManagerClient.DisableController(true);
    }
    [ClientRpc]
    private void StartRaceClientRpc()
    {
        // R�activer le contr�le du joueur
        // Par exemple, permettre aux joueurs de se d�placer
        _playerManagerClient.DisableController(false);

        _playerManagerClient.UpdateCompteurUI("");
        Debug.Log("Le jeu commence !");
    }







    private IEnumerator StartEndTimer()
    {
        float remainingTime = _gameEndTime;
        while (remainingTime > 0)
        {
            UpdateCountdownClientRpc("La partie se termine dans : \n" + remainingTime); // Mettre � jour le d�compte sur les clients
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }
        RaceFinishedClientRpc();
        TpPlayersStart();
        ResultPlayersList.Clear();
        _mapSelect.ToggleMapSelectVisibility(true);


    
    //detelete all objects from instantiateObjectNetwork 
    MapManager mapManager = MapManager.Instance;
    mapManager.DeleteAllObjectsNetwork();


        _gameStart.Value = false;
        NetworkManager.Singleton.SceneManager.LoadScene("Lobby", UnityEngine.SceneManagement.LoadSceneMode.Single);

    }

    public void RaceOver()
    {
        //_raceFinished = true;
        StartCoroutine(StartEndTimer());

    }

    private void SortPlayerManagers()
    {
        playerManagers = playerManagers.OrderBy(pm => pm.OwnerClientId).ToList();
    }


    public int CalculatePlayerRank(ulong _idClient)
    {

        if (ResultPlayersList.Contains(_idClient))
            return ResultPlayersList.IndexOf(_idClient);
        else
            ResultPlayersList.Add(_idClient);
        return ResultPlayersList.IndexOf(_idClient);

    }

    [ClientRpc]
    public void PlayerCrossedFinishLineClientRpc(String pseudo, int playerRank)
    {
        _result += playerRank + " - " + pseudo + "\n";
        _playerManagerClient.UpdateScoreUI(_result);
    }

    [ClientRpc]
    private void RaceFinishedClientRpc()
    {

        _playerManagerClient.UpdateCompteurUI("");

    }


}
