using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public UltEvents.UltEvent myEvent;
    ToolbarSystemIcon toolbarSystemIcon;
    ShopPurchasable shopPurchasable;
    ContentChecker contentChecker;
    EndGameManager endGameManager;




    public List<GameObject> antiClickObject;

    public float playerMoney;
    public float playerGems;
    public float playerDiamonds;
    private bool diamondsEndingActive = false;


    public GameObject LockMenu;
    public GameObject bankWidget;
    public GameObject stockWidget;
    public GameObject freeCoinWidget;
    public GameObject adsSystem;
    public Image cursorHoverImage;
    public TMP_Text cursorHoverText;
    public GameObject pauseMenu;
    public TMP_Text pauseMenuCounts;
    private int pauseMenuOpenCount = 5;
    private bool canOpenPauseMenu = true;
    public GameObject startGameObject;
    [Header("CameraApp")]
    public bool hasCamera = false;
    public GameObject cameraLock;
    [Header("DigistoreApp")]
    public GameObject surveysIcon;
    public GameObject bankIcon;
    public GameObject stockIcon;
    public GameObject shopIcon;
    public GameObject cameraIcon;
    public GameObject freeCoinIcon;
    public GameObject feetPicsIcon;
    public GameObject gaymTesterIcon;
    [Header("BankingApp")]
    public TMP_Text incomeText;
    public float incomeAmount = 0;
    public TMP_Text expensesText;
    public float expensesAmount = 0;
    public Image bankWidgetImage;
    public Sprite bankWidgetPurchased;
    [Header("Toolbar")]
    public GameObject errorWindow;
    public GameObject debtObject;
    public Slider debtSlider;
    public TMP_Text debtTimer;
    private bool debtTriggered = false;
    public TMP_Text timeDisplay;
    public TMP_Text dateDisplay;
    [Header("Shop")]
    public TMP_Text shopDescription;
    [Header("StockApp")]
    public Image stockWidgetImage;
    public Sprite stockWidgetPurchased;
    [Header("FreeCoin")]
    public Image freeCoinWidgetImage;
    public Sprite freeCoinWidgetPurchased;


    private void Awake()
    {
        contentChecker = FindObjectOfType<ContentChecker>();
        toolbarSystemIcon = FindObjectOfType<ToolbarSystemIcon>();
        endGameManager = FindObjectOfType<EndGameManager>();
    }
    void Start()
    {
        startGameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        incomeText.text = "+$" + incomeAmount.ToString("F2");
        expensesText.text = "-$" + expensesAmount.ToString("F2");
        timeDisplay.text = System.DateTime.Now.ToString("HH:mm:ss");
        dateDisplay.text = System.DateTime.Now.ToString("dd/MM/yyyy");
        if (playerMoney < 0 && !debtTriggered)
        {
            TriggerDebt();
        }
        else
        {
            if (playerMoney >= 0 && debtTriggered)
            {
                debtObject.SetActive(false);
                StopCoroutine(DebtTimer());
                debtTriggered = false;
            }
        }

        if (playerDiamonds >= 10 && !diamondsEndingActive && contentChecker.hasCurrencyDiamonds)
        {
            diamondsEndingActive = true;
            endGameManager.DiamondGameOver();
        }

        if(Input.GetKeyDown(KeyCode.Escape) && canOpenPauseMenu)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (pauseMenu.activeSelf)
            {
                Time.timeScale = 0f;    
                pauseMenuOpenCount--;
                pauseMenuCounts.text = pauseMenuOpenCount.ToString();
                if (pauseMenuOpenCount <= 0)
                {
                    canOpenPauseMenu = false;
                }
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void DisableAntiClickObjects()
    {
        foreach (GameObject obj in antiClickObject)
        {
            obj.SetActive(false);
        }
    }
    public void UnlockShop(float amount)
    {
        if (playerMoney >= amount)
        {
            LockMenu.SetActive(false);
            playerMoney -= amount;
            contentChecker.unlockedMainShop = true;
            expensesAmount += amount;
        }
        else
        {
            Debug.Log("not enough money");
        }
    }

    public void PurchaseAntivirus(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            adsSystem.GetComponent<AdsSystem>().enabled = false;
            adsSystem.GetComponentInChildren<SpriteRenderer>().enabled = false;
            shopPurchasable.itemPurchased = true;
            shopPurchasable.PurchaseItem();
            expensesAmount += shopPurchasable.itemPrice;

        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }

    public void PurchaseCamera(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            hasCamera = true;
            shopPurchasable.PurchaseItem();
            cameraLock.SetActive(false);
            expensesAmount += shopPurchasable.itemPrice;

        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }

    public void UnlockSurveys(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            surveysIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
            expensesAmount += shopPurchasable.itemPrice;
        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }
    public void UnlockBank(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            bankIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
            expensesAmount += shopPurchasable.itemPrice;

        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }
    public void UnlockStock(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            stockIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
            expensesAmount += shopPurchasable.itemPrice;

        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void UnlockFeetPics(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            feetPicsIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
            expensesAmount += shopPurchasable.itemPrice;

        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void UnlockFreeCoin(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            freeCoinIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
            expensesAmount += shopPurchasable.itemPrice;

        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }
    public void UnlockCamera(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            cameraIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
            expensesAmount += shopPurchasable.itemPrice;

        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }
    public void UnlockShopDigiStore(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            shopIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
            expensesAmount += shopPurchasable.itemPrice;

        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }
    public void UnlockGaymTester(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (playerMoney >= shopPurchasable.itemPrice)
        {
            gaymTesterIcon.SetActive(true);
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }

    public void ToolbarLockedApp()
    {
        errorWindow.SetActive(true);
    }
    private void TriggerDebt()
    {
        debtObject.SetActive(true);
        StartCoroutine(DebtTimer());
    }

    private IEnumerator DebtTimer()
    {
        debtTriggered = true;
        debtSlider.maxValue = 60f;
        debtSlider.value = 60f;
        while (debtSlider.value > 0)
        {
            int minutes = Mathf.FloorToInt(debtSlider.value / 60f);
            int seconds = Mathf.FloorToInt(debtSlider.value % 60f);
            debtTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return new WaitForSeconds(1f);
            debtSlider.value -= 1f;
        }
        debtTimer.text = "00:00";
        debtSlider.value = 0f;
        endGameManager.DebtGameOver();

    }

    public void MoveHierarchy(GameObject window)
    {
        window.transform.SetAsLastSibling();
    }








}
