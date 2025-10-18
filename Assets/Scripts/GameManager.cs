using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    [Header("DigistoreApp")]
    public GameObject surveysIcon;
    public GameObject bankIcon;
    public GameObject stockIcon;
    public GameObject shopIcon;
    public GameObject cameraIcon;
    public GameObject freeCoinIcon;
    public GameObject feetPicsIcon;
    [Header("BankingApp")]
    public TMP_Text incomeText;
    public float incomeAmount = 0;
    public TMP_Text expensesText;
    public float expensesAmount = 0;
    [Header("Toolbar")]
    public GameObject errorWindow;
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
        incomeText.text = "+$" + incomeAmount.ToString("F2");
        expensesText.text = "-$" + expensesAmount.ToString("F2");
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
            expensesAmount += amount;
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

    public void UnlockSurveys(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            surveysIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }
    public void UnlockBank(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            bankIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }
    public void UnlockStock(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            stockIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void UnlockFeetPics(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            feetPicsIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void UnlockFreeCoin(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            freeCoinIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }
    public void UnlockCamera(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            cameraIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }
    public void UnlockShopDigiStore(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            shopIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }

    public void ToolbarLockedApp()
    {
        errorWindow.SetActive(true);
    }


}
