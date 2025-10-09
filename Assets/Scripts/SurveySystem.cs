using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SurveySystem : MonoBehaviour
{
    GameManager gameManager;
    public GameObject quizPanel;
    public GameObject endPanel;
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

    private List<QuestionsAndAnswers> originalMathQuestions;
    private List<QuestionsAndAnswers> originalCelebsQuestions;
    private List<QuestionsAndAnswers> originalLanguageQuestions;
    private List<QuestionsAndAnswers> originalSportsQuestions;
    private List<QuestionsAndAnswers> originalVideoGamesQuestions;
    private List<QuestionsAndAnswers> originalAnimalsQuestions;
    private List<QuestionsAndAnswers> originalMythologyQuestions;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
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
        //else if (languageCat)
        //{

        //}
        //else if (sportsCat)
        //{

        //}
        //else if (videoGamesCat)
        //{

        //}
        //else if (animalsCat)
        //{

        //}
        //else if (mythologyCat)
        //{

        //}

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

    //private void GenerateMathQuestion()
    //{
    //    MathQuestions = new List<QuestionsAndAnswers>(originalMathQuestions);

    //    if (MathQuestions.Count > 0)
    //    {
    //        currentQuestion = Random.Range(0, MathQuestions.Count);

    //        QuestionText.text = MathQuestions[currentQuestion].Question;
    //        SetAnswers(MathQuestions);
    //    }
    //    else
    //    {
    //        QuestionText.text = "Survey Complete!";
    //        EndSurvey();
    //    }
    //}
    //private void GenerateCelebsQuestion()
    //{
    //    CelebsQuestions = new List<QuestionsAndAnswers>(originalCelebsQuestions);

    //    if (CelebsQuestions.Count > 0)
    //    {
    //        currentQuestion = Random.Range(0, CelebsQuestions.Count);

    //        QuestionText.text = CelebsQuestions[currentQuestion].Question;
    //        SetAnswers(CelebsQuestions);
    //    }
    //    else
    //    {
    //        QuestionText.text = "Survey Complete!";
    //        EndSurvey();
    //    }
    //}

    public void EndSurvey()
    {
        StopAllCoroutines();
        quizPanel.SetActive(false);
        endPanel.SetActive(true);
        mathCat = false;
        celebsCat = false;
        languageCat = false;
        sportsCat = false;
        videoGamesCat = false;
        animalsCat = false;
        mythologyCat = false;
        questionAmount = 0;
        
        gameManager.playerMoney += correctAnswers / 2f;
        moneyText.text = "You have earned $" + (correctAnswers / 2f).ToString("F2");
        questionsSummary.text = "You answered " + correctAnswers + " questions correctly!";
        if (correctAnswers >= 3)
        {
            endDescription.text = "Good job! You've earned some money for your correct answers.";
        }
        else
        {
            endDescription.text = "Thank you for your participation! Unfortunately you are not the subject we are looking for at the moment, but you can try again later.";
        }
    }
    public void ChooseMath()
    {
        correctAnswers = 0;
        mathCat = true;
        GenerateQuestion(MathQuestions, originalMathQuestions);
        StartCoroutine(QuestionTimer());
    }
    public void ChooseLang()
    {
        correctAnswers = 0;
        languageCat = true;
        GenerateQuestion(LanguageQuestions, originalLanguageQuestions);
        StartCoroutine(QuestionTimer());

    }
    public void ChooseCelebs()
    {
        correctAnswers = 0;
        celebsCat = true;
        GenerateQuestion(CelebsQuestions, originalCelebsQuestions);
        StartCoroutine(QuestionTimer());
    }
    public void ChooseSports()
    {
        correctAnswers = 0;
        sportsCat = true;
        GenerateQuestion(SportsQuestions, originalSportsQuestions);
        StartCoroutine(QuestionTimer());
    }
    public void ChooseVideoGames()
    {
        correctAnswers = 0;
        videoGamesCat = true;
        GenerateQuestion(VideoGamesQuestions, originalVideoGamesQuestions);
        StartCoroutine(QuestionTimer());
    }
    public void ChooseAnimals()
    {
        correctAnswers = 0;
        animalsCat = true;
        GenerateQuestion(AnimalsQuestions, originalAnimalsQuestions);
        StartCoroutine(QuestionTimer());
    }
    public void ChooseMythology()
    {
        correctAnswers = 0;
        mythologyCat = true;
        GenerateQuestion(MythologyQuestions, originalMythologyQuestions);
        StartCoroutine(QuestionTimer());
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
}
