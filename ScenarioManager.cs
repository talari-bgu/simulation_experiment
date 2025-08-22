using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScenarioManager : MonoBehaviour
{
    [Header("Side")]
    public GameObject sideA;
    public GameObject sideB;


    [Header("Corridor")]
    public HumanoidController remy1;
    public Transform remy1_goal1;
    public Transform remy1_goal2;
    public Transform remy1_goal2_rotation;
    public Transform remy1_goal3_sp;

    public HumanoidController leonard1;
    public Transform leonard1_goal;
    public Transform leonard1_sp;

    public HumanoidController joe1;
    public Transform joe1_goal1;
    public Transform joe1_goal1_rotation;
    public Transform joe1_goal2;
    public Transform joe1_goal3_sp;

    public HumanoidController kate1;
    public Transform kate1_goal1;
    public Transform kate1_goal2_sp;

    [Header("Brown room")]
    public HumanoidController remy2;
    public Transform remy2_goal1;
    public Transform remy2_goal2;
    public Transform remy2_goal3;
    public Transform remy2_goal4_sp;

    public HumanoidController josh1;
    public Transform josh1_goal1;
    public Transform josh1_goal2;
    public Transform josh1_goal3;
    public Transform josh1_goal4_sp;
    
    [Header("White room")]
    public HumanoidController joe2;
    public Transform joe2_goal1;
    public Transform joe2_goal2;
    public Transform joe2_goal3_sp;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            TriggerActivation("Trigger2");
        }
    }
    public void TriggerActivation(string triggerName)
    {
        switch (triggerName)
        {
            case "Trigger1":
                remy1.SetDestenation(remy1_goal1);
                leonard1.SetDestenation(leonard1_goal);
                break;

            case "Trigger2":
                //joe1.SetDestenation(joe1_goal1);
                break;
            
            case "Trigger3":
                // cant see them from here so will be actived
                josh1.gameObject.SetActive(true);
                kate1.gameObject.SetActive(true);

                // setup him for next session
                leonard1.SetDestenation(leonard1_sp);

                // right before obstacle two
                joe1.SetDestenation(joe1_goal1);
                break;

            case "Trigger4":
                // after passing joe1
                joe1.SetDestenation(joe1_goal2);
                break;

            case "Trigger5":
                // this is called from the script
                remy2.SetDestenation(remy2_goal1);
                break;

            case "Trigger6":
                // sphere collider with radius 6 of the door to exit brown room
                josh1.SetDestenation(josh1_goal1);              
                break;
            
            case "Trigger7":
                // right before the door to exit brown room
                kate1.SetDestenation(kate1_goal1);              
                break;
            
            case "Trigger8":
                // before the door to enter white room
                joe2.SetDestenation(joe2_goal1);
                break;
            
            // SideB activate

            case "Trigger9":
                // after inspecting middle waypoint
                joe2.SetDestenation(joe2_goal2);
                break;
            
            case "Trigger10":
                // before entering brown room

                // need to setup kate1 for next session
                kate1.SetDestenation(kate1_goal2_sp);
                joe2.SetDestenation(joe2_goal3_sp);
                
                josh1.SetDestenation(josh1_goal3);
                remy2.SetDestenation(remy2_goal3);
                break;
            
            case "Trigger11":
                // before left turn to the corridor

                // need to setup josh1 for next session
                josh1.SetDestenation(josh1_goal4_sp);

                joe1.SetDestenation(joe1_goal3_sp);
                break;            
            
            case "Trigger12":
                // next to toilet
                remy1.SetDestenation(remy1_goal2);
                break;

            case "Trigger13":
                remy1.SetDestenation(remy1_goal3_sp);
                break;

            default:
                break;

        }
    }

    public void HumanoidReached(string humanoidName, Transform goal_reached)
    {
        switch (humanoidName)
        {

            case "Joe1":
                if (goal_reached == joe1_goal1) joe1.SetDestenation(joe1_goal1_rotation);
                break;

            case "Remy2":
                if (goal_reached == remy2_goal1) remy2.SetDestenation(remy2_goal2);
                else if (goal_reached == remy2_goal3) remy2.SetDestenation(remy2_goal4_sp);
                
                break;

            case "Kate1":
                if (goal_reached == kate1_goal2_sp) kate1.gameObject.SetActive(false);
                break;

            case "Josh1":
                if (goal_reached == josh1_goal1) josh1.SetDestenation(josh1_goal2);
                else if (goal_reached == josh1_goal4_sp) josh1.gameObject.SetActive(false);
                break;

            case "Remy1":
                if (goal_reached == remy1_goal2) remy1.SetDestenation(remy1_goal2_rotation);
                break;


            default:
                Debug.Log("Method HumanoidReached in ScenarioManager failed. name: " + humanoidName);
                break;
        }
    }

    public void SetPath(int i)
    {
        if (i == 0)
        {
            sideB.SetActive(false);
            sideA.SetActive(true);
        }
        else if (i == 1)
        {
            sideA.SetActive(false);
            sideB.SetActive(true);

            // might be not fast enough to reach
            joe2.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            joe2.gameObject.transform.position = joe2_goal1.position;
            joe2.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
