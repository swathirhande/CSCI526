using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMiningTower : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float goldGenerationInterval = 5f;
    [SerializeField] private int goldAmount = 10;
    [SerializeField] private LayerMask castleMask;
    [SerializeField] private int health = 2;
    [SerializeField] private GameObject GoldMiningTowerPlot;

    private GameVariables gameVariables;
    private PopUpManager defenderCurrencyIncreasePopup;

    private bool isActive = true;  // Flag to keep the tower active

    private void Start()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        defenderCurrencyIncreasePopup = GameObject.Find("DefenderCurrencyIncreasePopup").GetComponent<PopUpManager>();
        StartCoroutine(GenerateGold());
    }

    private IEnumerator GenerateGold()
    {
        while (isActive)
        {
            yield return new WaitForSeconds(goldGenerationInterval);

            // Add gold to defender's currency (assuming there's a GameManager handling currency)
            gameVariables.resourcesInfo.defenseMoney += goldAmount;
            defenderCurrencyIncreasePopup.ShowMessage("+" + goldAmount.ToString());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != 6) return;

        EnemyStats enemyStats = other.gameObject.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(1000);
        }
        else
        {
            // Check for TutorialEnemyStats (Tutorial Level)
            TutorialHealth tutorialEnemyStats = other.gameObject.GetComponent<TutorialHealth>();
            if (tutorialEnemyStats != null)
            {
                tutorialEnemyStats.TakeDamage(1000);
            }
        }

        health--;
        if (health == 0)
        {
            Destroy(GoldMiningTowerPlot);
            Destroy(gameObject);
        }
    }
}