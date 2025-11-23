using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGeneratingState : IChangeState
{
    private Mesh mesh;

    public void OnProgress()
    {
        //progress
        //DataBase.Instance.stateIndex++;
    }

    public void OnExitState()
    {
    }
}