/* using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;

public class RobotAdminController : MonoBehaviour
{
    public GameObject head;
    public float jumpForce = 5f;

    public Rigidbody headRigidbody;
    public NavMeshAgent agentNavRobot;
    public FireController fireController;
    public XRGrabInteractable grabInteractable;


    public void SetSpeed(float speed)
    {
        agentNavRobot.speed = speed;
    }

    void Start()
    {
        headRigidbody = head.GetComponent<Rigidbody>();
        headRigidbody.isKinematic = true; // Emp�che la t�te de tomber imm�diatement
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DetachAndJumpHead();
        }
    }

    void DetachAndJumpHead()
    {
        if (head.transform.parent != null)
        {
            Debug.Log(headRigidbody);
            Debug.Log(agentNavRobot);
            Debug.Log(fireController);
            Debug.Log(grabInteractable);

            fireController.ToggleFire();
            head.transform.parent = null; // D�tache la t�te du corps
            headRigidbody.isKinematic = false; // Active la physique
            headRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Ajoute une force pour faire sauter la t�te
            SetSpeed(0);

            //grabInteractable.enabled = true;

        }
    }
}
*/