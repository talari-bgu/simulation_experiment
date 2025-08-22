using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ColliderObstacle : MonoBehaviour
{
    public RobotManager robotManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Robot")
        {
            robotManager.robotScript.AgentCollision();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Robot")
        {
            robotManager.ColliderObstacle();
        }
     
    }
}
