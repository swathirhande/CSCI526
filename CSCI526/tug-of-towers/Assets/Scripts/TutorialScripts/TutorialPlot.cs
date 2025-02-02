using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;

    [SerializeField] private Color highlightColor = Color.yellow; 
    private bool isHighlighted = false;
    private GameVariables gameVariables;

    private void OnMouseEnter()
    {
        
        if (!isHighlighted)
        {
            sr.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        
        if (!isHighlighted)
        {
            sr.color = startColor;
        }
    }

    public void ActivateHighlight()
    {
        if (sr != null && !isHighlighted)
        {
            sr.color = highlightColor; 
            isHighlighted = true;
        }
    }

    public void DeactivateHighlight()
    {
        if (isHighlighted && sr != null)
        {
            sr.color = startColor; 
            isHighlighted = false;
        }
    }

    /*  public void ActivateHighlight()
      {
          if (sr != null)
          {
              sr.color = highlightColor;
              isHighlighted = true;
          }
      }

      // Function to deactivate the highlight on Plot32
      public void DeactivateHighlight()
      {
          if (isHighlighted && sr != null)
          {
              sr.color = startColor;
              isHighlighted = false;
          }
      }*/

    private void Start()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        startColor = sr.color;
    }
    private void OnMouseDown()
    {
        if (tower != null) return;

        if (gameVariables.tutorialInfo.towerPlaceable == false)
        {
            return;
        }

        if (TutorialLevelManager.main.totalCount <= 0)
        {
            Debug.Log("Cannot place tower: totalCount is 0!");
            return; // Prevent tower placement
        }

        TutorialTower tTowerToBuild = TutorialBuildManager.main.GetSelectedTTower();

        if(tTowerToBuild.tcost > TutorialLevelManager.main.tcurrency)
        {
            Debug.Log("Can't afford this tower!");
            return;
        }

        TutorialLevelManager.main.tSpendCurrency(tTowerToBuild.tcost);
        tower = Instantiate(tTowerToBuild.perfabs, transform.position, Quaternion.identity);

        FindObjectOfType<TutorialUIManager>().RemoveHighlights();
    }


}
