using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RobotController : MonoBehaviour
{
    [SerializeField]
    private XRRayInteractor ray;

    [SerializeField]
    private InputActionReference createCubeAction;

    private bool hasBeenClicked = false;

    

    private void OnEnable()
    {
        createCubeAction.action.Enable();
    }

    private void OnDisable()
    {
        createCubeAction.action.Disable();
    }

    private void Update()
    {
        if (createCubeAction.action.triggered)
        {
            if (!hasBeenClicked)
            {
                FirstClicked(transform);
                hasBeenClicked = true;
            }
            else
            {
                
            }
        }
        else
        {
            
            hasBeenClicked = false;
        }
    }

    public void FirstClicked(Transform t)
    {
        CubeCreator.CreateCube(t);

        
        if (ray != null)
        {
            ray.enabled = false;
        }
    }
}