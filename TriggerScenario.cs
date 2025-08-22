using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScenario : MonoBehaviour
{
    public ScenarioManager scenarioManager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Robot")
        {
            scenarioManager.TriggerActivation(this.name);
        }
    }
}
