using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class CrowdedController : MonoBehaviour
{
    public CrowdedManager crowdedManager;

    private NavMeshAgent navMeshAgent;
    private ThirdPersonCharacter character;

    private Transform current_goal;

    public bool isHumanoidAnimator;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;

        character = gameObject.GetComponent<ThirdPersonCharacter>();
        
        isHumanoidAnimator = true;


    }
    private void Update()
    {
        if (isHumanoidAnimator)
        {
            character.Move(navMeshAgent.desiredVelocity, false, false);
        }
        
        if (navMeshAgent.hasPath && Vector3.Distance(navMeshAgent.destination, transform.position) <= 0.05)
        {
            ClearNavigation();
        }
    }

    public void SetDestenation(Transform goal)
    {
        isHumanoidAnimator = true;
        current_goal = goal;
        navMeshAgent.SetDestination(goal.position);
    }

    private void ClearNavigation()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
        crowdedManager.HumanoidReached(this.name, this.current_goal);
    }
}
