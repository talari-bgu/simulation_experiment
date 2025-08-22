using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerSA : MonoBehaviour
{
    public SAManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Robot")
        {
            manager.TriggerActivation();
            gameObject.SetActive(false);
        }
    }
}
