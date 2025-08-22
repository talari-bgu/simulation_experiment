using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.XR;

public class TransparencyManager : MonoBehaviour
{
    public SimulationManager simulationManager;
    public RobotManager robotManager;
    public TextMeshProUGUI description;

    public GameObject tbaCanvas;
    public TextMeshProUGUI question;
    public TextMeshProUGUI answer1;
    public TextMeshProUGUI answer2;

    [SerializeField] string tba_mode;
    [SerializeField] string exp_level;

    [SerializeField] string exp_level_old;
    [SerializeField] string exp_changed;

    [SerializeField] bool assistanceFlag = false;
    [SerializeField] bool stopFlag = false;
    [SerializeField] bool duplicateFlag = false;

    [SerializeField] bool isFirstQuery;
    
    

    private void Start()
    {
        tba_mode = simulationManager.tba_mode;
        exp_level = "";
        exp_changed = "";

        isFirstQuery = true;
    }

    private void Update()
    {
        if (assistanceFlag)
        {
            int loa = robotManager.robotScript.GetLoa();
            if (loa == 1) 
            { 
                description.text = "";
                duplicateFlag = false;
            }
            else if (loa == 2 && !duplicateFlag) 
            {
                description.text = "I think there are something in my way, please take control.";
                duplicateFlag = true;
            }
        }
    }

    public void StartHumanAssistance()
    {
        if (tba_mode == "high") { assistanceFlag = true; stopFlag = true; }
    }
    public void StopHumanAssistance()
    {
        assistanceFlag = false;
        duplicateFlag = false;
        if (stopFlag && robotManager.robotScript.GetLoa() == 1) 
        { 
            StartCoroutine(SetDescription("I think I can manage by myself if you want to change back to High Automation.", 5)); 
        }
        else if (stopFlag && robotManager.robotScript.GetLoa() == 2)
        {
            StartCoroutine(SetDescription("Obstacle passed.", 5));
        }

        stopFlag = false;
    }

    // this is the max size of a sentence that will fit to the screen.
    // |----------------------------------------------------------------------------------------------------------|

    // This method is called by robotManager
    // Session : 0 -> first, 1-> second
    // Control_mode : 0 -> RI first, 1 -> HI first
    public void RobotBehaviorMassage(int session, int control_mode)
    {
        // session 1, the robot is responsible now
        if (session == 0 && control_mode == 0) 
        {
            if (tba_mode == "high") { StartCoroutine(SetDescription("Now, I am responsible for switching between my automation level. I will begin with High automation.\n" +
                " During the session I might need your help in some cases.")); }
            else if (tba_mode =="low") { StartCoroutine(SetDescription("Now, I am responsible for switching between my automation level, I will begin with High automation.")); }
        }
        // session 1, the human is responsible now
        else if (session == 0 && control_mode == 1) 
        { 
            if (tba_mode == "high") { StartCoroutine(SetDescription("I need your help switching between automation levels manually because my sensor is broken. \n" +
                "The default for the beginning is Low automation")); }
            else if (tba_mode == "low") { StartCoroutine(SetDescription("I need your help switching between automation levels manually, the default for the beginning is Low automation.")); }
        }
        // session 2, the human is responsible now
        else if (session == 1 && control_mode == 0) 
        {
            if (tba_mode == "high") { StartCoroutine(SetDescription("I need your help switching between automation levels manually because my sensor is broken. \n" +
                "The default for the beginning is Low automation")); }
            else if (tba_mode == "low") { StartCoroutine(SetDescription("I need your help switching between automation levels manually, the default for the beginning is Low automation.")); }
        }
        // session 2, the robot is responsible now
        else if (session == 1 && control_mode == 1) 
        {
            if (tba_mode == "high") { StartCoroutine(SetDescription("Now, I am responsible for switching between my automation level. I will begin with High automation.\n" +
                " During the session I might need your help in some cases.")); }
            else if (tba_mode == "low") { StartCoroutine(SetDescription("Now, I am responsible for switching between my automation level, I will begin with High automation.")); }
        }
    }


