using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMail : MonoBehaviour
{
    public MailManager mailManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Robot")
        {
            mailManager.TriggerActivation(gameObject.name);
            gameObject.SetActive(false);
        }
    }
}
