using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GameTabUnlocker : MonoBehaviour
{
    ContentChecker contentChecker;
    GameManager gameManager;
    Game_behaviour gameBehaviour;
    ShopPurchasable shopPurchasable;
    [Header("Game Tab")]
    public List <GameObject> lockedSettings;
    public List <GameObject> lockedGameTitleOptions;
    public List <GameObject> lockedGraphicOptions;
    public List <GameObject> lockedCurrencyOptions;

    public Image gameBackground;
    public GameObject settingsLock;
    public GameObject volumeLock;
    public GameObject mouseSensLock;
    public GameObject fullscreenLock;
    public GameObject frameLimitLock;
    public GameObject resolutionLock;
    public GameObject vsyncLock;
    public GameObject languageLock;
    public GameObject nsfwLock;
    [Header("SFX Tab")]
    public GameObject sfxLock;
    public GameObject musicLock;

    private void Awake()
    {
        contentChecker = FindObjectOfType<ContentChecker>();
        gameManager = FindObjectOfType<GameManager>();
        gameBehaviour = FindObjectOfType<Game_behaviour>();
    }
    void Start()
    {
        gameBackground.enabled = false;
    }

    void Update()
    {
        
    }

    public void GreyOutImage(GameObject target)
    {
        Image targetImage = target.GetComponent<Image>();
        if (targetImage != null)
        {
            targetImage.color = new Color(1, 1, 1, 0.2f);
        }
    }

    public void UnlockSettingsMenu(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            settingsLock.SetActive(false);
            foreach (GameObject obj in lockedSettings)
            {
                Image img = obj.GetComponent<Image>();
                shopPurchasable = obj.GetComponent<ShopPurchasable>();
                shopPurchasable.isPurchasable = true;
                if (img != null)
                {
                    Color c = img.color;
                    //c.a = 0.5f; // Set alpha to 50%
                    img.color = new Color32(244, 244, 244, 255);
                }
            }
            contentChecker.hasSettings = true;
        }
    }
    public void UnlockGameOptions(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasGameTitle && gameManager.playerMoney >= itemPrice)
        {
            foreach (GameObject obj in lockedGameTitleOptions)
            {
                Image img = obj.GetComponent<Image>();
                shopPurchasable = obj.GetComponent<ShopPurchasable>();
                shopPurchasable.isPurchasable = true;
                if (img != null)
                {
                    Color c = img.color;
                    //c.a = 0.5f; // Set alpha to 50%
                    img.color = new Color32(244, 244, 244, 255);
                }
            }
            contentChecker.hasGameTitle = true;
        }
    }
    public void UnlockGraphicOptions(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasGraphicsOptions && gameManager.playerMoney >= itemPrice)
        {
            foreach (GameObject obj in lockedGraphicOptions)
            {
                Image img = obj.GetComponent<Image>();
                shopPurchasable = obj.GetComponent<ShopPurchasable>();
                shopPurchasable.isPurchasable = true;
                if (img != null)
                {
                    Color c = img.color;
                    //c.a = 0.5f; // Set alpha to 50%
                    img.color = new Color32(244, 244, 244, 255);
                }
            }
            contentChecker.hasGraphicsOptions = true;
        }
    }
    public void UnlockCurrencyOptions(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasCurrency && gameManager.playerMoney >= itemPrice)
        {
            foreach (GameObject obj in lockedCurrencyOptions)
            {
                Image img = obj.GetComponent<Image>();
                shopPurchasable = obj.GetComponent<ShopPurchasable>();
                shopPurchasable.isPurchasable = true;
                if (img != null)
                {
                    Color c = img.color;
                    //c.a = 0.5f; // Set alpha to 50%
                    img.color = new Color32(244, 244, 244, 255);
                }
            }
            contentChecker.hasCurrency = true;
        }
    }
    public void UnlockMenuBg(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasMenuBg && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            gameBackground.enabled = true;
            contentChecker.hasMenuBg = true;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }

    public void UnlockVolume(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasVolume && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            volumeLock.SetActive(false); 
            contentChecker.hasVolume = true;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void UnlockMouseSens(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasMouseSens && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            mouseSensLock.SetActive(false);
            contentChecker.hasMouseSens = true;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void UnlockFullscreen(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;

        if (!contentChecker.hasFullscreen && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            fullscreenLock.SetActive(false);
            contentChecker.hasFullscreen = true;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void UnlockFrameLimit(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;

        if (!contentChecker.hasFrameLimit && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            frameLimitLock.SetActive(false);
            contentChecker.hasFrameLimit = true;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void UnlockResolutionChange(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;

        if (!contentChecker.hasResolutionChange && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            resolutionLock.SetActive(false);
            contentChecker.hasResolutionChange = true;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void UnlockVsync(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;

        if (!contentChecker.hasVsync && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            vsyncLock.SetActive(false);
            contentChecker.hasVsync = true;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void UnlockLanguage(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;

        if (!contentChecker.hasLanguage && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            languageLock.SetActive(false);
            contentChecker.hasLanguage = true;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void UnlockNSFW(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;

        if (!contentChecker.hasNSFW && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            nsfwLock.SetActive(false);
            contentChecker.hasNSFW = true;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// VFX and MUSIC OPTIONS
    public void UnlockSFX(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasSFX && gameManager.playerMoney >= itemPrice)
        {
            sfxLock.SetActive(false);
            contentChecker.hasSFX = true;
        }
    }
    public void UnlockMusic(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasMusic && gameManager.playerMoney >= itemPrice)
        {
            musicLock.SetActive(false);
            contentChecker.hasMusic = true;
        }
    }
    public void UnlockMouseClick(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasMouseClick && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasMouseClick = true;
        }
    }
    public void UnlockUISFX(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasUISFX && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasUISFX = true;
        }
    }
    public void UnlockBossDeath(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasBossDeath && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasBossDeath = true;
        }
    }
    public void UnlockNextLevel(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasNextLevel && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasNextLevel = true;
        }
    }
    public void UnlockWeaponSwing(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasWeaponSwing && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasWeaponSwing = true;
        }
    }
    public void UnlockGemDrop(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasGemDrop && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasGemDrop = true;
        }
    }
    public void UnlockDiamondDrop(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasDiamondDrop && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasDiamondDrop = true;
        }
    }
    public void UnlockItemBuy(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasItemBuy && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasItemBuy = true;
        }
    }
    public void UnlockBattleAmbience(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasBattleAmbience && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasBattleAmbience = true;
        }
    }
    public void UnlockMainMenuMusic(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasMainMenuMusic && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasMainMenuMusic = true;
        }
    }
    public void UnlockBattleMusic(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasBattleMusic && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasBattleMusic = true;
        }
    }
    public void UnlockShopMusic(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasShopMusic && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasShopMusic = true;
        }
    }
    public void UnlockSettingsMusic(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasSettingsMusic && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasSettingsMusic = true;
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// SEASONAL TAB
    public void SeasonalHeroSkin(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasSeasonalHeroSkin && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasSeasonalHeroSkin = true;
        }
    }
    public void SeasonalCursorSkin(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasSeasonalCursorSkin && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasSeasonalCursorSkin = true;
        }
    }
    public void SeasonalWallpaperSkin(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasSeasonalWallpaperSkin && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasSeasonalWallpaperSkin = true;
        }
    }
    public void SeasonalBossSkin(GameObject target)
    {
        float itemPrice = target.GetComponent<ShopPurchasable>().itemPrice;
        if (!contentChecker.hasSeasonalBossSkin && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasSeasonalBossSkin = true;
        }
    }

}