    // this method is called by automationManager
    // this is the max size of a sentence that will fit to the screen.
    // |----------------------------------------------------------------------------------------------------------|
    public void LoaSwitchMassageDisplay(string scenario)
    {
        switch (scenario)
        {
            case "obstacle1":
                if (tba_mode == "low") { description.text = "Please take control and pass the obstacle."; }
                else if (tba_mode == "high")
                {
                    if (exp_level == "low") { description.text = "There are two chairs in front of me that I can't recognize, so please take control \nand pass them."; }
                    else { description.text = "There are two chairs in front of me that I can't recognize, so please take control \nand pass them around from the left."; }
                }
                break;
            case "obstacle1_cleared":
                if (tba_mode == "low") { StartCoroutine(SetDescription("The obstacle has been passed; I am taking the control back.")); }
                else if (tba_mode == "high")
                {
                    if (exp_level == "low") { StartCoroutine(SetDescription("The chairs have been passed thanks for your help; I am taking the control back.")); }
                    else { StartCoroutine(SetDescription("The chairs have been passed thanks for your help; I am taking the control back. \nNotice, I might need your help again.")); }
                }
                break;
            case "obstacle2":
                if (tba_mode == "low") { description.text = "Please take control and pass the obstacle."; }
                else if (tba_mode == "high")
                {
                    if (exp_level == "low") { description.text = "Two people stand in front of me, and I am afraid to hurt them, so please take control \nand pass them."; }
                    else { description.text = "Two people stand in front of me, and I am afraid to hurt them, so please take control \nand pass them around from the right."; }
                }
                break;
            case "obstacle2_cleared":
                if (tba_mode == "low") { StartCoroutine(SetDescription("The obstacle has been passed; I am taking the control back.")); }
                else if (tba_mode == "high")
                {
                    if (exp_level == "low") { StartCoroutine(SetDescription("The people have been passed thanks for your help; I am taking the control back.")); }
                    else { StartCoroutine(SetDescription("The people have been passed thanks for your help; I am taking the control back. \nNotice, I might need your help again.")); }
                }
                break;
            case "obstacle3":
                if (tba_mode == "low") { description.text = "Please take control and pass the obstacle."; }
                else if (tba_mode == "high") 
                {
                    if (exp_level == "low") { description.text = "There are some boxes in front of me that I can't recognize, so please take control."; }
                    else { description.text = "There are some boxes in front of me that I can't recognize, so please take control \nand move back a bit and pass one of the desks from the side."; }
                }
                break;
            case "obstacle3_cleared":
                if (tba_mode == "low") { StartCoroutine(SetDescription("The obstacle has been passed; I am taking the control back.")); }
                else if (tba_mode == "high") 
                {
                    if (exp_level == "low") { StartCoroutine(SetDescription("The boxes have been passed thanks for your help; I am taking the control back.")); }
                    else { StartCoroutine(SetDescription("The boxes have been passed thanks for your help; I am taking the control back. \nNotice, I might need your help again.")); }
                }
                break;

            // Path backwords
            case "obstacle4":
                if (tba_mode == "low") { description.text = "Please take control and pass the obstacle."; }
                else if (tba_mode == "high") 
                {
                    if (exp_level == "low") { description.text = "Two people stand in front of me, and I am afraid to hurt them, so please take control \nand pass them around from the right."; }
                    else { description.text = "Two people stand in front of me, and I am afraid to hurt them, so please take control \nand pass them."; }
                }
                break;
            case "obstacle4_cleared":
                if (tba_mode == "low") { StartCoroutine(SetDescription("The obstacle has been passed; I am taking the control back.")); }
                else if (tba_mode == "high") 
                {
                    if (exp_level == "low")  StartCoroutine(SetDescription("The people have been passed thanks for your help; I am taking the control back.")); 
                    else  StartCoroutine(SetDescription("The people have been passed thanks for your help; I am taking the control back. \nNotice, I might need your help again.")); 
                }
                break;
            case "obstacle5":
                if (tba_mode == "low") { description.text = "Please take control and pass the obstacle."; }
                else if (tba_mode == "high") 
                {
                    if (exp_level == "low") description.text = "There are some boxes in front of me that I can't recognize,\n so please take control and pass them.";
                    else  description.text = "There are some boxes in front of me that I can't recognize, so please take control\n and move back a bit and pass desks from the left side."; 
                }
                break;
            case "obstacle5_cleared":
                if (tba_mode == "low") { StartCoroutine(SetDescription("The obstacle has been passed; I am taking the control back.")); }
                else if (tba_mode == "high") 
                {
                    if (exp_level == "low") StartCoroutine(SetDescription("The boxes have been passed thanks for your help; I am taking the control back."));
                    else { StartCoroutine(SetDescription("The boxes have been passed thanks for your help; I am taking the control back. \nNotice, I might need your help again.")); }
                }
                break;
            case "obstacle6":
                if (tba_mode == "low") { description.text = "Please take control and pass the obstacle."; }
                else if (tba_mode == "high") 
                {
                    if (exp_level == "low") description.text = "There are benches in front of me that I can't recognize,\n so please take control and pass them";
                    else description.text = "There are benches in front of me that I can't recognize, so please take \ncontrol and pass them around from the left."; 
                }
                break;
            case "obstacle6_cleared":
                if (tba_mode == "low") { StartCoroutine(SetDescription("The obstacle has been passed; I am taking the control back.")); }
                else if (tba_mode == "high") 
                {
                    if (exp_level == "low") StartCoroutine(SetDescription("The benches have been passed thanks for your help; I am taking the control back."));
                    else  StartCoroutine(SetDescription("The benches have been passed thanks for your help; I am taking the control back. \nNotice, I might need your help again.")); 
                }
                break;

            default:
                Debug.Log("Problem in Transparency activation switch");
                break;
        }
    }
    

