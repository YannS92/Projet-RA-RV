using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

public class MapDataManager : MonoBehaviour
{

    public static MapDataManager Instance;

    public MapData CurrentMapData { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SaveMap()
    {

        string basePath = Application.streamingAssetsPath;
        string baseFileName = "mapData";
        string fileExtension = ".json";
        int counter = 0;

        string fileName = baseFileName + fileExtension;
        string filePath = Path.Combine(basePath, fileName);
        //Debug.Log(fileName);
        //Debug.Log(filePath);

        // Trouver un nom de fichier non utilisé
        while (File.Exists(filePath))
        {
            counter++;
            fileName = baseFileName + counter.ToString() + fileExtension;
            filePath = Path.Combine(basePath, fileName);
        }
        CurrentMapData = new MapData();

        // Collecter les données de chaque objet à sauvegarder
        foreach (var prefab in FindObjectsOfType<GameObject>())
        {
            if (prefab.CompareTag("Saveable"))
            {
                Vector3 finalPosition = new Vector3(0,0,0);
                float speed = 0;



                var movablePlatform = prefab.GetComponent<MovePlateformValueSlider>();

                if (movablePlatform)
                {


                    finalPosition = movablePlatform.posToMove; // Obtenez la position finale
                    speed = movablePlatform.speed;


                }

                var mapObject = new MapObject(
                    prefab.name,
                    prefab.transform.position,
                    prefab.transform.rotation,
                    prefab.transform.localScale,
                    finalPosition,
                    speed );


                CurrentMapData.AddObject(mapObject);
            }
        }

        // Sérialiser et sauvegarder
        string json = JsonUtility.ToJson(CurrentMapData);
        File.WriteAllText(filePath, json);
    }

    public MapData LoadMap(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<MapData>(json);
    }
}
