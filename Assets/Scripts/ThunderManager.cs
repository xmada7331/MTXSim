using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
//using UnityEngine.UIElements;

public class ThunderManager : MonoBehaviour
{

    public TMP_Text usernameHome;
    public AudioSource thunderAudioSource;
    public AudioClip foundMatch;
    public AudioClip messageAppear;
    public AudioClip register;
    public AudioClip rejected;
    public AudioClip matched;
    public AudioClip sendMessage;
    public AudioClip swipeNay;
    public AudioClip swipeYay;
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

    [Header("NPC Response and selection")]
    public TMP_Text responseDescription1;
    public TMP_Text responseDescription2;
    public TMP_Text responseDescription3;
    public List<string> mainText;
    public List<string> playerResponses;
    private string playerResponse1;
    private string playerResponse2;
    private string playerResponse3;
    public List<string> npcResponses;
    private string NPCResponse1;
    private string NPCResponse2;
    private string NPCResponse3;

    private bool triggersHappiness;
    private bool triggersSpiciness;
    private bool triggersRomanceness;

    private int happinessAmount;
    private int spicinessAmount;
    private int romancenessAmount;

    private List<string> originalMainText;
    private List<string> originalPlayerResponses; 
    private List<string> originalNpcResponses; 
    private int currentResponse;
    private List<string> poolMainText;
    private List<string> poolPlayerResponses;
    private List<string> poolNpcResponses;

    private int randomIndex1;
    private int randomIndex2;
    private int randomIndex3;

    public Transform chatContentParent;
    void Start()
    {
        originalMainText = new List<string>(mainText);
        originalPlayerResponses = new List<string>(playerResponses);
        originalNpcResponses = new List<string>(npcResponses);
        ResetPools();
        RandomizeChoice();
        GenerateNewProfile();
    }

    // Update is called once per frame
    void Update()
    {
        usernameHome.text = usernameInput.text;
        if (yayCount >= 3)
        {

            StartCoroutine(FoundMatch());
            yayCount = 0;
        }
    }
    private void ResetPools()
    {
        poolMainText = new List<string>(originalMainText);
        poolPlayerResponses = new List<string>(originalPlayerResponses);
        poolNpcResponses = new List<string>(originalNpcResponses);
    }
    public void RandomizeChoice()
    {
        // ensure pools initialized
        if (poolMainText == null || poolPlayerResponses == null || poolNpcResponses == null)
            ResetPools();

        // if there are fewer than 3 entries left, restore full set (you can change this behavior if you prefer)
        if (poolMainText.Count < 3 || poolPlayerResponses.Count < 3 || poolNpcResponses.Count < 3)
        {
            ResetPools();
        }

        int poolCount = Mathf.Min(poolMainText.Count, poolPlayerResponses.Count, poolNpcResponses.Count);
        if (poolCount == 0)
        {
            responseDescription1.text = "";
            responseDescription2.text = "";
            responseDescription3.text = "";
            playerResponse1 = playerResponse2 = playerResponse3 = "";
            NPCResponse1 = NPCResponse2 = NPCResponse3 = "";
            return;
        }

        // pick three unique indices from the pool
        List<int> available = new List<int>(poolCount);
        for (int i = 0; i < poolCount; i++) available.Add(i);

        int pickPos = Random.Range(0, available.Count);
        randomIndex1 = available[pickPos];
        available.RemoveAt(pickPos);

        pickPos = Random.Range(0, available.Count);
        randomIndex2 = available[pickPos];
        available.RemoveAt(pickPos);

        pickPos = Random.Range(0, available.Count);
        randomIndex3 = available[pickPos];
        available.RemoveAt(pickPos);

        // assign texts from pools
        responseDescription1.text = poolMainText[randomIndex1];
        responseDescription2.text = poolMainText[randomIndex2];
        responseDescription3.text = poolMainText[randomIndex3];

        playerResponse1 = poolPlayerResponses[randomIndex1];
        playerResponse2 = poolPlayerResponses[randomIndex2];
        playerResponse3 = poolPlayerResponses[randomIndex3];

        NPCResponse1 = poolNpcResponses[randomIndex1];
        NPCResponse2 = poolNpcResponses[randomIndex2];
        NPCResponse3 = poolNpcResponses[randomIndex3];
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
        thunderAudioSource.PlayOneShot(swipeYay);
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
        thunderAudioSource.PlayOneShot(swipeNay);
        animationRunning = true;
        profileAnimator.SetTrigger("Nay");
        yield return new WaitForSeconds(2f);
        profileAnimator.SetTrigger("Idle");
        animationRunning = false;
    }

