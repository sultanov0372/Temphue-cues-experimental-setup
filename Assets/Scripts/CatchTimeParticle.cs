using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTimeParticle : MonoBehaviour
{
    public void PrintAllCollisionTimes()
    {
        ParticleTriggerCanvas[] allTimers = FindObjectsOfType<ParticleTriggerCanvas>();

        if (allTimers.Length == 0)
        {
            Debug.Log("No TimerHandler instances found.");
            return;
        }

        foreach (ParticleTriggerCanvas handler in allTimers)
        {
            if (handler.collisionTime > 0)
            {
                Debug.Log($"{handler.gameObject.name} time: {handler.collisionTime:F2}s");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
