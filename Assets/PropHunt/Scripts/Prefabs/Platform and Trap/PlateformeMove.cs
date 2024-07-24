using UnityEngine;

public class PlateformeMove : MonoBehaviour
{
    public Vector3 deplacement; // Déplacement par rapport au point A
    private Vector3 pointA;
    private Vector3 pointB;
    public float speed = 1.0f;

    private float t = 0.0f; // Un paramètre pour contrôler l'interpolation
    private Vector3 lastPosition; // Pour stocker la dernière position de la plateforme

    void Start()
    {
        pointA = transform.position; // Point A est la position initiale de la plateforme
        pointB = pointA + deplacement; // Point B est calculé comme un déplacement par rapport à A
        lastPosition = transform.position;
    }

    void Update()
    {
        // Interpolation entre pointA et pointB
        Vector3 newPosition = Vector3.Lerp(pointA, pointB, Mathf.PingPong(t, 1));
        transform.position = newPosition;
        t += Time.deltaTime * speed;

        // Mise à jour de la dernière position après le déplacement
        lastPosition = transform.position;
    }
}