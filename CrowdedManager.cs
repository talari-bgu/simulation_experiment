using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrowdedManager : MonoBehaviour
{
    [Header("Animators")]
    public RuntimeAnimatorController foldering;
    public RuntimeAnimatorController humanoid;

    [Header("Corridor")]
    public CrowdedController lewis1;
    public Transform lewis1_goal1;
    public Transform lewis1_goal1_rotation;
    public Transform lewis1_goal2;
    public Transform lewis1_goal2_rotation;
    [Header("")]
    public CrowdedController remy1;
    public Transform remy1_goal1;
    public Transform remy1_goal2;
    [Header("")]
    public CrowdedController lewis2;
    public Transform lewis2_goal1;
    public Transform lewis2_goal2;
    [Header("")]
    public CrowdedController megan1;
    public Transform megan1_goal1;
    public Transform megan1_goal2;
    public Transform megan1_goal3;
    [Header("")]
    public CrowdedController louise1;
    public Transform louise1_goal1;
    public Transform louise1_goal2;
    public Transform louise1_goal3;
    [Header("")]
    public CrowdedController leonard1;
    public Transform leonard1_goal1;
    public Transform leonard1_goal2;
    public Transform leonard1_goal3;
    public Transform leonard1_goal3_rotation;



    public void StartScene()
    {
        lewis1.SetDestenation(lewis1_goal1);
        remy1.SetDestenation(remy1_goal1);
        lewis2.SetDestenation(lewis2_goal1);
        megan1.SetDestenation(megan1_goal1);
        louise1.SetDestenation(louise1_goal1);
        leonard1.SetDestenation(leonard1_goal1);
    }

    public void HumanoidReached(string humanoidName, Transform goal_reached)
    {
        switch (humanoidName)
        {

            case "Lewis1":
                if (goal_reached == lewis1_goal1)  lewis1.SetDestenation(lewis1_goal1_rotation); 
                else if (goal_reached == lewis1_goal1_rotation)  StartCoroutine(DoActionAndMove(lewis1, foldering, 5, lewis1_goal2)); 
                else if (goal_reached == lewis1_goal2)  lewis1.SetDestenation(lewis1_goal2_rotation); 
                else if (goal_reached == lewis1_goal2_rotation)  StartCoroutine(DoActionAndMove(lewis1, foldering, 5, lewis1_goal1)); 
                break;

            case "Remy1":
                if (goal_reached == remy1_goal1)  StartCoroutine(DoActionAndMove(remy1, foldering, 4, remy1_goal2));
                else if (goal_reached == remy1_goal2) StartCoroutine(DoActionAndMove(remy1, humanoid, 4, remy1_goal1));
                break;

            case "Lewis2":
                if (goal_reached == lewis2_goal1) StartCoroutine(DoActionAndMove(lewis2, humanoid, 4, lewis2_goal2));
                else if (goal_reached == lewis2_goal2) StartCoroutine(DoActionAndMove(lewis2, humanoid, 4, lewis2_goal1));
                break;

            case "Megan1":
                if (goal_reached == megan1_goal1) megan1.SetDestenation(megan1_goal2);
                else if (goal_reached == megan1_goal2) megan1.SetDestenation(megan1_goal3);
                else if (goal_reached == megan1_goal3) megan1.SetDestenation(megan1_goal1);
                break;

            case "Louise1":
                if (goal_reached == louise1_goal1) louise1.SetDestenation(louise1_goal2);
                else if (goal_reached == louise1_goal2) louise1.SetDestenation(louise1_goal3);
                else if (goal_reached == louise1_goal3) StartCoroutine(DoActionAndMove(louise1, foldering, 5, louise1_goal1));
                break;
            case "Leonard1":
                if (goal_reached == leonard1_goal1) leonard1.SetDestenation(leonard1_goal2);
                else if (goal_reached == leonard1_goal2) leonard1.SetDestenation(leonard1_goal3);
                else if (goal_reached == leonard1_goal3) leonard1.SetDestenation(leonard1_goal3_rotation);
                else if (goal_reached == leonard1_goal3_rotation) StartCoroutine(DoActionAndMove(leonard1, foldering, 5, leonard1_goal1));
                break;

            default:
                Debug.Log("Method HumanoidReached in CrowdedManager failed. name: " + humanoidName);
                break;
        }
    }

    private IEnumerator DoActionAndMove(CrowdedController character, RuntimeAnimatorController animator, float time, Transform goal)
    { 
        if (animator != humanoid) 
        { 
            character.gameObject.GetComponent<Animator>().runtimeAnimatorController = animator;
            character.isHumanoidAnimator = false;
        }
        
        yield return new WaitForSeconds(time);

        character.gameObject.GetComponent<Animator>().runtimeAnimatorController = humanoid;
        character.SetDestenation(goal);

    }

}
