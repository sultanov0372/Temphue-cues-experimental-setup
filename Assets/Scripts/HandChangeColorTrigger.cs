using GLTFast.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Material = UnityEngine.Material;

public class HandChangeColorTrigger : MonoBehaviour
{
    private bool isColliding = false; // To track if collision is ongoing
    private float collisionTime = 0f;


   // public Color collisionColor = Color.red; // The color to apply on collision
    private static readonly int ColorOverrideID = Shader.PropertyToID("_ColorOverride");

    private void Start()
    {
        Renderer handRenderer = GetComponentInChildren<Renderer>();
        if (handRenderer != null)
        {
            Debug.Log("Forcing material assignment...");
            handRenderer.material = new Material(handRenderer.material);  // Create a new instance
        }
    }

    // This method will be called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // if collides with mirrored avatars' hand
       
            Debug.Log("collision with: " + other.gameObject.name);

        if (other.CompareTag("Player")) // Ensure this only affects hands
        {
            var lerper = other.GetComponentInParent<HandColorLerp>();
            var colorSource = GetComponent<ColorOnTouch>();

            if (lerper != null && colorSource != null)
            {
                lerper.FadeToColor(colorSource.touchColor);
            }
        }

    }


    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            var lerper = other.GetComponentInParent<HandColorLerp>();
            if (lerper != null)
                lerper.ResetColor();
        }
        Debug.Log("exit collision with: " + other.gameObject.name);

           
    }   
}
