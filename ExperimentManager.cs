using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using System.IO;
using Unity.VisualScripting;
using System.Diagnostics;

public class ExperimentManager : MonoBehaviour
{
    [Header("Experiment")]
    public bool isRecorded;
    public int session = 0;

    [Header("Setting")]
    public bool loaTriggerActivate;
    public bool mailTriggerActivate;
    public bool saTriggerActivate;
    public bool popupDebuger = false;
    public float popupJoystickThreshhold = 0.5f;

    [Header("Objects")]
    public AutomationManager automationManager;
    public MailManager mailManager;
    public SAManager situationAwarenessManager;



    // FS - first session, SS - second session
    [Header("Records")]
    [SerializeField] private string participantID;
    [SerializeField] private string control_mode_FS;
    [SerializeField] private string control_mode_SS;
    [SerializeField] private string complexity;

    [SerializeField] private List<string> tba_mode_FS;
    [SerializeField] private List<string> tba_mode_SS;


    [SerializeField] private DateTime time_started_FS;
    [SerializeField] private DateTime time_started_SS;

    [SerializeField] private float distanceTraveled_FS;
    [SerializeField] private float distanceTraveled_SS;

    // Length should be 8
    [SerializeField] private List<DateTime> checkpointReached_dt_FS;
    [SerializeField] private List<DateTime> checkpointReached_dt_SS;

    // When human was in control
    // Unknown Length
    [SerializeField] private List<string> loaSwitch;
    [SerializeField] private List<string> loaSwitch_dt;

    // When robot was in control
    // Length should be 6
    [SerializeField] private List<string> robotLoaDecrease_dt;
    [SerializeField] private List<string> robotLoaIncrease_dt;

    // Unknown Length
    [SerializeField] private List<string> collision_FS;
    [SerializeField] private List<DateTime> collision_dt_FS;
    
    [SerializeField] private List<string> collision_SS;
    [SerializeField] private List<DateTime> collision_dt_SS;

    // Length should be 6
    [SerializeField] private List<int> sa_answers_FS;
    [SerializeField] private List<float> sa_answers_span_FS;

    [SerializeField] private List<int> sa_answers_SS;
    [SerializeField] private List<float> sa_answers_span_SS;

    [SerializeField] private DateTime time_finished_FS;

    // assisstance variables to split into 2 files
    [SerializeField] List<string> control_mode_FS_1;
    [SerializeField] List<string> control_mode_FS_2;

    [SerializeField] List<string> control_mode_SS_1;
    [SerializeField] List<string> control_mode_SS_2;

    // this is for second monitor popups
    private bool popupFlag;
    private Coroutine coroutine_handler;


    // Start is called before the first frame update
    void Start()
    {
        popupFlag = false;

        automationManager.gameObject.SetActive(loaTriggerActivate);
        mailManager.gameObject.SetActive(mailTriggerActivate);
        situationAwarenessManager.gameObject.SetActive(saTriggerActivate);

        tba_mode_FS = new List<string>();
        tba_mode_SS = new List<string>();

        checkpointReached_dt_FS = new List<DateTime>();
        checkpointReached_dt_SS = new List<DateTime>();

        loaSwitch = new List<string>();
        loaSwitch_dt = new List<string>();

        robotLoaDecrease_dt = new List<string>();
        robotLoaIncrease_dt = new List<string>();

        collision_FS = new List<string>();
        collision_dt_FS = new List<DateTime>();

        collision_SS = new List<string>();
        collision_dt_SS = new List<DateTime>();

        sa_answers_FS = new List<int>();
        sa_answers_span_FS = new List<float>();

        sa_answers_SS = new List<int>();
        sa_answers_span_SS = new List<float>();

    }

    // this is for the message on the second monitor.
    private void Update()
    {
        if (popupFlag && (Input.GetAxisRaw("VerticalC") > popupJoystickThreshhold ||
            Input.GetAxisRaw("VerticalC") < -popupJoystickThreshhold ||
            Input.GetAxisRaw("HorizontalC") < -popupJoystickThreshhold ||
            Input.GetAxisRaw("HorizontalC") >   popupJoystickThreshhold ||
            Input.GetButtonDown("SwitchMode")))
        {
            UnityEngine.Debug.Log("stoped");
            StopCoroutine(coroutine_handler);
            popupFlag = false;
        }

        if (Input.GetKeyDown(KeyCode.C)) 
        {
            WriteFile();
        }
    }

    // Start button right before the experiment will send SetExperimentProperies
    // need to make sure called SetPath is before.
    public void SetPath(int side)
    {
        if (loaTriggerActivate) automationManager.SetPath(side);
        if (mailTriggerActivate) mailManager.SetPath(side);
        if (saTriggerActivate) situationAwarenessManager.SetPath(side);


        if (side == 0 && session == 0) session = 1;
        else if (side == 0 && session == 1)
        {
            time_finished_FS = DateTime.Now;
            session = 2;
        }
    }

