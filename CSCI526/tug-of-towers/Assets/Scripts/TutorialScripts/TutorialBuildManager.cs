using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBuildManager : MonoBehaviour
{
    public static TutorialBuildManager main;

    [Header("References")]
    [SerializeField] private TutorialTower[] towers;
    private int selectedTower = 0; 

    private void Awake()
    {
        main = this;
    }

    public TutorialTower GetSelectedTTower()
    {
        return towers[selectedTower];
    }
    public void SetSelectedTTower(int _selectedTTower)
    {
        selectedTower = _selectedTTower;
    }
}
