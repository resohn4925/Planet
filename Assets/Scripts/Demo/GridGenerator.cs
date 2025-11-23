using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private List<Vector3> posLists = new();

    private int gridSize = 3;

    public void Init()
    {
        for(int x = 0; x < 4; x++)
        {
            for(int y = 0; y < 3; y++)
            {
                Vector3 pos = new Vector3(x, 0, y);
                posLists.Add(pos);
            }
        }
    }

    public bool TryGetGridPosition(Vector3 worldPosition, out Vector3 gridCenter)
    {
        gridCenter = Vector3.zero;

        int closestX = Mathf.RoundToInt(worldPosition.x);
        int closestZ = Mathf.RoundToInt(worldPosition.z);

        if (closestX >= 0 && closestX < gridSize && closestZ >= 0 && closestZ < gridSize)
        {
            gridCenter = new Vector3(closestX, 0, closestZ);
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;

        foreach(Vector3 pos in posLists)
        {
            Gizmos.DrawSphere(pos, 0.05f);
        }
    }
}
