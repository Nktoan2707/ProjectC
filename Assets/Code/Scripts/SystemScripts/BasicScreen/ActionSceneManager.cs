using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionSceneManager : MonoBehaviour
{

    public static List<ActionModel> initActionList = new List<ActionModel>();
    public static GameLevelSO currentGameLevelSO;


    [SerializeField] GameObject actionList;
    [SerializeField] GameObject actionQueue;

    private void Awake()
    {
        if (currentGameLevelSO == null)
        {
            Debug.LogError("Missing GameLevelSO!");
        }
    }


    private void Start()
    {
        LoadLevelActions();
        PlayScene(currentGameLevelSO.sceneName);
    }

    private void LoadLevelActions()
    {
        foreach (ActionName actionName in currentGameLevelSO.actionNameList)
        {
            initActionList.Add(new ActionModel(actionName));
        }
        actionList.GetComponent<ActionListManager>().updateActionList();
        actionQueue.GetComponent<ActionQueueManager>().updateActionQueue();
    }

    public static void PlayScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public static void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
