using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathTarget : MonoBehaviour
{
    private GameObject Agent;
    private NavMeshAgent navMeshAgent;
    private LineRenderer lineRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        Agent = GameObject.Find("Robot");
        navMeshAgent = Agent.GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        lineRenderer.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.hasPath)
        {
            DrawPath();
        }
        else if (!navMeshAgent.hasPath && lineRenderer.positionCount != 0)
        {
            lineRenderer.positionCount = 0;
        }
    }
    void LineSetup()
    {
        // need to be fixed 
        /*Color c1 = Color.white;
        Color c2 = new Color(1, 1, 1, 0);
        lineRenderer.startColor = c1;
        lineRenderer.endColor = c1;*/
        lineRenderer.startWidth = 0.4f;
        lineRenderer.endWidth = 0.4f;
        lineRenderer.positionCount = 0;
    }
    void DrawPath()
    {
        lineRenderer.positionCount = navMeshAgent.path.corners.Length;
        lineRenderer.SetPosition(0, Agent.transform.position);

        if (lineRenderer.positionCount < 2)
        {
            return;
        }

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 pointPosition = new Vector3(navMeshAgent.path.corners[i].x, 1.06f, navMeshAgent.path.corners[i].z);
            lineRenderer.SetPosition(i, pointPosition);
        }
    }


}
