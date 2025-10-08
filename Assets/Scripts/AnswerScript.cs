using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public SurveySystem surveySystem;
    public void Answer()
    {
        if (isCorrect && surveySystem.questionAmount < 5)
        {
            Debug.Log("Correct Answer");
            surveySystem.correct();
        }
        else if (!isCorrect && surveySystem.questionAmount < 5)
        {
            Debug.Log("Wrong Answer");
            surveySystem.wrong();
        }
        else
        {
                       Debug.Log("Quiz Ended");
            surveySystem.EndSurvey();
        }
    }
}
