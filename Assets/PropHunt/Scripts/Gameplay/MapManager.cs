using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MapManager : NetworkBehaviour
{
    // Start is called before the first frame update
    // L'instance unique de la classe
    private static MapManager _instance;
    public List<NetworkObject> instantiateObjectNetwork = new List<NetworkObject>();


    
    // Propri�t� publique pour acc�der � l'instance
    public static MapManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Cr�e l'instance si elle n'existe pas
                _instance = FindObjectOfType<MapManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("MapManager");
                    _instance = obj.AddComponent<MapManager>();
                }
            }
            return _instance;
        }
    }
    // Rendre le constructeur priv�
    private MapManager() { }


    string path = null;

    public List<Transform> SpawnPoints;
    public string _mapSelected = null;

    public TextMeshProUGUI _textmapSelected;
    public LaunchGameButton buttonLaunch;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject); // Optionnel, pour garder l'instance � travers les sc�nes
        }
        else
        {
            Destroy(gameObject); // S'assure qu'il n'y a pas d'autres instances
        }
    }

    public override void OnNetworkSpawn()

    {
        Debug.Log(IsServer);
        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
            return;

        path = Application.streamingAssetsPath + "/";
        _textmapSelected = GameObject.FindWithTag("MapSelect").GetComponent<TextMeshProUGUI>();
        _textmapSelected.text = "none";


        //Debug.Log("sperme");
        //Debug.Log(_textmapSelected);
        //for (int i = 0; i < nPropsToSpawn; i++)
        //{
        //    var sp = SpawnPoints[i];
        //    var transform = PropToSpawn.transform;
        //    var go = Instantiate(PropToSpawn.gameObject, sp.position, transform.rotation);
        //    go.GetComponent<NetworkObject>().Spawn();
        //}
    }

    public void setButton(LaunchGameButton button)
    {
        buttonLaunch = button;
        buttonLaunch.CheckIfInteractable();

    }

    public void SetMapSelected(string mapSelected)
    {
        _textmapSelected.text = mapSelected;
        buttonLaunch.CheckIfInteractable();


        _mapSelected = mapSelected;

    }
    // Cette fonction retourne un tableau de cha�nes contenant les noms de tous les fichiers dans le dossier sp�cifi�
    public string[] GetFileNamesInDirectory()
    {

        string directoryPath = Application.streamingAssetsPath;
        try
        {
            // Assurez-vous que le chemin sp�cifi� existe
            if (Directory.Exists(directoryPath))
            {
                // R�cup�re tous les fichiers dans le dossier
                string[] filePaths = Directory.GetFiles(directoryPath);

                // Pour stocker uniquement les noms de fichiers
                string[] fileNames = new string[filePaths.Length];

                for (int i = 0; i < filePaths.Length; i++)
                {
                    // Obtient le nom de fichier sans le chemin complet
                    fileNames[i] = Path.GetFileName(filePaths[i]);
                }

                return fileNames;
            }
            else
            {
                Debug.LogWarning("Directory does not exist: " + directoryPath);
                return new string[0];
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("An error occurred: " + ex.Message);
            return new string[0];
        }
    }


    public void LoadMap()
    {
        if (_mapSelected == null || _mapSelected == "none") return;
        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
            return;

        string filePath = Path.Combine(Application.streamingAssetsPath, _mapSelected);

        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        MapData mapData = JsonUtility.FromJson<MapData>(json);

        foreach (MapObject mapObject in mapData.MapObjects)
        {
            string ObjectIDClean = mapObject.ObjectID.Replace("(Clone)", "");
            Debug.Log(ObjectIDClean);
            GameObject prefab = Resources.Load<GameObject>(ObjectIDClean); // Assurez-vous que les pr�fabriqu�s sont dans un dossier Resources
            if (prefab == null)
            {
                Debug.LogError("Prefab not found for ID: " + ObjectIDClean);
                continue;
            }

            GameObject instance = Instantiate(prefab, mapObject.Position, mapObject.Rotation);
            //instance.transform.localScale = mapObject.Scale;

            NetworkObject NetObj = instance.GetComponent<NetworkObject>();
            if (NetObj != null)
            {
                instantiateObjectNetwork.Add(NetObj);
                NetObj.Spawn();
            }
            }
    }

    public void DeleteAllObjectsNetwork()
    {
        foreach (NetworkObject NetObj in instantiateObjectNetwork)
        {
            NetObj.Despawn(true);
        }
        instantiateObjectNetwork.Clear();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
