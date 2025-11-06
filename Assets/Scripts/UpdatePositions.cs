using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePositions : MonoBehaviour
{

    public Transform player;
    public Transform target;
    //public float saturationRange = 10.0f;

    void Update()
    {
        if (player != null && target != null)
        {
            // Set global shader properties
            Shader.SetGlobalVector("_GlobalPlayerPosition", player.position);
            Shader.SetGlobalVector("_GlobalTargetPosition", target.position);
        }
    }
}
