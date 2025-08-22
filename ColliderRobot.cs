using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderRobot : MonoBehaviour
{

    public ExperimentManager experimentManager;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.name.Contains("Trigger"))
        {
            print("Collided with: " + other.name);
            experimentManager.AddCollision(other.name);
        }
        
    }
}
