using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class SimulationManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject tutorial;
    public GameObject experiment;
    public GameObject user;
    public GameObject simulationEnvironment;
    public GameObject officeEnvironment;
    public GameObject blackScreen;

    public RobotManager robotManager;

    public string parNum ,control_mode, complexity, tba_mode;
    public GameObject crowded;
    public int email_num;

    public CanvasGroup _UIGroup;
    [SerializeField] bool fadeIn = false;
    [SerializeField] bool fadeOut = false;


    public void Awake()
    {
        StartApplicationGameObjects();
    }
    private void Update()
    {
        if (fadeIn)
        {
            if (_UIGroup.alpha < 1)
            {
                _UIGroup.alpha += 2 * Time.deltaTime;
                if (_UIGroup.alpha >= 1) fadeIn = false;
            }
        }
        if (fadeOut)
        {
            if (_UIGroup.alpha >= 0)
            {
                _UIGroup.alpha -= 2 * Time.deltaTime;
                if (_UIGroup.alpha == 0) {
                    robotManager.FadeOutEnd();
                    blackScreen.SetActive(false);
                    fadeOut = false;
                }
            }
        }
    }




    // This method is being called from the main menu button
    public void LoadTutorial(string number, int configuration, int email)
    {
        switch (configuration)
        {
            case 1:
                control_mode = "humanFirst";
                complexity = "low";
                tba_mode = "low";
                break;
            case 2:
                control_mode = "humanFirst";
                complexity = "high";
                tba_mode = "low";
                break;
            case 3:
                control_mode = "robotFirst";
                complexity = "low";
                tba_mode = "low";
                break;
            case 4:
                control_mode = "robotFirst";
                complexity = "high";
                tba_mode = "low";
                break;
            case 5:
                control_mode = "humanFirst";
                complexity = "high";
                tba_mode = "high";
                break;
            case 6:
                control_mode = "robotFirst";
                complexity = "high";
                tba_mode = "high";
                break;


            default:
                break;
        }
        
        parNum = number;

        email_num = email;


        tutorial.SetActive(true);
        user.SetActive(true);

        Destroy(mainMenu);

        // setup robot
        robotManager.SetTutorial();

        // setup environment
        tutorial.GetComponent<TutorialManager>().StartTutorial();
    }
    
    // This method is being called from the tutorial canvas
    public void UnloadTutorialLoadExperiment()
    {
        // black fade in
        blackScreen.SetActive(true);
        fadeIn = true;

        // close tutorial
        Destroy(tutorial);

        // setup environement
        simulationEnvironment.SetActive(true);

        // setup experiment
        experiment.SetActive(true);

        // creates crowded if required
        if (complexity == "high") Instantiate(crowded, simulationEnvironment.transform);

        // delays 1 sec for the instantiation
        StartCoroutine(DelayedStart());
    }


    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1);
        
        // passing the within factor so the manager will know what initiative
        StartCoroutine(robotManager.SetExperiment(control_mode));
        
        // assuming the command above takes less than 2 sec to completly finish and then fade out
        yield return new WaitForSeconds(0);
        fadeOut = true;
    }

    // This method is being called from the Dev button, and then wait for the set active and instantiate
    // and then call SetExperiment.
    public void DevStart()
    {
        Destroy(mainMenu);
        Destroy(tutorial);

        simulationEnvironment.SetActive(true);
        user.SetActive(true);
        experiment.SetActive(true);

        if (complexity == "high")
        {
            Instantiate(crowded);
        }

        StartCoroutine(DelayedStart());
    }

    private void StartApplicationGameObjects()
    {
        mainMenu.SetActive(true);
        tutorial.SetActive(false);
        experiment.SetActive(false);
        user.SetActive(true);
        simulationEnvironment.SetActive(false);
        officeEnvironment.SetActive(true);
        blackScreen.SetActive(false);
    }
}
