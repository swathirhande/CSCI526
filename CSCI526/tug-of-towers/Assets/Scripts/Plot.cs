using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color selectedColor; // Color when a tower is selected for moving
    [SerializeField] private int relocationCost = 50;
    [SerializeField] private GameObject popupPrefab; // Assign the PopUp prefab in the Inspector
    [SerializeField] private Canvas canvas; // Drag your "Option" Canvas here in the Inspector
    [SerializeField] private bool isMoneyTower = false;
    private GameObject popupInstance;

    private GameObject tower;
    private Color startColor;
    public static int numberOfTurretsPlaced = 0;

    private static bool isRelocating = false; // Relocation mode flag
    private static Plot relocatingPlot = null; // Reference to the plot being relocated from
    public static List<Plot> plotsWithTowers = new List<Plot>();

    private GameVariables gameVariables;

    private void Start()
    {
        startColor = sr.color;
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = (isRelocating && relocatingPlot == this) ? selectedColor : startColor;
    }

    private void OnMouseDown()
    {
        // Check if the pointer is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Pointer is over UI. Ignoring click.");
            return; // Prevent the click from affecting the plot
        }

        // Proceed with normal click handling
        if (isRelocating)
        {
            HandleRelocationMode();
            return;
        }

        if (tower != null)
        {
            ShowPopup();
            return;
        }

        PlaceTower();
    }


    private void ShowPopup()
    {
        if (popupInstance != null)
        {
            Destroy(popupInstance);
        }

        popupInstance = Instantiate(popupPrefab, canvas.transform);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        popupInstance.transform.position = screenPosition + new Vector3(0, 50, 0);

        Button relocateButton = popupInstance.transform.Find("Relocate").GetComponent<Button>();
        Button sellButton = popupInstance.transform.Find("Sell").GetComponent<Button>();

        if (relocateButton != null)
        {
            relocateButton.onClick.AddListener(StartRelocation);
        }

        if (sellButton != null)
        {
            sellButton.onClick.AddListener(SellTower);
        }
    }

    private void ClosePopup()
    {
        if (popupInstance != null)
        {
            Destroy(popupInstance);
            popupInstance = null;
        }
    }

    private void HandleRelocationMode()
    {
        // Check if the plot is restricted
        if (isMoneyTower)
        {
            Debug.Log("Cannot relocate towers to this plot!");
            return;
        }

        if (tower == null)
        {
            if (gameVariables.resourcesInfo.defenseMoney < relocationCost)
            {
                BuildManager.main.popupManager.ShowMessage("Not enough money to relocate!");
                return;
            }

            LevelManager.main.SpendCurrency(relocationCost);

            // Relocate the tower
            tower = relocatingPlot.tower;
            tower.transform.position = transform.position;
            plotsWithTowers.Remove(relocatingPlot);
            plotsWithTowers.Add(this);

            relocatingPlot.ClearTower();

            isRelocating = false;
            relocatingPlot = null;

            Debug.Log("Tower relocated successfully!");
        }
        else
        {
            Debug.Log("This plot is already occupied!");
        }

        ClosePopup();
    }

    private void SellTower()
    {
        if (tower == null) return;

        int sellAmount = Mathf.FloorToInt(BuildManager.main.GetTowerCost(tower) / 2f);
        LevelManager.main.IncreaseCurrency(sellAmount);
        
        Destroy(tower);
        tower = null;
        gameVariables.resourcesInfo.remainingTowers++;
        plotsWithTowers.Remove(this);
        Debug.Log($"Tower sold for {sellAmount}!");
        ClosePopup();
    }
    
    public void DestroyTower()
    {
        if (tower != null) // Ensure there is a tower to destroy
        {
            Destroy(tower); // Destroy the tower GameObject
            tower = null; // Clear the tower reference
            gameVariables.resourcesInfo.remainingTowers++; // Increment remaining towers

            // Remove this plot from the list of plots with towers
            plotsWithTowers.Remove(this);

            Debug.Log($"Tower on plot {name} destroyed.");
        }
        else
        {
            Debug.LogWarning($"No tower to destroy on plot {name}.");
        }
    }

    private void PlaceTower()
    {
        // Check if the plot is restricted
        if (isMoneyTower)
        {
            Debug.Log("Cannot place towers on this plot!");
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild == null) return;

        if (towerToBuild.cost > gameVariables.resourcesInfo.defenseMoney)
        {
            Debug.Log("Can't afford this tower!");
            return;
        }

        numberOfTurretsPlaced++;
        LevelManager.main.SpendCurrency(towerToBuild.cost);
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        gameVariables.resourcesInfo.remainingTowers--;
        plotsWithTowers.Add(this);
    }

    private void ClearTower()
    {
        tower = null;
        sr.color = startColor;
    }

    private void StartRelocation()
    {
        if (tower == null)
        {
            Debug.Log("No tower to relocate!");
            return;
        }

        isRelocating = true;
        relocatingPlot = this;
        Debug.Log("Relocation mode activated. Click on an empty plot to relocate.");

        ClosePopup();
    }
}
