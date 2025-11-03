using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FreeCoinSystem : MonoBehaviour
{
    GameManager gameManager;
    ShopPurchasable shopPurchasable;
    public GameObject adsObject;
    public List<TMP_Text> winners;
    public List<string> names;

    public float countdownTime = 5f;
    public TMP_Text timerCountdown;
    public TMP_Text timerCountdownWidget;
    public TMP_Text timerCountdownWidgetMinimise;
    public Animator coinAnimator;
    public Animator coinAnimatorWidget;
    public bool timerRunning = false;

    public TMP_Text maxBet;
    public TMP_Text yourBet;
    public TMP_Text yourBetWidget;
    public TMP_Text flipResult;
    public TMP_Text flipResultWidget;
    public GameObject notEnoughMoney;
    private float betAmount = 0f;
    public GameObject betLevel2Lock;
    public GameObject betLevel3Lock;
    public GameObject betLevel4Lock;
    public GameObject betLevel5Lock;

    private float maxBetAmount = 5f;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        StartCoroutine(WinnersRoulette());
    }


    void Update()
    {
        if (countdownTime <= 1)
        {
            StopAllCoroutines();
            timerRunning = false;
            countdownTime = 5f;
            coinAnimator.ResetTrigger("Flip");
        }
        yourBet.text = "Your bet:    $"+ betAmount.ToString("F0");
        yourBetWidget.text = "Your bet:    $"+ betAmount.ToString("F0");
        maxBet.text = "M: $"+ maxBetAmount.ToString("F0");
    }

    private IEnumerator WinnersRoulette()
    {
        while (true)
        {
            // Shift all winners down by one position
            for (int i = winners.Count - 1; i > 0; i--)
            {
                winners[i].text = winners[i - 1].text;
            }

            // Generate new winner for the top position
            int randomNameIndex = Random.Range(0, names.Count);
            int randomAmount = Random.Range(10, 300);
            winners[0].text = names[randomNameIndex] + " won $" + randomAmount + "!";

            // Wait for a random interval between 0.2s and 3s
            float waitTime = Random.Range(0.2f, 3f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator Countdown()
    {
        float remainingTime = countdownTime;
        while (remainingTime >= 0)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerCountdown.text = timeString;
            timerCountdownWidget.text = timeString;
            timerCountdownWidgetMinimise.text = timeString;
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }
        timerRunning = false;
        countdownTime = 5f;
        coinAnimator.SetTrigger("Idle");

    }
    public void StartFlip()
    {
        if (gameManager.playerMoney >= betAmount)
        {
            StartCoroutine(FlipCoin());
            adsObject.SetActive(true);
        }
        else
        {
            StartCoroutine(NotEnoughMoney());
        }
    }
    public void StartFlipWidget()
    {
        if (gameManager.playerMoney >= betAmount)
        {
            StartCoroutine(FlipCoinWidget());
        }
        else
        {
            StartCoroutine(NotEnoughMoney());
        }
    }
    private IEnumerator FlipCoin()
    {
        if (!timerRunning)
        {
            timerRunning = true;
            coinAnimator.SetTrigger("Flip");
            if (Random.value > 0.5f)
            {
                yield return new WaitForSeconds(1.5f);
                gameManager.playerMoney += betAmount;
                flipResult.text = "You won $" + betAmount.ToString("F2") + "!";
                flipResult.color = Color.green;
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
                gameManager.playerMoney -= betAmount;
                flipResult.text = "You lost $" + betAmount.ToString("F2") + "!";
                flipResult.color = Color.red;
            }
            StartCoroutine(Countdown());
            yield return null;
        }

    }
    private IEnumerator FlipCoinWidget()
    {
        if (!timerRunning)
        {
            timerRunning = true;
            coinAnimatorWidget.SetTrigger("FlipWidget");
            if (Random.value > 0.5f)
            {
                yield return new WaitForSeconds(1.5f);
                gameManager.playerMoney += betAmount;
                flipResultWidget.text = "You won $" + betAmount.ToString("F2") + "!";
                flipResultWidget.color = Color.green;
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
                gameManager.playerMoney -= betAmount;
                flipResultWidget.text = "You lost $" + betAmount.ToString("F2") + "!";
                flipResultWidget.color = Color.red;
            }
            StartCoroutine(Countdown());
            yield return null;
        }

    }
    private IEnumerator NotEnoughMoney()
    {
        notEnoughMoney.SetActive(true);
        yield return new WaitForSeconds(2f);
        notEnoughMoney.SetActive(false);
    }
    public void RaiseBet()
    {
        if (betAmount < maxBetAmount)
        {
            betAmount += 1f;
        }
    }
    public void LowerBet()
    {
        if (betAmount > 0)
        {
            betAmount -= 1f;
        }
    }

    public void RaiseMaxBet1(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            maxBetAmount = 10; 
            betLevel2Lock.SetActive(false);
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void RaiseMaxBet2(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice && maxBetAmount == 10)
        {
            maxBetAmount = 15;
            betLevel3Lock.SetActive(false);
            shopPurchasable.PurchaseItem();

        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void RaiseMaxBet3(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice && maxBetAmount == 15)
        {
            maxBetAmount = 25;
            betLevel4Lock.SetActive(false);
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void RaiseMaxBet4(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice && maxBetAmount == 25)
        {
            maxBetAmount = 50;
            betLevel5Lock.SetActive(false);
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
    public void RaiseMaxBet5(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice && maxBetAmount == 50)
        {
            maxBetAmount = 100;
            shopPurchasable.PurchaseItem();
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }
}



