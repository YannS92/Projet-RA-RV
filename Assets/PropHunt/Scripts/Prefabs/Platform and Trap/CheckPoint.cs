using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckPoint : MonoBehaviour
{

    bool IsValid = false;
    // Start is called before the first frame update
    Transform GreenLightObject; 
    Transform RedLightObject;
    void Start()
    {
        GreenLightObject = gameObject.transform.GetChild(1);
        RedLightObject = gameObject.transform.GetChild(2);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (IsValid) return;

        PlayerManager playerColManager = collision.gameObject.transform.root.gameObject.GetComponent<PlayerManager>();


        if (NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>() == playerColManager)
        {
            Debug.Log("checkpoint valided");
            // Modifier la position de spawn avec Y augmenté de 3 unités
            Vector3 spawnPosition = gameObject.transform.position + new Vector3(0, 3, 0);
            playerColManager.SetSpawn(spawnPosition);
            GreenLightObject.gameObject.SetActive(true);
            RedLightObject.gameObject.SetActive(false);

            IsValid = true;


        }

    }
}
