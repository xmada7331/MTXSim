using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GaymTesterManager : MonoBehaviour
{
    GameManager gameManager;


    public AudioSource gaymAudioSourceKeyboard;
    public AudioSource gaymAudioSourceMain;
    public AudioClip cookieClick;
    public AudioClip fruitClick;
    public AudioClip bananaClick;
    public AudioClip screamerClick;
    public GameObject allGamesWindow;
    public GameObject formWindow;
    public GameObject successWindow;
    public GameObject screamerWindow;
    public GameObject bananamaniaWindow;
    public GameObject gingersnapWindow;

    public GameObject endScreen;
    public GameObject backButton;

    public Slider countdownSlider;
    public TMP_Text errorMessage;
    [Header("ClickerGame")]
    public GameObject cookieBlock;
    public TMP_Text scoreText;
    private int clicksShown;
    private int clicks;
    public Animator cookieAnimator;
    [Header("Bananamania")]
    public GameObject bananaBlock;
    public GameObject bananaGameWindow;
    public List<GameObject> fruits;
    public List<GameObject> bananas;
    public TMP_Text bananaScore;
    private int bananaScoreInt;
    public Sprite bananaSprite1;
    public Sprite bananaSprite2;
    public Sprite bananaSprite3;
    public Sprite fruitSprite1;
    public Sprite fruitSprite2;
    public Sprite fruitSprite3;
    public Sprite fruitSprite4;
    public Sprite fruitSprite5;
    public Sprite fruitSprite6;
    [Header("Screamer")]
    public GameObject screamerBlock;
    public Slider screamSlider;
    public Image screamImage;
    public Sprite screamImageStage1;
    public Sprite screamImageStage2;
    public Sprite screamImageStage3;
    public Sprite screamImageStage4;
    private int screamerScoreInt;
    public TMP_Text screamerScore;
    [Header("Survey Form")]
    public TMP_Dropdown languageDropdown;
    public TMP_InputField nameInput;
    public TMP_InputField playersNumber;
    public TMP_InputField mostEnjoyable;
    public TMP_InputField leastEnjoyable;
    public TMP_InputField bug1;
    public TMP_InputField bug2;
    public TMP_InputField bug3;
    public TMP_InputField bug4;
    public TMP_InputField bug5;
    public TMP_InputField bug6;
    public TMP_InputField feedback;
    [Header("Success Window")]
    public Slider successSlider;
    public TMP_Text moneyMessage;
    [Tooltip("Duration in seconds for the MoneyCalc slider to fill up.")]
    public float calculationDuration = 3f;
    private float moneyEarned;
    public Image successImage1;
    public Image successImage2;
    private Coroutine successCoroutine;

    [Header("Payout Balancing")]
    [Tooltip("Global multiplier applied to the final calculated payout.")]
    public float globalPayoutMultiplier = 1f;
    [Tooltip("Base payout for completing the QA testing.")]
    public float basePayout = 3.0f;
    [Tooltip("Bonus payout for filling out tester profile info.")]
    public float profileCompletionBonus = 1.0f;
    [Tooltip("Bonus payout per non-empty bug report (up to 6 bugs).")]
    public float bugReportMultiplier = 0.90f;
    [Tooltip("Bonus payout per character across feedback and bug descriptions.")]
    public float textCharacterMultiplier = 0.035f;
    [Tooltip("Maximum allowed total payout cap.")]
    public float maxPayoutCap = 25f;

    [Header("Minigame Multipliers")]
    [Tooltip("Money earned per score point in Cookie Clicker.")]
    public float cookieScoreMultiplier = 0.18f;
    [Tooltip("Money earned per score point in Bananamania.")]
    public float bananaScoreMultiplier = 0.18f;
    [Tooltip("Money earned per score point in Screamer.")]
    public float screamerScoreMultiplier = 0.035f;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {

    }

    void Update()
    {
        if (screamerWindow.activeSelf && !screamerBlock.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseScream();
            screamerScoreInt += 2;
            screamerScore.text = "Score: " + screamerScoreInt.ToString();
        }
        if (screamSlider.value < 25)
        {
            screamImage.sprite = screamImageStage2;
            gaymAudioSourceMain.pitch = 1f;
        }
        else if (screamSlider.value < 50)
        {
            screamImage.sprite = screamImageStage4;
            gaymAudioSourceMain.pitch = 1.5f;
        }
        else if (screamSlider.value < 75)
        {
            screamImage.sprite = screamImageStage3;
            gaymAudioSourceMain.pitch = 2f;
        }
        else
        {
            screamImage.sprite = screamImageStage1;
            gaymAudioSourceMain.pitch = 2.5f;
        }
    }
    public void RouletteGame()
    {
        int gameChoice = Random.Range(1, 4);
        allGamesWindow.SetActive(false);
        switch (gameChoice)
        {
            case 1:
                screamerWindow.SetActive(true);
                allGamesWindow.SetActive(true);
                break;
            case 2:
                bananamaniaWindow.SetActive(true);
                allGamesWindow.SetActive(true);
                break;
            case 3:
                gingersnapWindow.SetActive(true);
                allGamesWindow.SetActive(true);
                break;
        }
    }
    private void OnDisable()
    {
        if (successCoroutine != null)
        {
            StopCoroutine(successCoroutine);
            successCoroutine = null;
        }
    }

    public void ResetAllGames()
    {
        if (successCoroutine != null)
        {
            StopCoroutine(successCoroutine);
            successCoroutine = null;
        }

        if (successSlider != null)
        {
            successSlider.value = successSlider.maxValue;
        }

        gaymAudioSourceMain.pitch = 1f;
        screamerWindow.SetActive(false);
        bananamaniaWindow.SetActive(false);
        gingersnapWindow.SetActive(false);
        allGamesWindow.SetActive(false);

        bananaScoreInt = 0;
        bananaScore.text = "Score: " + bananaScoreInt.ToString();
        bananaBlock.SetActive(true);

        clicks = 0;
        clicksShown = 0;
        scoreText.text = "Score: " + clicksShown.ToString();
        cookieBlock.SetActive(true);

        screamerScoreInt = 0;
        screamSlider.value = 0f;
        screamerScore.text = "Score: " + screamerScoreInt.ToString();
        screamerBlock.SetActive(true);

        countdownSlider.value = countdownSlider.maxValue;
    }

    public void ClickCookie()
    {
        clicks++;
        if (clicks % 3 == 0)
        {
            clicksShown++;
            cookieAnimator.SetTrigger("CookieTrigger");
            gaymAudioSourceMain.PlayOneShot(cookieClick);
        }
        scoreText.text = "Score: " + clicksShown.ToString();
    }

    public void CountdownTrigger()
    {
        StartCoroutine(GameTimeCountdown());
    }

    public void SuccessTrigger()
    {
        // Prevent starting if the form window is still active (e.g. validation failed)
        if (formWindow != null && formWindow.activeSelf)
        {
            return;
        }

        if (successCoroutine == null)
        {
            StartSuccessCalculation();
        }
    }

    public void StartSuccessCalculation()
    {
        if (successCoroutine != null)
        {
            StopCoroutine(successCoroutine);
            successCoroutine = null;
        }

        CalculatePayout();
        successCoroutine = StartCoroutine(SuccessCountdown());
    }

    public void CalculatePayout()
    {
        float dropdownIndexPlusOne = (languageDropdown != null) ? (languageDropdown.value + 1f) : 1f;
        float multiplier = 1f + 0.02f * dropdownIndexPlusOne;

        // Base reward for completing QA testing
        float baseReward = basePayout;

        // Profile completion bonus
        bool profileComplete = (nameInput != null && !string.IsNullOrWhiteSpace(nameInput.text))
            && (playersNumber == null || !string.IsNullOrWhiteSpace(playersNumber.text));
        float profileBonus = profileComplete ? profileCompletionBonus : 0f;

        // Bugs reported bonus (up to 6 bugs)
        TMP_InputField[] bugs = new[]
        {
            bug1, bug2, bug3, bug4, bug5, bug6
        };
        int bugCount = 0;
        int bugChars = 0;
        foreach (var b in bugs)
        {
            if (b != null && !string.IsNullOrWhiteSpace(b.text))
            {
                bugCount++;
                bugChars += b.text.Trim().Length;
            }
        }
        float bugBonus = bugCount * bugReportMultiplier;

        // Feedback character length bonus
        int textChars = 0;
        if (mostEnjoyable != null) textChars += mostEnjoyable.text.Trim().Length;
        if (leastEnjoyable != null) textChars += leastEnjoyable.text.Trim().Length;
        if (feedback != null) textChars += feedback.text.Trim().Length;
        textChars += bugChars;
        float textBonus = textChars * textCharacterMultiplier;

        // Minigame score rewards
        float cookieBonus = clicksShown * cookieScoreMultiplier;
        float bananaBonus = Mathf.Max(0, bananaScoreInt) * bananaScoreMultiplier;
        float screamerBonus = Mathf.Max(0, screamerScoreInt) * screamerScoreMultiplier;

        float minigameBonus = cookieBonus + bananaBonus + screamerBonus;

        float subtotal = baseReward + profileBonus + bugBonus + textBonus + minigameBonus;
        moneyEarned = subtotal * multiplier * globalPayoutMultiplier;

        float cap = Mathf.Max(0f, maxPayoutCap);
        if (moneyEarned > cap)
        {
            moneyEarned = cap;
        }
        if (moneyEarned <= 0f)
        {
            moneyEarned = 0f;
        }
    }

    public IEnumerator GameTimeCountdown()
    {
        float timeLeft = 15f;
        countdownSlider.maxValue = timeLeft;
        countdownSlider.value = timeLeft;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
            countdownSlider.value = timeLeft;
        }
        allGamesWindow.SetActive(false);
        screamerWindow.SetActive(false);
        bananamaniaWindow.SetActive(false);
        gingersnapWindow.SetActive(false);
        endScreen.SetActive(true);
        ClearAllInputs();
    }

    public IEnumerator SuccessCountdown()
    {
        float duration = Mathf.Max(0.5f, calculationDuration);
        if (successSlider != null)
        {
            successSlider.maxValue = duration;
            successSlider.minValue = 0f;
            successSlider.value = duration;
        }

        if (moneyMessage != null) moneyMessage.text = "Calculating...";
        if (successImage2 != null) successImage2.gameObject.SetActive(false);
        if (successImage1 != null) successImage1.gameObject.SetActive(true);
        if (backButton != null) backButton.SetActive(false);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            if (successSlider != null)
            {
                // Smoothly counts down to 0, which visually fills the inverted slider
                successSlider.value = (1f - progress) * duration;
            }
            yield return null;
        }

        if (successSlider != null)
        {
            successSlider.value = 0f;
        }

        if (successImage2 != null) successImage2.gameObject.SetActive(true);
        if (successImage1 != null) successImage1.gameObject.SetActive(false);
        if (backButton != null) backButton.SetActive(true);

        if (moneyMessage != null)
        {
            moneyMessage.text = "You have earned $" + moneyEarned.ToString("F2") + " for your feedback!";
        }

        if (gameManager != null)
        {
            gameManager.playerMoney += moneyEarned;
            gameManager.incomeAmount += moneyEarned;
        }

        successCoroutine = null;
    }

    public void SendForm()
    {
        bool requiredMissing =
            (nameInput == null || string.IsNullOrWhiteSpace(nameInput.text)) ||
            (mostEnjoyable == null || string.IsNullOrWhiteSpace(mostEnjoyable.text)) ||
            (leastEnjoyable == null || string.IsNullOrWhiteSpace(leastEnjoyable.text));

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
        backButton.SetActive(false);

        StartSuccessCalculation();
    }

    private bool AnyBugFilled()
    {
        TMP_InputField[] bugs = new[]
        {
            bug1, bug2, bug3, bug4, bug5, bug6
        };

        foreach (var b in bugs)
        {
            if (b != null && !string.IsNullOrWhiteSpace(b.text))
                return true;
        }

        return false;
    }

    public void StartBananamania()
    {
        bananaScoreInt = 0;
        bananaScore.text = "Score: " + bananaScoreInt.ToString();

        // Position range
        const float minX = -500f;
        const float maxX = 500f;
        const float minY = -280f;
        const float maxY = 200f;

        // Pools for random sprites
        Sprite[] bananaPool = new[] { bananaSprite1, bananaSprite2, bananaSprite3 };
        Sprite[] fruitPool = new[] { fruitSprite1, fruitSprite2, fruitSprite3, fruitSprite4, fruitSprite5, fruitSprite6 };

        // Helper to set position, sprite and random rotation for a GameObject
        void PlaceAndStyle(GameObject g, Sprite[] pool)
        {
            if (g == null) return;

            // Re-enable item
            g.SetActive(true);

            // Parent under bananaGameWindow so coordinates/anchoredPosition make sense
            if (bananaGameWindow != null)
                g.transform.SetParent(bananaGameWindow.transform, false);

            // Random position
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);

            // Assign sprite if available (support UI Image and SpriteRenderer)
            Sprite chosen = null;
            if (pool != null && pool.Length > 0)
            {
                // pick a non-null sprite from pool (fallback if several null)
                int attempts = 0;
                while (attempts < 8)
                {
                    Sprite candidate = pool[Random.Range(0, pool.Length)];
                    if (candidate != null) { chosen = candidate; break; }
                    attempts++;
                }
            }

            var uiImage = g.GetComponent<Image>();
            if (uiImage != null)
            {
                if (chosen != null) uiImage.sprite = chosen;
                // set anchored position and random rotation on RectTransform
                var rt = g.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(x, y);
                float zRot = Random.Range(0f, 360f);
                rt.localEulerAngles = new Vector3(0f, 0f, zRot);
                // optional horizontal flip
                if (Random.value < 0.5f)
                    rt.localScale = new Vector3(-Mathf.Abs(rt.localScale.x), rt.localScale.y, rt.localScale.z);
                else
                    rt.localScale = new Vector3(Mathf.Abs(rt.localScale.x), rt.localScale.y, rt.localScale.z);
            }
            else
            {
                var sr = g.GetComponent<SpriteRenderer>();
                if (sr != null && chosen != null) sr.sprite = chosen;

                g.transform.localPosition = new Vector3(x, y, g.transform.localPosition.z);
                float zRot = Random.Range(0f, 360f);
                g.transform.localEulerAngles = new Vector3(0f, 0f, zRot);
                // optional horizontal flip
                var ls = g.transform.localScale;
                if (Random.value < 0.5f)
                    g.transform.localScale = new Vector3(-Mathf.Abs(ls.x), ls.y, ls.z);
                else
                    g.transform.localScale = new Vector3(Mathf.Abs(ls.x), ls.y, ls.z);
            }
        }

        if (fruits != null)
        {
            foreach (var f in fruits)
                PlaceAndStyle(f, fruitPool);
        }

        if (bananas != null)
        {
            foreach (var b in bananas)
                PlaceAndStyle(b, bananaPool);
        }
    }

    public void ClickBanana(GameObject clicked)
    {
        bananaScoreInt--;
        bananaScore.text = "Score: " + bananaScoreInt.ToString();
        clicked.SetActive(false);
        gaymAudioSourceMain.PlayOneShot(bananaClick);
    }
    public void ClickFruit(GameObject clicked)
    {
        bananaScoreInt++;
        bananaScore.text = "Score: " + bananaScoreInt.ToString();
        clicked.SetActive(false);
        gaymAudioSourceMain.PlayOneShot(fruitClick);
    }
    private void IncreaseScream()
    {
        screamSlider.value += 6f;
        gaymAudioSourceMain.PlayOneShot(screamerClick);
    }

    private IEnumerator ScreamerSliderDecrease()
    {
        while (screamSlider.value > -2)
        {
            yield return new WaitForSeconds(0.2f);
            screamSlider.value -= 5f;
        }
    }

    public void StartScreamer()
    {
        screamSlider.maxValue = 100f;
        screamSlider.value = 0f;
        StartCoroutine(ScreamerSliderDecrease());
    }

    private void ClearAllInputs()
    {
        StartCoroutine(ResetAudioSource());
        TMP_InputField[] inputs = new TMP_InputField[]
        {
            nameInput, playersNumber,
            mostEnjoyable, leastEnjoyable,
            bug1, bug2, bug3, bug4, bug5, bug6,
            feedback
        };
        foreach (var input in inputs)
        {
            if (input != null)
                input.text = string.Empty;
        }
        if (languageDropdown != null)
            languageDropdown.value = 0;
    }

    public void StopCoroutines()
    {
        if (successCoroutine != null)
        {
            StopCoroutine(successCoroutine);
            successCoroutine = null;
        }
        StopAllCoroutines();
    }

    private IEnumerator ResetAudioSource()
    {
        gaymAudioSourceKeyboard.volume = 0;
        yield return new WaitForSeconds(0.5f);
        gaymAudioSourceKeyboard.volume = 0.25f;
    }
}
