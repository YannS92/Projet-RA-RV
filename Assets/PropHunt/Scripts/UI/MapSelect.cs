using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // N�cessaire pour utiliser List

public class MapSelect : MonoBehaviour
{
    public GameObject togglePrefab; // R�f�rence � votre prefab Toggle
    public Transform scrollViewContent; // R�f�rence au contenu de votre ScrollView
    public MapManager mapManager;

    private List<Toggle> toggles = new List<Toggle>(); // Liste pour garder une trace des toggles

    void Start()
    {
        if(mapManager == null) mapManager = FindObjectOfType<MapManager>();
        string[] fileNames = mapManager.GetFileNamesInDirectory();
        foreach (string fileName in fileNames)
        {
            CreateToggleForFile(fileName);
        }
    }

    void CreateToggleForFile(string fileName)
    {
        GameObject toggleObj = Instantiate(togglePrefab, scrollViewContent);
        toggleObj.GetComponentInChildren<Text>().text = fileName;
        Toggle toggle = toggleObj.GetComponent<Toggle>();
        toggles.Add(toggle); // Ajoutez le nouveau toggle � la liste

        toggle.onValueChanged.AddListener(delegate { OnToggleChanged(toggle, fileName); });
    }

    void OnToggleChanged(Toggle changedToggle, string fileName)
    {
        if (changedToggle.isOn)
        {
            foreach (Toggle toggle in toggles)
            {
                if (toggle != changedToggle)
                {
                    toggle.isOn = false;
                }
            }

            // Traitez la s�lection ici
            mapManager.SetMapSelected(fileName);

        }
    }

    public void ToggleMapSelectVisibility(bool state)
    {
        gameObject.SetActive(state);
    }
}
