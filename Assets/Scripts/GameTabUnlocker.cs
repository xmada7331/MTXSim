using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TMP_Text gameTitle;
    public Image menuBackground;
    public GameObject scoreObject;
    public TMP_Text scoreText;
    public GameObject clicksObject;
    public TMP_Text clicksCount;
    public Image desktopIcon;
    public GameObject quitButton;
    public GameObject fpsCounter;
    public GameObject tutorialBlock;

    public GameObject graphicsOptionsBlock;
    public GameObject highGraphicsBlock;
    public GameObject ultraGraphicsBlock;
    public GameObject epicGraphicsBlock;
    public GameObject postProcessGraphicsBlock;

    //public GameObject currencies;
    public GameObject currencyGemsBlock;
    public GameObject currencyGemsBlock2;
    public GameObject currencyGemsBlock3;
    public GameObject currencyDiamondsBlock;
    public GameObject currencyDiamondsBlock2;
    public GameObject currencyDiamondsBlock3;
    [Header("SFX Tab")]
    public GameObject sfxLock;
    public GameObject musicLock;
    [Header("Skins Tab")]
    public List<Sprite> cursorSkins;
    public List<Sprite> playerSkins;
    public List<Sprite> monsterSkins;
    public List<string> monsterNames;
    public List<Sprite> weaponSkins;

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
            gameManager.expensesAmount += itemPrice;
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
            gameTitle.text = "MTX Sim";
            gameManager.expensesAmount += itemPrice;
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
            gameManager.expensesAmount += itemPrice;
            graphicsOptionsBlock.SetActive(false);
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
            gameManager.expensesAmount += itemPrice;
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
            shopPurchasable.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }

    public void UnlockVolume(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasVolume && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            if (volumeLock != null) volumeLock.SetActive(false);
            contentChecker.hasVolume = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockMouseSens(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasMouseSens && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            if (mouseSensLock != null) mouseSensLock.SetActive(false);
            contentChecker.hasMouseSens = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockFullscreen(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;

        if (!contentChecker.hasFullscreen && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            fullscreenLock.SetActive(false);
            contentChecker.hasFullscreen = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void UnlockFrameLimit(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;

        if (!contentChecker.hasFrameLimit && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            frameLimitLock.SetActive(false);
            contentChecker.hasFrameLimit = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void UnlockResolutionChange(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;

        if (!contentChecker.hasResolutionChange && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            resolutionLock.SetActive(false);
            contentChecker.hasResolutionChange = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void UnlockVsync(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;

        if (!contentChecker.hasVsync && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            vsyncLock.SetActive(false);
            contentChecker.hasVsync = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void UnlockLanguage(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;

        if (!contentChecker.hasLanguage && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            languageLock.SetActive(false);
            contentChecker.hasLanguage = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void UnlockNSFW(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;

        if (!contentChecker.hasNSFW && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            nsfwLock.SetActive(false);
            contentChecker.hasNSFW = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
        else
        {
            Debug.Log("not enough money / purchased already");
        }
    }
    public void GameTitle(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {

            gameManager.expensesAmount += itemPrice;
        }
    }
    public void MenuBackground(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            menuBackground.enabled = true;
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void Score(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            scoreObject.SetActive(true);
            gameManager.expensesAmount += itemPrice;
        }

    }
    public void ClicksCount(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            clicksObject.SetActive(true);
            gameManager.expensesAmount += itemPrice;
        }

    }
    public void DesktopIcon(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            desktopIcon.color = new Color32(255, 255, 255, 255);
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void QuitButton(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            quitButton.SetActive(true);
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void FPScounter(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            fpsCounter.SetActive(true);
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void TutorialBlock(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            tutorialBlock.SetActive(false);
            gameManager.expensesAmount += itemPrice;
        }
    }

    public void CurrencyGems(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasCurrencyGems && gameManager.playerMoney >= itemPrice && purch.isPurchasable)
        {
            currencyGemsBlock.SetActive(false);
            currencyGemsBlock2.SetActive(false);
            currencyGemsBlock3.SetActive(false);
            contentChecker.hasCurrencyGems = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void CurrencyDiamonds(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasCurrencyDiamonds && gameManager.playerMoney >= itemPrice && purch.isPurchasable)
        {
            currencyDiamondsBlock.SetActive(false);
            currencyDiamondsBlock2.SetActive(false);
            currencyDiamondsBlock3.SetActive(false);
            contentChecker.hasCurrencyDiamonds = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// VFX and MUSIC OPTIONS
    public void UnlockSFX(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasSFX && gameManager.playerMoney >= itemPrice)
        {
            sfxLock.SetActive(false);
            contentChecker.hasSFX = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockMusic(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasMusic && gameManager.playerMoney >= itemPrice)
        {
            musicLock.SetActive(false);
            contentChecker.hasMusic = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockMouseClick(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasMouseClick && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasMouseClick = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockUISFX(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasUISFX && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasUISFX = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockBossDeath(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasBossDeath && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasBossDeath = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockNextLevel(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasNextLevel && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasNextLevel = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockWeaponSwing(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasWeaponSwing && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasWeaponSwing = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockGemDrop(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasGemDrop && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasGemDrop = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockDiamondDrop(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasDiamondDrop && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasDiamondDrop = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockItemBuy(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasItemBuy && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasItemBuy = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockBattleAmbience(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasBattleAmbience && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasBattleAmbience = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockMainMenuMusic(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasMainMenuMusic && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasMainMenuMusic = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockBattleMusic(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasBattleMusic && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasBattleMusic = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockShopMusic(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasShopMusic && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasShopMusic = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void UnlockSettingsMusic(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasSettingsMusic && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasSettingsMusic = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// SEASONAL TAB
    public void SeasonalHeroSkin(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasSeasonalHeroSkin && gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //more content to do here
            contentChecker.hasSeasonalHeroSkin = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void SeasonalCursorSkin(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasSeasonalCursorSkin && gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //more content to do here
            contentChecker.hasSeasonalCursorSkin = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void SeasonalWallpaperSkin(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasSeasonalWallpaperSkin && gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //more content to do here
            contentChecker.hasSeasonalWallpaperSkin = true;
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void SeasonalBossSkin(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasSeasonalBossSkin && gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //more content to do here
            contentChecker.hasSeasonalBossSkin = true;
            gameManager.expensesAmount += itemPrice;
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////// GRAPHICS OPTIONS
    public void HighGraphics()
    {
        highGraphicsBlock.SetActive(false);
    }
    public void UltraGraphics()
    {
        ultraGraphicsBlock.SetActive(false);
    }
    public void EpicGraphics()
    {
        epicGraphicsBlock.SetActive(false);
    }
    public void PostProcessGraphics()
    {
        postProcessGraphicsBlock.SetActive(false);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////// FEATURED SHOP MAIN
    public void AutoClick(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            gameBehaviour.hasAutoClick = true;
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void DamageUp(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            gameBehaviour.clickDmg *= 1.5f;
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void PopUpDamage(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            gameBehaviour.hasPopUpUpgrade = true;
            gameManager.expensesAmount += itemPrice;
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// SKINS TAB
    public void Cursor1(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //logic here
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void Cursor2(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //logic here
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void Cursor3(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //logic here
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void PlayBg1(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //logic here
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void PlayBg2(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //logic here
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void PlayBg3(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //logic here
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void PlayBg4(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //logic here
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void PlayBg5(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //logic here
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void PlayerSkin1(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
            {

                // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
                if (playerSkins != null && playerSkins.Count > 0)
                {
                    if (gameBehaviour.playerSkin == null) gameBehaviour.playerSkin = new List<Sprite>();
                    gameBehaviour.playerSkin.Add(playerSkins[0]);
                }
                gameManager.expensesAmount += itemPrice;
            }
        }
    }
    public void PlayerSkin2(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
            {

                // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
                if (playerSkins != null && playerSkins.Count > 1)
                {
                    if (gameBehaviour.playerSkin == null) gameBehaviour.playerSkin = new List<Sprite>();
                    gameBehaviour.playerSkin.Add(playerSkins[1]);
                }
                gameManager.expensesAmount += itemPrice;
            }
        }
    }
    public void PlayerSkin3(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
            {

                // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
                if (playerSkins != null && playerSkins.Count > 2)
                {
                    if (gameBehaviour.playerSkin == null) gameBehaviour.playerSkin = new List<Sprite>();
                    gameBehaviour.playerSkin.Add(playerSkins[2]);
                }
                gameManager.expensesAmount += itemPrice;
            }
        }
    }
    public void PlayerSkin4(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
            {

                // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
                if (playerSkins != null && playerSkins.Count > 3)
                {
                    if (gameBehaviour.playerSkin == null) gameBehaviour.playerSkin = new List<Sprite>();
                    gameBehaviour.playerSkin.Add(playerSkins[3]);
                }
                gameManager.expensesAmount += itemPrice;
            }
        }
    }
    public void PlayerSkin5(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
            {

                // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
                if (playerSkins != null && playerSkins.Count > 4)
                {
                    if (gameBehaviour.playerSkin == null) gameBehaviour.playerSkin = new List<Sprite>();
                    gameBehaviour.playerSkin.Add(playerSkins[4]);
                }
                gameManager.expensesAmount += itemPrice;
            }
        }
    }
    public void MonsterSkin1(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {

            // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
            if (monsterSkins != null && monsterSkins.Count > 0)
            {
                if (gameBehaviour.bossSprites == null) gameBehaviour.bossSprites = new List<Sprite>();
                gameBehaviour.bossSprites.Add(monsterSkins[0]);
            }
            // Take first name from monsterNames and add to Game_behaviour.bossNames
            if (monsterNames != null && monsterNames.Count > 0)
            {
                if (gameBehaviour.bossNames == null) gameBehaviour.bossNames = new List<string>();
                gameBehaviour.bossNames.Add(monsterNames[0]);
            }
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void MonsterSkin2(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {

            // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
            if (monsterSkins != null && monsterSkins.Count > 1)
            {
                if (gameBehaviour.bossSprites == null) gameBehaviour.bossSprites = new List<Sprite>();
                gameBehaviour.bossSprites.Add(monsterSkins[1]);
            }
            // Take first name from monsterNames and add to Game_behaviour.bossNames
            if (monsterNames != null && monsterNames.Count > 1)
            {
                if (gameBehaviour.bossNames == null) gameBehaviour.bossNames = new List<string>();
                gameBehaviour.bossNames.Add(monsterNames[1]);
            }
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void MonsterSkin3(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {

            // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
            if (monsterSkins != null && monsterSkins.Count > 2)
            {
                if (gameBehaviour.bossSprites == null) gameBehaviour.bossSprites = new List<Sprite>();
                gameBehaviour.bossSprites.Add(monsterSkins[2]);
            }
            // Take first name from monsterNames and add to Game_behaviour.bossNames
            if (monsterNames != null && monsterNames.Count > 2)
            {
                if (gameBehaviour.bossNames == null) gameBehaviour.bossNames = new List<string>();
                gameBehaviour.bossNames.Add(monsterNames[2]);
            }
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void MonsterSkin4(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {

            // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
            if (monsterSkins != null && monsterSkins.Count > 3)
            {
                if (gameBehaviour.bossSprites == null) gameBehaviour.bossSprites = new List<Sprite>();
                gameBehaviour.bossSprites.Add(monsterSkins[3]);
            }
            // Take first name from monsterNames and add to Game_behaviour.bossNames
            if (monsterNames != null && monsterNames.Count > 3)
            {
                if (gameBehaviour.bossNames == null) gameBehaviour.bossNames = new List<string>();
                gameBehaviour.bossNames.Add(monsterNames[3]);
            }
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void MonsterSkin5(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {

            // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
            if (monsterSkins != null && monsterSkins.Count > 4)
            {
                if (gameBehaviour.bossSprites == null) gameBehaviour.bossSprites = new List<Sprite>();
                gameBehaviour.bossSprites.Add(monsterSkins[4]);
            }
            // Take first name from monsterNames and add to Game_behaviour.bossNames
            if (monsterNames != null && monsterNames.Count > 4)
            {
                if (gameBehaviour.bossNames == null) gameBehaviour.bossNames = new List<string>();
                gameBehaviour.bossNames.Add(monsterNames[4]);
            }
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void WeaponSkin1(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (weaponSkins != null && weaponSkins.Count > 0)
            {
                if (gameBehaviour.weaponSkin == null) gameBehaviour.weaponSkin = new List<Sprite>();
                gameBehaviour.weaponSkin.Add(weaponSkins[0]);
            }
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void WeaponSkin2(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (weaponSkins != null && weaponSkins.Count > 1)
            {
                if (gameBehaviour.weaponSkin == null) gameBehaviour.weaponSkin = new List<Sprite>();
                gameBehaviour.weaponSkin.Add(weaponSkins[1]);
            }
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void WeaponSkin3(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (weaponSkins != null && weaponSkins.Count > 2)
            {
                if (gameBehaviour.weaponSkin == null) gameBehaviour.weaponSkin = new List<Sprite>();
                gameBehaviour.weaponSkin.Add(weaponSkins[2]);
            }
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void WeaponSkin4(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (weaponSkins != null && weaponSkins.Count > 3)
            {
                if (gameBehaviour.weaponSkin == null) gameBehaviour.weaponSkin = new List<Sprite>();
                gameBehaviour.weaponSkin.Add(weaponSkins[3]);
            }
            gameManager.expensesAmount += itemPrice;
        }
    }
    public void WeaponSkin5(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (weaponSkins != null && weaponSkins.Count > 4)
            {
                if (gameBehaviour.weaponSkin == null) gameBehaviour.weaponSkin = new List<Sprite>();
                gameBehaviour.weaponSkin.Add(weaponSkins[4]);
            }
            gameManager.expensesAmount += itemPrice;
        }
    }

}
