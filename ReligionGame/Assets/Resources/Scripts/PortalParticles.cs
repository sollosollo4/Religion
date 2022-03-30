using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalParticles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (ParticleSystem pS in GetComponentsInChildren<ParticleSystem>())
        {
            pS.Play();
        }
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
