using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TutorialManager tutorialManager;

    public List<Dialogue> dialogues;
    public TextMeshProUGUI subtitle;

    [SerializeField] private Queue<string> sentences;
    [SerializeField] private int currentAct;

    void Start()
    {
        foreach (Transform child in this.transform) { dialogues.Add(child.gameObject.GetComponent<Dialogue>()); }
        sentences = new Queue<string>();
    }
    
    public void StartDialogue(int index)
    {
        currentAct = index;
        sentences.Clear();

        foreach (string sentence in dialogues[currentAct].sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0) { 
            tutorialManager.NextAct();
            return;
        }

        string sentence = sentences.Dequeue();
        subtitle.text = sentence;

        if (currentAct == 0 && sentences.Count == 0) tutorialManager.clickHere.SetActive(false);
        else if (currentAct == 1 && sentences.Count == 0) tutorialManager.WaitForAction();
        else if (currentAct == 6 && sentences.Count == 0) tutorialManager.ShowLoa();
        else if (currentAct == 7 && sentences.Count == 1) tutorialManager.SetDestination();
        else if (currentAct == 7 && sentences.Count == 0) {tutorialManager.robotScript.CanSwitchLOA(true); tutorialManager.WaitForAction(); }
        else if (currentAct == 8 && sentences.Count == 0) tutorialManager.continue_go.SetActive(true);
        else if (currentAct == 9 && sentences.Count == 2) {tutorialManager.robotScript.CanSwitchLOA(true); tutorialManager.WaitForAction(); }
        else if (currentAct == 9 && sentences.Count == 1) { tutorialManager.continue_go.SetActive(true); tutorialManager.robotScript.CanSwitchLOA(false); }
        else if (currentAct == 9 && sentences.Count == 0) { StartCoroutine(tutorialManager.SetHiObstacle1()); }
        else if (currentAct == 10 && sentences.Count == 0) { tutorialManager.continue_go.SetActive(true); }
        else if (currentAct == 11 && sentences.Count == 3) StartCoroutine(tutorialManager.SetHiObstacle2(0));
        else if (currentAct == 11 && sentences.Count == 2) tutorialManager.continue_go.SetActive(true); 
        else if (currentAct == 12 && sentences.Count == 3) tutorialManager.MoveToActTwelve();
        else if (currentAct == 12 && sentences.Count == 2) tutorialManager.SetActTwelve();
        else if (currentAct == 13 && sentences.Count == 0)
        {
            tutorialManager.continue_go.SetActive(false);
            tutorialManager.endButton.SetActive(true);
        }
    }
}
