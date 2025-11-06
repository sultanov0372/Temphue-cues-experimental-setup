using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CatchTimeColorHS : MonoBehaviour
{
    //public GameObject Timer;
    public void PrintAllCollisionTimes()
    {
        HandChangeColorTrigger1[] allTimers = FindObjectsOfType<HandChangeColorTrigger1>();

        if (allTimers.Length == 0)
        {
            Debug.Log("No TimerHandler instances found.");
            return;
        }

        foreach (HandChangeColorTrigger1 handler in allTimers)
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
