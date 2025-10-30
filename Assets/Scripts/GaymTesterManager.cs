using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GaymTesterManager : MonoBehaviour
{
    GameManager gameManager;


    public GameObject allGamesWindow;
    public GameObject formWindow;
    public GameObject successWindow;
    public GameObject game1;
    public GameObject game2;
    public GameObject game3;

    public GameObject endScreen;

    public Slider countdownSlider;
    public TMP_Text errorMessage;
    [Header("ClickerGame")]
    public TMP_Text scoreText;
    private int clicksShown;
    private int clicks;
    [Header("Survey Form")]
    public TMP_Dropdown languageDropdown;
    public TMP_InputField nameInput;
    public TMP_InputField dateDD;
    public TMP_InputField dateMM;
    public TMP_InputField dateYY;
    public TMP_InputField playersNumber;
    public TMP_InputField mostEnjoyable;
    public TMP_InputField leastEnjoyable;
    public TMP_InputField bug1;
    public TMP_InputField bug2;
    public TMP_InputField bug3;
    public TMP_InputField bug4;
    public TMP_InputField bug5;
    public TMP_InputField bug6;
    public TMP_InputField bug7;
    public TMP_InputField bug8;
    public TMP_InputField bug9;
    public TMP_InputField feedback;
    [Header("Success Window")]
    public Slider successSlider;
    public TMP_Text moneyMessage;
    private float moneyEarned;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ClickCookie()
    {
        clicks++;
        if (clicks%3==0)
        {
            clicksShown++;
        }
        scoreText.text = "Score: " + clicksShown.ToString();
    }

    public void CountdownTrigger()
    {
        StartCoroutine(GameTimeCountdown());
    }
    public void SuccessTrigger()
    {
        float dropdownIndexPlusOne = (languageDropdown != null) ? (languageDropdown.value + 1f) : 1f;
        float multiplier = 1f + 0.01f * dropdownIndexPlusOne;

        StartCoroutine(SuccessCountdown());
        float baseAmount = (mostEnjoyable.text.Length + leastEnjoyable.text.Length
            + bug1.text.Length + bug2.text.Length + bug3.text.Length
            + bug4.text.Length + bug5.text.Length + bug6.text.Length
            + bug7.text.Length + bug8.text.Length + bug9.text.Length
            + feedback.text.Length) * 0.05f;

        moneyEarned = baseAmount * multiplier;
        if (moneyEarned > 15f)
        {
            moneyEarned = 15f;
        }
    }

    public IEnumerator GameTimeCountdown()
    {
        float timeLeft = 10f;
        countdownSlider.maxValue = timeLeft;
        countdownSlider.value = timeLeft;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
            countdownSlider.value = timeLeft;
        }
        allGamesWindow.SetActive(false);
        endScreen.SetActive(true);
    }
    public IEnumerator SuccessCountdown()
    {
        float timeLeft = 10f;
        successSlider.maxValue = timeLeft;
        successSlider.value = timeLeft;
        while (timeLeft > 0)
        {
            moneyMessage.text = "Calculating...";
            yield return new WaitForSeconds(1f);
            timeLeft--;
            successSlider.value = timeLeft;
        }
        moneyMessage.text = "You have earned $" + moneyEarned.ToString("F2") + " for your feedback!";
    }

    public void SendForm()
    {
        bool requiredMissing =
            string.IsNullOrWhiteSpace(nameInput.text) ||
            string.IsNullOrWhiteSpace(dateDD.text) ||
            string.IsNullOrWhiteSpace(dateMM.text) ||
            string.IsNullOrWhiteSpace(dateYY.text) ||
            string.IsNullOrWhiteSpace(playersNumber.text) ||
            string.IsNullOrWhiteSpace(mostEnjoyable.text) ||
            string.IsNullOrWhiteSpace(leastEnjoyable.text);

        if (requiredMissing)
        {
            errorMessage.gameObject.SetActive(true);
            errorMessage.text = "Please fill in all required fields.";
            return;
        }

        if (!AnyBugFilled())
        {
            errorMessage.gameObject.SetActive(true);
            errorMessage.text = "Please report at least one bug.";
            return;
        }

        errorMessage.gameObject.SetActive(false);
        formWindow.SetActive(false);
        successWindow.SetActive(true);
    }

    private bool AnyBugFilled()
    {
        TMP_InputField[] bugs = new[]
        {
            bug1, bug2, bug3, bug4, bug5, bug6, bug7, bug8, bug9
        };

        foreach (var b in bugs)
        {
            if (b != null && !string.IsNullOrWhiteSpace(b.text))
                return true;
        }

        return false;
    }
}
