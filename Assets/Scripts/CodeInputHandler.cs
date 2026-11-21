using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeInputHandler : MonoBehaviour
{
    GameManager gameManager;
    public GameObject cheatImage;
    private string inputSequence = "";
    private Dictionary<string, System.Action> codes = new Dictionary<string, System.Action>();

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        codes.Add("givemoneys", GiveMoney);
        codes.Add("givegemzz", GiveGems);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    char keyChar = KeyCodeToChar(keyCode);
                    if (keyChar != '\0')
                    {
                        inputSequence += keyChar;
                        if (inputSequence.Length > 10) // Adjust the length based on the longest code
                        {
                            inputSequence = inputSequence.Substring(inputSequence.Length - 10);
                        }

                        foreach (var code in codes)
                        {
                            if (inputSequence.EndsWith(code.Key))
                            {
                                code.Value.Invoke();
                                inputSequence = ""; // Reset the input sequence
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    private char KeyCodeToChar(KeyCode keyCode)
    {
        if (keyCode >= KeyCode.A && keyCode <= KeyCode.Z)
        {
            return (char)('a' + (keyCode - KeyCode.A));
        }
        return '\0';
    }
    private void TriggerCheatImage()
    {
               if (cheatImage != null)
        {
            StartCoroutine(ShowCheatImage());
        }
    }

    private IEnumerator ShowCheatImage()
    {
        cheatImage.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        cheatImage.SetActive(false);
    }

    private void GiveMoney()
    {
        
        if (gameManager != null)
        {
            TriggerCheatImage();
            gameManager.playerMoney += 200;
            //Debug.Log("Cheat code activated: Gave $200");
        }
    }
    private void GiveGems()
    {

        if (gameManager != null)
        {
            TriggerCheatImage();
            gameManager.playerGems += 200;
            //Debug.Log("Cheat code activated: Gave 200 Gems");
        }
    }
}
