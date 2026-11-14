using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopPurchasable : MonoBehaviour
{
    GameManager gameManager;
    ContentChecker contentChecker;
    public Image itemImage;
    public float itemPrice;
    public Transform transformToWiggle;
    public bool itemPurchased = false;
    public bool isPurchasable = false;
    public string purchaseDescription;
    private bool hasRecorded = false;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        itemImage = GetComponent<Image>();
        transformToWiggle = GetComponent<Transform>();
        contentChecker = FindObjectOfType<ContentChecker>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PurchaseItem()
    {
        if (gameManager.playerMoney >= itemPrice && !itemPurchased && isPurchasable)
        {
            Color purchasedColor = new Color(1, 1, 1, 0.2f);
            itemPurchased = true;
            itemImage.color = purchasedColor;
            gameManager.playerMoney -= itemPrice;
            if (!hasRecorded)
            {
                hasRecorded = true;
                PurchaseHistoryManager.RecordPurchase(itemPrice, GetPurchaseDescription());
            }
        }
        else if (!itemPurchased)
        {
          WiggleItem();
        }
    }
    public void PurchaseItemGems()
    {
        if (gameManager.playerGems >= itemPrice && !itemPurchased && isPurchasable && contentChecker.hasCurrencyGems)
        {
            Color purchasedColor = new Color(1, 1, 1, 0.2f);
            itemPurchased = true;
            itemImage.color = purchasedColor;
            gameManager.playerGems -= itemPrice;
        }
    }
    public void PurchaseItemDiamonds()
    {
        if (gameManager.playerDiamonds >= itemPrice && !itemPurchased && isPurchasable && contentChecker.hasCurrencyDiamonds)
        {
            Color purchasedColor = new Color(1, 1, 1, 0.2f);
            itemPurchased = true;
            itemImage.color = purchasedColor;
            gameManager.playerDiamonds -= itemPrice;
        }
        else if (!itemPurchased)
        {
            WiggleItem();
        }
    }
    public void WiggleItem()
    {
        StartCoroutine(WiggleRoutine());
    }
    private IEnumerator WiggleRoutine()
    {

        float duration = 0.5f;
        float speed = 50f;
        float distance = 5f;
        float time = 0f;

        Vector3 pos = transformToWiggle.localPosition;
        Vector3 startPos = transformToWiggle.localPosition;

        while (time <= duration)
        {
            pos.x = Mathf.Lerp(Mathf.Sin(time * speed) * distance, 0, time / duration);
            transformToWiggle.localPosition = pos;
            time += Time.deltaTime;
            //itemImage.color = new Color32(200,0,0,200);
            yield return null;
        }

        transformToWiggle.localPosition = startPos;
        //itemImage.color = new Color32(244, 244, 244, 255);

    }

    public void PurchaseBankWidget()
    {
        if(gameManager.playerMoney >= itemPrice && !itemPurchased && isPurchasable)
        {
            gameManager.playerMoney -= itemPrice;
            Transform lockParent = transform.parent;
            Transform locklockParent = lockParent.parent;
            gameManager.bankWidget.SetActive(true);
            Destroy(locklockParent.gameObject);
            gameManager.bankWidgetImage.sprite = gameManager.bankWidgetPurchased;
            gameManager.mainAudioSource.PlayOneShot(gameManager.purchaseWidget);
            gameManager.expensesAmount += itemPrice;
            PurchaseHistoryManager.RecordPurchase(itemPrice, "Bank widget");
        }
        else if (!itemPurchased)
        {
            WiggleItem();
        }
    }
    public void PurchaseStockWidget()
    {
        if (gameManager.playerMoney >= itemPrice && !itemPurchased && isPurchasable)
        {
            gameManager.playerMoney -= itemPrice;
            Transform lockParent = transform.parent;
            Transform locklockParent = lockParent.parent;
            gameManager.stockWidget.SetActive(true);
            Destroy(locklockParent.gameObject);
            gameManager.stockWidgetImage.sprite = gameManager.stockWidgetPurchased;
            gameManager.mainAudioSource.PlayOneShot(gameManager.purchaseWidget);
            gameManager.expensesAmount += itemPrice;
            PurchaseHistoryManager.RecordPurchase(itemPrice, "Stock widget");
        }
        else if (!itemPurchased)
        {
            WiggleItem();
        }
    }
    public void PurchaseFreeCoinWidget()
    {
        if (gameManager.playerMoney >= itemPrice && !itemPurchased && isPurchasable)
        {
            gameManager.playerMoney -= itemPrice;
            Transform lockParent = transform.parent;
            Transform locklockParent = lockParent.parent;
            gameManager.freeCoinWidget.SetActive(true);
            Destroy(locklockParent.gameObject);
            gameManager.freeCoinWidgetImage.sprite = gameManager.freeCoinWidgetPurchased;
            gameManager.mainAudioSource.PlayOneShot(gameManager.purchaseWidget);
            gameManager.expensesAmount += itemPrice;
            PurchaseHistoryManager.RecordPurchase(itemPrice, "FreeCoin widget");
        }
        else if (!itemPurchased)
        {
            WiggleItem();
        }
    }

    public void HoverItem()
    {
        if (!itemPurchased)
        {
            itemImage.color = new Color32(244, 244, 244, 100);
            TMP_Text childText = GetComponentInChildren<TMP_Text>();
            childText.color = new Color32(244, 244, 244, 100); 
        }
    }

    public void UnhoverItem(TMP_Text priceText)
    {
        if (!itemPurchased)
        {
            itemImage.color = new Color32(244, 244, 244, 255);
            TMP_Text childText = GetComponentInChildren<TMP_Text>();
            childText.color = new Color32(244, 244, 244, 255); 
            priceText.gameObject.SetActive(false);
        }
    }

    public void PurchasedItem()
    {
        itemPurchased = true;
        itemImage.color = new Color32(255, 255, 255, 105);
        if (!hasRecorded)
        {
            hasRecorded = true;
            PurchaseHistoryManager.RecordPurchase(itemPrice, GetPurchaseDescription());
        }
    }

    public void HoverWithPrice(TMP_Text priceText)
    {
        if (!itemPurchased)
        {
            priceText.gameObject.SetActive(true);
            priceText.text = "$" + itemPrice.ToString("F2");
        }
        else
        {
            priceText.text = "PURCHASED";
        }
    }

    public void UnhoverWithPrice(TMP_Text priceText)
    {
            priceText.gameObject.SetActive(false);
    }

    public void HoverShopDescription( string description)
    {
        if (!itemPurchased)
        {
            gameManager.shopDescription.gameObject.SetActive(true);
            gameManager.shopDescription.text = description;
        }
    }  

    public void UnhoverShopDescription()
    {
        gameManager.shopDescription.gameObject.SetActive(false);
    }

    public string GetPurchaseDescription()
    {
        if (!string.IsNullOrEmpty(purchaseDescription))
            return purchaseDescription;

        if (IsInsideParent("Mtx_window"))
        {
            return "MTX setting";
        }

        if (IsInsideParent("DigiStore_window"))
        {
            Transform p = transform;
            while (p != null && p.name != "DigiStore_window")
            {
                string n = p.name.ToLowerInvariant();
                if (n.Contains("freecoin")) return "FreeCoin app";
                if (n.Contains("survey")) return "Surveys app";
                if (n.Contains("bank")) return "Bank app";
                if (n.Contains("stock")) return "Stock app";
                if (n.Contains("camera")) return "Camera app";
                if (n.Contains("shop")) return "Shop app";
                if (n.Contains("gt") || n.Contains("gaym")) return "GaymTester app";
                if (n.Contains("thunder")) return "Thunder app";
                if (n.Contains("pic")) return "FeetPics app";
                p = p.parent;
            }
            return "DigiStore app";
        }

        if (IsInsideParent("Shop_window"))
        {
            string n = gameObject.name.ToLowerInvariant();
            string parentN = transform.parent != null ? transform.parent.name.ToLowerInvariant() : "";
            if (n.Contains("webcam") || parentN.Contains("webcam")) return "DigiStore camera";
            if (n.Contains("antivirus")) return "Antivirus";
            if (n.Contains("pause")) return "Pause menu";
            if (n.Contains("wallpaper") || n.Contains("wp")) return "Wallpaper";
            if (n.Contains("cursor")) return "Cursor";
            if (n.Contains("power")) return "Power app";
            if (n.Contains("feet")) return "FeetPics pack";
            return "Shop item";
        }

        if (IsInsideParent("FreeCoin_window"))
        {
            return "FreeCoin upgrade";
        }

        return gameObject.name;
    }

    private bool IsInsideParent(string parentName)
    {
        Transform curr = transform;
        while (curr != null)
        {
            if (curr.name == parentName) return true;
            curr = curr.parent;
        }
        return false;
    }
}
