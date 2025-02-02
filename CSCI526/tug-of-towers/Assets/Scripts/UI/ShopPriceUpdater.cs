using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopPriceUpdater : MonoBehaviour
{
    public BuildManager buildManager;
    public TextMeshProUGUI[] priceTexts;
    // Start is called before the first frame update
    void Start()
    {
        UpdateShopPrices();
    }

    // Update is called once per frame
    void UpdateShopPrices()
    {
        for (int i = 0; i < buildManager.towers.Length; i++)
        {
            if (i < priceTexts.Length)
            {
                priceTexts[i].text = "$" + buildManager.towers[i].cost.ToString();
            }
        }
    }
}
