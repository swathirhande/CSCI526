using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    //public GoogleFormSubmit formSubmitter;


    [Header("Start Points and Paths")]
    public Transform startPoint1;  
    public Transform[] path1;      

    public Transform startPoint2;  
    public Transform[] path2;      

    public Transform startPoint3;  
    public Transform[] path3;      

    private Transform selectedStartPoint;  
    private Transform[] selectedPath;   

    private GameVariables gameVariables;

    [Header("Key GameObjects")]
    public GameObject aKeyObject;
    public GameObject sKeyObject;
    public GameObject dKeyObject;

    private void Awake()
    {
        main = this;
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
    }

    private void Start()
    {
        SetStartPoint(1); // Default to path 1
        if (SceneManager.GetActiveScene().name == "Level3"|| SceneManager.GetActiveScene().name == "Level2")
        {
            DisableAllKeys();
            EnableOnlyThisKey(dKeyObject);
        }
        
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level3" || SceneManager.GetActiveScene().name == "Level2")
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SetStartPoint(3);
                EnableOnlyThisKey(aKeyObject);

                Debug.Log("Path 3 selected.");
            }
            else if (Input.GetKeyDown(KeyCode.S) && SceneManager.GetActiveScene().name == "Level3")
            {
                SetStartPoint(2);
                EnableOnlyThisKey(sKeyObject);
                Debug.Log("Path 2 selected.");
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                SetStartPoint(1);
                EnableOnlyThisKey(dKeyObject);
                Debug.Log("Path 1 selected.");
            }
        }
    }

    public void SetStartPoint(int point)
    {
        if (point == 1)
        {
            selectedStartPoint = startPoint1;
            selectedPath = path1;
        }
        else if (point == 2)
        {
            selectedStartPoint = startPoint2;
            selectedPath = path2;
        }
        else if (point == 3)
        {
            selectedStartPoint = startPoint3;
            selectedPath = path3;
        }
    }

    
    public Transform GetSelectedStartPoint()
    {
        return selectedStartPoint;
    }

    
    public Transform[] GetSelectedPath()
    {
        return selectedPath;
    }

    public void DecreaseAttackerCurrency(int amount)
    {
        gameVariables.resourcesInfo.attackMoney -= amount;
        Debug.Log("new attacker currency is " + gameVariables.resourcesInfo.attackMoney);
    }

    public void IncreaseCurrency(int amount)
    {
        if (gameVariables.resourcesInfo.defenseMoney < 301)
        {
            gameVariables.resourcesInfo.defenseMoney += amount;
        }

    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= gameVariables.resourcesInfo.defenseMoney)
        {
            gameVariables.resourcesInfo.defenseMoney -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough currency!");
            return false;
        }
    }

    private void EnableOnlyThisKey(GameObject keyObject)
    {
        DisableAllKeys(); // Disable all keys first
        keyObject.SetActive(true); // Enable the specific key object
    }
    private void DisableAllKeys()
    {
        aKeyObject.SetActive(false);
        sKeyObject.SetActive(false);
        dKeyObject.SetActive(false);
    }
}

