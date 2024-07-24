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
        // Instanciez le préfab de mousse au même emplacement que l'extincteur
        GameObject mousseInstance = Instantiate(moussePrefab, transform.position, transform.rotation);

        // Activez l'effet sonore de la mousse sur l'extincteur
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && sonMousse != null)
        {
            audioSource.PlayOneShot(sonMousse);
        }

        // Détruisez la mousse après un certain temps (défini par dureeDeVieMousse)
        Destroy(mousseInstance, dureeDeVieMousse);
    }
}
