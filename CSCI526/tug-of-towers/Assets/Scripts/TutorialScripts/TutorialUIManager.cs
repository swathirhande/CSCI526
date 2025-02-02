using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class TutorialUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI InstructionTxt; 
    [SerializeField] private GameObject turretImage;
    [SerializeField] private TextMeshProUGUI totalCountText;
    [SerializeField] private GameObject mask0;
    [SerializeField] private GameObject mask1;
    [SerializeField] private Button defenderConfirmButton;

    [Header("Plot Highlight")]
    public TutorialPlot plot55; 

    private Color yellowColor = Color.yellow; 
    public UnityEngine.UI.Button towerButton;

    public bool enableManualEnemySpawn = false;
    public bool hasSpawnedOnce = false;
    private GameVariables gameVariables;

    private enum TutorialStep
    {
        RoleDecision,
        DefenderIntro,
        DefenderTutorial,
        ShowTowerSelection,
        AttackerIntro,
        AttackerTutorial,
        End
    }
    private TutorialStep nextStep = TutorialStep.RoleDecision;
    private TutorialStep currentStep = TutorialStep.RoleDecision;


    private void Start()
    {
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        AdvanceStep(); 
    }

    private void AdvanceStep()
    {
        switch (nextStep)
        {
            case TutorialStep.RoleDecision:
                currentStep = TutorialStep.RoleDecision;
                StartCoroutine(RoleDecision());
                nextStep = TutorialStep.DefenderIntro;
                break;
            case TutorialStep.DefenderIntro:
                currentStep = TutorialStep.DefenderIntro;
                StartCoroutine(DefenderIntro());
                nextStep = TutorialStep.DefenderTutorial;
                break;
            case TutorialStep.DefenderTutorial:
                currentStep = TutorialStep.DefenderTutorial;
                StartCoroutine(DefenderTutorial());
                nextStep = TutorialStep.ShowTowerSelection;
                break;
            case TutorialStep.ShowTowerSelection:
                currentStep = TutorialStep.ShowTowerSelection;
                StartCoroutine(HandleTurretImageVisibility());
                nextStep = TutorialStep.AttackerIntro;
                break;
            case TutorialStep.AttackerIntro:
                currentStep = TutorialStep.AttackerIntro;
                StartCoroutine(AttackerIntro());
                nextStep = TutorialStep.AttackerTutorial;
                break;
            case TutorialStep.AttackerTutorial:
                currentStep = TutorialStep.AttackerTutorial;
                StartCoroutine(AttackerTutorial());
                nextStep = TutorialStep.End;
                break;
            case TutorialStep.End:
                StartCoroutine(BackToMain());
                break;
        }
    }


    bool isPanel1Confirmed = false;
    bool isPanel0Confirmed = false;
    private bool hasFinishRoleDecision = false;

    private IEnumerator RoleDecision()
    {
        mask0.SetActive(true);
        Transform panel0 = mask0.transform.Find("Panel0");
        Transform panel1 = mask0.transform.Find("Panel1");
        GameObject panel0Confirm = panel0.Find("Confirm")?.gameObject;
        GameObject panel1Confirm = panel1.Find("Confirm")?.gameObject;
        Button panel0InstructButton = panel0.Find("instruct")?.GetComponent<Button>();
        TextMeshProUGUI panel0InstructText = panel0InstructButton?.GetComponentInChildren<TextMeshProUGUI>();
        Button panel1InstructButton = panel1.Find("instruct")?.GetComponent<Button>();
        TextMeshProUGUI panel1InstructText = panel1InstructButton?.GetComponentInChildren<TextMeshProUGUI>();

        panel1Confirm.SetActive(false);
        panel0Confirm.SetActive(false);

        panel1InstructButton.onClick.AddListener(() =>
        {
            isPanel1Confirmed = !isPanel1Confirmed;
            panel1Confirm.SetActive(isPanel1Confirmed);
            panel1InstructText.text = isPanel1Confirmed ? "Click to Cancel" : "Click to Confirm";
            StartCoroutine(CheckAndAdvanceWithDelay());
        });

        if (!hasFinishRoleDecision) { StartCoroutine(WaitForKeyPress(panel0Confirm, panel0InstructText)); }
    
        yield return null;
    }

    private IEnumerator WaitForKeyPress(GameObject panel0Confirm, TextMeshProUGUI panel0InstructText)
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                isPanel0Confirmed = !isPanel0Confirmed;
                panel0Confirm.SetActive(isPanel0Confirmed);
                panel0InstructText.text = isPanel0Confirmed ? "Type 1 to Cancel" : "Type 1 to Confirm";
                StartCoroutine(CheckAndAdvanceWithDelay());
            }
            yield return null;
        }
    }
    private IEnumerator CheckAndAdvanceWithDelay()
    {
        if (isPanel0Confirmed && isPanel1Confirmed && !hasFinishRoleDecision)
        {
            hasFinishRoleDecision = true;
            yield return new WaitForSeconds(1);
            mask0.SetActive(false);
            AdvanceStep();
        }
    }


    private IEnumerator DefenderIntro()
    {
        Transform panel1 = mask1.transform.Find("Panel1");
        GameObject InstructButton = panel1.Find("instruct")?.gameObject;
        TextMeshProUGUI InstructText = InstructButton?.GetComponentInChildren<TextMeshProUGUI>();
        Button NextButton = panel1.Find("next")?.GetComponent<Button>();

        mask1.SetActive(true);
        InstructText.text = "Now is Defender Tutorial";
        yield return new WaitForSeconds(3);
        NextButton.gameObject.SetActive(true);

        bool buttonClicked = false;
        NextButton.onClick.AddListener(() => buttonClicked = true);
        yield return new WaitUntil(() => buttonClicked);
        NextButton.gameObject.SetActive(false);


        InstructText.text = "Your goal is to survive until the countdown ends.";
        yield return new WaitForSeconds(3);
        NextButton.gameObject.SetActive(true);

        buttonClicked = false;
        NextButton.onClick.AddListener(() => buttonClicked = true);
        yield return new WaitUntil(() => buttonClicked);
        NextButton.gameObject.SetActive(false);

        InstructText.text = "Click the tower and build it on the map.";
        yield return new WaitForSeconds(3);
        NextButton.gameObject.SetActive(true);

        buttonClicked = false;
        NextButton.onClick.AddListener(() => buttonClicked = true);
        yield return new WaitUntil(() => buttonClicked);
        NextButton.gameObject.SetActive(false);

        gameVariables.tutorialInfo.towerPlaceable = true;

        mask1.SetActive(false);
        AdvanceStep();
        yield return null;

    }
    

    private IEnumerator DefenderTutorial()
    {
        yield return new WaitForSeconds(1);
        HighlightPlot();
        AdvanceStep();
    }

    private IEnumerator AttackerIntro()
    {
        Transform panel1 = mask1.transform.Find("Panel1");
        GameObject InstructButton = panel1.Find("instruct")?.gameObject;
        TextMeshProUGUI InstructText = InstructButton?.GetComponentInChildren<TextMeshProUGUI>();
        Button NextButton = panel1.Find("next")?.GetComponent<Button>();

        yield return new WaitForSeconds(3);
        mask1.SetActive(true);
        InstructText.text = "Now is Attacker Tutorial";
        yield return new WaitForSeconds(3);
        NextButton.gameObject.SetActive(true);

        bool buttonClicked = false;
        NextButton.onClick.AddListener(() => buttonClicked = true);
        yield return new WaitUntil(() => buttonClicked);
        NextButton.gameObject.SetActive(false);

        InstructText.text = "Your goal is to reduce the Castle's life to 0 before the countdown ends.";
        yield return new WaitForSeconds(3);
        NextButton.gameObject.SetActive(true);

        buttonClicked = false;
        NextButton.onClick.AddListener(() => buttonClicked = true);
        yield return new WaitUntil(() => buttonClicked);
        NextButton.gameObject.SetActive(false);

        InstructText.text = "Press keyboard button 1 to spawn Enemies.";
        yield return new WaitForSeconds(3);
        NextButton.gameObject.SetActive(true);

        buttonClicked = false;
        NextButton.onClick.AddListener(() => buttonClicked = true);
        yield return new WaitUntil(() => buttonClicked);
        NextButton.gameObject.SetActive(false);

        mask1.SetActive(false);
        AdvanceStep();
        yield return null;

    }

    private IEnumerator HandleTurretImageVisibility()
    {
        if (turretImage != null)
        {
            turretImage.SetActive(false); 
            yield return new WaitForSeconds(3); 
            turretImage.SetActive(true);
        }
    }

    private IEnumerator AttackerTutorial()
    {
        enableManualEnemySpawn = true;
        if (currentStep == TutorialStep.AttackerTutorial)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Alpha1));
            yield return new WaitForSeconds(2);
            enableManualEnemySpawn = false;
            Transform panel1 = mask1.transform.Find("Panel1");
            GameObject InstructButton = panel1.Find("instruct")?.gameObject;
            TextMeshProUGUI InstructText = InstructButton?.GetComponentInChildren<TextMeshProUGUI>();
            Button NextButton = panel1.Find("next")?.GetComponent<Button>();

            mask1.SetActive(true);
            InstructText.text = "Try to spawn more enemies and reduce Castle's life to 0";
            yield return new WaitForSeconds(3);
            NextButton.gameObject.SetActive(true);

            bool buttonClicked = false;
            NextButton.onClick.AddListener(() => buttonClicked = true);
            yield return new WaitUntil(() => buttonClicked);
            NextButton.gameObject.SetActive(false);

            mask1.SetActive(false);

            enableManualEnemySpawn = true;


        }
        yield return null;
    }

    private IEnumerator BackToMain()
    {
        Transform panel1 = mask1.transform.Find("Panel1");
        GameObject InstructButton = panel1.Find("instruct")?.gameObject;
        TextMeshProUGUI InstructText = InstructButton?.GetComponentInChildren<TextMeshProUGUI>();
        Button NextButton = panel1.Find("next")?.GetComponent<Button>();

        mask1.SetActive(true);
        InstructText.text = "Congratulations! \nYou have completed the tutorial!";
        yield return new WaitForSeconds(3);
        NextButton.gameObject.SetActive(true);

        bool buttonClicked = false;
        NextButton.gameObject.SetActive(false);
        SceneManager.LoadScene("Menu");
        mask1.SetActive(false);
    }


    private void HighlightPlot()
    {
        if (plot55 != null)
        {
            plot55.ActivateHighlight();
        }
    }

    
    public void RemoveHighlights()
    {
        
        if (plot55 != null)
        {
            plot55.DeactivateHighlight();
        }
    }
    public void HideTurretImage()
    {
        Debug.Log("Hide turret image");
        turretImage.SetActive(false);
        if (turretImage != null)
        {
            turretImage.SetActive(false);
        }
    }
    public void UpdateCastleLife(int life)
    {
        if(currentStep == TutorialStep.AttackerTutorial) 
        { 
            gameVariables.resourcesInfo.defenseLife = life;
            if (life == 0)
            {
                enableManualEnemySpawn = false;
                AdvanceStep();
            }
        }
    }
    public void UpdateTotalCount(int count)
    {
        if (count == 0)
        {
            gameVariables.tutorialInfo.continueSpawn = false;
            AdvanceStep();
        }
        else if (count == 1)
        {
            gameVariables.tutorialInfo.continueSpawn = true;
        }
        totalCountText.text = count.ToString(); 
    }

    public void DisableTowerButton()
    {
        if (towerButton != null)
        {
            towerButton.interactable = false; // Disable the button
            Debug.Log("Tower button disabled.");
        }
    }

}