    public void SelectMessage1()
    {
        thunderAudioSource.PlayOneShot(sendMessage);
        TMP_Text playerMessage = Instantiate(playerTextPrefab, chatContentParent);
        playerMessage.text = playerResponse1;
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        GenerateNPCResponse1();
        StartCoroutine(SelectedQuestion());
    }
    public void SelectMessage2()
    {
        
        thunderAudioSource.PlayOneShot(sendMessage);
        TMP_Text playerMessage = Instantiate(playerTextPrefab, chatContentParent);
        playerMessage.text = playerResponse2;
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        GenerateNPCResponse2();
        StartCoroutine(SelectedQuestion());
    }
    public void SelectMessage3()
    {
        thunderAudioSource.PlayOneShot(sendMessage);
        TMP_Text playerMessage = Instantiate(playerTextPrefab, chatContentParent);
        playerMessage.text = playerResponse3;
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        GenerateNPCResponse3();
        StartCoroutine(SelectedQuestion());
    }

    public void GenerateNPCResponse1()
    {
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        StartCoroutine(MessageCooldown1());
    }
    public void GenerateNPCResponse2()
    {
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        StartCoroutine(MessageCooldown2());
    }
    public void GenerateNPCResponse3()
    {
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        StartCoroutine(MessageCooldown3());
    }

