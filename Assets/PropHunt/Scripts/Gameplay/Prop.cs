using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Prop : MonoBehaviour
{
    public GameObject BodyGameObject;
    public Rigidbody Rigidbody;
    public string PropName = "Prop";

    private void Awake()
    {
        if(BodyGameObject == null)
        {
            Debug.LogError("Not Body found. Assuming it is named " + gameObject.name + "Body");
            BodyGameObject = transform.Find(gameObject.name + "Body").gameObject;
            if(BodyGameObject == null)
            {
                Debug.LogError("No child found, please fix the Prop.");
            }
        }
        if(Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
    }
}
