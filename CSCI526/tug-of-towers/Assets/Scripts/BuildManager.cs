using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    //[SerializeField] private GameObject[] towerPrefabs;
    public Tower[] towers;
    [SerializeField] public PopUpManager popupManager;
    private GameVariables gameVariables;

    private int selectedTower = 0;

    private void Awake()
    {
        main = this;
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
    }

    public Tower GetSelectedTower()
    {
        if (gameVariables.resourcesInfo.remainingTowers > 0)
        {
            return towers[selectedTower];
        }

        else return null;
    }

    public int GetTowerCost(GameObject tower)
    {
        if (tower == null)
        {
            return 0;
        }

        // Strip the "(Clone)" suffix from the tower's name
        string towerName = tower.name.Replace("(Clone)", "").Trim();
        
        // Find the prefab in the towers array
        foreach (Tower t in towers)
        {
            if (t.prefab.name == towerName) // Compare names without "(Clone)"
            {
                return t.cost;
            }
        }
        
        return 0;
    }

    public void SetSelectedTower(int _selectedTower)
    {
        if (gameVariables.resourcesInfo.remainingTowers <= 0)
        {
            popupManager.ShowMessage("Defender has reached the tower limit");
            return;
        }

        if (towers[_selectedTower].cost <= gameVariables.resourcesInfo.defenseMoney)
        {
            selectedTower = _selectedTower;

        }
        else
        {
            popupManager.ShowMessage("Not enough currency to spawn " + (towers[_selectedTower].name));
        }
    }
}
