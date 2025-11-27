using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTabUnlocker : MonoBehaviour
{
    ContentChecker contentChecker;
    GameManager gameManager;
    Game_behaviour gameBehaviour;
    ShopPurchasable shopPurchasable;

    public AudioSource purchaseSound;
    public AudioSource playAudioSource;
    public AudioSource mainMenuAudioSource;
    public AudioClip purchaseClip;
    public AudioClip purchaseClip2;
    public AudioSource denyPurchaseSound;
    public AudioClip denyPurchaseClip;
    public AudioClip weaponHit;
    public AudioClip weaponSwing;
    public AudioClip gemDrop;
    public AudioClip diamondDrop;
    [Header("Game Tab")]
    public List<GameObject> lockedSettings;
    public List<GameObject> lockedGameTitleOptions;
    public List<GameObject> lockedGraphicOptions;
    public List<GameObject> lockedCurrencyOptions;

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
    public List<AudioClip> monsterSounds;
    public List<Sprite> weaponSkins;
    public List<Sprite> backgroundSkins;

    private void Awake()
    {
        contentChecker = FindObjectOfType<ContentChecker>();
        gameManager = FindObjectOfType<GameManager>();
        gameBehaviour = FindObjectOfType<Game_behaviour>();
    }
    void Start()
    {
        //gameBackground.enabled = false;

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
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            gameManager.expensesAmount += itemPrice;
            settingsLock.SetActive(false);
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
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
            purch.PurchaseItem();
            contentChecker.hasSettings = true;
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip); 
            }
        }
    }
    public void UnlockGameOptions(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasGameTitle && gameManager.playerMoney >= itemPrice)
        {
            gameTitle.text = "MTX Sim";
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
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
            purch.PurchaseItem();
            contentChecker.hasGameTitle = true;
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void UnlockGraphicOptions(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasGraphicsOptions && gameManager.playerMoney >= itemPrice)
        {
            gameManager.expensesAmount += itemPrice;
            graphicsOptionsBlock.SetActive(false);
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
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
            purch.PurchaseItem();
            contentChecker.hasGraphicsOptions = true;
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void UnlockCurrencyOptions(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasCurrency && gameManager.playerMoney >= itemPrice)
        {
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
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
            purch.PurchaseItem();
            contentChecker.hasCurrency = true;
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void UnlockMenuBg(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasMenuBg && contentChecker.hasSettings && gameManager.playerMoney >= itemPrice)
        {
            gameBackground.enabled = true;
            contentChecker.hasMenuBg = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            denyPurchaseSound.PlayOneShot(denyPurchaseClip);
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void GameTitle(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void MenuBackground(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            menuBackground.enabled = true;
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void Score(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            scoreObject.SetActive(true);
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }

    }
    public void ClicksCount(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            clicksObject.SetActive(true);
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
            purch.WiggleItem();
        }

    }
    public void DesktopIcon(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            desktopIcon.color = new Color32(255, 255, 255, 255);
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void QuitButton(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            quitButton.SetActive(true);
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void FPScounter(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            fpsCounter.SetActive(true);
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void TutorialBlock(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            tutorialBlock.SetActive(false);
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// SFX and MUSIC OPTIONS
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip); 
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void UnlockWeaponHit(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasWeaponHit && gameManager.playerMoney >= itemPrice)
        {
            //more content to do here
            contentChecker.hasWeaponHit = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void UnlockBattleAmbience(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasBattleAmbience && gameManager.playerMoney >= itemPrice)
        {
            gameBehaviour.battleIntens1.volume = 1f;
            contentChecker.hasBattleAmbience = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void UnlockMainMenuMusic(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasMainMenuMusic && gameManager.playerMoney >= itemPrice)
        {
            mainMenuAudioSource.volume = 1f;
            contentChecker.hasMainMenuMusic = true;
            purch.PurchaseItem();
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    //public void UnlockShopMusic(GameObject target)
    //{
    //    var purch = target.GetComponent<ShopPurchasable>();
    //    float itemPrice = purch.itemPrice;
    //    if (!contentChecker.hasShopMusic && gameManager.playerMoney >= itemPrice)
    //    {
    //        //more content to do here
    //        contentChecker.hasShopMusic = true;
    //        purch.PurchaseItem();
    //        gameManager.expensesAmount += itemPrice;
    //        if (contentChecker.hasUISFX)
    //        {
    //            purchaseSound.PlayOneShot(purchaseClip);
    //        }

    //    }
    //    else
    //    {
    //        purch.WiggleItem();
    //        if (contentChecker.hasUISFX)
    //        {
    //            denyPurchaseSound.PlayOneShot(denyPurchaseClip);
    //        }
    //    }
    //}
    //public void UnlockSettingsMusic(GameObject target)
    //{
    //    var purch = target.GetComponent<ShopPurchasable>();
    //    float itemPrice = purch.itemPrice;
    //    if (!contentChecker.hasSettingsMusic && gameManager.playerMoney >= itemPrice)
    //    {
    //        //more content to do here
    //        contentChecker.hasSettingsMusic = true;
    //        purch.PurchaseItem();
    //        gameManager.expensesAmount += itemPrice;
    //        if (contentChecker.hasUISFX)
    //        {
    //            purchaseSound.PlayOneShot(purchaseClip);
    //        }

    //    }
    //    else
    //    {
    //        purch.WiggleItem();
    //        if (contentChecker.hasUISFX)
    //        {
    //            denyPurchaseSound.PlayOneShot(denyPurchaseClip);
    //        }
    //    }
    //}
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// SEASONAL TAB
    public void SeasonalHeroSkin(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasSeasonalHeroSkin && gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            //more content to do here
            contentChecker.hasSeasonalHeroSkin = true;
            purch.PurchaseItemGems();
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            purch.PurchaseItemGems();
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            purch.PurchaseItemGems();
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
            purch.PurchaseItemGems();

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////// GRAPHICS OPTIONS
    public void HighGraphics(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasGraphicsHigh && contentChecker.hasGraphicsOptions && gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            highGraphicsBlock.SetActive(false);
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void UltraGraphics(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasGraphicsUltra && contentChecker.hasGraphicsOptions && gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            ultraGraphicsBlock.SetActive(false);
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void EpicGraphics(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasGraphicsEpic && contentChecker.hasGraphicsOptions && gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            epicGraphicsBlock.SetActive(false);
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }

    }

    public void PostProcessGraphics(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (!contentChecker.hasPostProcess && contentChecker.hasGraphicsOptions && gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            postProcessGraphicsBlock.SetActive(false);
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////// FEATURED SHOP MAIN
    public void AutoClick(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            gameBehaviour.hasAutoClick = true;
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void DamageUp(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            gameBehaviour.clickDmg *= 2f;
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void PopUpDamage(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerMoney >= itemPrice)
        {
            purch.PurchaseItem();
            gameBehaviour.hasPopUpUpgrade = true;
            gameManager.expensesAmount += itemPrice;
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// SKINS TAB
    public void PlayBg1(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (backgroundSkins != null && backgroundSkins.Count > 0)
            {
                if (gameBehaviour.backgroundSkin == null) gameBehaviour.backgroundSkin = new List<Sprite>();
                gameBehaviour.backgroundSkin.Add(backgroundSkins[0]);
                gameManager.expensesAmount += itemPrice;
                purch.PurchaseItemGems();


            }
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void PlayBg2(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (backgroundSkins != null && backgroundSkins.Count > 1)
            {
                if (gameBehaviour.backgroundSkin == null) gameBehaviour.backgroundSkin = new List<Sprite>();
                gameBehaviour.backgroundSkin.Add(backgroundSkins[1]);
                gameManager.expensesAmount += itemPrice;
                purch.PurchaseItemGems();


            }
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void PlayBg3(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (backgroundSkins != null && backgroundSkins.Count > 2)
            {
                if (gameBehaviour.backgroundSkin == null) gameBehaviour.backgroundSkin = new List<Sprite>();
                gameBehaviour.backgroundSkin.Add(backgroundSkins[2]);
                gameManager.expensesAmount += itemPrice;
                purch.PurchaseItemGems();


            }
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void PlayBg4(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (backgroundSkins != null && backgroundSkins.Count > 3)
            {
                if (gameBehaviour.backgroundSkin == null) gameBehaviour.backgroundSkin = new List<Sprite>();
                gameBehaviour.backgroundSkin.Add(backgroundSkins[3]);
                gameManager.expensesAmount += itemPrice;
                purch.PurchaseItemGems();


            }
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void PlayBg5(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {
            if (backgroundSkins != null && backgroundSkins.Count > 4)
            {
                if (gameBehaviour.backgroundSkin == null) gameBehaviour.backgroundSkin = new List<Sprite>();
                gameBehaviour.backgroundSkin.Add(backgroundSkins[4]);
                gameManager.expensesAmount += itemPrice;
                purch.PurchaseItemGems();


            }
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void PlayerSkin1(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {

            if (playerSkins != null && playerSkins.Count > 0)
            {
                if (gameBehaviour.playerSkin == null) gameBehaviour.playerSkin = new List<Sprite>();
                gameBehaviour.playerSkin.Add(playerSkins[0]);
                purch.PurchaseItemGems();


            }
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void PlayerSkin2(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;
        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {

            // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
            if (playerSkins != null && playerSkins.Count > 1)
            {
                if (gameBehaviour.playerSkin == null) gameBehaviour.playerSkin = new List<Sprite>();
                gameBehaviour.playerSkin.Add(playerSkins[1]);
                purch.PurchaseItemGems();


            }
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void PlayerSkin3(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;

        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {

            // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
            if (playerSkins != null && playerSkins.Count > 2)
            {
                if (gameBehaviour.playerSkin == null) gameBehaviour.playerSkin = new List<Sprite>();
                gameBehaviour.playerSkin.Add(playerSkins[2]);
                purch.PurchaseItemGems();


            }
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void PlayerSkin4(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;

        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {

            // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
            if (playerSkins != null && playerSkins.Count > 3)
            {
                if (gameBehaviour.playerSkin == null) gameBehaviour.playerSkin = new List<Sprite>();
                gameBehaviour.playerSkin.Add(playerSkins[3]);
                purch.PurchaseItemGems();


            }
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }
    public void PlayerSkin5(GameObject target)
    {
        var purch = target.GetComponent<ShopPurchasable>();
        float itemPrice = purch.itemPrice;

        if (gameManager.playerGems >= itemPrice && contentChecker.hasCurrencyGems)
        {

            // Take first sprite from monsterSkins and add to Game_behaviour.bossSprites
            if (playerSkins != null && playerSkins.Count > 4)
            {
                if (gameBehaviour.playerSkin == null) gameBehaviour.playerSkin = new List<Sprite>();
                gameBehaviour.playerSkin.Add(playerSkins[4]);
                purch.PurchaseItemGems();


            }
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
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
            if (monsterSounds != null && monsterSounds.Count > 0)
            {
                if (gameBehaviour.bossDeathSounds == null) gameBehaviour.bossDeathSounds = new List<AudioClip>();
                gameBehaviour.bossDeathSounds.Add(monsterSounds[0]);
            }
            purch.PurchaseItemGems();

            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (monsterSounds != null && monsterSounds.Count > 1)
            {
                if (gameBehaviour.bossDeathSounds == null) gameBehaviour.bossDeathSounds = new List<AudioClip>();
                gameBehaviour.bossDeathSounds.Add(monsterSounds[1]);
            }
            purch.PurchaseItemGems();

            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (monsterSounds != null && monsterSounds.Count > 2)
            {
                if (gameBehaviour.bossDeathSounds == null) gameBehaviour.bossDeathSounds = new List<AudioClip>();
                gameBehaviour.bossDeathSounds.Add(monsterSounds[2]);
            }
            purch.PurchaseItemGems();

            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (monsterSounds != null && monsterSounds.Count > 3)
            {
                if (gameBehaviour.bossDeathSounds == null) gameBehaviour.bossDeathSounds = new List<AudioClip>();
                gameBehaviour.bossDeathSounds.Add(monsterSounds[3]);
            }
            purch.PurchaseItemGems();

            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            if (monsterSounds != null && monsterSounds.Count > 4)
            {
                if (gameBehaviour.bossDeathSounds == null) gameBehaviour.bossDeathSounds = new List<AudioClip>();
                gameBehaviour.bossDeathSounds.Add(monsterSounds[4]);
            }
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
            purch.PurchaseItemGems();
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            purch.PurchaseItemGems();
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            purch.PurchaseItemGems();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(purchaseClip);
            }

        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            purch.PurchaseItemGems();
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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
            purch.PurchaseItemGems();
            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
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

            if (contentChecker.hasUISFX)
            {
                purchaseSound.PlayOneShot(purchaseClip);
            }
        }
        else
        {
            purch.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                denyPurchaseSound.PlayOneShot(denyPurchaseClip);
            }
        }
    }

}