    private IEnumerator MessageCooldown1()
    {
        yield return new WaitForSeconds(1f);
        TMP_Text npcMessage = Instantiate(NPCTextPrefab, chatContentParent);
        thunderAudioSource.PlayOneShot(messageAppear);
        npcMessage.text = "...";
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        //yield return new WaitForSeconds(Random.Range(1f,4f));
        yield return new WaitForSeconds(1f);
        Destroy(npcMessage.gameObject);
        //yield return new WaitForSeconds(Random.Range(0.2f,1.5f));
        yield return new WaitForSeconds(1f);
        currentResponse = randomIndex1;
        if (currentResponse >= 0 && currentResponse < poolMainText.Count)
        {
            poolPlayerResponses.RemoveAt(currentResponse);
            poolMainText.RemoveAt(currentResponse);
            poolNpcResponses.RemoveAt(currentResponse);
        }
        if (questionCount < 7)
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            TMP_Text npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            npcMessage2.text = NPCResponse1;
            yield return new WaitForSeconds(.1f);
            firstPath.SetActive(true);

            RandomizeChoice();
        }
        else if(questionCount >= 7 && happinessLevel >= 5 || questionCount >= 7 && spicinessLevel >= 5 || questionCount >= 7 && romancenessLevel >= 5)
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            TMP_Text npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            npcMessage2.text = "Let's meet up!";
            CheckEnding();
        }
        else
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            TMP_Text npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            npcMessage2.text = "I don't think we're a match.";
            CheckEnding();

        }
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        //yield return new WaitForSeconds(1f);
    }
    private IEnumerator MessageCooldown2()
    {
        yield return new WaitForSeconds(1f);
        TMP_Text npcMessage = Instantiate(NPCTextPrefab, chatContentParent);
        thunderAudioSource.PlayOneShot(messageAppear);
        npcMessage.text = "...";
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        //yield return new WaitForSeconds(Random.Range(1f,4f));
        yield return new WaitForSeconds(1f);
        Destroy(npcMessage.gameObject);
        //yield return new WaitForSeconds(Random.Range(0.2f,1.5f));
        yield return new WaitForSeconds(1f);
        currentResponse = randomIndex2;
        if (currentResponse >= 0 && currentResponse < poolMainText.Count)
        {
            poolPlayerResponses.RemoveAt(currentResponse);
            poolMainText.RemoveAt(currentResponse);
            poolNpcResponses.RemoveAt(currentResponse);
        }

        if (questionCount < 7)
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            TMP_Text npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            npcMessage2.text = NPCResponse2;
            yield return new WaitForSeconds(.1f);
            firstPath.SetActive(true);

            RandomizeChoice();
        }
        else if (questionCount >= 7 && happinessLevel >= 5 || questionCount >= 7 && spicinessLevel >= 5 || questionCount >= 7 && romancenessLevel >= 5)
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            TMP_Text npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            npcMessage2.text = "Let's meet up!";
            CheckEnding();
        }
        else
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            TMP_Text npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            npcMessage2.text = "I don't think we're a match.";
            CheckEnding();

        }
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        //yield return new WaitForSeconds(1f);
    }
    private IEnumerator MessageCooldown3()
    {
        yield return new WaitForSeconds(1f);
        TMP_Text npcMessage = Instantiate(NPCTextPrefab, chatContentParent);
        thunderAudioSource.PlayOneShot(messageAppear);
        npcMessage.text = "...";
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        //yield return new WaitForSeconds(Random.Range(1f,4f));
        yield return new WaitForSeconds(1f);
        Destroy(npcMessage.gameObject);
        //yield return new WaitForSeconds(Random.Range(0.2f,1.5f));
        yield return new WaitForSeconds(1f);

        currentResponse = randomIndex3;
        if (currentResponse >= 0 && currentResponse < poolMainText.Count)
        {
            poolPlayerResponses.RemoveAt(currentResponse);
            poolMainText.RemoveAt(currentResponse);
            poolNpcResponses.RemoveAt(currentResponse);
        }


        if (questionCount < 7)
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            TMP_Text npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            npcMessage2.text = NPCResponse3;
            yield return new WaitForSeconds(.1f);
            firstPath.SetActive(true);
            RandomizeChoice();
        }
        else if (questionCount >= 7 && happinessLevel >= 5 || questionCount >= 7 && spicinessLevel >= 5 || questionCount >= 7 && romancenessLevel >= 5)
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            TMP_Text npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            npcMessage2.text = "Let's meet up!";
            CheckEnding();
        }
        else
        {
            thunderAudioSource.PlayOneShot(messageAppear);
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
        thunderAudioSource.PlayOneShot(foundMatch);
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

        ResetPools();
        RandomizeChoice();

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

    private IEnumerator SelectedQuestion()
    {
        questionCount++;
        firstPath.SetActive(false);
        if (Random.value < 0.5f)
        {
            triggersHappiness = true;
            happinessAmount = 1;
        }
        if (Random.value < 0.5f)
        {
            triggersSpiciness = true;
            spicinessAmount = 1;
        }
        if (Random.value < 0.5f)
        {
            triggersRomanceness = true;
            romancenessAmount = 1;
        }
        else
        {
            happinessAmount = 0;
            spicinessAmount = 0;
            romancenessAmount = 0;
        }


        if (triggersHappiness)
        {
            happinessLevel += happinessAmount;
            happinessSlider.value = happinessLevel;
        }
        if (triggersSpiciness)
        {
            spicinessLevel += spicinessAmount;
            spicinessSlider.value = spicinessLevel;
        }
        if (triggersRomanceness)
        {
            romancenessLevel += romancenessAmount;
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
        thunderAudioSource.PlayOneShot(rejected);
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
        thunderAudioSource.PlayOneShot(matched);
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
