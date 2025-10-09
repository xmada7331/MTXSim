using System.Collections;
using TMPro;
using UnityEngine;

public class StockSystem : MonoBehaviour
{
    GameManager gameManager;
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

    private bool isBuyingA;
    private bool isBuyingB;
    private bool isBuyingC;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
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

        if (isBuyingA)
        {
            coinValue.text = aStockPrice.ToString("F4");
            if (aStockPrice <= 0)
            {
                aStockPrice = Random.Range(0.01f, 0.4f);
            }
        }
        else if (isBuyingB)
        {
            coinValue.text = bStockPrice.ToString("F4");
            if (bStockPrice <= 0)
            {
                bStockPrice = Random.Range(0.01f, 0.4f);
            }
        }
        else if (isBuyingC)
        {
            coinValue.text = cStockPrice.ToString("F4");
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
        isBuyingA = true;
        isBuyingB = false;
        isBuyingC = false;

    }
    public void BuyBStock()
    {
        isBuyingA = false;
        isBuyingB = true;
        isBuyingC = false;
    }
    public void BuyCStock()
    {
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
                aCoinAmount++;
                Debug.Log("bought A stock");
            }
            else
            {
                Debug.Log("not enough money");
            }
        }
        else if (isBuyingB)
        {
            if (gameManager.playerMoney >= bStockPrice)
            {
                bCoinAmount++;
                gameManager.playerMoney -= bStockPrice;
                Debug.Log("bought B stock");
            }
            else
            {
                Debug.Log("not enough money");
            }
        }
        else if (isBuyingC)
        {
            if (gameManager.playerMoney >= cStockPrice)
            {
                cCoinAmount++;
                gameManager.playerMoney -= cStockPrice;
                Debug.Log("bought C stock");
            }
            else
            {
                Debug.Log("not enough money");
            }
        }
    }

    public void SellStock()
    {
        if (isBuyingA)
        {
            if (aCoinAmount > 0)
            {
                aCoinAmount--;
                gameManager.playerMoney += aStockPrice;
                Debug.Log("sold A stock");
            }
            else
            {
                Debug.Log("no stocks to sell");
            }
        }
        else if (isBuyingB)
        {
            if (bCoinAmount > 0)
            {
                bCoinAmount--;
                gameManager.playerMoney += bStockPrice;
                Debug.Log("sold B stock");
            }
            else
            {
                Debug.Log("no stocks to sell");
            }
        }
        else if (isBuyingC)
        {
            if (cCoinAmount > 0)
            {
                cCoinAmount--;
                gameManager.playerMoney += cStockPrice;
                Debug.Log("sold C stock");
            }
            else
            {
                Debug.Log("no stocks to sell");
            }
        }
    }
}
