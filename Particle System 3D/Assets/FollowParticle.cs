using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allows the camera to follow the generated particle
/// </summary>
public class FollowParticle : MonoBehaviour
{
    
    public ParticleSystem pSystem;
    ParticleSystem.Particle[] particle;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    //The camera follows the particle with this routine. The transform.postion of the camera is set to the position of the particle with an added offset 
    void LateUpdate()
    {        
        InitializeIfNeeded();
        int particlesAlive = pSystem.GetParticles(particle);
        Debug.Log(particle[0].position);
        transform.position = particle[0].position + offset + pSystem.GetComponent<Transform>().position;
    }

    /// <summary>
    /// If there is no particle or even particle system given, this function initialzes it if needed
    /// </summary>
    void InitializeIfNeeded()
    {
        if (pSystem == null)
            pSystem = GetComponent<ParticleSystem>();

        if (particle == null || particle.Length < pSystem.main.maxParticles)
            particle = new ParticleSystem.Particle[pSystem.main.maxParticles];
    }
}
