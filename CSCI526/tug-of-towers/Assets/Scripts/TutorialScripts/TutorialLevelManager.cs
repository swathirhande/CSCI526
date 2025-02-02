using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelManager : MonoBehaviour
{
    public static TutorialLevelManager main;

    [Header("Game Settings")]
    public int castleLife = 10;  // Initialize castle life to 10
    public int enemiesAtEndPoint = 0;

    public Transform tstartPoint;
    public Transform[] tpath;
    public int tcurrency;
    public int totalCount;

    private TutorialUIManager tutorialUIManager;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        tcurrency = 200;
        totalCount = 2;
        tutorialUIManager = FindObjectOfType<TutorialUIManager>();
        tutorialUIManager.UpdateTotalCount(totalCount);
    }

    public void IncreaseTutorialCurrency(int tamount)
    {
        tcurrency += tamount;
    }

    public bool tSpendCurrency(int tamount)
    {
        if (tamount <= tcurrency)
        {
            //buy item
            tcurrency -= tamount;
            if (tutorialUIManager != null)
            {
                tutorialUIManager.HideTurretImage();
            }
            totalCount--;
            if (totalCount <= 0)
            {
                tutorialUIManager.DisableTowerButton(); 
            }
            if (tutorialUIManager != null)
            {
                tutorialUIManager.UpdateTotalCount(totalCount); 
            }
            return true;
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item!");
            return false;
        }
    }

    public void EnemyReachedEndPoint()
    {
        enemiesAtEndPoint++;
        if (castleLife > 0)
        {
            castleLife--;
        }
        if (tutorialUIManager != null)
        {
            tutorialUIManager.UpdateCastleLife(castleLife); 
        }

        
        if (castleLife == 0)
        {
            Debug.Log("Attacker win!");
        }
    }

}
