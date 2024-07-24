using UnityEngine;

public class BombeController : MonoBehaviour
{
    public GameObject prefabMousse; // Assurez-vous d'associer le prefab dans l'inspecteur.
    public string prefabNameToDetect = "tiago_dual"; // Nom du pr�fab � d�tecter.
    private Collision currentCollision; // Variable pour stocker la collision d�tect�e.

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision d�tect�e avec : " + collision.gameObject.name);

        // V�rifiez si l'objet de collision a le nom du pr�fab � d�tecter ou est le sol.
        if (collision.gameObject.name == prefabNameToDetect || collision.gameObject.name == "Floor")
        {
            // Stockez la collision d�tect�e.
            currentCollision = collision;
            Exploder();
        }
    }

    void Exploder()
    {
        Debug.Log("Explosion!");

        // Activez le prefab de particules.
        prefabMousse.SetActive(true);

        // D�placez le prefab de particules � l'emplacement de la collision d�tect�e.
        prefabMousse.transform.position = currentCollision.transform.position;

        // D�truisez la sph�re.
        Destroy(gameObject);
    }
}