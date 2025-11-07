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
        else if (!itemPurchased)
        {
            WiggleItem();
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


}
