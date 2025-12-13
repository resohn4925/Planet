using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginState : IChangeState
{
    /// <summary>
    /// 初始化database
    /// </summary>
    public void OnProgress()
    {
        //progress
        //DataBase.Instance.stateIndex = 0;

    }

    public void OnExitState()
    {
    }
}