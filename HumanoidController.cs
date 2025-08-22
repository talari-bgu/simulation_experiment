using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class HumanoidController : MonoBehaviour
{
    public ScenarioManager scenarioManager;

    private NavMeshAgent navMeshAgent;
    private ThirdPersonCharacter character;

    private Transform current_goal;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;

        character = gameObject.GetComponent<ThirdPersonCharacter>();
    }
    private void Update()
    {
        character.Move(navMeshAgent.desiredVelocity, false, false);

        if (navMeshAgent.hasPath && Vector3.Distance(navMeshAgent.destination, transform.position) <= 0.05)
        {
            ClearNavigation();
        }
    }

    public void SetDestenation(Transform goal)
    {
        current_goal = goal;
        navMeshAgent.SetDestination(goal.position);
    }

    private void ClearNavigation()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
        scenarioManager.HumanoidReached(this.name, this.current_goal);
    }
}
