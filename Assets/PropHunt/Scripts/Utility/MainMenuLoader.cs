using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    void Start()
    {
        //We're not connected to anything yet, so we're using the classic SceneManager
        SceneManager.LoadScene("MainMenu");
    }

}
