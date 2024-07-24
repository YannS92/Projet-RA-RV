using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Show(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseMenu()
    {

        Show(false);
    }


    public void Show(bool value)
    {
        gameObject.SetActive(value);

    }

}
