// -----------------------------------------------------------------------
// This file is part of MGameClient 
//
// (c) lichenyu02   (2025/12/2 17:9:23)
// 
// For the full copyright and license information, please view the LICENSE
// file that was distributed with this source code.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Terrain/TerrainData"), fileName = ("NewTerrainData"))]
public class TerrainDataSO : ScriptableObject
{
    [Header("资源配置")]
    public int rows;
    public int columns;
    public int layers;
    public float spacing;

    [Header("地形状态数据")]
    public List<bool> isActiveList = new List<bool>();
    public List<bool> isSlopeList = new List<bool>();
    public List<float> slopeRotation = new List<float>();
    public List<bool> isCliffList = new List<bool>();
    public List<float> cliffRotation = new List<float>();

    [Header("元数据")]
    public string saveTime;
    public int version = 1;
}
