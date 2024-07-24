using UnityEngine;

public class ExtincteurScript : MonoBehaviour
{
    public GameObject moussePrefab;
    public AudioClip sonMousse;
    public float dureeDeVieMousse = 3f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))  // Remplacez KeyCode.B par le bouton que vous souhaitez utiliser
        {
            EmettreMousse();
        }
    }

    private void EmettreMousse()
    {
        // Instanciez le pr�fab de mousse au m�me emplacement que l'extincteur
        GameObject mousseInstance = Instantiate(moussePrefab, transform.position, transform.rotation);

        // Activez l'effet sonore de la mousse sur l'extincteur
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && sonMousse != null)
        {
            audioSource.PlayOneShot(sonMousse);
        }

        // D�truisez la mousse apr�s un certain temps (d�fini par dureeDeVieMousse)
        Destroy(mousseInstance, dureeDeVieMousse);
    }
}
