using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    ShopPurchasable shopPurchasable;
    GameManager gameManager;
    private int currentCursorIndex = 0;

    public List<Texture2D> cursorSprites;

    public Texture2D cursorSpriteDefault;
    public Texture2D cursorSpriteHand;
    public Texture2D cursorSpriteBrown;
    public Texture2D cursorSpriteFuck;
    private void Awake()
    {
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
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
}
