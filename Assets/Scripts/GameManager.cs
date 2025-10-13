using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public UltEvents.UltEvent myEvent;
    ToolbarSystemIcon toolbarSystemIcon;
    ShopPurchasable shopPurchasable;
    ContentChecker contentChecker;

    public List<GameObject> antiClickObject;

    public float playerMoney;
    public float playerGems;
    public float playerDiamonds;


    public GameObject LockMenu;
    public GameObject bankWidget;
    public GameObject stockWidget;
    public GameObject freeCoinWidget;
    public GameObject adsSystem;
    [Header("CameraApp")]
    public bool hasCamera = false;
    public GameObject cameraLock;
    private void Awake()
    {
        contentChecker = FindObjectOfType<ContentChecker>();
        toolbarSystemIcon = FindObjectOfType<ToolbarSystemIcon>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisableAntiClickObjects()
    {
        foreach (GameObject obj in antiClickObject)
        {
            obj.SetActive(false);
        }
    }
    public void UnlockShop(float amount)
    {
        if (playerMoney >= amount)
        {
            LockMenu.SetActive(false);
            playerMoney -= amount;
            contentChecker.unlockedMainShop = true;
        }
        else
        {
            Debug.Log("not enough money");
        }
    }

    public void PurchaseAntivirus(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            adsSystem.GetComponent<AdsSystem>().enabled = false;
            adsSystem.GetComponentInChildren<SpriteRenderer>().enabled = false;
            shopPurchasable.itemPurchased = true;
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }

    public void PurchaseCamera(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            hasCamera = true;
            shopPurchasable.PurchaseItem();
            cameraLock.SetActive(false);
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }


}