    // is called after passing trigger2 in automationManager
    public void NotEnoughL2Time()
    {
        StartCoroutine(SetDescription("Don’t forget u are in control, if u wants my help u can change to High Automation"));
    }

    // robotManager should call this method 
    public void SetExplanationModeWindow()
    {
        // if second time
        if (!isFirstQuery)
        {
            question.text = "Do you like the explanation as is?";
            answer1.text = "Yes";
            if (exp_level == "low") { answer2.text = "Change to High."; }
            else if (exp_level == "high") { answer2.text = "Change to Low."; }
        }
        tbaCanvas.SetActive(true);

        if (Time.timeScale != 0) Time.timeScale = 0;
    }

    // buttons call this method
    public void QueryClicked(int answer)
    {
        // need to notify experiment of the change
        if (isFirstQuery)
        {
            if (answer == 1) { exp_level = "high";}
            else if (answer == 2) { exp_level = "low"; }
            exp_level_old = exp_level;
            robotManager.ShowStartButton();
        }
        else if(!isFirstQuery) 
        { 
            if (answer == 1) exp_changed = "no";
            else if (answer == 2) 
            {  
                if (exp_level == "low")  exp_level = "high"; 
                else if (exp_level == "high")  exp_level = "low";
                exp_changed = "yes";

            }
            Time.timeScale = 1;
        }
        tbaCanvas.SetActive(false);
        isFirstQuery = false;
        
    }

    public List<string> GetTbaRecords()
    {
        List<string> temp = new List<string>();
        temp.Add(tba_mode);
        temp.Add(exp_level_old);
        temp.Add(exp_changed);
        return temp;
    }

    private IEnumerator SetDescription(string text, int duration = 10)
    {
        description.text = text;
        yield return new WaitForSeconds(duration);
        description.text = "";
    }

}
