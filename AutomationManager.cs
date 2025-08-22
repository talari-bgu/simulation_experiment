using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AutomationManager : MonoBehaviour
{
    public SimulationManager simulationManager;
    public RobotManager robotManager;
    public TransparencyManager transparencyManager;
    public ExperimentManager experimentManager;

    public GameObject sideA;
    public List<GameObject> sideATriggers;
    public GameObject sideB;
    public List<GameObject> sideBTriggers;

    public void TriggerActivation(string trigger)
    {
        switch (trigger)
        {
            case "Trigger1":
                // Robot is in control
                if ((simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 2))
                {
                    robotManager.ChangeLOA(1);
                    transparencyManager.LoaSwitchMassageDisplay("obstacle1");
                    experimentManager.startWindowPopup(10);
                }
                // Human is in control
                else if ((simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 2))
                {
                    transparencyManager.StartHumanAssistance();
                    robotManager.AssistantArrowsActivation(false);
                }

                break;
            case "Trigger2":
                // Robot is in control
                if ((simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 2))
                {
                    robotManager.ChangeLOA(2);
                    transparencyManager.LoaSwitchMassageDisplay("obstacle1_cleared");
                }
                // Human is in control
                else if ((simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 2))
                {
                    transparencyManager.StopHumanAssistance();
                    robotManager.AssistantArrowsActivation(true);

                    // Naama condition
                    if (robotManager.CheckTimeInL2() < 3f) { transparencyManager.NotEnoughL2Time(); }
                }
                break;

                case "Trigger3":
                // Robot is in control
                if ((simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 2))
                {
                    robotManager.ChangeLOA(1);
                    transparencyManager.LoaSwitchMassageDisplay("obstacle2");
                    experimentManager.startWindowPopup(10);
                }
                // Human is in control
                else if ((simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 2))
                {
                    transparencyManager.StartHumanAssistance();
                    robotManager.AssistantArrowsActivation(false);
                }
                break;
            case "Trigger4":
                // Robot is in control
                if ((simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 2))
                {
                    robotManager.ChangeLOA(2);
                    transparencyManager.LoaSwitchMassageDisplay("obstacle2_cleared");
                }
                // Human is in control
                else if ((simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 2))
                {
                    transparencyManager.StopHumanAssistance();
                    robotManager.AssistantArrowsActivation(true);
                }
                break;

            case "Trigger5":
                // Robot is in control
                if ((simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 2))
                {
                    robotManager.ChangeLOA(1);
                    transparencyManager.LoaSwitchMassageDisplay("obstacle3");
                    experimentManager.startWindowPopup(10);
                }
                // Human is in control
                else if ((simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 2))
                {
                    transparencyManager.StartHumanAssistance();
                    robotManager.AssistantArrowsActivation(false);
                }
                break;
            case "Trigger6":
                // Robot is in control
                if ((simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 2))
                {
                    robotManager.ChangeLOA(2);
                    transparencyManager.LoaSwitchMassageDisplay("obstacle3_cleared");
                }
                // Human is in control
                else if ((simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 2))
                {
                    transparencyManager.StopHumanAssistance();
                    robotManager.AssistantArrowsActivation(true);
                }
                break;

            case "Trigger7":
                // Robot is in control
                if ((simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 2))
                {
                    robotManager.ChangeLOA(1);
                    transparencyManager.LoaSwitchMassageDisplay("obstacle4");
                    experimentManager.startWindowPopup(10);
                }
                // Human is in control
                else if ((simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 2))
                {
                    transparencyManager.StartHumanAssistance();
                    robotManager.AssistantArrowsActivation(false);
                }
                break;
            case "Trigger8":
                // Robot is in control
                if ((simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 2))
                {
                    robotManager.ChangeLOA(2);
                    transparencyManager.LoaSwitchMassageDisplay("obstacle4_cleared");
                }
                // Human is in control
                else if ((simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 2))
                {
                    transparencyManager.StopHumanAssistance();
                    robotManager.AssistantArrowsActivation(true);
                }
                break;

            case "Trigger9":
                // Robot is in control
                if ((simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 2))
                {
                    robotManager.ChangeLOA(1);
                    transparencyManager.LoaSwitchMassageDisplay("obstacle5");
                    experimentManager.startWindowPopup(10);
                }
                // Human is in control
                else if ((simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 2))
                {
                    transparencyManager.StartHumanAssistance();
                    robotManager.AssistantArrowsActivation(false);
                }
                break;
            case "Trigger10":
                // Robot is in control
                if ((simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 2))
                {
                    robotManager.ChangeLOA(2);
                    transparencyManager.LoaSwitchMassageDisplay("obstacle5_cleared");
                }
                // Human is in control
                else if ((simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 2))
                {
                    transparencyManager.StopHumanAssistance();
                    robotManager.AssistantArrowsActivation(true);
                }
                break;

            case "Trigger11":
                // Robot is in control
                if ((simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 2))
                {
                    robotManager.ChangeLOA(1);
                    transparencyManager.LoaSwitchMassageDisplay("obstacle6");
                    experimentManager.startWindowPopup(10);
                }
                // Human is in control
                else if ((simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 2))
                {
                    transparencyManager.StartHumanAssistance();
                    robotManager.AssistantArrowsActivation(false);
                }
                break;
            case "Trigger12":
                // Robot is in control
                if ((simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 2))
                {
                    robotManager.ChangeLOA(2);
                    transparencyManager.LoaSwitchMassageDisplay("obstacle6_cleared");
                }
                // Human is in control
                else if ((simulationManager.control_mode == "humanFirst" && robotManager.GetSession() == 1) ||
                    (simulationManager.control_mode == "robotFirst" && robotManager.GetSession() == 2))
                {
                    transparencyManager.StopHumanAssistance();
                    robotManager.AssistantArrowsActivation(true);
                }
                break;



            default:
                Debug.Log("Problem in Automation activation switch");
                break;

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