    // Automation calls this method
    public void startWindowPopup(float time)
    {
        coroutine_handler = StartCoroutine(startWindowPopupCoroutine(time));
    }

    private IEnumerator startWindowPopupCoroutine(float time)
    {
        UnityEngine.Debug.Log("started");
        popupFlag = true;
        yield return new WaitForSeconds(time);

        if (popupDebuger)
        {         
            UnityEngine.Debug.Log("poped");
        }
        else
        {
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.FileName = @"C:\Users\Tal\Desktop\Experiment\PSTools\PsExec.exe";
            p.StartInfo.Arguments = @"-i 1 \\DESKTOP-VIV9D5P -u tamarama -p 123456 -s wscript.exe C:\Users\tamarama\robot.vbs";
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            string errormessage = p.StandardError.ReadToEnd();
            
            p.WaitForExit();
            UnityEngine.Debug.Log("popedkjkjk");
        }
        popupFlag = false;
    }

    // the method should receive current controller: robot/human as a string
    public void SetExperimentProperties(string id, string switch_mode, string complexity_mode)
    {
        participantID = id;
        complexity = complexity_mode;

        if (session == 1)
        {
            time_started_FS = DateTime.Now;
            control_mode_FS = switch_mode;
        }
        else if (session == 2)
        {
            time_started_SS = DateTime.Now;
            control_mode_SS = switch_mode;
        }

    }

    // send empty if - low
    public void SetTbaMode(List<string> tba)
    {
        if (session == 1) tba_mode_FS = tba;
        else if (session == 2) tba_mode_SS = tba;
    }

    public void SetDistanceTraveled(float distance)
    {
        if (session == 1) distanceTraveled_FS = distance;
        else if (session == 2) distanceTraveled_SS = distance;
    }
    public void CheckpointReached(DateTime dateTime)
    {
        if (session == 1) checkpointReached_dt_FS.Add(dateTime);
        else if (session == 2) checkpointReached_dt_SS.Add(dateTime);
    }

