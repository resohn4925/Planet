using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;
using System.Drawing;
using static MarchingCube.MarchingCubeData;
using static MarchingCube;


namespace Utility
{
    public class UCalculate
    {
        private static readonly Vector2Int[] Directions = new Vector2Int[]
        {
        new Vector2Int(0, 1), new Vector2Int(0, -1),   // 上, 下
        new Vector2Int(-1, 0), new Vector2Int(1, 0),   // 左, 右
        new Vector2Int(-1, 1), new Vector2Int(1, 1),   // 左上, 右上
        new Vector2Int(-1, -1), new Vector2Int(1, -1)  // 左下, 右下
        };

        #region 计算周围点
        /// <summary>
        /// 计算周围点
        /// </summary>
        /// <param name="pointPos"></param>
        /// <param name="face"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static List<Vector3> CalculateSurroundPoint(Vector3 pointPos, Face face, int size)
        {
            List<Vector3> results = new List<Vector3>();
            foreach (var dir in Directions)
            {
                results.Add(GetWrappedPoint((int)pointPos.x + dir.x, (int)pointPos.y + dir.y, face, size));
            }
            return results;
        }

        private static Vector3 GetWrappedPoint(int x, int y, Face face, int size)
        {
            // 1. 正常范围
            if (x >= 1 && x <= size && y >= 1 && y <= size)
                return new Vector3(x, y, (int)face);

            // 在边角
            if (x < 1 && y > size) return HandleCorner(face, "TopLeft", size);
            if (x > size && y > size) return HandleCorner(face, "TopRight", size);
            if (x < 1 && y < 1) return HandleCorner(face, "BottomLeft", size);
            if (x > size && y < 1) return HandleCorner(face, "BottomRight", size);

            // 3. 处理四条边 (Edge cases)
            return HandleEdge(x, y, face, size);
        }

        private static Vector3 HandleCorner(Face face, string corner, int size)
        {
            // corner 参数：TopLeft, TopRight, BottomLeft, BottomRight
            // 返回值：Vector3(nx, ny, (int)nextFace)

            switch (face)
            {
                case Face.Front:
                    if (corner == "TopLeft") return new Vector3(1, 1, (int)Face.Up);      // 进到 Top 的左下
                    if (corner == "TopRight") return new Vector3(size, 1, (int)Face.Up);   // 进到 Top 的右下
                    if (corner == "BottomLeft") return new Vector3(1, size, (int)Face.Down); // 进到 Down 的左上
                    if (corner == "BottomRight") return new Vector3(size, size, (int)Face.Down); // 进到 Down 的右上
                    break;

                case Face.Back:
                    // Back 面的 X 轴与 Front 相反
                    if (corner == "TopLeft") return new Vector3(size, size, (int)Face.Up);
                    if (corner == "TopRight") return new Vector3(1, size, (int)Face.Up);
                    if (corner == "BottomLeft") return new Vector3(size, 1, (int)Face.Down);
                    if (corner == "BottomRight") return new Vector3(1, 1, (int)Face.Down);
                    break;

                case Face.Left:
                    if (corner == "TopLeft") return new Vector3(1, size, (int)Face.Up);   // Up 面的左侧
                    if (corner == "TopRight") return new Vector3(1, 1, (int)Face.Up);      // Up 面的左下
                    if (corner == "BottomLeft") return new Vector3(1, 1, (int)Face.Down);    // Down 面的左下
                    if (corner == "BottomRight") return new Vector3(1, size, (int)Face.Down); // Down 面的左上
                    break;

                case Face.Right:
                    if (corner == "TopLeft") return new Vector3(size, 1, (int)Face.Up);   // Up 面的右下
                    if (corner == "TopRight") return new Vector3(size, size, (int)Face.Up); // Up 面的右上
                    if (corner == "BottomLeft") return new Vector3(size, size, (int)Face.Down); // Down 面的右上
                    if (corner == "BottomRight") return new Vector3(size, 1, (int)Face.Down);    // Down 面的右下
                    break;

                case Face.Up:
                    if (corner == "TopLeft") return new Vector3(size, size, (int)Face.Back);
                    if (corner == "TopRight") return new Vector3(1, size, (int)Face.Back);
                    if (corner == "BottomLeft") return new Vector3(1, size, (int)Face.Front);
                    if (corner == "BottomRight") return new Vector3(size, size, (int)Face.Front);
                    break;

                case Face.Down:
                    if (corner == "TopLeft") return new Vector3(1, 1, (int)Face.Front);
                    if (corner == "TopRight") return new Vector3(size, 1, (int)Face.Front);
                    if (corner == "BottomLeft") return new Vector3(size, 1, (int)Face.Back);
                    if (corner == "BottomRight") return new Vector3(1, 1, (int)Face.Back);
                    break;
            }

            // 容错处理：返回当前面中心点（不应触发）
            return new Vector3(1, 1, (int)face);
        }

