using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FreeCoinSystem : MonoBehaviour
{
    public GameObject adsObject;
    public List<TMP_Text> winners;
    public List<string> names;

    public float countdownTime = 5f;
    public TMP_Text timerCountdown;
    public TMP_Text timerCountdownWidget;
    public TMP_Text timerCountdownWidgetMinimise;
    public Animator coinAnimator;
    public bool timerRunning = false;
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
        StartCoroutine(FlipCoin());
        adsObject.SetActive(true);
    }
    private IEnumerator FlipCoin()
    {
            if (!timerRunning)
            {
                timerRunning = true;
                coinAnimator.SetTrigger("Flip");
                StartCoroutine(Countdown());
                yield return null; 
            }
       
    }
}



