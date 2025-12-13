using System.Collections.Generic;
using UnityEngine;

namespace Planet
{
    public class PlanetData
    {
        public int meshNum;
        public float planetRadius;
    }

    /// <summary>
    /// planet表面网格数据
    /// </summary>
    public class MeshData
    {
        public CubeFace faceType;
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector3> normals = new List<Vector3>();
        public List<Vector2> uvs = new List<Vector2>();
        public List<int> triangles = new List<int>();

        public MeshData(CubeFace faceType)
        {
            this.faceType = faceType;
        }
    }

    public class ModuleData
    {

    }

    public enum CubeFace
    {
        Top,
        Bottom,
        Front,
        Back,
        Left,
        Right
    }
}
