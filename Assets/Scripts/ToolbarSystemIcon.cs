using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarSystemIcon : MonoBehaviour
{
    ShopPurchasable shopPurchasable;
    GameManager gameManager;
    public List<Sprite> wallpapers;
    public Image desktopWallpaper;
    private int currentWallpaperIndex = 0;

    public Sprite wallpaper1;
    public Sprite wallpaper2;
    public Sprite wallpaper3;
    public Sprite wallpaper4;

    public GameObject powerBlock;
    public GameObject wpChangeBlock;
    public GameObject wpDefaultBlock;
    public GameObject cursorBlock;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WallpaperChange()
    {
        if (wallpapers.Count == 0) return;

        // Increment index and wrap around
        desktopWallpaper.color = Color.white;
        currentWallpaperIndex = (currentWallpaperIndex + 1) % wallpapers.Count;
        desktopWallpaper.sprite = wallpapers[currentWallpaperIndex];
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void DefaultWallpaper()
    {
        desktopWallpaper.color = new Color32(97,146,183,255);
        desktopWallpaper.sprite = null;
        currentWallpaperIndex = 0;
    }

    public void PurchaseWallpaper1(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            if (!wallpapers.Contains(wallpaper1))
            {
                gameManager.playerMoney -= shopPurchasable.itemPrice;
                wallpapers.Add(wallpaper1);
                shopPurchasable.PurchasedItem();
            } 
        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }
    public void PurchaseWallpaper2(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            if (!wallpapers.Contains(wallpaper2))
            {
                gameManager.playerMoney -= shopPurchasable.itemPrice;
                wallpapers.Add(wallpaper2);
                shopPurchasable.PurchasedItem();
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }
    public void PurchaseWallpaper3(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            if (!wallpapers.Contains(wallpaper3))
            {
                gameManager.playerMoney -= shopPurchasable.itemPrice;
                wallpapers.Add(wallpaper3);
                shopPurchasable.PurchasedItem();
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void PurchaseWallpaper4(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            if (!wallpapers.Contains(wallpaper4))
            {
                gameManager.playerMoney -= shopPurchasable.itemPrice;
                wallpapers.Add(wallpaper4);
                shopPurchasable.PurchasedItem();
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }

    public void PurchasePower(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            powerBlock.SetActive(false);
            gameManager.playerMoney -= shopPurchasable.itemPrice;
            shopPurchasable.PurchasedItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void PurchaseWpChange(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            wpChangeBlock.SetActive(false);
            gameManager.playerMoney -= shopPurchasable.itemPrice;
            shopPurchasable.PurchasedItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void PurchaseWpDefault(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            wpDefaultBlock.SetActive(false);
            gameManager.playerMoney -= shopPurchasable.itemPrice;
            shopPurchasable.PurchasedItem();
        }
        else
        { 
            shopPurchasable.WiggleItem();
        }
    }
    public void PurchaseCursor(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            cursorBlock.SetActive(false);
            gameManager.playerMoney -= shopPurchasable.itemPrice;
            shopPurchasable.PurchasedItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
}
