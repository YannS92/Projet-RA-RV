using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CubeCreator : MonoBehaviour
{
    public static void CreateCube(Transform t)
    {
        // Create a cube at the specified position
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Adjust the position to appear slightly in front and above the position of t
        cube.transform.position = t.position + t.forward * 0.5f + Vector3.up * 0.25f;

        // Apply the "Grabbable" layer to the cube
        cube.layer = LayerMask.NameToLayer("Grabbable");

        // Add XRGrabInteractable component to the cube
        XRGrabInteractable grabInteractable = cube.AddComponent<XRGrabInteractable>();

        // Add BoxCollider component to the cube
        BoxCollider boxCollider = cube.AddComponent<BoxCollider>();

        // Configure the BoxCollider size
        boxCollider.size = new Vector3(1f, 1f, 1f);

        // Apply scale transformations
        cube.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        // Load the Audio Affordance prefab
        GameObject audioAffordancePrefab = Resources.Load<GameObject>("Audio Affordance");

        // Check if the prefab is loaded correctly
        if (audioAffordancePrefab != null)
        {
            // Instantiate the prefab as a child of the cube
            GameObject audioAffordance = Instantiate(audioAffordancePrefab, cube.transform);


        }
        else
        {
            Debug.LogError("Audio Affordance prefab not found. Make sure the path is correct.");
        }
    }
}