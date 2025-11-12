using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CurrencyDisplayManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField]Game_behaviour game_Behaviour;

    public GameObject MainShop;
    public GameObject MainMenu;
    public GameObject GameWindow;
    public GameObject SettingsWindow;

    [Header("Main Shop")]
    public TMP_Text playerMoneyMainShop;
    public TMP_Text playerGemsMainShop;
    public TMP_Text playerDiamondsMainShop;
    [Header("The Game")]
    public TMP_Text playerMoneyGame;
    public GameObject playerMoneyGameObject;
    public TMP_Text playerGemsGame;
    public GameObject playerGemsGameObject;
    public TMP_Text playerDiamondsGame;
    public GameObject playerDiamondsGameObject;
    [Header("Locked Shop")]
    public TMP_Text purchaseNowButton;
    public TMP_Text lockedShopMoney;
    [Header("Shop Tabs")]
    public TMP_Text playerShopMoney;
    public TMP_Text playerShopGems;
    public TMP_Text playerShopDiamonds;
    [Header("Battle Pass")]
    public Slider progressSlider;
    public TMP_Text bpProgress;
    [Header("Banking App")]
    public TMP_Text bankAccount;
    public TMP_Text bankApp;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerMoneyMainShop.text = "$" + gameManager.playerMoney.ToString("F2");
        playerGemsMainShop.text = "G " + gameManager.playerGems.ToString();
        playerDiamondsMainShop.text = "D " + gameManager.playerDiamonds.ToString();
        playerMoneyGame.text = "$" + gameManager.playerMoney.ToString("F2");
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
        lockedShopMoney.text = "YOUR BALANCE: $" + gameManager.playerMoney.ToString("F2");

        playerShopMoney.text = "$" + gameManager.playerMoney.ToString("F0");
        playerShopGems.text = "G " + gameManager.playerGems.ToString("F0");
        playerShopDiamonds.text = "D " + gameManager.playerDiamonds.ToString("F0");

        progressSlider.value = game_Behaviour.defeatedEnemies;
        bpProgress.text = game_Behaviour.defeatedEnemies.ToString();

        ////////////Banking App
        bankAccount.text = "$" + gameManager.playerMoney.ToString("F2");
        bankApp.text = "$" + gameManager.playerMoney.ToString("F2");

        if (MainShop.activeSelf || MainMenu.activeSelf || GameWindow.activeSelf || SettingsWindow.activeSelf)
        {
            //playerMoneyMainShop.enabled = true;
            //playerGemsMainShop.enabled = true;
            //playerDiamondsMainShop.enabled = true;

            playerMoneyGameObject.SetActive(false);
            playerGemsGameObject.SetActive(false);
            playerDiamondsGameObject.SetActive(false);
        }
        else
        {
            //playerMoneyMainShop.enabled = false;
            //playerGemsMainShop.enabled = false;
            //playerDiamondsMainShop.enabled = false;

            playerMoneyGameObject.SetActive(value: true);
            playerGemsGameObject.SetActive(true);
            playerDiamondsGameObject.SetActive(true);
        }
    }


}