        private static Vector3 HandleEdge(int x, int y, Face face, int size)
        {
            int nx = x;
            int ny = y;
            Face nextFace = face;

            // --- 向上溢出 ---
            if (y > size)
            {
                ny = 1; // 默认进入邻居面的底部
                switch (face)
                {
                    case Face.Front: nextFace = Face.Up; nx = x; ny = 1; break;
                    case Face.Back: nextFace = Face.Up; nx = size - x + 1; ny = size; break; // 翻转进入Top顶部
                    case Face.Left: nextFace = Face.Up; nx = 1; ny = x; break;              // 坐标轴转换
                    case Face.Right: nextFace = Face.Up; nx = size; ny = size - x + 1; break; // 坐标轴转换
                    case Face.Up: nextFace = Face.Back; nx = size - x + 1; ny = size; break; // 翻转进入Back顶部
                    case Face.Down: nextFace = Face.Front; nx = x; ny = 1; break;
                }
            }
            // --- 向下溢出 ---
            else if (y < 1)
            {
                ny = size; // 默认进入邻居面的顶部
                switch (face)
                {
                    case Face.Front: nextFace = Face.Down; nx = x; ny = size; break;
                    case Face.Back: nextFace = Face.Down; nx = size - x + 1; ny = 1; break;    // 翻转进入Bottom底部
                    case Face.Left: nextFace = Face.Down; nx = 1; ny = size - x + 1; break;
                    case Face.Right: nextFace = Face.Down; nx = size; ny = x; break;
                    case Face.Up: nextFace = Face.Front; nx = x; ny = size; break;
                    case Face.Down: nextFace = Face.Back; nx = size - x + 1; ny = 1; break;
                }
            }
            // --- 向左溢出 ---
            else if (x < 1)
            {
                nx = size; // 默认进入邻居面的右侧
                switch (face)
                {
                    case Face.Front: nextFace = Face.Left; nx = size; ny = y; break;
                    case Face.Back: nextFace = Face.Right; nx = size; ny = y; break;
                    case Face.Left: nextFace = Face.Back; nx = size; ny = y; break;
                    case Face.Right: nextFace = Face.Front; nx = size; ny = y; break;
                    case Face.Up: nextFace = Face.Left; nx = y; ny = size; break;            // 侧跳
                    case Face.Down: nextFace = Face.Left; nx = size - y + 1; ny = 1; break;    // 侧跳
                }
            }
            // --- 向右溢出 ---
            else if (x > size)
            {
                nx = 1; // 默认进入邻居面的左侧
                switch (face)
                {
                    case Face.Front: nextFace = Face.Right; nx = 1; ny = y; break;
                    case Face.Back: nextFace = Face.Left; nx = 1; ny = y; break;
                    case Face.Left: nextFace = Face.Front; nx = 1; ny = y; break;
                    case Face.Right: nextFace = Face.Back; nx = 1; ny = y; break;
                    case Face.Up: nextFace = Face.Right; nx = size - y + 1; ny = size; break;
                    case Face.Down: nextFace = Face.Right; nx = y; ny = 1; break;
                }
            }

            return new Vector3(nx, ny, (int)nextFace);
        }
        #endregion

