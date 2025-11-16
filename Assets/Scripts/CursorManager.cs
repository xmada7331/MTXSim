using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    ShopPurchasable shopPurchasable;
    GameManager gameManager;
    ContentChecker contentChecker;
    private int currentCursorIndex = 0;

    public List<Texture2D> cursorSprites;

    public Texture2D cursorSpriteDefault;
    public Texture2D cursorSpriteHand;
    public Texture2D cursorSpriteBrown;
    public Texture2D cursorSpriteFuck;

    public Texture2D cursorSpriteShop1;
    public Texture2D cursorSpriteShop2;
    public Texture2D cursorSpriteShop3;
    private void Awake()
    {
        contentChecker = FindObjectOfType<ContentChecker>();
        shopPurchasable = FindObjectOfType<ShopPurchasable>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        Cursor.SetCursor(cursorSpriteDefault, new Vector2(10,0), CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateCursor()
    {
        if (cursorSprites.Count == 0) return;

        currentCursorIndex = (currentCursorIndex + 1) % cursorSprites.Count;
        Cursor.SetCursor(cursorSprites[currentCursorIndex], new Vector2(10, 0), CursorMode.Auto);
    }

    public void PurchaseHandCursor(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            if (!cursorSprites.Contains(cursorSpriteHand))
            {
                gameManager.playerMoney -= shopPurchasable.itemPrice;
                cursorSprites.Add(cursorSpriteHand);
                shopPurchasable.PurchasedItem();
                Cursor.SetCursor(cursorSpriteHand, new Vector2(10, 0), CursorMode.Auto);

            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void PurchaseBrownCursor(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            if (!cursorSprites.Contains(cursorSpriteBrown))
            {
                gameManager.playerMoney -= shopPurchasable.itemPrice;
                cursorSprites.Add(cursorSpriteBrown);
                shopPurchasable.PurchasedItem();
                Cursor.SetCursor(cursorSpriteBrown, new Vector2(10, 0), CursorMode.Auto);

            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void PurchaseFuckCursor(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            if (!cursorSprites.Contains(cursorSpriteFuck))
            {
                gameManager.playerMoney -= shopPurchasable.itemPrice;
                cursorSprites.Add(cursorSpriteFuck);
                shopPurchasable.PurchasedItem();
                Cursor.SetCursor(cursorSpriteFuck, new Vector2(10, 0), CursorMode.Auto);

            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }

    public void PurchaseShopCursor1(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerGems >= shopPurchasable.itemPrice && contentChecker.hasCurrencyGems)
        {
            if (!cursorSprites.Contains(cursorSpriteShop1))
            {
                cursorSprites.Add(cursorSpriteShop1);
                shopPurchasable.PurchaseItemGems();
                Cursor.SetCursor(cursorSpriteShop1, new Vector2(10, 0), CursorMode.Auto);
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void PurchaseShopCursor2(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerGems >= shopPurchasable.itemPrice && contentChecker.hasCurrencyGems)
        {
            if (!cursorSprites.Contains(cursorSpriteShop2))
            {
                cursorSprites.Add(cursorSpriteShop2);
                shopPurchasable.PurchaseItemGems();
                Cursor.SetCursor(cursorSpriteShop2, new Vector2(10, 0), CursorMode.Auto);
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void PurchaseShopCursor3(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerGems >= shopPurchasable.itemPrice && contentChecker.hasCurrencyGems)
        {
            if (!cursorSprites.Contains(cursorSpriteShop3))
            {
                cursorSprites.Add(cursorSpriteShop3);
                shopPurchasable.PurchaseItemGems();
                Cursor.SetCursor(cursorSpriteShop3, new Vector2(10, 0), CursorMode.Auto);
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
}
