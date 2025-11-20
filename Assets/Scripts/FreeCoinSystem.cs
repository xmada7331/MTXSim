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
    public GameObject bugIcon;
    public List<TMP_Text> winners;
    public List<string> names;

    public float countdownTime = 7f;
    public TMP_Text timerCountdown;
    public TMP_Text timerCountdownWidget;
    public TMP_Text timerCountdownWidgetMinimise;
    public GameObject timerParent;
    public Animator coinAnimator;
    public Animator coinAnimatorWidget;
    public bool timerRunning = false;

    public TMP_Text maxBet;
    public TMP_Text yourBet;
    public TMP_Text yourBetWidget;
    public TMP_Text flipResult;
    public TMP_Text flipResultWidget;
    public GameObject notEnoughMoney;
    public TMP_Text notEnoughMoneyText;
    private float betAmount = 0f;
    public GameObject betLevel2Lock;
    public GameObject betLevel3Lock;
    public GameObject betLevel4Lock;
    public GameObject betLevel5Lock;

    private float maxBetAmount = 5f;

    public AudioSource coinAudioSource;
    public AudioClip coinFlip;
    public AudioClip coinWinClip;
    public AudioClip coinLoseClip;

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
            countdownTime = 7f;
            coinAnimator.ResetTrigger("Flip");
        }
            yourBet.text = "Your bet:    $" + betAmount.ToString("F0");
        yourBetWidget.text = "$"+ betAmount.ToString("F0");
        maxBet.text = "$"+ maxBetAmount.ToString("F0");
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
            int randomAmount = Random.Range(10, 100);
            winners[0].text = names[randomNameIndex] + " won $" + randomAmount + "!";

            // Wait for a random interval between 0.2s and 3s
            float waitTime = Random.Range(0.2f, 3f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator Countdown()
    {
        timerParent.SetActive(true);
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
        timerParent.SetActive(false);

    }
    public void StartFlip()
    {
        if (gameManager.playerMoney >= betAmount && betAmount > 0)
        {

            StartCoroutine(FlipCoin());
            adsObject.SetActive(true);
            bugIcon.SetActive(true);
        }
        else if (betAmount <= 0)
        {
            notEnoughMoneyText.text = "Please place a bet!";
            StartCoroutine(NotEnoughMoney());
        }
        else
        {
            notEnoughMoneyText.text = "Not enough money!";
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
            coinAudioSource.PlayOneShot(coinFlip);
            if (Random.value > 0.55f)
            {
                yield return new WaitForSeconds(1.5f);
                gameManager.playerMoney += betAmount;
                flipResult.text = "You won $" + betAmount.ToString("F2") + "!";
                flipResult.color = Color.green;
                coinAudioSource.PlayOneShot(coinWinClip);
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
                gameManager.playerMoney -= betAmount;
                flipResult.text = "You lost $" + betAmount.ToString("F2") + "!";
                flipResult.color = Color.red;
                coinAudioSource.PlayOneShot(coinLoseClip);

            }
            coinAnimator.SetTrigger("Idle");
            StartCoroutine(Countdown());
            yield return null;
        }

    }
    private IEnumerator FlipCoinWidget()
    {
        if (!timerRunning)
        {
            timerRunning = true;
            coinAnimatorWidget.SetTrigger("Flip");
            if (Random.value > 0.5f)
            {
                yield return new WaitForSeconds(1.5f);
                gameManager.playerMoney += betAmount;
                flipResultWidget.text = "Won $" + betAmount.ToString("F2") + "!";
                flipResultWidget.color = Color.green;
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
                gameManager.playerMoney -= betAmount;
                flipResultWidget.text = "Lost $" + betAmount.ToString("F2") + "!";
                flipResultWidget.color = Color.red;
            }
            coinAnimatorWidget.SetTrigger("Idle");
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



