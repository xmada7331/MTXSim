using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentChecker : MonoBehaviour
{
    GameManager gameManager;
    Game_behaviour gameBehaviour;

    [Header("Main Shop")]
    public bool unlockedMainShop = false;
    [Header("Game Tab")]
    public bool hasSettings = false;
    public bool hasPauseMenu = false;
    public bool hasVolume = false;
    public bool hasMouseSens = false;
    public bool hasFullscreen = false;
    public bool hasFrameLimit = false;
    public bool hasResolutionChange = false;
    public bool hasVsync = false;
    public bool hasLanguage = false;
    public bool hasNSFW = false;
    public bool hasGameTitle = false;
    public bool hasMenuBg = false;
    public bool hasTutorial = false;
    public bool hasClicksCounter = false;
    public bool hasDesktopIcon = false;
    public bool hasQuitButton = false;
    public bool hasGraphicsOptions = false;
    public bool hasPostProcess = false;
    public bool hasGraphicsMedium = false;
    public bool hasGraphicsHigh = false;
    public bool hasGraphicsUltra = false;
    public bool hasGraphicsEpic = false;
    public bool hasGraphicsPro = false;
    public bool hasGraphicsTrueFan = false;
    public bool hasCurrency = false;
    public bool hasCurrencyDollars = false;
    public bool hasCurrencyGems = false;
    public bool hasCurrencyDiamonds = false;
    [Header("SFX Tab")]
    public bool hasSFX = false;
    public bool hasMusic = false;
    public bool hasMouseClick = false;
    public bool hasUISFX = false;
    public bool hasBossDeath = false;
    public bool hasNextLevel = false;
    public bool hasWeaponSwing = false;
    public bool hasGemDrop = false;
    public bool hasDiamondDrop = false;
    public bool hasItemBuy = false;
    public bool hasBattleAmbience = false;
    public bool hasBattleMusic = false;
    public bool hasMainMenuMusic  = false;
    public bool hasShopMusic  = false;
    public bool hasSettingsMusic  = false;
    [Header("Seasonal Tab")]
    public bool hasSeasonalHeroSkin = false;
    public bool hasSeasonalCursorSkin = false;
    public bool hasSeasonalWallpaperSkin = false;
    public bool hasSeasonalBossSkin = false;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameBehaviour = FindObjectOfType<Game_behaviour>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
