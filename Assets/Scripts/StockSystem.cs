using System.Collections;
using TMPro;
using UnityEngine;

public class StockSystem : MonoBehaviour
{
    GameManager gameManager;
    ShopPurchasable shopPurchasable;

    public AudioSource stockAudioSource;
    public AudioClip selectCoin;
    public AudioClip buyCoin;
    public AudioClip cashoutWallet;
    public float aStockPrice;
    public float bStockPrice;
    public float cStockPrice;

    public float aCoinAmount;
    public float bCoinAmount;
    public float cCoinAmount;

    public TMP_Text coinValue;
    public TMP_Text aCoinInWallet;
    public TMP_Text bCoinInWallet;
    public TMP_Text cCoinInWallet;

    public TMP_Text aCoinWidget;
    public TMP_Text bCoinWidget;
    public TMP_Text cCoinWidget;
    public TMP_Text walletAmountWidget;

    public TMP_Text yourWalletValue;
    public TMP_Text ddcText;
    public TMP_Text bbcText;
    public TMP_Text bgcText;

    private bool isBuyingA;
    private bool isBuyingB;
    private bool isBuyingC;

    public TMP_Text wallet;
    public TMP_Text cashoutText;
    public GameObject notEnoughText;
    public GameObject cashoutWindow;
    public float amountInWallet;





    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        FindMissingReferences();
    }

    private void FindMissingReferences()
    {
        if (yourWalletValue == null || ddcText == null || bbcText == null || bgcText == null)
        {
            TMP_Text[] allTexts = Resources.FindObjectsOfTypeAll<TMP_Text>();
            foreach (var t in allTexts)
            {
                if (yourWalletValue == null && t.gameObject.name == "YourWallet") yourWalletValue = t;
                else if (ddcText == null && t.gameObject.name == "DDCText") ddcText = t;
                else if (bbcText == null && t.gameObject.name == "BBCText") bbcText = t;
                else if (bgcText == null && t.gameObject.name == "BGCText") bgcText = t;
            }
        }
    }
    void Start()
    {
        StartCoroutine(RandomizeAStockPrice());
        StartCoroutine(RandomizeBStockPrice());
        StartCoroutine(RandomizeCStockPrice());
    }

    // Update is called once per frame
    void Update()
    {
        aCoinInWallet.text = aCoinAmount.ToString("F0");
        bCoinInWallet.text = bCoinAmount.ToString("F0");
        cCoinInWallet.text = cCoinAmount.ToString("F0");

        aCoinWidget.text = "$" + aStockPrice.ToString("F4");
        bCoinWidget.text = "$" + bStockPrice.ToString("F4");
        cCoinWidget.text = "$" + cStockPrice.ToString("F4");

        if (ddcText != null)
        {
            ddcText.text = "DDC (" + aCoinAmount.ToString("F0") + "):";
        }
        if (bbcText != null)
        {
            bbcText.text = "BBC (" + bCoinAmount.ToString("F0") + "):";
        }
        if (bgcText != null)
        {
            bgcText.text = "BGC (" + cCoinAmount.ToString("F0") + "):";
        }

        amountInWallet = ((aStockPrice * aCoinAmount) + (bStockPrice * bCoinAmount) + (cStockPrice * cCoinAmount));
        wallet.text = "$" + amountInWallet.ToString("F4");
        cashoutText.text = "Would you like to cashout all stocks worth $" + amountInWallet.ToString("F4") + " for a fee of $50?";



        walletAmountWidget.text = "$" + amountInWallet;

        if (isBuyingA)
        {
            coinValue.text = "Coin Value: $" + aStockPrice.ToString("F4");
            if (yourWalletValue != null)
            {
                yourWalletValue.text = "Your value: $" + (aCoinAmount * aStockPrice).ToString("F4");
            }
            if (aStockPrice <= 0)
            {
                aStockPrice = Random.Range(0.01f, 0.4f);
            }
        }
        else if (isBuyingB)
        {
            coinValue.text = "Coin Value: $" + bStockPrice.ToString("F4");
            if (yourWalletValue != null)
            {
                yourWalletValue.text = "Your value: $" + (bCoinAmount * bStockPrice).ToString("F4");
            }
            if (bStockPrice <= 0)
            {
                bStockPrice = Random.Range(0.01f, 0.4f);
            }
        }
        else if (isBuyingC)
        {
            coinValue.text = "Coin Value: $" + cStockPrice.ToString("F4");
            if (yourWalletValue != null)
            {
                yourWalletValue.text = "Your value: $" + (cCoinAmount * cStockPrice).ToString("F4");
            }
            if (cStockPrice <= 0)
            {
                cStockPrice = Random.Range(0.01f, 0.4f);
            }
        }
    }

    private IEnumerator RandomizeAStockPrice()
    {
        while (true)
        {
            aStockPrice = 1 + Random.Range(-0.1f, .2f);
            yield return new WaitForSeconds(1f); // Refresh every 1 second
        }
    }

    private IEnumerator RandomizeBStockPrice()
    {
        while (true)
        {
            bStockPrice = 1 + Random.Range(-0.15f, .25f);
            yield return new WaitForSeconds(5f); // Refresh every 2 seconds
        }
    }

    private IEnumerator RandomizeCStockPrice()
    {
        while (true)
        {
            cStockPrice = 1 + Random.Range(-0.2f, .3f);
            yield return new WaitForSeconds(10f); // Refresh every 3 seconds
        }
    }

    public void BuyAStock()
    {
        if (!isBuyingA)
        {
            stockAudioSource.PlayOneShot(selectCoin);
        }
        isBuyingA = true;
        isBuyingB = false;
        isBuyingC = false;


    }
    public void BuyBStock()
    {
        if (!isBuyingB)
        {
            stockAudioSource.PlayOneShot(selectCoin);
        }
        isBuyingA = false;
        isBuyingB = true;
        isBuyingC = false;

    }
    public void BuyCStock()
    {
        if (!isBuyingC)
        {
            stockAudioSource.PlayOneShot(selectCoin);
        }
        isBuyingA = false;
        isBuyingB = false;
        isBuyingC = true;

    }

    public void BuyStock()
    {
        if (isBuyingA)
        {
            if (gameManager.playerMoney >= aStockPrice)
            {
                gameManager.playerMoney -= aStockPrice;
                amountInWallet += aStockPrice;
                aCoinAmount++;
                stockAudioSource.PlayOneShot(buyCoin);
                gameManager.expensesAmount += aStockPrice;
                PurchaseHistoryManager.RecordPurchase(aStockPrice, "Stock coin A");
                //Debug.Log("bought A stock");
            }
            else
            {
                //Debug.Log("not enough money");
            }
        }
        else if (isBuyingB)
        {
            if (gameManager.playerMoney >= bStockPrice)
            {
                amountInWallet += bStockPrice;
                bCoinAmount++;
                gameManager.playerMoney -= bStockPrice;
                stockAudioSource.PlayOneShot(buyCoin);
                gameManager.expensesAmount += bStockPrice;
                PurchaseHistoryManager.RecordPurchase(bStockPrice, "Stock coin B");


                //Debug.Log("bought B stock");
            }
            else
            {
                //Debug.Log("not enough money");
            }
        }
        else if (isBuyingC)
        {
            if (gameManager.playerMoney >= cStockPrice)
            {
                amountInWallet += cStockPrice;
                cCoinAmount++;
                gameManager.playerMoney -= cStockPrice;
                stockAudioSource.PlayOneShot(buyCoin);
                gameManager.expensesAmount += cStockPrice;
                PurchaseHistoryManager.RecordPurchase(cStockPrice, "Stock coin C");

                //Debug.Log("bought C stock");
            }
            else
            {
                //Debug.Log("not enough money");
            }
        }
    }

    //public void SellStock()
    //{
    //    if (isBuyingA)
    //    {
    //        if (aCoinAmount > 0)
    //        {
    //            aCoinAmount--;
    //            amountInWallet += aStockPrice;
    //            Debug.Log("sold A stock");
    //        }
    //        else
    //        {
    //            Debug.Log("no stocks to sell");
    //        }
    //    }
    //    else if (isBuyingB)
    //    {
    //        if (bCoinAmount > 0)
    //        {
    //            bCoinAmount--;
    //            amountInWallet += bStockPrice;
    //            Debug.Log("sold B stock");
    //        }
    //        else
    //        {
    //            Debug.Log("no stocks to sell");
    //        }
    //    }
    //    else if (isBuyingC)
    //    {
    //        if (cCoinAmount > 0)
    //        {
    //            cCoinAmount--;
    //            amountInWallet += cStockPrice;
    //            Debug.Log("sold C stock");
    //        }
    //        else
    //        {
    //            Debug.Log("no stocks to sell");
    //        }
    //    }
    //}

    public void CashoutWallet()
    {
        if (amountInWallet >= 1)
        {
            gameManager.incomeAmount += amountInWallet;
            aCoinAmount = 0;
            bCoinAmount = 0;
            cCoinAmount = 0;
            gameManager.playerMoney += amountInWallet - 50;
            //Debug.Log("Cashed out $" + (amountInWallet - 50).ToString("F4"));
            amountInWallet = 0;
            cashoutWindow.SetActive(false);
            stockAudioSource.PlayOneShot(cashoutWallet);
            

        }
        else
        {
            //Debug.Log("Not enough to cashout");
            StartCoroutine(CantCashout());
        }
    }

    private IEnumerator CantCashout()
    {
        notEnoughText.SetActive(true);
        yield return new WaitForSeconds(2f);
        notEnoughText.SetActive(false);
    }



}
