using System.Collections;
using TMPro;
using UnityEngine;

public class CardLinking : MonoBehaviour
{
    ContentChecker contentChecker;

    public AudioSource cardAudioSource;
    public AudioClip successLink;

    public TMP_InputField cardNumber;
    public TMP_InputField expiryDateMonth;
    public TMP_InputField expiryDateYear;
    public TMP_InputField cardName;
    public TMP_InputField securityCode;

    public GameObject errorMessage;
    public GameObject parentWindow;
    public GameObject mainWindow;
    public GameObject successWindow;
    public GameObject linkCardBlock;
    public GameObject settingsBlock;

    private Coroutine hideErrorCoroutine;

    private void Awake()
    {
        contentChecker = FindObjectOfType<ContentChecker>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ShowError(string message)
    {
        if (errorMessage == null) return;

        if (hideErrorCoroutine != null)
        {
            StopCoroutine(hideErrorCoroutine);
        }

        errorMessage.SetActive(true);
        TMP_Text errorText = errorMessage.GetComponentInChildren<TMP_Text>(true);
        if (errorText != null)
        {
            errorText.text = message;
        }

        hideErrorCoroutine = StartCoroutine(HideErrorAfterDelay(2f));
    }

    private IEnumerator HideErrorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (errorMessage != null)
        {
            errorMessage.SetActive(false);
        }
        hideErrorCoroutine = null;
    }

    public void ConfirmCardInfo()
    {
        if (hideErrorCoroutine != null)
        {
            StopCoroutine(hideErrorCoroutine);
            hideErrorCoroutine = null;
        }
        if (errorMessage != null)
        {
            errorMessage.SetActive(false);
        }

        string cardNum = cardNumber.text.Trim();
        string monthText = expiryDateMonth.text.Trim();
        string yearText = expiryDateYear.text.Trim();
        string nameText = cardName.text.Trim();
        string cvvText = securityCode.text.Trim();

        if (cardNum.Length == 0 || monthText.Length == 0 || yearText.Length == 0 || nameText.Length == 0 || cvvText.Length == 0)
        {
            ShowError("Please fill in all fields.");
            return;
        }

        if (cardNum.Length != 16)
        {
            ShowError("Card number must be 16 digits.");
            return;
        }

        if (monthText.Length != 2 || yearText.Length != 2)
        {
            ShowError("Expiry date must be in MM/YY format.");
            return;
        }

        // Validate month numeric value and range (01..12). Reject "00" and >12.
        if (!int.TryParse(monthText, out int month) || month < 1 || month > 12)
        {
            ShowError("Please enter correct month.");
            return;
        }

        if (cvvText.Length != 3)
        {
            ShowError("Security code must be 3 digits.");
            return;
        }

        SuccesfullyLink();
    }

    private void SuccesfullyLink()
    {
        StartCoroutine(CloseMainWindow());
        contentChecker.hasCardLinked = true;
        mainWindow.SetActive(false);
        linkCardBlock.SetActive(false);
        successWindow.SetActive(true);
        settingsBlock.SetActive(false);
        cardAudioSource.PlayOneShot(successLink);
    }

    private IEnumerator CloseMainWindow()
    {
        yield return new WaitForSeconds(5f);
        parentWindow.SetActive(false);
    }
    public void SkipConfirm()
    {
        SuccesfullyLink();
    }
}
