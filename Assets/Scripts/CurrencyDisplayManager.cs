using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CurrencyDisplayManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField]Game_behaviour game_Behaviour;

    [Header("Main Shop")]
    public TMP_Text playerMoneyMainShop;
    public TMP_Text playerGemsMainShop;
    public TMP_Text playerDiamondsMainShop;
    [Header("The Game")]
    public TMP_Text playerMoneyGame;
    public TMP_Text playerGemsGame;
    public TMP_Text playerDiamondsGame;
    [Header("Locked Shop")]
    public TMP_Text purchaseNowButton;
    public TMP_Text lockedShopMoney;
    [Header("Shop Tabs")]
    public TMP_Text playerMoneyGameTab;
    public TMP_Text playerMoneySkinsTab;
    public TMP_Text playerMoneySFXTab;
    public TMP_Text playerMoneySeasonalTab;
    public TMP_Text playerMoneyDLCTab;
    public TMP_Text playerMoneyBPTab;
    [Header("Battle Pass")]
    public Slider progressSlider;
    public TMP_Text bpProgress;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerMoneyMainShop.text = "$" + gameManager.playerMoney.ToString();
        playerGemsMainShop.text = gameManager.playerGems.ToString();
        playerDiamondsMainShop.text = gameManager.playerDiamonds.ToString();
        playerMoneyGame.text = "$" + gameManager.playerMoney.ToString();
        playerGemsGame.text = gameManager.playerGems.ToString();
        playerDiamondsGame.text = gameManager.playerDiamonds.ToString();
        if (gameManager.playerMoney >= 20)
        {
            purchaseNowButton.text = "PURCHASE NOW!";
            purchaseNowButton.color = Color.green;
        }
        else
        {
            purchaseNowButton.text = "NOT ENOUGH MONEY!";
            purchaseNowButton.color = Color.red;

        }
        lockedShopMoney.text = "YOUR BALANCE: $" + gameManager.playerMoney.ToString();
        playerMoneyGameTab.text = "$" + gameManager.playerMoney.ToString();
        playerMoneySkinsTab.text = "$" + gameManager.playerMoney.ToString();
        playerMoneySFXTab.text = "$" + gameManager.playerMoney.ToString();
        playerMoneySeasonalTab.text = "$" + gameManager.playerMoney.ToString();
        playerMoneyDLCTab.text = "$" + gameManager.playerMoney.ToString();
        playerMoneyBPTab.text = "$" + gameManager.playerMoney.ToString();
        progressSlider.value = game_Behaviour.defeatedEnemies;
        bpProgress.text = game_Behaviour.defeatedEnemies.ToString();

    }


}
