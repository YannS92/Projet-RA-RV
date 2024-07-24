using UnityEngine;

public class BombeController : MonoBehaviour
{
    public GameObject prefabMousse; // Assurez-vous d'associer le prefab dans l'inspecteur.
    public string prefabNameToDetect = "tiago_dual"; // Nom du préfab à détecter.
    private Collision currentCollision; // Variable pour stocker la collision détectée.

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision détectée avec : " + collision.gameObject.name);

        // Vérifiez si l'objet de collision a le nom du préfab à détecter ou est le sol.
        if (collision.gameObject.name == prefabNameToDetect || collision.gameObject.name == "Floor")
        {
            // Stockez la collision détectée.
            currentCollision = collision;
            Exploder();
        }
    }

    void Exploder()
    {
        Debug.Log("Explosion!");

        // Activez le prefab de particules.
        prefabMousse.SetActive(true);

        // Déplacez le prefab de particules à l'emplacement de la collision détectée.
        prefabMousse.transform.position = currentCollision.transform.position;

        // Détruisez la sphère.
        Destroy(gameObject);
    }
}