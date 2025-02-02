using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq; // Added for scene management

public class TimeSystem : MonoBehaviour
{
    
    public float countdownRate = 1f;
    public GoogleFormSubmit googleFormSubmit;
    [SerializeField] private PopUpManager CurrencyIncreasePupup;

    private GameVariables gameVariables;
    private Calculation calculation;
    public TimeSpan remainingTime;
    private bool isCountingDown = true;
    private int initialHours;
    private int initialMinutes;
    private int initialSeconds;
    private float attackMoneyIncreasePeriod;
    private EnemySpawner spawner;
    int turretsPlaced;

    // Arrays to store money data
    public int[] attackMoneyData = new int[24]; // Adjust size based on game duration
    public int[] defenderMoneyData = new int[24]; // Adjust size based on game duration
    private int moneyDataIndex = 0; // Index to track array positions

    // Timer for recording money data every 5 seconds
    private float moneyDataTimer = 0f;
    private const float moneyDataInterval = 5f; // Interval of 5 seconds

    public List<float> type1EnemyTime = new List<float>();
    public List<float> type2EnemyTime = new List<float>();

    // Array to store the number of attackers spawned at each 20-second interval
    public int[] attackersSpawnedArray = new int[6];
    public int arrayIndex = 0;
    private float intervalTimer = 0f;
    private const float intervalDuration = 19f; // Interval of 20 seconds
    public int[] towerSpawnedArray = new int[6];
    int count = 0;
    int countTower = 0;
    public void Init()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        calculation = GetComponent<Calculation>();
        spawner = FindObjectOfType<EnemySpawner>();
        turretsPlaced = Plot.numberOfTurretsPlaced;
        string[] timeParts = gameVariables.systemInfo.currentTimeString.Split(':');
        if (timeParts.Length == 3)
        {
            // Convert the string values into integers
            if (int.TryParse(timeParts[0], out int hours) && int.TryParse(timeParts[1], out int minutes) && int.TryParse(timeParts[2], out int seconds))
            {
                initialHours = hours;
                initialMinutes = minutes;
                initialSeconds = seconds;
            }
            else
            {
                Debug.LogError("Invalid time format in currentTimeString.");
            }
        }
        else
        {
            Debug.LogError("Invalid time format in currentTimeString.");
        }
        remainingTime = new TimeSpan(initialHours, initialMinutes, initialSeconds);
        attackMoneyIncreasePeriod = gameVariables.statisticsInfo.attackMoneyIncreasePeriod;
        StartCoroutine(CountdownTimer());
    }
    public int[] GetAttackersSpawnedArray()
    {
        return attackersSpawnedArray;
    }

    public int[] GetTowerSpawnedArray()
    {
        return towerSpawnedArray;
    }

    public int GetArrayIndex()
    {
        return arrayIndex;
    }

    public string GetElapsedTime()
    {
        return (120f - (int)remainingTime.TotalSeconds).ToString();
    }

    IEnumerator CountdownTimer()
    {
        //new code
        if (remainingTime == null)
        {
           // Debug.LogWarning("Remaining time is not set in the tutorial scene.");
            yield break; // Exit coroutine if remainingTime is null
        }


        if (CurrencyIncreasePupup == null)
        {
           // Debug.Log("CurrencyIncreasePupup is not assigned.");
            yield break;
        }

        if (gameVariables?.statisticsInfo == null || gameVariables?.resourcesInfo == null)
        {
           // Debug.LogError("One or more gameVariables components (statisticsInfo or resourcesInfo) or ResourcesInfo is missing.");
            yield break;
        }
        
        //new code
        float elapsedTime = 0f;
        while (isCountingDown)
        {
            yield return new WaitUntil(() => gameVariables.systemInfo.pause == 0);
            if (remainingTime.TotalSeconds > 0)
            {
                yield return new WaitForSeconds(countdownRate);
                remainingTime = remainingTime.Subtract(new TimeSpan(0, 0, 1));
                gameVariables.systemInfo.currentTimeString = remainingTime.ToString();
                elapsedTime += countdownRate;
                intervalTimer += countdownRate; // Track time for each 20-second interval
                moneyDataTimer += countdownRate;

                // Check if the 20-second interval has passed
                if (intervalTimer >= intervalDuration && arrayIndex < attackersSpawnedArray.Length)
                {
                    attackersSpawnedArray[arrayIndex] = spawner.numberOfEnemiesSpawned - count;
                    count = spawner.numberOfEnemiesSpawned;
                    turretsPlaced = Plot.numberOfTurretsPlaced;
                    towerSpawnedArray[arrayIndex] = turretsPlaced - countTower;
                    countTower = turretsPlaced;
                    arrayIndex++;
                    intervalTimer = 0f; // Reset interval timer for the next interval
                }

                // Record money data every 5 seconds
                if (moneyDataTimer >= moneyDataInterval && moneyDataIndex < attackMoneyData.Length)
                {
                    attackMoneyData[moneyDataIndex] = gameVariables.resourcesInfo.attackMoney;
                    defenderMoneyData[moneyDataIndex] = gameVariables.resourcesInfo.defenseMoney;
                    moneyDataIndex++;
                    moneyDataTimer = 0f; // Reset the timer
                }

                if (elapsedTime >= attackMoneyIncreasePeriod)
                {
                    if (gameVariables.resourcesInfo.attackMoney + gameVariables.statisticsInfo.attackMoneyRate >= ResourcesInfo.maxAttackMoney)
                        CurrencyIncreasePupup.ShowMessage("+" + (ResourcesInfo.maxAttackMoney - gameVariables.resourcesInfo.attackMoney));
                    else
                        CurrencyIncreasePupup.ShowMessage("+" + (gameVariables.statisticsInfo.attackMoneyRate));

                    calculation.ApplyAttackMoney();
                    elapsedTime = 0;
                }
            }
            else
            {
                isCountingDown = false;
                Debug.Log("Countdown end");
                // Load the DefenderWin scene when time reaches 0
                FormSubmit("Defender");
               
                SceneManager.LoadScene("DefenderWin"); // Added line to load the DefenderWin scene
            }
        }
    }

    public void FormSubmit(string win)
    {
        String SessionID = DateTime.UtcNow.Ticks.ToString();
        while (arrayIndex != 6)
        {
            attackersSpawnedArray[arrayIndex] = spawner.numberOfEnemiesSpawned - count;
            count = spawner.numberOfEnemiesSpawned;
            turretsPlaced = Plot.numberOfTurretsPlaced;
            towerSpawnedArray[arrayIndex] = turretsPlaced - countTower;
            countTower = turretsPlaced;
            arrayIndex++;
        }
        while (moneyDataIndex < 24)
        {
            attackMoneyData[moneyDataIndex] = 0;
            defenderMoneyData[moneyDataIndex] = 0;
            moneyDataIndex++;
        }
        float enemy1, enemy2;
        if (type1EnemyTime.Count == 0)
        {
            enemy1 = 0f;
        }
        else
        {
            enemy1= type1EnemyTime.Average();
        }
        if(type2EnemyTime.Count == 0)
        {
            enemy2 = 0f;
        }
        else
        {
            enemy2 = type2EnemyTime.Average();
        }
        
        if (googleFormSubmit != null)
        {
           

            // Call the SubmitData function
            googleFormSubmit.SubmitData(SessionID, win, attackersSpawnedArray, towerSpawnedArray, (120f - (int)remainingTime.TotalSeconds).ToString(), attackMoneyData, defenderMoneyData,enemy1,enemy2);
        }
        else
        {
            Debug.LogError("GoogleFormSubmit component not assigned!");
        }
    }

    public void TogglePause()
    {
        gameVariables.systemInfo.pause = gameVariables.systemInfo.pause == 0 ? 1 : 0;
        gameVariables.systemInfo.pauseShow = gameVariables.systemInfo.pause == 1 ? "Paused" : "Unpaused";
    }
}
