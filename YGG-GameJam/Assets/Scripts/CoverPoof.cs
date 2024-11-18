using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverPoof : MonoBehaviour
{
    public ParticleSystem coverParticle;

    public void Poof()
    {
        coverParticle.Play();
    }
}
