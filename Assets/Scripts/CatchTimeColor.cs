using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTimeColor : MonoBehaviour
{
    public void PrintAllCollisionTimes()
    {
        HandColorCanvas[] allTimers = FindObjectsOfType<HandColorCanvas>();

        if (allTimers.Length == 0)
        {
            Debug.Log("No TimerHandler instances found.");
            return;
        }

        foreach (HandColorCanvas handler in allTimers)
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
