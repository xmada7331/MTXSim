using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
//using UnityEngine.UIElements;

public class ThunderManager : MonoBehaviour
{
    ThunderNPCResponse thunderNPCResponse;
    [Header("Register Window")]
    public GameObject registerWindow;
    public GameObject register2Window;
    public GameObject afterLoginWindow;
    public GameObject afterLoginLoading;
    public GameObject afterLoginSuccess;
    public TMP_Text afterLoginUsername;
    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;
    public TMP_InputField countryInput;
    public TMP_InputField genderInput;
    public TMP_InputField dayDateInput;
    public TMP_InputField monthDateInput;
    public TMP_InputField yearDateInput;
    public TMP_InputField pronouns1Input;
    public TMP_InputField pronouns2Input;
    public TMP_Text errorMessage;

    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_InputField emailInput;
    public TMP_Text errorMessage2;
    [Header("Swipe Window")]
    public Animator foundMatchAnimator;
    private int yayCount = 0;
    public int questionCount = 0;
    private string currentProfileName = "";
    private Sprite currentProfilePicture;
    public TMP_Text randomName;
    public TMP_Text randomAge;
    public TMP_Text randomGender;
    public TMP_Text randomDescription;
    public TMP_Text randomInterest1;
    public TMP_Text randomInterest2;
    public TMP_Text randomInterest3;
    public Image profilePicture;
    public List<Sprite> profilePictures;
    public List<string> names;
    public List<string> ages;
    public List<string> genders;
    public List<string> descriptions;
    public List<string> interest1;
    public List<string> interest2;
    public List<string> interest3;

    [Header("Chat Window")]
    private bool animationRunning = false;
    public TMP_Text chat_name;
    public Image chat_profilePicture;
    public ScrollRect chatScroll;
    public Animator profileAnimator;
    public Animator rejectAnimator;
    public Animator matchAnimator;

    public GameObject chatWindow;
    public GameObject swipeWindow;

    public GameObject choice1;
    public GameObject choice2;
    public GameObject choice3;

    public GameObject firstPath;


    public TMP_Text playerTextPrefab;
    public TMP_Text NPCTextPrefab;

    public Slider happinessSlider;
    public Slider spicinessSlider;
    public Slider romancenessSlider;

    public int happinessLevel = 0;
    public int spicinessLevel = 0;
    public int romancenessLevel = 0;

    private float selectDelay = 3f;

    public Transform chatContentParent;
    void Start()
    {
        GenerateNewProfile();
    }

    // Update is called once per frame
    void Update()
    {
        if (yayCount >= 3)
        {

            StartCoroutine(FoundMatch());
            yayCount = 0;
        }
    }

    public void TriggerYay()
    {
        if (!animationRunning)
        {
            StartCoroutine(YayTriggered());
            GenerateNewProfile();
        }
    }

    private IEnumerator YayTriggered()
    {
        currentProfileName = randomName.text;
        currentProfilePicture = profilePicture.sprite;
        yayCount++;
        animationRunning = true;
        profileAnimator.SetTrigger("Yay");
        yield return new WaitForSeconds(2f);
        profileAnimator.SetTrigger("Idle");
        animationRunning = false;
    }

    public void TriggerNay()
    {
        if (!animationRunning)
        {
            StartCoroutine(NayTriggered());
            GenerateNewProfile();
        }
    }

    private IEnumerator NayTriggered()
    {
        animationRunning = true;
        profileAnimator.SetTrigger("Nay");
        yield return new WaitForSeconds(2f);
        profileAnimator.SetTrigger("Idle");
        animationRunning = false;
    }

    public void SelectMessage1(GameObject target)
    {
        TMP_Text newText = Instantiate(playerTextPrefab, chatContentParent);
        newText.text = target.GetComponent<ThunderNPCResponse>().playerResponse;
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        GenerateNPCResponse(target);
        StartCoroutine(SelectedQuestion(target));
    }

    public void GenerateNPCResponse(GameObject target)
    {
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        StartCoroutine(MessageCooldown(target));
    }

