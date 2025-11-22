using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextAppear : MonoBehaviour
{
    public TMP_Text Text;
    public float delay = 0.1f;
    public string CurrentText;
    public GameObject extraText;
    public float extraItemDelay = 3f;
    void Start()
    {
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        Text.text = "";
        
        string originalText = CurrentText;
        string displayedText = "";
        int alphaIndex = 0;

        foreach(char c in CurrentText.ToCharArray())
        {
            alphaIndex++;
            Text.text = originalText;
            displayedText = Text.text.Insert(alphaIndex, "<color=#00000000>");
            Text.text = displayedText;

            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(extraItemDelay);
        extraText.SetActive(true);

    }


}
