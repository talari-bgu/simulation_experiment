using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialTriggerHandler : MonoBehaviour
{
    public TutorialManager tutorialManager;

    public int type;
    public bool destroy;

    // types: 0 - to activate loa, 1 - to activate collider, 2 - to activate last trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Robot")
        {
            if (type == 0 && tutorialManager.robotScript.GetLoa() == 1) StartCoroutine(tutorialManager.SetHiObstacle2(1));
            else if (type == 2) tutorialManager.TriggerActivation(this.name);
            if (destroy) Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Robot")
        {
            if (type == 1)
            {
                StartCoroutine(tutorialManager.SetHiObstacle2(2));
            }
        }
    }
}
