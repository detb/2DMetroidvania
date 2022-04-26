using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonInfo : MonoBehaviour
{
    public int ItemID;

    public TextMeshProUGUI priceText;

    public GameObject Shop;
    
    // Update is called once per frame
    void Update()
    {
        priceText.text = "$" + Shop.GetComponent<ShopManager>().shopItems[2, ItemID];
    }
}