    private IEnumerator MessageCooldown(GameObject target)
    {
        yield return new WaitForSeconds(1f);
        TMP_Text npcMessage = Instantiate(NPCTextPrefab, chatContentParent);
        npcMessage.text = "...";
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        //yield return new WaitForSeconds(Random.Range(1f,4f));
        yield return new WaitForSeconds(1f);
        Destroy(npcMessage.gameObject);
        //yield return new WaitForSeconds(Random.Range(0.2f,1.5f));
        yield return new WaitForSeconds(1f);
        if (questionCount < 7)
        {
            TMP_Text npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            npcMessage2.text = target.GetComponent<ThunderNPCResponse>().NPCResponse;
            yield return new WaitForSeconds(.1f);
            firstPath.SetActive(true);
            choice1.GetComponent<ThunderNPCResponse>().RandomizeChoice();
            choice2.GetComponent<ThunderNPCResponse>().RandomizeChoice();
            choice3.GetComponent<ThunderNPCResponse>().RandomizeChoice();
        }
        else if(questionCount >= 7 && happinessLevel > 5 || questionCount >= 7 && spicinessLevel > 5 || questionCount >= 7 && romancenessLevel > 5)
        {
            firstPath.SetActive(false);
            TMP_Text npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            npcMessage2.text = "Let's meet up!.";
            CheckEnding();
        }
        else
        {
            firstPath.SetActive(false);
            TMP_Text npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            npcMessage2.text = "I don't think we're a match.";
            CheckEnding();

        }
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        //yield return new WaitForSeconds(1f);


    }
    private IEnumerator ScrollToBottomAnimated(float duration)
    {
        // wait a frame to allow layout changes to register
        yield return null;

        // Force immediate layout rebuild so content size is correct
        Canvas.ForceUpdateCanvases();
        var contentRT = chatContentParent as RectTransform;
        if (contentRT != null)
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(contentRT);
        Canvas.ForceUpdateCanvases();

        if (chatScroll == null)
            yield break;

        // Determine start and target positions (0 = bottom, 1 = top for ScrollRect)
        float start = chatScroll.verticalNormalizedPosition;
        float target = 0f;

        // If already at (or extremely close to) target, snap and exit
        if (Mathf.Approximately(start, target))
        {
            chatScroll.verticalNormalizedPosition = target;
            chatScroll.velocity = Vector2.zero;
            yield break;
        }

        float elapsed = 0f;
        // Use unscaled delta so animation works even if Time.timeScale changes
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // smooth interpolation (ease-out)
            float ease = Mathf.SmoothStep(0f, 1f, t);
            chatScroll.verticalNormalizedPosition = Mathf.Lerp(start, target, ease);
            chatScroll.velocity = Vector2.zero;
            yield return null;
        }

