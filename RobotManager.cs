using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RobotManager : MonoBehaviour
{
    public SimulationManager simulationManager;
    public ExperimentManager experimentManager;
    public RobotController robotScript;
    public TransparencyManager transparencyManager;


    public UIController UIController;
    public CostMapManager costMapManager;
    public ScenarioManager scenarioManager;

    public TextMeshProUGUI description;

    public GameObject setExplanationLevelQuery;

    public GameObject continueButton;
    public GameObject secondContinueButton;

    public GameObject startExperimentButton;
    public GameObject secondSessionButton;
    public GameObject robotCollider;

    private Transform firstPathTransform;
    private int waypointCount;
    [SerializeField] private int session = 1;

    [SerializeField] private string within_factor;

    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private int currentWaypointIndex;

    [SerializeField] DateTime timeRobotSwitched;
    [SerializeField] float distanceTraveled = 0;
    [SerializeField] Vector3 lastPos;

    [SerializeField] bool obstacle1Passed;
    [SerializeField] float timeInL2;


    private void Start()
    {
        waypointCount = waypoints.Count;
        description.text = "";

        obstacle1Passed = false;
        timeInL2 = 0;
    }

    private void Update()
    {
        // if human in control
        if ((session == 1 && within_factor == "humanFirst") || (session == 2 && within_factor == "robotFirst"))
        {
            if (!obstacle1Passed && robotScript.GetLoa() == 2)
            {
                timeInL2 += Time.deltaTime;
            }

        }
    }

    public float CheckTimeInL2()
    {
        obstacle1Passed = true;
        return timeInL2;
    }


    public void SetTutorial()
    {
        robotCollider.SetActive(false);
        description.gameObject.SetActive(false);
    }


    // simulationManager calls this method after tutorial ends
    // dev also use that method
    public IEnumerator SetExperiment(string within)
    {
        // need to turn off navagent to move the object
        robotScript.GetNavMeshAgent().enabled = false;
        robotScript.gameObject.transform.position = new Vector3(29f, 1.73f, 43f);
        robotScript.gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        robotScript.GetNavMeshAgent().enabled = true;

        experimentManager.SetPath(0);
        // need to wait for set actives
        yield return new WaitForSeconds(1);

        costMapManager.BakeCostMap(0);
        // need to wait until bake is done
        yield return new WaitForSeconds(1);
        
        scenarioManager.SetPath(0);

        robotCollider.SetActive(true);
        description.gameObject.SetActive(true);
        
        currentWaypointIndex = 1;
        robotScript.SetDestination(waypoints[currentWaypointIndex].position);
        robotScript.SetTutorialMode(false);

        within_factor = within;
        if (within_factor == "robotFirst")
        {
            robotScript.SetLoa(2, true, false);
            robotScript.CanSwitchLOA(false);
            robotScript.AssistantArrowsActivation(false);
        }
        else if (within_factor == "humanFirst")
        {
            robotScript.SetLoa(1, true, false);
            robotScript.CanSwitchLOA(true);
            robotScript.AssistantArrowsActivation(true);
        }

        Time.timeScale = 0;

        // setting the intro
        description.text = "My name is Navi robot. Navigating the office, crossing obstacles, and scanning specific points is my goal. I have \r\n" +
            "two options of automation level (LOA): Low automation meaning you are responsible for my navigation using \r\n" +
            "the joystick. High automation meaning that I can navigate by myself.";
        continueButton.SetActive(true);

    }

    // simulationManager calls this method when black screen has faded
    // wont call this for now
    public void FadeOutEnd()
    {
        /*if (simulationManager.tba_mode == "high" && (within_factor == "robotFirst" && session == 1))
        {
            transparencyManager.SetExplanationModeWindow();
            description.text = "";
        }*/
    }

    public void ContinueAfterIntroduction()
    {
        continueButton.SetActive(false);
        ShowSessionBehaviour();
    }

    
    public void ShowSessionBehaviour()
    {
        if (session == 1)
        {
            if (within_factor == "robotFirst") 
            {
                transparencyManager.RobotBehaviorMassage(0, 0);
            }
            else if (within_factor == "humanFirst") 
            {
                transparencyManager.RobotBehaviorMassage(0, 1);
            }
        }
        else if (session == 2)
        {
            if (within_factor == "robotFirst")
            {
                transparencyManager.RobotBehaviorMassage(1, 0);
            }
            else if (within_factor == "humanFirst")
            {
                transparencyManager.RobotBehaviorMassage(1, 1);
            }
        }
        StartCoroutine(SecondContinue());
    }

    public IEnumerator SecondContinue()
    {
        yield return new WaitForSecondsRealtime(3);

        secondContinueButton.SetActive(true);
    }
    public void SecondContinueClicked()
    {
        if (session == 1)
        {
            if (within_factor == "robotFirst" && simulationManager.tba_mode == "high")
            {
                Debug.Log("query");
                transparencyManager.SetExplanationModeWindow();
                description.text = "Please answer the question so I'll know you better.";
            }
            else
            {
                ShowStartButton();
                Debug.Log("start screen");
            }
        }
        else if (session == 2)
        {
            if (within_factor == "humanFirst" && simulationManager.tba_mode == "high")
            {
                transparencyManager.SetExplanationModeWindow();
                description.text = "Please answer the question so I'll know you better.";
                Debug.Log("query");
            }
            else
            {
                ShowStartButton();
                Debug.Log("start screen");
            }
        }
        secondContinueButton.SetActive(false);
    }

    public void ShowStartButton()
    {
        if (session == 1)
        {
            startExperimentButton.SetActive(true);
            description.text = "Please press start when ready.";
        }
        else if (session == 2)
        {
            secondSessionButton.SetActive(true);
            description.text = "Start when ready.";
        }
    }

    // This method is being called right before the experiment starts
    public void StartButtonClicked()
    {        
        startExperimentButton.SetActive(false);

        if (within_factor == "robotFirst")
        {
            experimentManager.SetExperimentProperties(simulationManager.parNum, "robot", simulationManager.complexity);
        }
        else if (within_factor == "humanFirst")
        {
            experimentManager.SetExperimentProperties(simulationManager.parNum, "human", simulationManager.complexity);
        }
        //
        description.text = "";

        // calculation distance traveled
        lastPos = robotScript.gameObject.transform.position;
        InvokeRepeating("UpdateDistanceTraveled", 1.0f, 1.0f);

        Time.timeScale = 1;
    }



    private void UpdateDistanceTraveled()
    {
        Vector3 distanceVector = robotScript.gameObject.transform.position - lastPos;
        float deltaDistance = distanceVector.magnitude;
        distanceTraveled += deltaDistance;
        lastPos = robotScript.gameObject.transform.position;
    }


    private IEnumerator SetPathBack()
    {
        experimentManager.SetPath(1);
        yield return new WaitForSeconds(1);
        costMapManager.BakeCostMap(1);
        yield return new WaitForSeconds(1);

        scenarioManager.SetPath(1);

        // if high tba and robot is in control
        if (simulationManager.tba_mode == "high")
        {
            if ((within_factor == "robotFirst" && session == 1) || (within_factor == "humanFirst" && session == 2))
            {
                // wait till inspecting is over
                yield return new WaitForSeconds(8);
                transparencyManager.SetExplanationModeWindow();
            }
        }
    }

    private IEnumerator SetSecondSession()
    {
        experimentManager.SetDistanceTraveled(distanceTraveled);
        distanceTraveled = 0f;
        experimentManager.SetTbaMode(transparencyManager.GetTbaRecords());
        // the method SetPath take the finish time
        // need to report before calling setpath

        experimentManager.SetPath(0);
        yield return new WaitForSeconds(1);
        costMapManager.BakeCostMap(0);
        yield return new WaitForSeconds(1);

        scenarioManager.SetPath(0);

        session = 2;
        currentWaypointIndex--;
        robotScript.SetDestination(waypoints[currentWaypointIndex].position);
        if (within_factor == "robotFirst")
        {
            robotScript.SetLoa(1, true, false);
            robotScript.CanSwitchLOA(true);
            robotScript.AssistantArrowsActivation(true);
        }
        else if (within_factor == "humanFirst")
        {
            robotScript.SetLoa(2, true, false);
            robotScript.CanSwitchLOA(false);
            robotScript.AssistantArrowsActivation(false);
        }

        ShowSessionBehaviour();
        Time.timeScale = 0;
    }

    // this is called thorugh button right before second session
    public void StartSecondSession()
    {
        if (within_factor == "robotFirst") experimentManager.SetExperimentProperties(simulationManager.parNum, "human", simulationManager.complexity);
        else if (within_factor == "humanFirst") experimentManager.SetExperimentProperties(simulationManager.parNum, "robot", simulationManager.complexity);

        description.text = "";

        robotScript.CanMove(true);

        secondSessionButton.SetActive(false);
        Time.timeScale = 1;
    }

    public void WaypointReached()
    {
        robotScript.CanMove(false);
        switch (currentWaypointIndex)
        {
            case 0:
                if (session == 2) { ExperimentDone(); }
                break;

            // small room
            case 1:
                experimentManager.CheckpointReached(DateTime.Now);
                robotScript.RotationAdjustement(new Vector3(16.4f, 0, 40.5f));
                break;

            // before brown room
            case 2:
                experimentManager.CheckpointReached(DateTime.Now);
                robotScript.RotationAdjustement(new Vector3(-7.4f, 0, 6.22f));
                break;

            // brown room
            case 3:
                experimentManager.CheckpointReached(DateTime.Now);
                robotScript.RotationAdjustement(new Vector3(-11.14f, 0, 10f));
                scenarioManager.TriggerActivation("Trigger5");
                break;

            // white room
            case 4:
                experimentManager.CheckpointReached(DateTime.Now);
                robotScript.RotationAdjustement(new Vector3(-14.4f, 0, 38f));
                StartCoroutine(SetPathBack());
                break;

            // brown room
            case 5:
                experimentManager.CheckpointReached(DateTime.Now);
                robotScript.RotationAdjustement(new Vector3(-11.14f, 0, 10f));
                break;

            // before brown room
            case 6:
                experimentManager.CheckpointReached(DateTime.Now);
                robotScript.RotationAdjustement(new Vector3(-7.4f, 0, 6.22f));
                break;

            // small room
            case 7:
                experimentManager.CheckpointReached(DateTime.Now);
                robotScript.RotationAdjustement(new Vector3(16.4f, 0, 40.5f));
                break;

            // starting point
            case 8:
                if (session == 1) 
                {
                    StartCoroutine(SetSecondSession());
                }
                break;

            default:
                Debug.Log("Error: WaypointReached in robotManager");
                break;
        }
    }


    public void WaypointRotationAdjusted()
    {
        UIController.StartInspectingUI(5);

    }

    public void InspectingDone()
    {
        if (session == 1)
        {
                currentWaypointIndex++;
                robotScript.SetDestination(waypoints[currentWaypointIndex].position);
                robotScript.CanMove(true);
        }
        else
        { 
                currentWaypointIndex--;
                robotScript.SetDestination(waypoints[currentWaypointIndex].position);
                robotScript.CanMove(true);
        }
    }
    

    public void LOASwitched(int loa)
    {
        if (experimentManager.gameObject.activeSelf && 
            ((session == 1 && within_factor == "humanFirst") || (session == 2 && within_factor == "robotFirst")))
        {
            experimentManager.LOASwitch(loa, DateTime.Now);
        }
    }

    public int GetSession() { return session;}

    public void ChangeLOA(int loa) {
        robotScript.SetLoa(loa, true);  
        if (loa == 1) { experimentManager.RobotLoaDecrease(DateTime.Now); }
        else { experimentManager.RobotLoaIncrease(DateTime.Now); }
    }

    public void ColliderObstacle()
    {
        robotScript.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        robotScript.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void AssistantArrowsActivation(bool mode) { robotScript.AssistantArrowsActivation(mode); }

    public void ExperimentDone()
    {
        // stop distance refresh
        CancelInvoke();
        experimentManager.SetDistanceTraveled(distanceTraveled);
        experimentManager.SetTbaMode(transparencyManager.GetTbaRecords());

        experimentManager.WriteFile();

        description.alignment = TextAlignmentOptions.Center;
        description.text = "We appreciate your involvement in the experiment. Please contact the experimenter.";
        Time.timeScale = 0;
    }
}
