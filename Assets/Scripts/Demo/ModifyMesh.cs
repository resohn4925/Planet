using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMesh : MonoBehaviour
{
    public GameObject targetMesh;

    [Header("几何参数")]
    public float polyRadius;//六边形半径
    public float unitX;//目标点在单位正方形中的横坐标
    public float unitY;//目标点在单位正方形中的纵坐标

    [Header("顶点小球半径")]
    public float sphereRadius;//顶点小球半径

    TestMeshData testMeshData = new();

    /// <summary>
    /// 使用double插值算法把目标点的位置变换到mesh中
    /// </summary>
    public void ApplyModifyMesh()
    {
        if (targetMesh == null)
        {
            Debug.LogError("target mesh is null");
            return;
        }

        MeshFilter meshFilter = targetMesh.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("no mesh found in the target mesh.");
            return;
        }

        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            float x = vertices[i].x;

            {
                //vertices[i].x = Mathf.Sqrt(x);
                vertices[i].y = vertices[i].y / 2;
            }
        }

        Debug.Log("ready");

        mesh.vertices = vertices;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public void GenerateMesh()
    {
        testMeshData.GenerateVertices(polyRadius);
    }

    /// <summary>
    /// 获取变换后的目标点坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMappedPoint()
    {
        Vector3 mappedVertex = new Vector3();

        return mappedVertex;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, Vector3.right * polyRadius / 2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, Vector3.forward * polyRadius / 2f);

        //依次绘制顶点
        Gizmos.color = Color.yellow;
        foreach (Vector3 var in testMeshData.vertices)
        {
            Gizmos.DrawSphere(var, sphereRadius);
        }
    }
}

public class TestMeshData
{
    public List<Vector3> vertices = new List<Vector3>();//顶点世界坐标

    /// <summary>
    /// 生成一个平行四边形
    /// </summary>
    /// <param name="radius"></param>
    public void GenerateVertices(float radius)
    {
        vertices.Clear();

        //vertices.Add(new Vector3(0, 0, 0));
        //vertices.Add(new Vector3(radius / 2, radius * Mathf.Sqrt(3) / 2, 0));
        //vertices.Add(new Vector3(radius * 3 / 2, radius * Mathf.Sqrt(3) / 2, 0));
        //vertices.Add(new Vector3(radius, 0, 0));
    }
}
