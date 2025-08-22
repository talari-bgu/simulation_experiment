using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCollider : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colided with: " + collision.gameObject.name);
    }
}
