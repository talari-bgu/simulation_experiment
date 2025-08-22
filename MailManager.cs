using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MailManager : MonoBehaviour
{
    public bool isLaptop = true;
    public bool debugMode = true;

    public SimulationManager simulationManager;
    public RobotManager robotManager;

    public GameObject sideA;
    public List<GameObject> sideATriggers;
    public GameObject sideB;
    public List<GameObject> sideBTriggers;

    [SerializeField] string fileName_dest;
    [SerializeField] int sender_num;
    [SerializeField] int session;

    private void Awake()
    {
        fileName_dest = @"C:\Users\tuli1\anaconda3\python.exe";
        if (isLaptop) fileName_dest = @"C:\Users\Tal\anaconda3\python.exe";
    }

    private void Start()
    {
        sender_num = simulationManager.email_num;
        session = 0;
    }
    public void TriggerActivation(string name)
    {
        switch (name)
        {
            case "Trigger 1":
                if (session == 1) StartCoroutine(SendMail(0));
                else if (session == 2) StartCoroutine(SendMail(8));
                break;

            case "Trigger 2":
                if (session == 1) StartCoroutine(SendMail(1));
                else if (session == 2) StartCoroutine(SendMail(9));
                break;

            case "Trigger 3":
                if (session == 1) StartCoroutine(SendMail(2));
                else if (session == 2) StartCoroutine(SendMail(10));
                break;

            case "Trigger 4":
                if (session == 1) StartCoroutine(SendMail(3));
                else if (session == 2) StartCoroutine(SendMail(11));
                break;

            case "Trigger 5":
                if (session == 1) StartCoroutine(SendMail(4));
                else if (session == 2) StartCoroutine(SendMail(12));
                break;

            case "Trigger 6":
                if (session == 1) StartCoroutine(SendMail(5));
                else if (session == 2) StartCoroutine(SendMail(13));
                break;
                
            case "Trigger 7":
                if (session == 1) StartCoroutine(SendMail(6));
                else if (session == 2) StartCoroutine(SendMail(14));
                break;
                
            case "Trigger 8":
                if (session == 1) StartCoroutine(SendMail(7));
                else if (session == 2) StartCoroutine(SendMail(15));
                break;

            default:
                break;
        }
            

    }


    public IEnumerator SendMail(int mail_index)
    {
        if (debugMode) UnityEngine.Debug.Log("sent: " + mail_index.ToString());
        else
        {
            var psi = new ProcessStartInfo();
            psi.FileName = fileName_dest;

            var script = @"C:\Users\Tal\Desktop\Experiment\sendMailScript.py";

            string emailSenderAdress = sender_num.ToString();
            string email = mail_index.ToString();

            psi.Arguments = $"\"{script}\" \"{emailSenderAdress}\" \"{mail_index}\"";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            Process.Start(psi);

            yield break;
        }
    }



    public void SetPath(int side)
    {
        if (side == 0) 
        {
            sideB.SetActive(false);
            sideA.SetActive(true);
            for (int i = 0; i < sideATriggers.Count; i++)
            {
                sideATriggers[i].SetActive(true);
            }
            if (session == 0) session = 1;
            else if (session == 1) session = 2;
        }
        else if (side == 1)
        {
            sideA.SetActive(false);
            sideB.SetActive(true);
            for (int i = 0; i < sideBTriggers.Count; i++)
            {
                sideBTriggers[i].SetActive(true);
            }
        }
    }
}
