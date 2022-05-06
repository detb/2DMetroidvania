using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonInfo : MonoBehaviour
{
    public int ItemID;

    public TextMeshProUGUI priceText;

    public GameObject Shop;
    private ShopManager sm;

    private void Start()
    {
        sm = Shop.GetComponent<ShopManager>();
    }

    void Update()
    {
        priceText.text = "$" + sm.shopItems[2, ItemID];
    }
}
