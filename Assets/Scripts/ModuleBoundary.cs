using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleBoundary : MonoBehaviour
{
    List<Vector3> posList = new List<Vector3>();

    private void OnDrawGizmos()
    {
        if(posList.Count == 0)
        {
            posList.Add(new Vector3(this.transform.position.x - 2.5f, this.transform.position.y - 2.5f, this.transform.position.z - 2.5f));
            posList.Add(new Vector3(this.transform.position.x - 2.5f, this.transform.position.y - 2.5f, this.transform.position.z + 2.5f));
            posList.Add(new Vector3(this.transform.position.x + 2.5f, this.transform.position.y - 2.5f, this.transform.position.z + 2.5f));
            posList.Add(new Vector3(this.transform.position.x + 2.5f, this.transform.position.y - 2.5f, this.transform.position.z - 2.5f));
            posList.Add(new Vector3(this.transform.position.x - 2.5f, this.transform.position.y + 2.5f, this.transform.position.z - 2.5f));
            posList.Add(new Vector3(this.transform.position.x - 2.5f, this.transform.position.y + 2.5f, this.transform.position.z + 2.5f));
            posList.Add(new Vector3(this.transform.position.x + 2.5f, this.transform.position.y + 2.5f, this.transform.position.z + 2.5f));
            posList.Add(new Vector3(this.transform.position.x + 2.5f, this.transform.position.y + 2.5f, this.transform.position.z - 2.5f));
        }

        Gizmos.color = Color.yellow;
        foreach (Vector3 pos in posList)
        {
            Gizmos.DrawSphere(pos, 0.2f);
        }

        Gizmos.color = Color.white;
        int[,] edges = new int[12, 2]
       {
            {0, 1}, {1, 2}, {2, 3}, {3, 0},
            {4, 5}, {5, 6}, {6, 7}, {7, 4},
            {0, 4}, {1, 5}, {2, 6}, {3, 7}
       };

        for (int i = 0; i < 12; i++)
        {
            Gizmos.DrawLine(posList[edges[i, 0]], posList[edges[i, 1]]);
        }
    }
}