        // ensure final state
        chatScroll.verticalNormalizedPosition = target;
        chatScroll.velocity = Vector2.zero;
    }

    private IEnumerator RandomizeProfile()
    {
        int randomPictureIndex = Random.Range(0, profilePictures.Count);
        int randomNameIndex = Random.Range(0, names.Count);
        int randomAgeIndex = Random.Range(0, ages.Count);
        int randomGenderIndex = Random.Range(0, genders.Count);
        int randomDescriptionIndex = Random.Range(0, descriptions.Count);
        int randomInterest1Index = Random.Range(0, interest1.Count);
        int randomInterest2Index = Random.Range(0, interest2.Count);
        int randomInterest3Index = Random.Range(0, interest3.Count);
        yield return new WaitForSeconds(.7f);
        int index = Random.Range(0, names.Count);
        randomName.text = names[randomNameIndex];
        randomAge.text = ages[randomAgeIndex];
        randomGender.text = genders[randomGenderIndex];
        randomDescription.text = descriptions[randomDescriptionIndex];
        randomInterest1.text = interest1[randomInterest1Index];
        randomInterest2.text = interest2[randomInterest2Index];
        randomInterest3.text = interest3[randomInterest3Index];
        profilePicture.sprite = profilePictures[randomPictureIndex];

    }

    public void GenerateNewProfile()
    {
        StartCoroutine(RandomizeProfile());
    }

    private IEnumerator FoundMatch()
    {
        firstPath.SetActive(true);
        for (int i = chatContentParent.childCount - 1; i >= 0; i--)
        {
            var child = chatContentParent.GetChild(i);

            Destroy(child.gameObject);
        }
        happinessLevel = 0;
        spicinessLevel = 0;
        romancenessLevel = 0;
        happinessSlider.value = happinessLevel;
        spicinessSlider.value = spicinessLevel;
        romancenessSlider.value = romancenessLevel;
        chat_name.text = currentProfileName;
        chat_profilePicture.sprite = currentProfilePicture;
        yayCount = 0;
        questionCount = 0;
        foundMatchAnimator.SetTrigger("Found");
        yield return new WaitForSeconds(2f);
        swipeWindow.SetActive(false);
        chatWindow.SetActive(true);
        yield return new WaitForSeconds(1f);
        foundMatchAnimator.SetTrigger("Idle");
    }

    public void FirstRegisterPass()
    {
        bool requiredMissing =
            string.IsNullOrEmpty(firstNameInput.text) ||
            string.IsNullOrEmpty(lastNameInput.text) ||
            string.IsNullOrEmpty(countryInput.text) ||
            string.IsNullOrEmpty(genderInput.text) ||
            string.IsNullOrEmpty(dayDateInput.text) ||
            string.IsNullOrEmpty(monthDateInput.text) ||
            string.IsNullOrEmpty(yearDateInput.text) ||
            string.IsNullOrEmpty(pronouns1Input.text) ||
            string.IsNullOrEmpty(pronouns2Input.text);
        if (requiredMissing)
        {
            StartCoroutine(ShowErrorEmpty());
            return;
        }
        if (!int.TryParse(yearDateInput.text, out int year) ||
            !int.TryParse(monthDateInput.text, out int month) ||
            !int.TryParse(dayDateInput.text, out int day))
        {
            StartCoroutine(ShowErrorAge());
            return;
        }

        // Validate date and check 18+
        if (!IsAtLeastAge(year, month, day, 18))
        {
            StartCoroutine(ShowErrorAge());
            return;
        }
        else
        {
            registerWindow.SetActive(false);
            register2Window.SetActive(true);
        }

    }

    private IEnumerator ShowErrorEmpty()
    {
        errorMessage.gameObject.SetActive(true);
        errorMessage.text = "Please fill all fields.";
        yield return new WaitForSeconds(2f);
        errorMessage.gameObject.SetActive(false);
    }
    private IEnumerator ShowErrorAge()
    {
        errorMessage.gameObject.SetActive(true);
        errorMessage.text = "You must be 18 or above.";
        yield return new WaitForSeconds(2f);
        errorMessage.gameObject.SetActive(false);
    }

    private bool IsAtLeastAge(int year, int month, int day, int requiredYears = 18)
    {
        try
        {
            var dob = new System.DateTime(year, month, day);
            var today = System.DateTime.Today;
            int age = today.Year - dob.Year;
            if (today < dob.AddYears(age)) age--;
            return age >= requiredYears;
        }
        catch
        {
            // invalid date (e.g., Feb 30) or other error -> treat as invalid / not old enough
            return false;
        }
    }

    public void SecondRegisterPass()
    {
        bool requiredMissing =
            string.IsNullOrEmpty(usernameInput.text) ||
            string.IsNullOrEmpty(passwordInput.text) ||
            string.IsNullOrEmpty(confirmPasswordInput.text) ||
            string.IsNullOrEmpty(emailInput.text);
        if (requiredMissing)
        {
            StartCoroutine(ShowError2Empty());
            return;
        }
        if (passwordInput.text != confirmPasswordInput.text)
        {
            StartCoroutine(ShowErrorPasswordMismatch());
            return;
        }
        if (!emailInput.text.Contains("@") || !emailInput.text.Contains("."))
        {
            StartCoroutine(ShowError2Email());
            return;
        }
        register2Window.SetActive(false);
        afterLoginWindow.SetActive(true);
        StartCoroutine(AfterLoginSuccess());
    }
    private IEnumerator ShowError2Empty()
    {
        errorMessage2.gameObject.SetActive(true);
        errorMessage2.text = "Please fill all fields.";
        yield return new WaitForSeconds(2f);
        errorMessage2.gameObject.SetActive(false);
    }
    private IEnumerator ShowError2Email()
    {
        errorMessage2.gameObject.SetActive(true);
        errorMessage2.text = "enter correct email.";
        yield return new WaitForSeconds(2f);
        errorMessage2.gameObject.SetActive(false);
    }

    private IEnumerator ShowErrorPasswordMismatch()
    {
        errorMessage2.gameObject.SetActive(true);
        errorMessage2.text = "Passwords do not match.";
        yield return new WaitForSeconds(2f);
        errorMessage2.gameObject.SetActive(false);
    }

    private IEnumerator AfterLoginSuccess()
    {
        afterLoginWindow.SetActive(true);
        afterLoginLoading.SetActive(true);
        yield return new WaitForSeconds(5f);
        afterLoginLoading.SetActive(false);
        afterLoginSuccess.SetActive(true);
        afterLoginUsername.text = "Hello, " + usernameInput.text;
    }

    private IEnumerator SelectedQuestion(GameObject target)
    {
        questionCount++;
        thunderNPCResponse = target.GetComponent<ThunderNPCResponse>();
        firstPath.SetActive(false);
        if (Random.value < 0.5f)
        {
            thunderNPCResponse.triggersHappiness = true;
            thunderNPCResponse.happinessAmount = 1;
        }
        if (Random.value < 0.5f)
        {
            thunderNPCResponse.triggersSpiciness = true;
            thunderNPCResponse.spicinessAmount = 1;
        }
        if (Random.value < 0.5f)
        {
            thunderNPCResponse.triggersRomanceness = true;
            thunderNPCResponse.romancenessAmount = 1;
        }
        else
        {
            thunderNPCResponse.happinessAmount = 0;
            thunderNPCResponse.spicinessAmount = 0;
            thunderNPCResponse.romancenessAmount = 0;
        }


        if (thunderNPCResponse.triggersHappiness)
        {
            happinessLevel += thunderNPCResponse.happinessAmount;
            happinessSlider.value = happinessLevel;
        }
        if (thunderNPCResponse.triggersSpiciness)
        {
            spicinessLevel += thunderNPCResponse.spicinessAmount;
            spicinessSlider.value = spicinessLevel;
        }
        if (thunderNPCResponse.triggersRomanceness)
        {
            romancenessLevel += thunderNPCResponse.romancenessAmount;
            romancenessSlider.value = romancenessLevel;
        }
        yield return new WaitForSeconds(selectDelay);
        

    }

    private void CheckEnding()
    {
        if (happinessLevel >= 5 || spicinessLevel >= 5 || romancenessLevel >= 5)
        {
            StartCoroutine(MatchedAnimation());
        }
        else
        {
            StartCoroutine(RejectionAnimation());
        }
    }

    private IEnumerator RejectionAnimation()
    {
        questionCount = 0;
        yield return new WaitForSeconds(1f);
        firstPath.SetActive(false);
        rejectAnimator.SetTrigger("Rejected");
        yield return new WaitForSeconds(1f);
        chatWindow.SetActive(false);
        swipeWindow.SetActive(true);
        yield return new WaitForSeconds(1f);
        firstPath.SetActive(true);
        yield return new WaitForSeconds(5f);
        rejectAnimator.ResetTrigger("Rejected");
        rejectAnimator.SetTrigger("Idle");
        yield return null;

    }
    private IEnumerator MatchedAnimation()
    {
        questionCount = 0;
        yield return new WaitForSeconds(1f);
        firstPath.SetActive(false);
        matchAnimator.SetTrigger("Matched");
        yield return new WaitForSeconds(1f);
        chatWindow.SetActive(false);
        swipeWindow.SetActive(true);
        yield return new WaitForSeconds(1f);
        firstPath.SetActive(true);
        yield return new WaitForSeconds(5f);
        matchAnimator.ResetTrigger("Matched");
        matchAnimator.SetTrigger("Idle");
        yield return null;

    }

}