        #region 计算桥梁点
        /// <summary>
        /// 计算角点的桥接虚点
        /// </summary>
        /// <param name="point">当前点的坐标 (x, y, z)</param>
        /// <param name="face">当前面枚举</param>
        /// <param name="marchingCubeDatas">所有面的数据集合</param>
        /// <param name="size">建筑区大小 (通常为 3)</param>
        public static List<Vector3> CalculateBridge(Vector3 point, Face face, MarchingCubeData[] marchingCubeDatas, int size)
        {
            List<Vector3> results = new List<Vector3>();

            int x = (int)point.x; // 平面 X (1-3)
            int y = (int)point.y; // 平面 Y (1-3)
            int z = (int)point.z; // 深度 Z

            // 1. 识别角点，定义探测方向（针对平面 x, y）
            Vector2Int dirX = Vector2Int.zero; 
            Vector2Int dirY = Vector2Int.zero;
            
            // 判定是否在 3x3 的四个角点（1,1 / 1,3 / 3,1 / 3,3）
            if (x == 1 && y == size)         { dirX = new Vector2Int(-1, 0); dirY = new Vector2Int(0, 1);  } // 左上
            else if (x == size && y == size) { dirX = new Vector2Int(1, 0);  dirY = new Vector2Int(0, 1);  } // 右上
            else if (x == 1 && y == 1)       { dirX = new Vector2Int(-1, 0); dirY = new Vector2Int(0, -1); } // 左下
            else if (x == size && y == 1)    { dirX = new Vector2Int(1, 0);  dirY = new Vector2Int(0, -1); } // 右下
            else return results; // 非角点不产生桥接

            // 2. 跨面探测：获取邻居面在有效建筑区 (1-3) 内的实点信息
            // HandleEdge 内部逻辑：输入溢出坐标，返回 (邻居面nx, 邻居面ny, 邻居面Face)
            Vector3 n1 = HandleEdge(x + dirX.x, y + dirX.y, face, size);
            Vector3 n2 = HandleEdge(x + dirY.x, y + dirY.y, face, size);

            // 3. 检查邻居面的数据状态 (isActive)
            // 传入 z 保持深度一致
            bool active1 = GetIsActiveFromGlobal(n1, z, marchingCubeDatas, size);
            bool active2 = GetIsActiveFromGlobal(n2, z, marchingCubeDatas, size);

            // 4. 填充结果：返回当前面坐标系下的“虚点”坐标
            // 这些点的坐标值会包含 0 或 4 (size+1)，用于 Marching Cube 闭合网格
            if (active1)
            {
                // 添加 X 方向溢出的虚点
                results.Add(new Vector3(x + dirX.x, y + dirX.y, z));
            }

            if (active2)
            {
                // 添加 Y 方向溢出的虚点
                results.Add(new Vector3(x + dirY.x, y + dirY.y, z));
            }

            // 5. 对角线桥接：如果两个邻居面边缘都激活，则添加对角线位置的虚点 (例如 0, 4, z)
            if (active1 && active2)
            {
                results.Add(new Vector3(x + dirX.x, y + dirY.y, z));
            }

            return results;
        }

        /// <summary>
        /// 从全局面数据中获取 isActive 状态
        /// </summary>
        private static bool GetIsActiveFromGlobal(Vector3 info, int z, MarchingCubeData[] datas, int size)
        {
            int ix = (int)info.x;
            int iy = (int)info.y;
            int faceEnumVal = (int)info.z;
            
            // 获取对应面的数据（Face 枚举从 1 开始，数组索引从 0 开始）
            var faceData = datas[faceEnumVal - 1];
            
            // 检查转换后的坐标是否落在邻居面的 3x3 有效建筑区内
            if (ix >= 1 && ix <= size && iy >= 1 && iy <= size)
            {
                // 数组索引：[x, y, z]
                return faceData.objPointArray[ix, iy, z].isActive;
            }
            return false;
        }
        
        #endregion
    }
}