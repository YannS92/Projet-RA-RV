using intervales.utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public Camera vrCamera;
    public Camera flyCam;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            vrCamera.enabled = !vrCamera.enabled;
            flyCam.enabled = !flyCam.enabled;
        }
    }
}
