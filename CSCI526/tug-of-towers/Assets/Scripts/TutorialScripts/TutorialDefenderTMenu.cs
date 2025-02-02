using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialDefenderTMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI defenderCurrencyUI;

    private void OnGUI()
    {
        defenderCurrencyUI.text = TutorialLevelManager.main.tcurrency.ToString();
    }

    public void SetTSelected()
    {

    }

}
