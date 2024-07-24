using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TrapCreator : MonoBehaviour
{
    public GameObject trapBumperPrefab;

    public void CreateTrapBumper(Transform t)
    {
        if (trapBumperPrefab == null)
        {
            Debug.LogError("Le préfab trapBumperPrefab n'est pas référencé. Assurez-vous de le faire dans l'inspecteur Unity.");
            return;
        }

        Debug.Log("Création du piège bumper!");

        // Charger le préfab "Trap Bumper" depuis les ressources
        GameObject trapBumper = Instantiate(trapBumperPrefab);

        // Ajouter le composant XRGrabInteractable au trap bumper
        XRGrabInteractable existingGrabInteractable = trapBumper.GetComponent<XRGrabInteractable>();

        if (existingGrabInteractable == null)
        {
            XRGrabInteractable grabInteractable = trapBumper.AddComponent<XRGrabInteractable>();
            Debug.Log("XRGrabInteractable ajouté avec succès!");
        }
        else
        {
            Debug.LogWarning("Le composant XRGrabInteractable est déjà attaché à trapBumper.");
        }



        // Ajuster la position pour qu'il apparaisse à la position spécifiée
        trapBumper.transform.position = new Vector3(2.049f, 1.42f, -1.299f) + new Vector3(0.1f, 0.1f, 0.1f); // Incremental adjustment

        // Ajuster l'échelle (scale)
        trapBumper.transform.localScale = new Vector3(4f, 4f, 4f);

        // Ajouter le layer "Props"
        trapBumper.layer = LayerMask.NameToLayer("Props");




    }
}