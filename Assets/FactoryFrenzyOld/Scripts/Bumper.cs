using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float force = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        CharacterController character = other.GetComponent<CharacterController>();

        if (character != null)
        {
            character.gameObject.SendMessage("ApplyBumperForce", force);
        }
    }
}
