using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticleSystem;

    void Start()
    {
        if (fireParticleSystem == null)
        {
            fireParticleSystem = GetComponent<ParticleSystem>();
            fireParticleSystem.Stop();

        }
    }

    public void ToggleFire()
    {
        if (fireParticleSystem.isPlaying)
        {
            fireParticleSystem.Stop();
        }
        else
        {
            fireParticleSystem.Play();
        }
    }
}
