using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubeState : IChangeState
{
    public void OnProgress()
    {
        //progress
        //DataBase.Instance.stateIndex++;
        //Debug.Log(DataBase.Instance.stateIndex);
    }

    public void OnExitState()
    {
    }
}
