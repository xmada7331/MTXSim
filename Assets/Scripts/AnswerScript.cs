using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public SurveySystem surveySystem;
    public void Answer()
    {
        if (surveySystem.questionAmount < 4) // For first 4 questions, process and go to next
        {
            if (isCorrect)
            {
                Debug.Log("Correct Answer");
                surveySystem.correct();
            }
            else
            {
                Debug.Log("Wrong Answer");
                surveySystem.wrong();
            }
        }
        else if (surveySystem.questionAmount == 4) // For the 5th question, process and end
        {
            if (isCorrect)
            {
                Debug.Log("Correct Answer");
                surveySystem.correct();
            }
            else
            {
                Debug.Log("Wrong Answer");
                surveySystem.wrong();
            }
            Debug.Log("Quiz Ended");
            surveySystem.EndSurvey();
        }
        // No need for an else branch, as survey will be ended after 5 answers
    }
}
