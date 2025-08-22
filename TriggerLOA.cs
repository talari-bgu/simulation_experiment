using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLOA : MonoBehaviour
{
    public AutomationManager automationManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Robot")
        {
            automationManager.TriggerActivation(this.name);
            gameObject.SetActive(false);
        }
    }
}
