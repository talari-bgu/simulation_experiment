using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SAManager : MonoBehaviour
{
    public ExperimentManager experimentManager;

    public GameObject canvas;
    public TextMeshProUGUI question;
    public TextMeshProUGUI answer1;
    public TextMeshProUGUI answer2;
    public TextMeshProUGUI answer3;
    public TextMeshProUGUI answer4;

    public List<Question> questions;

    public GameObject sideA;
    public List<GameObject> sideATriggers;
    public GameObject sideB;
    public List<GameObject> sideBTriggers;

    public GameObject paint1;
    public GameObject cube;
    public GameObject paint2;
    public GameObject sphere;

    public Material greenMaterial;
    public Material redMaterial;
    public Material blueMaterial;
    public Material orangeMaterial;
    public Material yellowMaterial;
    public Material purpleMaterial;

    private int currentQuestionIndex;
    private DateTime timePopped;
    private DateTime timeAnswered;

    [SerializeField] int session;

    private void Start()
    {
        currentQuestionIndex = 0;
        session = 0;
    }


    public void TriggerActivation()
    {
        SetQuestion();
    }

    private void SetQuestion()
    {
        Time.timeScale = 0;
        timePopped = DateTime.Now;

        canvas.SetActive(true);

        question.text = questions[currentQuestionIndex].question;
        answer1.text = questions[currentQuestionIndex].answers[0];
        answer2.text = questions[currentQuestionIndex].answers[1];
        answer3.text = questions[currentQuestionIndex].answers[2];
        answer4.text = questions[currentQuestionIndex].answers[3];

        currentQuestionIndex++;
    }
    public void ChooseAnswer(int answerIndex)
    {
        timeAnswered = DateTime.Now;
        TimeSpan timeDifference = timeAnswered - timePopped;
        experimentManager.AddSaAnswer(answerIndex,(float)timeDifference.TotalSeconds);

        canvas.SetActive(false);
        Time.timeScale = 1;
    }

    private void SetSecondSession()
    {
        paint1.GetComponent<Renderer>().material = purpleMaterial;
        cube.GetComponent<Renderer>().material = yellowMaterial;
        paint2.GetComponent<Renderer>().material = blueMaterial;
        sphere.GetComponent<Renderer>().material = redMaterial;

        questions[0].answers[0] = "Purple"; // answer
        questions[0].answers[1] = "Green";
        questions[0].answers[2] = "Yellow";
        questions[0].answers[3] = "Blue";

        questions[1].answers[0] = "Green";
        questions[1].answers[1] = "Red";
        questions[1].answers[2] = "Yellow"; // answer
        questions[1].answers[3] = "Purple";

        questions[2].answers[0] = "Red";
        questions[2].answers[1] = "Green";
        questions[2].answers[2] = "Blue"; // answer
        questions[2].answers[3] = "Orange";

        questions[3].answers[0] = "Red"; // answer
        questions[3].answers[1] = "Blue";
        questions[3].answers[2] = "Purple";
        questions[3].answers[3] = "Yellow";

        currentQuestionIndex = 0;



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
            else if (session == 1) SetSecondSession();
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
