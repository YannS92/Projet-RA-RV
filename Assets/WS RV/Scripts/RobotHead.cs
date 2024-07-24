using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotHead : MonoBehaviour
{
    public GameObject head;
    public Transform anchorPoint;
    public float snapDistance = 1f; // Distance à partir de laquelle la tête se rattache

    private Rigidbody headRigidbody;
    public NavMeshAgent agentNavRobot;
    public float initialSpeed = 0f;



    void Start()
    {
        headRigidbody = head.GetComponent<Rigidbody>();
        initialSpeed = agentNavRobot.speed;
    }

    void Update()
    {
       // if (head.transform.parent != null) ;
            //TryReattachHead();

    }

    void TryReattachHead()
    {
        Debug.Log("t");
        if (Vector3.Distance(head.transform.position, anchorPoint.position) < snapDistance)
        {
            Debug.Log("GOOD");

            head.transform.position = anchorPoint.position;
            head.transform.rotation = anchorPoint.rotation;
            head.transform.parent = anchorPoint;
            headRigidbody.isKinematic = true;
            //agentNavRobot.speed = initialSpeed;
        }
    }
}