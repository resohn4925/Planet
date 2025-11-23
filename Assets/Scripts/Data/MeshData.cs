using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeshData
{
    public CubeFace faceType;
    public List<Vector3> vertices = new List<Vector3>();//顶点世界坐标
    public List<Vector3> normals = new List<Vector3>();//法线存储
    public List<Vector2> uvs = new List<Vector2>();//顶点归一化平面坐标
    public List<int> triangles = new List<int>();

    public MeshData(CubeFace faceType)
    {
        this.faceType = faceType;
    }

    public void ClearData()
    {
        vertices.Clear();
        normals.Clear();
        uvs.Clear();
        triangles.Clear();
    }

    public void GenerateVertices(int meshNum, float meshSize)
    {
        if (meshNum <= 0)
            return;
        int verNum = meshNum + 1;

        for (int y = 0; y < verNum; y++)
        {
            for (int x = 0; x < verNum; x++)
            {

                Vector2 uv = new Vector2(
                   x / (float)meshNum,
                   y / (float)meshNum
                    );

                Vector3 pos = new Vector3(0, 0, 0);
                pos = new Vector3(
                (uv.x - 0.5f) * meshSize * meshNum,
                0.5f * meshSize * meshNum,
                (uv.y - 0.5f) * meshSize * meshNum
                );

                //map positions to other faces
                pos = Rotation(faceType) * pos;

                vertices.Add(pos);
                normals.Add(Vector3.up);
                uvs.Add(uv);
            }
        }
    }

    public Quaternion Rotation(CubeFace faceType)
    {
        switch (faceType)
        {
            case CubeFace.Top:
                return Quaternion.Euler(180, 0, 0);
            case CubeFace.Front:
                return Quaternion.Euler(90, 0, 0);
            case CubeFace.Back:
                return Quaternion.Euler(-90, 0, 0);
            case CubeFace.Left:
                return Quaternion.Euler(0, 0, 90);
            case CubeFace.Right:
                return Quaternion.Euler(0, 0, -90);
            default:
                return Quaternion.Euler(0, 0, 0);
        }

    }

    public void Normalize(bool var, int meshNum, float meshSize, float sphereRadius)
    {
        if (!var) return;

        float halfSize = 0.5f * meshSize * meshNum;

        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 scaledVertex = vertices[i] / halfSize;//归一化坐标

            float x = scaledVertex.x;
            float y = scaledVertex.y;
            float z = scaledVertex.z;

            float x2 = x * x;
            float y2 = y * y;
            float z2 = z * z;
            float scale = 1f / Mathf.Sqrt(x2 + y2 + z2);

            float nx = x * (1f - 0.25f * (y2 + z2) + 0.125f * y2 * z2) * scale;
            float ny = y * (1f - 0.25f * (x2 + z2) + 0.125f * x2 * z2) * scale;
            float nz = z * (1f - 0.25f * (x2 + y2) + 0.125f * x2 * y2) * scale;

            Vector3 mapped = new Vector3(nx, ny, nz);
            float magnitude = mapped.magnitude;

            if (magnitude > Mathf.Epsilon)
            {
                vertices[i] = (mapped / magnitude) * sphereRadius;
                normals[i] = (mapped / magnitude);
            }
            else
            {
                vertices[i] = Vector3.up * sphereRadius;
                normals[i] = Vector3.up;
            }

            //if(faceType == CubeFace.Top)
            //{
            //    Debug.Log(vertices[i].x + ", " + vertices[i].y + ", " + vertices[i].z);
            //}
        }
    }

    public void GenerateTriangles(int meshNum)
    {
        int verNum = meshNum + 1;
        for (int y = 0; y < meshNum; y++)
        {
            for (int x = 0; x < meshNum; x++)
            {
                int current = y * verNum + x;
                int next = (y + 1) * verNum + x;

                triangles.Add(current);
                triangles.Add(next + 1);
                triangles.Add(current + 1);

                triangles.Add(current);
                triangles.Add(next);
                triangles.Add(next + 1);
            }
        }
    }

    public float Compensate(float var)
    {
        float x = 2f * var - 1f;
        float compensatedX = Mathf.Atan(1.5f * x) / Mathf.Atan(1.5f);

        return (compensatedX + 1f) / 2f;
    }
}
