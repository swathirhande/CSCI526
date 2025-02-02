using System.Collections;
using UnityEngine;

public class Calculation : MonoBehaviour
{
    private GameVariables gameVariables;

    private void Start()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
    }

    public void ApplyAttackMoney()
    {
        gameVariables.resourcesInfo.attackMoney += gameVariables.statisticsInfo.attackMoneyRate;


        if (gameVariables.resourcesInfo.attackMoney > ResourcesInfo.maxAttackMoney)
            gameVariables.resourcesInfo.attackMoney = ResourcesInfo.maxAttackMoney;
    }
}
