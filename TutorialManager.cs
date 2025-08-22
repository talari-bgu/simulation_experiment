using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


// Tell about experiment
// show how to move forward backward and rotate

// show nav path and tell about loa
// show buttom and let practice 

// 

public class TutorialManager : MonoBehaviour
{
    public RobotController robotScript;
    public DialogueManager dialogueManager;
    public CanvasGroup loaCanvasGroup;
    public CanvasGroup arrowCanvasGroup;

    public GameObject obstacle;
    public GameObject loa_trigger;
    public GameObject pass_trigger;

    public GameObject blocks;
    public GameObject continue_go;
    public GameObject loaUI_go;
    public GameObject arrowUI_go;
    public GameObject clickHere;
    public GameObject endButton;




    [SerializeField] private float threshhold = 0.5f;
    [SerializeField] int currentAct;
    [SerializeField] private bool waitingAction = false;
    private void Start()
    {

        loaCanvasGroup.alpha = 0;
        arrowCanvasGroup.alpha = 0;

        robotScript.SetTutorialMode(true);

        endButton.SetActive(false);
    }

    private void Update()
    {
        if (waitingAction)
        {
            if (currentAct == 1 && (Input.GetAxisRaw("VerticalC") > threshhold || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                dialogueManager.DisplayNextSentence();
            }
            else if (currentAct == 2 && (Input.GetAxisRaw("VerticalC") < -threshhold || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                dialogueManager.DisplayNextSentence();
            }
            else if (currentAct == 3 && (Input.GetAxisRaw("HorizontalC") < -threshhold || Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                dialogueManager.DisplayNextSentence();
            }
            else if (currentAct == 4 && (Input.GetAxisRaw("HorizontalC") > threshhold || Input.GetKeyDown(KeyCode.RightArrow)))
            {
                dialogueManager.DisplayNextSentence();
                waitingAction = false;
            }
            else if (currentAct == 7 && (Input.GetButtonDown("SwitchMode")  || Input.GetKeyDown(KeyCode.Alpha2)))
            {
                dialogueManager.DisplayNextSentence();
                robotScript.CanSwitchLOA(false);
            }
            else if (currentAct == 9 && (Input.GetButtonDown("SwitchMode") || Input.GetKeyDown(KeyCode.Alpha1)))
            {
                dialogueManager.DisplayNextSentence();
            }
            else if (currentAct == 10 && robotScript.GetNavMeshAgent().hasPath && Vector3.Distance(robotScript.GetNavMeshAgent().destination, robotScript.gameObject.transform.position - new Vector3(0, 0.8f, 0)) <= 2)
            {
                dialogueManager.DisplayNextSentence();
                robotScript.GetNavMeshAgent().ResetPath();
                robotScript.GetNavMeshAgent().isStopped = true;
            }
            else if (currentAct == 11 && robotScript.GetNavMeshAgent().hasPath && Vector3.Distance(robotScript.GetNavMeshAgent().destination, robotScript.gameObject.transform.position - new Vector3(0, 0.8f, 0)) <= 2)
            {
                dialogueManager.DisplayNextSentence();
                robotScript.GetNavMeshAgent().ResetPath();
                robotScript.GetNavMeshAgent().isStopped = true;

                Destroy(obstacle);
                Destroy(loa_trigger);
                Destroy(pass_trigger);
            }
        }
    }



    // Set robot position
    public void StartTutorial()
    {
        StartCoroutine(TutorialSetup());
    }

    private IEnumerator TutorialSetup()
    {
        robotScript.GetNavMeshAgent().enabled = false;
        robotScript.gameObject.transform.position = new Vector3(0.5f, 1.8f, 24.5f);
        robotScript.GetNavMeshAgent().GetComponent<NavMeshAgent>().enabled = true;

        yield return new WaitForSeconds(1);

        ActActivate(0);
    }

    private void ActActivate(int index)
    {
        currentAct = index;
        dialogueManager.StartDialogue(currentAct);
        if (currentAct == 0)
        {
            robotScript.CanSwitchLOA(false);
        }
        if (currentAct == 5)
        {
            continue_go.SetActive(true);
        }
    }

    public void NextAct()
    {
        ActActivate(currentAct + 1);
    }

    public void WaitForAction()
    {
        waitingAction = true;
        continue_go.SetActive(false);
    }

    public void ShowLoa()
    {
        loaCanvasGroup.alpha = 1;
    }

    public void SetDestination()
    {
        robotScript.SetDestination(new Vector3(-14f, 1, 24.5f));
        arrowCanvasGroup.alpha = 1;
    }

    public IEnumerator SetHiObstacle1()
    {
        continue_go.SetActive(false);

        obstacle.SetActive(true);

        // move the robot
        robotScript.GetNavMeshAgent().enabled = false;
        robotScript.gameObject.transform.position = new Vector3(-10f, 1f, 24.5f);
        robotScript.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        robotScript.GetNavMeshAgent().enabled = true;

        yield return new WaitForSeconds(1f);

        robotScript.SetLoa(1, true);
        robotScript.CanSwitchLOA(false);
        robotScript.SetDestination(new Vector3(2f, 1, 24.5f));

        dialogueManager.DisplayNextSentence();

        waitingAction = true;
    }

    // scenario: 0 - first time, 1 - no high mode, 2 - collided
    public IEnumerator SetHiObstacle2(int scenario)
    {
        continue_go.SetActive(false);
        waitingAction = false;
        obstacle.GetComponent<TutorialTriggerHandler>().type = 1;
        pass_trigger.SetActive(true);
        loa_trigger.SetActive(true);

        // move the robot
        robotScript.GetNavMeshAgent().enabled = false;
        robotScript.gameObject.transform.position = new Vector3(-16f, 1f, 24.5f);
        robotScript.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        robotScript.GetNavMeshAgent().enabled = true;

        if (scenario == 1) dialogueManager.subtitle.text = "Now, you're on Low automation. start by switching to High. Right before the obstacle \n" +
                "switch to Low and pass manually. You have to do that because the robot failed\n to detect the obstacle, that's why the orange line go through the obstacle.";

        else if (scenario == 2) dialogueManager.subtitle.text = "You've collided with the obstacle. start by switching to High. Right before the obstacle \n" +
                "switch to Low and pass manually. You have to do that because the robot failed\n to detect the obstacle, that's why the orange line go through the obstacle.";

        yield return new WaitForSeconds(1f);

        robotScript.SetLoa(1, true);
        robotScript.CanSwitchLOA(true);
        robotScript.SetDestination(new Vector3(2f, 1, 24.5f));

        waitingAction = true;
    }

    public void MoveToActTwelve()
    {
        // move the robot
        robotScript.GetNavMeshAgent().enabled = false;
        robotScript.gameObject.transform.position = new Vector3(-9.5f, 1.71f, 24.5f);
        robotScript.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        robotScript.GetNavMeshAgent().enabled = true;
    }
    public void SetActTwelve()
    {
        Destroy(blocks);
        continue_go.SetActive(false);
        robotScript.SetDestination(new Vector3(-3.5f, 1, 6.5f));
        robotScript.SetLoa(2, true);
        robotScript.CanSwitchLOA(false);
    }
    public void TriggerActivation(string name)
    {
        if (name == "Trigger_obs")
        {
            dialogueManager.subtitle.text = "You've passed the obstacle, please switch to High automation";
        }
        else if (name == "Trigger1")
        {
            robotScript.SetLoa(1, true);
            robotScript.CanSwitchLOA(false);
            dialogueManager.DisplayNextSentence();
        }

        else if (name == "Trigger2")
        {
            robotScript.SetLoa(2, true);
            robotScript.CanSwitchLOA(true);
            dialogueManager.DisplayNextSentence();
            continue_go.SetActive(true);
        }
    }

    public void ButtonSetOff()
    {
        StartCoroutine(ButtonDelay());
    }

    private IEnumerator ButtonDelay()
    {
        continue_go.GetComponent<Button>().enabled = false;
        continue_go.GetComponent<CanvasGroup>().alpha = 0;
        // Change color
        yield return new WaitForSeconds(3);
        continue_go.GetComponent<Button>().enabled = true;
        continue_go.GetComponent<CanvasGroup>().alpha = 1;
    }

}
