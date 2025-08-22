using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class CostMapManager : MonoBehaviour
{
    public GameObject costMap0;
    public GameObject costMap1;

    public NavMeshSurface robotSurface;
    public NavMeshSurface humanSurface;


    public void BakeCostMap(int index)
    {
        if (index == 0)
        {
            costMap0.SetActive(true);
            costMap1.SetActive(false);
        }
        else if (index == 1)
        {
            costMap0.SetActive(false);
            costMap1.SetActive(true);
        }

        robotSurface.BuildNavMesh();
        humanSurface.BuildNavMesh();
    }
}
