using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface Info { }

public class SystemInfo : Info
{
    public string currentTimeString = "00:02:00";
    public int pause = 0; // 0 means continue, 1 means pause
    public string pauseShow = "Paused";
}

public class ResourcesInfo : Info
{
    public int attackMoney = 100;
    public int defenseMoney = 200;
    public int defenseLife = 10;
    public int remainingTowers = 8;
    public const int maxAttackMoney = 400;
}

public class StatisticsInfo : Info
{
    public int attackMoneyRate = 90;
    public float attackMoneyIncreasePeriod = 5f;
}

public class TutorialInfo : Info
{
    public bool continueSpawn = false;
    public bool towerPlaceable = false;

}

public class GameVariables : MonoBehaviour
{
    public SystemInfo systemInfo;
    public ResourcesInfo resourcesInfo;
    public StatisticsInfo statisticsInfo;
    public TutorialInfo tutorialInfo;

    public static string attackerName = "";
    public static string defenderName = "";

    private GameObject systems;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetValues();
    }

    private void ResetValues()
    {
        systemInfo = new SystemInfo();
        resourcesInfo = new ResourcesInfo();
        statisticsInfo = new StatisticsInfo();
        tutorialInfo = new TutorialInfo();

        systems = GameObject.Find("IndependentSystems");
        if (systems != null)
        {
            systems.GetComponent<TimeSystem>().Init();
            // systems.GetComponent<MapSystem>().Init();
            // systems.GetComponent<ConstructionSystem>().Init();
        }
        else
        {
            Debug.LogError("IndependentSystems GameObject not found!");
        }
    }

    // Method to dynamically get a variable by name
    public KeyValuePair<Info, FieldInfo>? GetVariable(string variableName)
    {
        FieldInfo field;

        field = systemInfo.GetType().GetField(variableName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
            return new KeyValuePair<Info, FieldInfo>(systemInfo, field);

        field = resourcesInfo.GetType().GetField(variableName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
            return new KeyValuePair<Info, FieldInfo>(resourcesInfo, field);

        field = statisticsInfo.GetType().GetField(variableName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
            return new KeyValuePair<Info, FieldInfo>(statisticsInfo, field);

        return null;
    }
}
