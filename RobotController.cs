using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public RobotManager robotManager;
    public UIController uiController;

    public float manualMoveSpeed = 10.0f;
    public float manualRotSpeed = 30.0f;
    public int positionalRotateDegree;
    public bool isController;


    private Rigidbody robotRb;
    private NavMeshAgent navMeshAgent;

    [SerializeField] private int horizontalInput;
    [SerializeField] private int verticalInput;

    [SerializeField] private float horizontalCInput;
    [SerializeField] private float verticalCInput;
    [SerializeField] private bool switchModeCInput;

    [SerializeField] private int loa;
    [SerializeField] private bool canSwitchLOA;
    [SerializeField] private bool canMove;
    [SerializeField] private bool assistanceArrowDisplay = true;

    [SerializeField] private bool waypointRotatinAdjustment;
    [SerializeField] private Vector3 rotationAdjustmentPosition;
    [SerializeField] private bool isTutorial;
    [SerializeField] private bool isCollided;


    void Start()
    {
        robotRb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        loa = 1;
        //navMeshAgent.isStopped = true;
        uiController.UISetLevel("low");
        uiController.UISetArrow("disable");

        canMove = true;
        canSwitchLOA = true;
        waypointRotatinAdjustment = false;
        isCollided = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Keyboard
        horizontalInput = (int)Input.GetAxisRaw("Horizontal");
        verticalInput = (int)Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(navMeshAgent.isStopped);
        }
        // Joystick
        horizontalCInput = Input.GetAxisRaw("HorizontalC");
        verticalCInput = Input.GetAxisRaw("VerticalC");
        switchModeCInput = Input.GetButtonDown("SwitchMode");

        if (Input.GetKeyDown(KeyCode.Alpha1)) { SetLoa(1, false);}

        else if (Input.GetKeyDown(KeyCode.Alpha2)) { SetLoa(2, false);}

        else if (switchModeCInput) { JoystickLoaSwitch(); }
    }

    private void FixedUpdate()
    {
        // Destination reached
        if (navMeshAgent.hasPath && Vector3.Distance(navMeshAgent.destination, transform.position - new Vector3(0, 0.8f, 0)) <= 1)
        {
            ClearNavigation();
        }

        if (waypointRotatinAdjustment)
        {
            CheckForRotationAdjustment(rotationAdjustmentPosition);
        }

        // Rotate before Auto-Nav if angle is too big
        else if (!isCollided && navMeshAgent.hasPath && loa == 2 && navMeshAgent.path.corners.Length >= 2)
        {
            CheckForRotationAdjustment(navMeshAgent.path.corners[1], positionalRotateDegree);
        }
        else if (assistanceArrowDisplay && navMeshAgent.hasPath && loa == 1 && navMeshAgent.path.corners.Length >= 2)
        {
            CheckForUIArrow(navMeshAgent.path.corners[1]);
        }

        // Manual movement
        if (canMove &&  loa == 1)
        {
            if (!isController) Move(horizontalInput, verticalInput);
            else
            {
                if (Mathf.Abs(horizontalCInput) < 0.2f) horizontalCInput = 0f;
                if (Mathf.Abs(verticalCInput) < 0.2f) verticalCInput = 0f;
                Move(horizontalCInput, verticalCInput);
            }
        }

    }
    public void SetDestination(Vector3 target)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        navMeshAgent.SetDestination(target);
        if (loa == 1) { navMeshAgent.isStopped = true; }
        else if (loa == 2) {  navMeshAgent.isStopped = false; }
    }
    private void ClearNavigation()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
        if (!isTutorial) robotManager.WaypointReached();
    }
    // Needs to be damped
    private void Move(float horizontalInput, float verticalInput)
    {
        Vector3 eulerSpeed = new Vector3(0, horizontalInput * manualRotSpeed, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerSpeed * Time.deltaTime);
        robotRb.MoveRotation(robotRb.rotation * deltaRotation);

        Vector3 forwardVector = robotRb.rotation * Vector3.forward;
        Vector3 moveForward = new Vector3(forwardVector[0] * verticalInput, 0, forwardVector[2] * verticalInput);
        robotRb.MovePosition(robotRb.position + moveForward * manualMoveSpeed * Time.fixedDeltaTime);
    }
    private float DesiredDirectionAngle(Vector3 directionVector)
    {
        Vector3 pathDir = (directionVector - transform.position); // With Y value
        pathDir.y = 0;
        pathDir = pathDir.normalized;
        return Vector3.SignedAngle(pathDir, transform.forward, transform.up);
    }

    private void CheckForUIArrow(Vector3 targetRotationPosition, int acceptableAngle = 45)
    {
        float desiredDirAngle = DesiredDirectionAngle(targetRotationPosition);

        if (desiredDirAngle >= acceptableAngle && desiredDirAngle <= 180) uiController.UISetArrow("left");

        else if (desiredDirAngle > -180 && desiredDirAngle <= -acceptableAngle) uiController.UISetArrow("right");

        else uiController.UISetArrow("disable");
    }
    private void CheckForRotationAdjustment(Vector3 targetRotationPosition, int acceptableAngle = 5)
    {
        float desiredDirAngle = DesiredDirectionAngle(targetRotationPosition);

        if (desiredDirAngle >= acceptableAngle && desiredDirAngle <= 180)
        {
            RotateInPosition(-1);
        }
        else if (desiredDirAngle > -180 && desiredDirAngle <= -acceptableAngle)
        {
            RotateInPosition(1);
        }
        else if (waypointRotatinAdjustment)
        {
            waypointRotatinAdjustment = false;
            robotManager.WaypointRotationAdjusted();
        }
        else
        {
            if (navMeshAgent.isStopped == true) navMeshAgent.isStopped = false;
        }
    }
    private void RotateInPosition(int direction)
    {
        if (navMeshAgent.isStopped == false) navMeshAgent.isStopped = true;
        Move(direction, 0);
    }
    public void RotationAdjustement(Vector3 positionDirection)
    {
        waypointRotatinAdjustment = true;
        rotationAdjustmentPosition = positionDirection;
    }
    public NavMeshAgent GetNavMeshAgent() { return navMeshAgent; }  

    public int GetLoa() { return loa; }

    public void SetLoa(int desiredLoa, bool force, bool record = true) 
    {
        if (!canSwitchLOA && !force) return;

        if (desiredLoa == 1)
        {
            loa = 1;
            navMeshAgent.isStopped = true;
            uiController.UISetLevel("low");
            if (record) robotManager.LOASwitched(1);
            isCollided = false;
        }
        else if (desiredLoa == 2)
        {
            loa = 2;
            navMeshAgent.isStopped = false;
            uiController.UISetLevel("high");
            uiController.UISetArrow("disable");
            if (record) robotManager.LOASwitched(2);
        }
    }
    public void JoystickLoaSwitch()
    {
        if (loa == 1) SetLoa(2, false);

        else if (loa == 2) SetLoa(1, false);
    }

    public void CanSwitchLOA(bool boolean) { canSwitchLOA = boolean; }

    public void CanMove(bool mode) { canMove = mode; }

    public void SetTutorialMode(bool tutorial) { isTutorial = tutorial; AssistantArrowsActivation(!tutorial); }

    public void AssistantArrowsActivation(bool mode)
    { 
        assistanceArrowDisplay = mode; 
        if (!mode) { uiController.UISetArrow("disable"); }
    }

    public void AgentCollision() { navMeshAgent.isStopped = true; isCollided = true; }

}
