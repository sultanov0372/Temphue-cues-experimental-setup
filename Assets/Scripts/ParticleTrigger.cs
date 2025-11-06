using GLTFast.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Material = UnityEngine.Material;

public class ParticleTrigger : MonoBehaviour
{

    public ParticleSystem leftHandPrefab;
    public ParticleSystem rightHandPrefab;
   // private ParticleSystem activeParticleSystem; // Reference to the instantiated particle system
   // private bool isColliding = false; // To track if collision is ongoing
   // private float collisionTime = 0f;

    //public int handLayer;

    private Dictionary<Collider, ParticleSystem> activeParticles = new Dictionary<Collider, ParticleSystem>();

    public int leftHandLayer = 12;
    public int rightHandLayer = 13;


    // This method will be called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // if collides with mirrored avatars' hand
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("collision with: " + other.gameObject.name);

            HandIdentifier hand = other.GetComponent<HandIdentifier>();

            if (hand != null)
            {
                int layerToUse = (hand.handType == HandType.Left) ? leftHandLayer : rightHandLayer;

                ParticleSystem psPrefab = (hand.handType == HandType.Left)
                ? leftHandPrefab
                : rightHandPrefab;

                // Instantiate particle system and assign correct layer
                ParticleSystem ps = InstantiateParticleSystem(psPrefab, other.transform.position, layerToUse);
                ps.Play();

                activeParticles[other] = ps;
            }
        }
    }

    private ParticleSystem InstantiateParticleSystem(ParticleSystem effectPrefab, Vector3 position, int layer)
    {
        ParticleSystem newEffect = Instantiate(effectPrefab, position, Quaternion.identity);
        newEffect.gameObject.layer = layer;

        // If the particle system has children, update their layers too
        foreach (Transform child in newEffect.transform)
        {
            child.gameObject.layer = layer;
        }

        return newEffect;
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("exit collision with: " + other.gameObject.name);

            if (activeParticles.TryGetValue(other, out ParticleSystem ps))
            {
                ps.Stop();
                Destroy(ps.gameObject);
                activeParticles.Remove(other);
            }
        }
    }
}