    public void LOASwitch(int loa, DateTime dateTime)
    {
        loaSwitch.Add(loa.ToString());
        loaSwitch_dt.Add(dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
    }
    public void RobotLoaDecrease(DateTime dateTime)
    {
        robotLoaDecrease_dt.Add(dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
    }

    public void RobotLoaIncrease(DateTime dateTime)
    {
        robotLoaIncrease_dt.Add(dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
    }
    public void AddCollision(string name)
    {
        if (session == 1)
        {
            collision_FS.Add(name);
            collision_dt_FS.Add(DateTime.Now);
        }
        else if (session == 2)
        {
            collision_SS.Add(name);
            collision_dt_SS.Add(DateTime.Now);
        }
    }

    public void AddSaAnswer(int answer, float timeTookSecs)
    {
        if (session == 1)
        {
            sa_answers_FS.Add(answer);
            sa_answers_span_FS.Add(timeTookSecs);
        }
        else if (session == 2)
        {
            sa_answers_SS.Add(answer);
            sa_answers_span_SS.Add(timeTookSecs);
        }
    }

    public void WriteFile()
    {
        // example code
        if (isRecorded)
        {
            DateTime time_finished_SS = DateTime.Now;

            // Create dir
            string dir = @"C:\Users\Tal\Desktop\Experiment\Records\" + participantID.ToString();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            // Name the file
            string csvFilePath_FS = dir + "/session1.csv";
            string csvFilePath_SS = dir + "/session2.csv";



            // fitting the parameters to the scenario
            if (control_mode_FS == "robot")
            {
                control_mode_FS_1 = robotLoaDecrease_dt;
                control_mode_FS_2 = robotLoaIncrease_dt;

                control_mode_SS_1 = loaSwitch;
                control_mode_SS_2 = loaSwitch_dt;
            }
            else if (control_mode_FS == "human")
            {
                control_mode_FS_1 = loaSwitch;
                control_mode_FS_2 = loaSwitch_dt;

                control_mode_SS_1 = robotLoaDecrease_dt;
                control_mode_SS_2 = robotLoaIncrease_dt;
            }

            // Preparing session1 file
            // Looking for the longest column
            int[] lengthArray = new int[] {checkpointReached_dt_FS.Count, control_mode_FS_1.Count, collision_FS.Count };
            int maxTemp = 1;
            for (int i = 0; i < lengthArray.Length; i++)
            {
                if (lengthArray[i] > maxTemp)
                {
                    maxTemp = lengthArray[i];
                }
            }

            // Open the file for writing
            using (StreamWriter streamWriter = new StreamWriter(csvFilePath_FS))
            {
                // Header
                streamWriter.WriteLine("participantID, switch_mode, complexity, tba_mode, start_dt, end_dt, distanceTraveled, checkpoints_dt, control_parameter1, control_parameter2, " +
                    "saAnswer, saAnswer_span, collision, collision_dt");


                for (int i = 0; i < maxTemp; i++)
                {
                    // 14 parameters
                    string participantID = i < 1 ? this.participantID : "";
                    string control_mode = i < 1 ? this.control_mode_FS : "";
                    string complexity = i < 1 ? this.complexity : "";
                    string tba = i < 3 ? this.tba_mode_FS[i] : "";
                    string start_dt = i < 1 ? this.time_started_FS.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
                    string end_dt = i < 1 ? time_finished_FS.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
                    string distanceTraveled = i < 1 ? this.distanceTraveled_FS.ToString() : "";
                    string checkpoint_dt = i < this.checkpointReached_dt_FS.Count ? this.checkpointReached_dt_FS[i].ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
                    string control_parameter1 = i < this.control_mode_FS_1.Count ? this.control_mode_FS_1[i] : "";
                    string control_parameter2 = i < this.control_mode_FS_2.Count ? this.control_mode_FS_2[i] : "";
                    string saAnswer = i < this.sa_answers_FS.Count ? this.sa_answers_FS[i].ToString() : "";
                    string saAnswerSpan = i < this.sa_answers_span_FS.Count ? this.sa_answers_span_FS[i].ToString() : "";
                    string collision = i < this.collision_FS.Count ? this.collision_FS[i] : "";
                    string collision_dt = i <this.collision_dt_FS.Count ? this.collision_dt_FS[i].ToString("yyyy-MM-dd HH:mm:ss.fff") : "";


                    string dataRow = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}",
                                                    participantID,
                                                    control_mode,
                                                    complexity,
                                                    tba,
                                                    start_dt,
                                                    end_dt,
                                                    distanceTraveled,
                                                    checkpoint_dt,
                                                    control_parameter1,
                                                    control_parameter2,
                                                    saAnswer,
                                                    saAnswerSpan,
                                                    collision,
                                                    collision_dt);

                    streamWriter.WriteLine(dataRow);
                }
            }

            // Preparing session2 file
            // Looking for the longest column
            lengthArray = new int[] { checkpointReached_dt_SS.Count, control_mode_SS_1.Count, collision_SS.Count };
            maxTemp = 1;
            for (int i = 0; i < lengthArray.Length; i++)
            {
                if (lengthArray[i] > maxTemp)
                {
                    maxTemp = lengthArray[i];
                }
            }

            // Open the file for writing
            using (StreamWriter streamWriter = new StreamWriter(csvFilePath_SS))
            {
                // Header
                streamWriter.WriteLine("participantID, switch_mode, complexity, tba_mode, start_dt, end_dt, distanceTraveled, checkpoints_dt, control_parameter1, control_parameter2, " +
                    "saAnswer, saAnswer_span, collision, collision_dt");


                for (int i = 0; i < maxTemp; i++)
                {
                    // 14 parameters
                    string participantID = i < 1 ? this.participantID : "";
                    string control_mode = i < 1 ? this.control_mode_SS : "";
                    string complexity = i < 1 ? this.complexity : "";
                    string tba = i < 3 ? this.tba_mode_SS[i] : "";
                    string start_dt = i < 1 ? this.time_started_SS.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
                    string end_dt = i < 1 ? time_finished_SS.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
                    string distanceTraveled = i < 1 ? this.distanceTraveled_SS.ToString() : "";
                    string checkpoint_dt = i < this.checkpointReached_dt_SS.Count ? this.checkpointReached_dt_SS[i].ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
                    string control_parameter1 = i < this.control_mode_SS_1.Count ? this.control_mode_SS_1[i] : "";
                    string control_parameter2 = i < this.control_mode_SS_2.Count ? this.control_mode_SS_2[i] : "";
                    string saAnswer = i < this.sa_answers_SS.Count ? this.sa_answers_SS[i].ToString() : "";
                    string saAnswerSpan = i < this.sa_answers_span_SS.Count ? this.sa_answers_span_SS[i].ToString() : "";
                    string collision = i < this.collision_SS.Count ? this.collision_SS[i] : "";
                    string collision_dt = i < this.collision_dt_SS.Count ? this.collision_dt_SS[i].ToString("yyyy-MM-dd HH:mm:ss.fff") : "";


                    string dataRow = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}",
                                                    participantID,
                                                    control_mode,
                                                    complexity,
                                                    tba,
                                                    start_dt,
                                                    end_dt,
                                                    distanceTraveled,
                                                    checkpoint_dt,
                                                    control_parameter1,
                                                    control_parameter2,
                                                    saAnswer,
                                                    saAnswerSpan,
                                                    collision,
                                                    collision_dt);

                    streamWriter.WriteLine(dataRow);
                }
            }
        }
    }

}

/*
 * I want to measure:
 * 1. Time took for the simulation: time start, time end, also check point raech time
 * 2. Time taken when robot requested loa decreasement
 * 3. Count and time of loa switch when parcipant was responsible for the switch
 * 4. Collision - Time and data
 * 
*/