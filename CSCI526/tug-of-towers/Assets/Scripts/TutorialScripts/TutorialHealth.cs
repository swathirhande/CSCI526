using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float hitPoint = 4;
    [SerializeField] private int currencyWorth = 50;

    private bool isDestroyed = false;

    public void TakeDamage(float tdmg)
    {
        hitPoint -= tdmg;

        if (hitPoint <= 0 && !isDestroyed)
        {
            TutorialEnemySpawner.onTEnemyDestroy.Invoke();
            TutorialLevelManager.main.IncreaseTutorialCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
