using intervales.utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamMarker : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject markerPrefab;
    public Camera flyCam;
    private GameObject lastMarker = null;
    private float markerHeight = 1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && flyCam.enabled)
        {
            Ray ray = flyCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Supprimer le dernier marqueur
                if (lastMarker != null)
                {
                    Destroy(lastMarker);
                }

                // Positionner le nouveau marqueur à la hauteur spécifiée
                Vector3 markerPosition = hit.point;
                markerPosition.y = markerHeight;

                // Créer le nouveau marqueur
                lastMarker = Instantiate(markerPrefab, markerPosition, Quaternion.identity);
            }
        }
    }
}
