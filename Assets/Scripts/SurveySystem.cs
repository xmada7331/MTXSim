using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SurveySystem : MonoBehaviour
{
    GameManager gameManager;

    public AudioSource audioIntens1;
    public AudioSource audioIntens2;

    public GameObject quizPanel;
    public GameObject endPanel;
    public GameObject mainMenu;
    public TMP_Text moneyText;
    public TMP_Text endDescription;
    public TMP_Text questionsSummary;
    public List<QuestionsAndAnswers> MathQuestions;
    public List<QuestionsAndAnswers> CelebsQuestions;
    public List<QuestionsAndAnswers> LanguageQuestions;
    public List<QuestionsAndAnswers> SportsQuestions;
    public List<QuestionsAndAnswers> VideoGamesQuestions;
    public List<QuestionsAndAnswers> AnimalsQuestions;
    public List<QuestionsAndAnswers> MythologyQuestions;
    public GameObject[] options;
    public int currentQuestion;
    public bool mathCat = false;
    public bool celebsCat = false;
    public bool languageCat = false;
    public bool sportsCat = false;
    public bool videoGamesCat = false;
    public bool animalsCat = false;
    public bool mythologyCat = false;

    public int questionAmount = 0;
    private int lastQuestionIndex = -1;
    public int correctAnswers;

    public TMP_Text QuestionText;
    public Slider questionTimer;
    public GameObject surveyCooldownWindow;
    public Slider surveyCooldown;
    public TMP_Text surveyCooldownTimerText;

    [Header("Cooldown Settings")]
    public float cooldownDuration = 25f;
    private float currentCooldown = 0f;
    private bool isCooldownActive = false;
    private Coroutine cooldownCoroutine = null;

    private List<QuestionsAndAnswers> originalMathQuestions;
    private List<QuestionsAndAnswers> originalCelebsQuestions;
    private List<QuestionsAndAnswers> originalLanguageQuestions;
    private List<QuestionsAndAnswers> originalSportsQuestions;
    private List<QuestionsAndAnswers> originalVideoGamesQuestions;
    private List<QuestionsAndAnswers> originalAnimalsQuestions;
    private List<QuestionsAndAnswers> originalMythologyQuestions;

    public GameObject happyEmoji;
    public GameObject unhappyEmoji;

    [Header("Category Selection Info")]
    public TMP_Text categoryTitleText;
    public TMP_Text categoryDescriptionText;

    private string defaultCategoryTitle = "Choose category";
    private string defaultCategoryDescription = "Every category provides the same amount of money upon completion";

    private readonly Dictionary<string, (string title, string description)> categoryData = new Dictionary<string, (string, string)>(System.StringComparer.OrdinalIgnoreCase)
    {
        { "Mythology", ("Mythology", "How well do you know ancient tales and myths?") },
        { "Math", ("Math", "Test your calculation skills with numbers, algebra, and logic!") },
        { "Languages", ("Language", "Explore words, meanings, and linguistics from around the world!") },
        { "Language", ("Language", "Explore words, meanings, and linguistics from around the world!") },
        { "Sports", ("Sports", "Show off your knowledge of athletics, rules, and famous games!") },
        { "Video games", ("Video Games", "How well do you know gaming history, consoles, and classic titles?") },
        { "Animals", ("Animals", "Discover fascinating facts about the animal kingdom and wildlife!") },
        { "Popculture", ("Pop Culture", "Test your knowledge of movies, celebrities, music, and media!") },
        { "Celebs", ("Pop Culture", "Test your knowledge of movies, celebrities, music, and media!") }
    };
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        // Safety check: if cooldown finished, ensure wait/cooldown screen is hidden and main menu is restored
        if (!isCooldownActive && surveyCooldownWindow != null && surveyCooldownWindow.activeSelf)
        {
            surveyCooldownWindow.SetActive(false);
            if (mainMenu != null && (endPanel == null || !endPanel.activeSelf) && (quizPanel == null || !quizPanel.activeSelf))
            {
                mainMenu.SetActive(true);
            }
        }
    }

    private void Start()
    {
        originalMathQuestions = new List<QuestionsAndAnswers>(MathQuestions);
        originalCelebsQuestions = new List<QuestionsAndAnswers>(CelebsQuestions);
        originalLanguageQuestions = new List<QuestionsAndAnswers>(LanguageQuestions);
        originalSportsQuestions = new List<QuestionsAndAnswers>(SportsQuestions);
        originalVideoGamesQuestions = new List<QuestionsAndAnswers>(VideoGamesQuestions);
        originalAnimalsQuestions = new List<QuestionsAndAnswers>(AnimalsQuestions);
        originalMythologyQuestions = new List<QuestionsAndAnswers>(MythologyQuestions);

        SetupButtonHoverEffects();
    }

    private void OnEnable()
    {
        SetupButtonHoverEffects();
    }
    public void correct()
    {
        questionAmount++;
        questionTimer.value = 10f;
        correctAnswers++;
        if (mathCat)
        {
            MathQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(MathQuestions, originalMathQuestions);
        }
        else if (celebsCat)
        {
            CelebsQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(CelebsQuestions, originalCelebsQuestions);
        }
        else if (languageCat)
        {
            LanguageQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(LanguageQuestions, originalLanguageQuestions);
        }
        else if (sportsCat)
        {
            SportsQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(SportsQuestions, originalSportsQuestions);
        }
        else if (videoGamesCat)
        {
            VideoGamesQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(VideoGamesQuestions, originalVideoGamesQuestions);
        }
        else if (animalsCat)
        {
            AnimalsQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(AnimalsQuestions, originalAnimalsQuestions);
        }
        else if (mythologyCat)
        {
            MythologyQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(MythologyQuestions, originalMythologyQuestions);
        }
        // Add other categories as needed
    }
    public void wrong()
    {
        questionAmount++;
        questionTimer.value = 10f;
        if (mathCat)
        {
            MathQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(MathQuestions, originalMathQuestions);
        }
        else if (celebsCat)
        {
            CelebsQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(CelebsQuestions, originalCelebsQuestions);
        }
        else if (languageCat)
        {
            LanguageQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(LanguageQuestions, originalLanguageQuestions);
        }
        else if (sportsCat)
        {
            SportsQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(SportsQuestions, originalSportsQuestions);
        }
        else if (videoGamesCat)
        {
            VideoGamesQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(VideoGamesQuestions, originalVideoGamesQuestions);
        }
        else if (animalsCat)
        {
            AnimalsQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(AnimalsQuestions, originalAnimalsQuestions);
        }
        else if (mythologyCat)
        {
            MythologyQuestions.RemoveAt(currentQuestion);
            GenerateQuestion(MythologyQuestions, originalMythologyQuestions);
        }
        // Add other categories as needed
    }

    private void SetAnswers(List<QuestionsAndAnswers> questions)
    {
        // Get current question
        var q = questions[currentQuestion];

        // Create a list of answer indices
        int answerCount = q.Answers.Length;
        List<int> indices = new List<int>();
        for (int i = 0; i < answerCount; i++)
            indices.Add(i);

        // Shuffle indices
        for (int i = 0; i < indices.Count; i++)
        {
            int swapIdx = Random.Range(i, indices.Count);
            int temp = indices[i];
            indices[i] = indices[swapIdx];
            indices[swapIdx] = temp;
        }

        // Assign answers to buttons and set isCorrect
        for (int i = 0; i < options.Length; i++)
        {
            var answerScript = options[i].GetComponent<AnswerScript>();
            answerScript.isCorrect = false;

            int answerIdx = indices[i];
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = q.Answers[answerIdx];

            // CorrectAnswer is 1-based, so subtract 1
            if (answerIdx == q.CorrectAnswer - 1)
                answerScript.isCorrect = true;
        }
    }

    public void EndSurvey()
    {
        audioIntens1.volume = .15f;
        audioIntens2.volume = 0f;
        StopAllCoroutines();
        quizPanel.SetActive(false);
        endPanel.SetActive(true);
        ResetCategoryHoverText();
        mathCat = false;
        celebsCat = false;
        languageCat = false;
        sportsCat = false;
        videoGamesCat = false;
        animalsCat = false;
        mythologyCat = false;
        questionAmount = 0;

        gameManager.playerMoney += correctAnswers;
        gameManager.incomeAmount += correctAnswers;
        moneyText.text = "You have earned $" + correctAnswers.ToString("F2");
        questionsSummary.text = "You answered " + correctAnswers + " questions correctly!";
        if (correctAnswers >= 3)
        {
            endDescription.text = "Good job! You've earned some money for your correct answers.";
            happyEmoji.SetActive(true);
            unhappyEmoji.SetActive(false);
        }
        else
        {
            endDescription.text = "Thank you for your participation! Unfortunately you are not the subject we are looking for at the moment, but you can try again later.";
            unhappyEmoji.SetActive(true);
            happyEmoji.SetActive(false);
        }

        // Start cooldown count immediately when quiz is finished
        StartSurveyCooldown();
    }
    public void ChooseMath()
    {
        ResetCategoryHoverText();
        correctAnswers = 0;
        mathCat = true;
        GenerateQuestion(MathQuestions, originalMathQuestions);
        StartCoroutine(QuestionTimer());
        audioIntens1.volume = 0f;
        audioIntens2.volume = .5f;
    }
    public void ChooseLang()
    {
        ResetCategoryHoverText();
        correctAnswers = 0;
        languageCat = true;
        GenerateQuestion(LanguageQuestions, originalLanguageQuestions);
        StartCoroutine(QuestionTimer());
        audioIntens1.volume = 0f;
        audioIntens2.volume = .5f;


    }
    public void ChooseCelebs()
    {
        ResetCategoryHoverText();
        correctAnswers = 0;
        celebsCat = true;
        GenerateQuestion(CelebsQuestions, originalCelebsQuestions);
        StartCoroutine(QuestionTimer());
        audioIntens1.volume = 0f;
        audioIntens2.volume = .5f;
    }
    public void ChooseSports()
    {
        ResetCategoryHoverText();
        correctAnswers = 0;
        sportsCat = true;
        GenerateQuestion(SportsQuestions, originalSportsQuestions);
        StartCoroutine(QuestionTimer());
        audioIntens1.volume = 0f;
        audioIntens2.volume = .5f;
    }
    public void ChooseVideoGames()
    {
        ResetCategoryHoverText();
        correctAnswers = 0;
        videoGamesCat = true;
        GenerateQuestion(VideoGamesQuestions, originalVideoGamesQuestions);
        StartCoroutine(QuestionTimer());
        audioIntens1.volume = 0f;
        audioIntens2.volume = .5f;
    }
    public void ChooseAnimals()
    {
        ResetCategoryHoverText();
        correctAnswers = 0;
        animalsCat = true;
        GenerateQuestion(AnimalsQuestions, originalAnimalsQuestions);
        StartCoroutine(QuestionTimer());
        audioIntens1.volume = 0f;
        audioIntens2.volume = .5f;
    }
    public void ChooseMythology()
    {
        ResetCategoryHoverText();
        correctAnswers = 0;
        mythologyCat = true;
        GenerateQuestion(MythologyQuestions, originalMythologyQuestions);
        StartCoroutine(QuestionTimer());
        audioIntens1.volume = 0f;
        audioIntens2.volume = .5f;
    }
    private void GenerateQuestion(List<QuestionsAndAnswers> questions, List<QuestionsAndAnswers> originalQuestions)
    {
        if (questions.Count == 0)
        {
            // Restore the original questions for replayability
            questions.AddRange(originalQuestions);
        }

        if (questions.Count > 0)
        {
            lastQuestionIndex = currentQuestion;
            currentQuestion = Random.Range(0, questions.Count);
            QuestionText.text = questions[currentQuestion].Question;
            SetAnswers(questions);
        }
        else
        {
            QuestionText.text = "Survey Complete!";
            EndSurvey();
        }
    }

    private IEnumerator QuestionTimer()
    {
        questionTimer.maxValue = 10f;
        questionTimer.value = 10f;
        while (questionTimer.value > 0)
        {
            questionTimer.value -= Time.deltaTime;
            yield return null;
        }
        // Time's up, treat as wrong answer
        if (questionAmount < 4)
        {
            wrong();
            StartCoroutine(QuestionTimer());
        }
        else
        {
            wrong();
            EndSurvey();
        }
    }
    public void StartSurveyCooldown()
    {
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }
        cooldownCoroutine = StartCoroutine(SurveyCooldownRoutine());
    }

    private void UpdateCooldownTimerText(float remainingSeconds)
    {
        if (surveyCooldownTimerText == null && surveyCooldownWindow != null)
        {
            Transform timerTr = surveyCooldownWindow.transform.Find("Timer");
            if (timerTr != null)
            {
                surveyCooldownTimerText = timerTr.GetComponent<TMP_Text>();
            }
        }

        if (surveyCooldownTimerText == null) return;

        int totalSeconds = Mathf.CeilToInt(Mathf.Max(0f, remainingSeconds));
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;
        surveyCooldownTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    private IEnumerator SurveyCooldownRoutine()
    {
        isCooldownActive = true;
        currentCooldown = cooldownDuration;
        if (surveyCooldown != null)
        {
            surveyCooldown.maxValue = cooldownDuration;
            surveyCooldown.value = cooldownDuration;
        }
        UpdateCooldownTimerText(currentCooldown);

        while (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
            if (surveyCooldown != null)
            {
                surveyCooldown.value = currentCooldown;
            }
            UpdateCooldownTimerText(currentCooldown);
            yield return null;
        }

        currentCooldown = 0f;
        UpdateCooldownTimerText(0f);
        isCooldownActive = false;
        cooldownCoroutine = null;

        // If wait/cooldown screen is currently showing, close it and return to main menu
        if (surveyCooldownWindow != null && surveyCooldownWindow.activeSelf)
        {
            surveyCooldownWindow.SetActive(false);
            if (mainMenu != null) mainMenu.SetActive(true);
        }
    }

    public void TriggerSurveyCooldown()
    {
        OnBackButtonClicked();
    }

    public void OnBackButtonClicked()
    {
        if (endPanel != null) endPanel.SetActive(false);

        if (isCooldownActive && currentCooldown > 0f)
        {
            // Cooldown is still in progress, show the cooldown/wait screen with remaining time
            if (mainMenu != null) mainMenu.SetActive(false);
            if (surveyCooldownWindow != null) surveyCooldownWindow.SetActive(true);
            UpdateCooldownTimerText(currentCooldown);
        }
        else
        {
            // Cooldown already finished while on endPanel, go straight to main menu
            if (surveyCooldownWindow != null) surveyCooldownWindow.SetActive(false);
            if (mainMenu != null) mainMenu.SetActive(true);
        }
    }
public void SetupButtonHoverEffects()
    {
        // 1. Start button in mainMenu
        if (mainMenu != null)
        {
            Transform startBt = mainMenu.transform.Find("Start_bt");
            if (startBt != null)
            {
                ApplyHoverEffect(startBt.gameObject, withScale: true);
            }
        }

        // 2. Categories under Surveys_window/Categories
        Transform surveyWin = null;
        if (mainMenu != null && mainMenu.transform.parent != null)
            surveyWin = mainMenu.transform.parent;
        else if (quizPanel != null && quizPanel.transform.parent != null && quizPanel.transform.parent.parent != null)
            surveyWin = quizPanel.transform.parent.parent;

        if (surveyWin != null)
        {
            Transform catParent = surveyWin.Find("Categories");
            if (catParent != null)
            {
                // Auto-wire UI references if null
                if (categoryTitleText == null)
                {
                    Transform chooseImg = catParent.Find("Choose image");
                    if (chooseImg != null) categoryTitleText = chooseImg.GetComponentInChildren<TMP_Text>();
                }
                if (categoryDescriptionText == null)
                {
                    Transform catDesc = catParent.Find("Cat_description");
                    if (catDesc != null) categoryDescriptionText = catDesc.GetComponent<TMP_Text>();
                }

                if (categoryTitleText != null && !string.IsNullOrEmpty(categoryTitleText.text))
                    defaultCategoryTitle = categoryTitleText.text;
                if (categoryDescriptionText != null && !string.IsNullOrEmpty(categoryDescriptionText.text))
                    defaultCategoryDescription = categoryDescriptionText.text;

                for (int i = 0; i < catParent.childCount; i++)
                {
                    Transform cTr = catParent.GetChild(i);
                    // Add hover and scale pop to any category with an Image and EventTrigger or Button
                    if (cTr.GetComponent<UnityEngine.UI.Image>() != null &&
                        (cTr.GetComponent<UnityEngine.EventSystems.EventTrigger>() != null || cTr.GetComponent<Button>() != null))
                    {
                        ApplyHoverEffect(cTr.gameObject, withScale: true);

                        UIHoverEffect hover = cTr.GetComponent<UIHoverEffect>();
                        if (hover != null)
                        {
                            string cName = cTr.name;
                            hover.onHoverEnterAction = () => OnCategoryHoverEnter(cName);
                            hover.onHoverExitAction = () => OnCategoryHoverExit(cName);
                        }
                    }
                }
            }
        }

        // 3. Quiz answer options
        if (options != null)
        {
            foreach (var opt in options)
            {
                if (opt != null)
                    ApplyHoverEffect(opt, withScale: false);
            }
        }

        // 4. Back button in endPanel
        if (endPanel != null)
        {
            Transform backBt = endPanel.transform.Find("Back_bt");
            if (backBt != null)
            {
                ApplyHoverEffect(backBt.gameObject, withScale: false);
            }
        }
    }

    public void OnCategoryHoverEnter(string categoryName)
    {
        if (categoryData.TryGetValue(categoryName, out var info))
        {
            if (categoryTitleText != null)
                categoryTitleText.text = info.title;
            if (categoryDescriptionText != null)
                categoryDescriptionText.text = info.description;
        }
        else
        {
            if (categoryTitleText != null)
                categoryTitleText.text = categoryName;
        }
    }

    public void OnCategoryHoverExit(string categoryName)
    {
        if (categoryData.TryGetValue(categoryName, out var info))
        {
            if (categoryTitleText != null && categoryTitleText.text == info.title)
                categoryTitleText.text = defaultCategoryTitle;
            if (categoryDescriptionText != null && categoryDescriptionText.text == info.description)
                categoryDescriptionText.text = defaultCategoryDescription;
        }
        else
        {
            if (categoryTitleText != null && categoryTitleText.text == categoryName)
                categoryTitleText.text = defaultCategoryTitle;
        }
    }

    public void ResetCategoryHoverText()
    {
        if (categoryTitleText != null)
            categoryTitleText.text = defaultCategoryTitle;
        if (categoryDescriptionText != null)
            categoryDescriptionText.text = defaultCategoryDescription;
    }

    private void ApplyHoverEffect(GameObject go, bool withScale = false)
    {
        if (go == null) return;
        UIHoverEffect h = go.GetComponent<UIHoverEffect>() ?? go.AddComponent<UIHoverEffect>();
        h.useExplicitHoverColor = true;
        h.hoverColor = new Color(0.88f, 0.94f, 1f, 1f);
        h.useExplicitPressedColor = true;
        h.pressedColor = new Color(0.75f, 0.85f, 0.95f, 1f);
        if (withScale)
        {
            h.enableScaleEffect = true;
            h.hoverScaleMultiplier = 1.08f;
            h.pressedScaleMultiplier = 1.02f;
            h.scaleAnimationDuration = 0.1f;
        }
    }
}
