using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public interface IChangeState
{
    void OnProgress();
    void OnExitState();
}

public class StateMgr : MonoBehaviour
{
    public Button nextBtn;
    private DataState dataState;
    private IChangeState currentState;
    private bool isStateMachineRunning;

    private void Start()
    {
        Init();
        nextBtn.onClick.AddListener(OnClick);
        //StartCoroutine(StateMachineCoroutine());
    }

    private void OnClick()
    {
        if (isStateMachineRunning)
        {
            currentState.OnProgress();
            currentState.OnExitState();

            if (!ChangeState())
            {
                isStateMachineRunning = false;
            }
        }

    }

    private void Init()
    {
        isStateMachineRunning = true;
        dataState = DataState.Origin;
        currentState = SetState(dataState);
    }

    private IEnumerator StateMachineCoroutine()
    {
        while (isStateMachineRunning)
        {
            if (currentState != null)
            {
                currentState.OnProgress();
                yield return null;
                currentState.OnExitState();
            }

            if (!ChangeState())
            {
                isStateMachineRunning = false;
                yield break;
            }
        }
        yield return null;
    }

    /// <summary>
    /// 状态切换规则
    /// </summary>
    /// <param name="dataState"></param>
    private bool ChangeState()
    {
        switch (dataState)
        {
            case DataState.Origin:
                dataState = DataState.MeshGenerate;
                break;
            case DataState.MeshGenerate:
                dataState = DataState.MarchingCube;
                break;
            default:
                return false;
        }
        currentState = SetState(dataState);
        return true;
    }

    private IChangeState SetState(DataState dataState)
    {
        switch (dataState)
        {
            case DataState.Origin:
                return new OriginState();
            case DataState.MeshGenerate:
                return new MeshGeneratingState();
            case DataState.MarchingCube:
                return new MarchingCubeState();
            default:
                Debug.LogError("无法设置状态!");
                return null;
        }
    }
}

public enum DataState
{
    Origin,
    MeshGenerate,
    MarchingCube
}