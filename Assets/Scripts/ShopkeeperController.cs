using UnityEngine;

public class ShopkeeperController : MonoBehaviour
{
    private ShopManager sm;
    private bool inShop;

    private void Start()
    {
        sm = GameObject.Find("Shop").GetComponent<ShopManager>();
    }
    
    void Update()
    {
        if(!Input.GetButtonDown("Interact") || !inShop) return;
        if (!sm.IsShopOpen())
            sm.OpenShop();
        else
        {
            sm.CloseShop();
            inShop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        inShop = true;
    }
}
