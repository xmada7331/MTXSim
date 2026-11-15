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

    [Header("Profile Picture Sprite Arrays")]
    public Sprite[] additionalBgSprites;
    public Sprite[] bodySprites;
    public Sprite[] headSprites;
    public Sprite[] faceSprites;
    public Sprite[] hairSprites;

    [Header("Profile Picture Layers")]
    public Image additionalBgImage;
    public Image bodyImage;
    public Image headImage;
    public Image faceImage;
    public Image hairImage;

    private Sprite currentAdditionalBg;
    private Sprite currentBody;
    private Sprite currentHead;
    private Sprite currentFace;
    private Sprite currentHair;

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

    [Header("Chat Buttons")]
    public Button sendMoneyButton;
    private bool isDonatingMoney = false;

    [Header("Chat List / Contacts")]
    public GameObject chatListWindow;
    public Transform contactListParent;
    public GameObject contactItemPrefab;

    [Header("Contacts Database")]
    public List<ThunderContact> contacts = new List<ThunderContact>();
    [HideInInspector] public ThunderContact activeContact;
    public GameManager gameManager;

    private string currentProfileAge = "";
    private string currentProfileDescription = "";
    private List<GameObject> spawnedContactItems = new List<GameObject>();
    private GameObject contactTemplate = null;

    public GameObject choice1;
    public GameObject choice2;
    public GameObject choice3;

    public GameObject firstPath;


    public GameObject playerTextPrefab;
    public GameObject NPCTextPrefab;

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
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
        if (sendMoneyButton == null && chatWindow != null)
        {
            Button[] btns = chatWindow.GetComponentsInChildren<Button>(true);
            foreach (var b in btns)
            {
                if (b.name.ToLower().Contains("sendmoney") || b.name.ToLower().Contains("donate"))
                {
                    sendMoneyButton = b;
                    break;
                }
            }
        }
        originalMainText = new List<string>(mainText);
        originalPlayerResponses = new List<string>(playerResponses);
        originalNpcResponses = new List<string>(npcResponses);
        ResetPools();
        RandomizeChoice();
        EnsureProfileImageReferences();
        RandomizeProfileParts();
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

        if (activeContact != null)
        {
            activeContact.currentChoice1 = responseDescription1 != null ? responseDescription1.text : "";
            activeContact.currentChoice2 = responseDescription2 != null ? responseDescription2.text : "";
            activeContact.currentChoice3 = responseDescription3 != null ? responseDescription3.text : "";
            activeContact.currentPlayerResp1 = playerResponse1;
            activeContact.currentPlayerResp2 = playerResponse2;
            activeContact.currentPlayerResp3 = playerResponse3;
            activeContact.currentNpcResp1 = NPCResponse1;
            activeContact.currentNpcResp2 = NPCResponse2;
            activeContact.currentNpcResp3 = NPCResponse3;
            activeContact.firstPathActive = true;
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
        thunderAudioSource.PlayOneShot(swipeYay);
        currentProfileName = randomName.text;
        currentProfilePicture = (headImage != null && headImage.sprite != null) ? headImage.sprite : (profilePicture != null ? profilePicture.sprite : null);
        currentAdditionalBg = additionalBgImage != null ? additionalBgImage.sprite : null;
        currentBody = bodyImage != null ? bodyImage.sprite : null;
        currentHead = headImage != null ? headImage.sprite : null;
        currentFace = faceImage != null ? faceImage.sprite : null;
        currentHair = hairImage != null ? hairImage.sprite : null;
        if (randomAge != null) currentProfileAge = randomAge.text;
        if (randomDescription != null) currentProfileDescription = randomDescription.text;
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
        EnsureActiveContact();
        thunderAudioSource.PlayOneShot(sendMessage);
        GameObject playerMessage = Instantiate(playerTextPrefab, chatContentParent);
        TMP_Text playerText = playerMessage.GetComponentInChildren<TMP_Text>();
        if (playerText != null) playerText.text = playerResponse1;
        if (activeContact != null)
        {
            activeContact.chatHistory.Add(new ChatMessageData { isPlayer = true, messageText = playerResponse1 });
        }
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        GenerateNPCResponse1();
        StartCoroutine(SelectedQuestion());
    }
    public void SelectMessage2()
    {
        
        EnsureActiveContact();
        thunderAudioSource.PlayOneShot(sendMessage);
        GameObject playerMessage = Instantiate(playerTextPrefab, chatContentParent);
        TMP_Text playerText = playerMessage.GetComponentInChildren<TMP_Text>();
        if (playerText != null) playerText.text = playerResponse2;
        if (activeContact != null)
        {
            activeContact.chatHistory.Add(new ChatMessageData { isPlayer = true, messageText = playerResponse2 });
        }
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        GenerateNPCResponse2();
        StartCoroutine(SelectedQuestion());
    }
    public void SelectMessage3()
    {
        EnsureActiveContact();
        thunderAudioSource.PlayOneShot(sendMessage);
        GameObject playerMessage = Instantiate(playerTextPrefab, chatContentParent);
        TMP_Text playerText = playerMessage.GetComponentInChildren<TMP_Text>();
        if (playerText != null) playerText.text = playerResponse3;
        if (activeContact != null)
        {
            activeContact.chatHistory.Add(new ChatMessageData { isPlayer = true, messageText = playerResponse3 });
        }
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
        GameObject npcMessage = Instantiate(NPCTextPrefab, chatContentParent);
        TMP_Text npcText = npcMessage.GetComponentInChildren<TMP_Text>();
        thunderAudioSource.PlayOneShot(messageAppear);
        if (npcText != null) npcText.text = "...";
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        //yield return new WaitForSeconds(Random.Range(1f,4f));
        yield return new WaitForSeconds(1f);
        Destroy(npcMessage);
        //yield return new WaitForSeconds(Random.Range(0.2f,1.5f));
        yield return new WaitForSeconds(1f);
        currentResponse = randomIndex1;
        if (currentResponse >= 0 && currentResponse < poolMainText.Count)
        {
            poolPlayerResponses.RemoveAt(currentResponse);
            poolMainText.RemoveAt(currentResponse);
            poolNpcResponses.RemoveAt(currentResponse);
        }
        if (IsAnyStatAbove75Percent())
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            GameObject npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text npcText2 = npcMessage2.GetComponentInChildren<TMP_Text>();
            if (npcText2 != null) npcText2.text = "Let's meet up!";
            if (activeContact != null)
            {
                activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = "Let's meet up!" });
                activeContact.isMatchedSuccess = true;
                activeContact.firstPathActive = false;
            }
            StartCoroutine(MatchedAnimation());
        }
        else if (questionCount < 7)
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            GameObject npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text npcText2 = npcMessage2.GetComponentInChildren<TMP_Text>();
            if (npcText2 != null) npcText2.text = NPCResponse1;
            if (activeContact != null)
            {
                activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = NPCResponse1 });
            }
            yield return new WaitForSeconds(.1f);
            firstPath.SetActive(true);

            RandomizeChoice();
        }
        else if(questionCount >= 7 && happinessLevel >= 5 || questionCount >= 7 && spicinessLevel >= 5 || questionCount >= 7 && romancenessLevel >= 5)
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            GameObject npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text npcText2 = npcMessage2.GetComponentInChildren<TMP_Text>();
            if (npcText2 != null) npcText2.text = "Let's meet up!";
            if (activeContact != null) activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = "Let's meet up!" });
            CheckEnding();
        }
        else
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            GameObject npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text npcText2 = npcMessage2.GetComponentInChildren<TMP_Text>();
            if (npcText2 != null) npcText2.text = "I don't think we're a match.";
            if (activeContact != null) activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = "I don't think we're a match." });
            CheckEnding();

        }
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        //yield return new WaitForSeconds(1f);
    }
    private IEnumerator MessageCooldown2()
    {
        yield return new WaitForSeconds(1f);
        GameObject npcMessage = Instantiate(NPCTextPrefab, chatContentParent);
        TMP_Text npcText = npcMessage.GetComponentInChildren<TMP_Text>();
        thunderAudioSource.PlayOneShot(messageAppear);
        if (npcText != null) npcText.text = "...";
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        //yield return new WaitForSeconds(Random.Range(1f,4f));
        yield return new WaitForSeconds(1f);
        Destroy(npcMessage);
        //yield return new WaitForSeconds(Random.Range(0.2f,1.5f));
        yield return new WaitForSeconds(1f);
        currentResponse = randomIndex2;
        if (currentResponse >= 0 && currentResponse < poolMainText.Count)
        {
            poolPlayerResponses.RemoveAt(currentResponse);
            poolMainText.RemoveAt(currentResponse);
            poolNpcResponses.RemoveAt(currentResponse);
        }
        if (IsAnyStatAbove75Percent())
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            GameObject npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text npcText2 = npcMessage2.GetComponentInChildren<TMP_Text>();
            if (npcText2 != null) npcText2.text = "Let's meet up!";
            if (activeContact != null)
            {
                activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = "Let's meet up!" });
                activeContact.isMatchedSuccess = true;
                activeContact.firstPathActive = false;
            }
            StartCoroutine(MatchedAnimation());
        }
        else if (questionCount < 7)
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            GameObject npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text npcText2 = npcMessage2.GetComponentInChildren<TMP_Text>();
            if (npcText2 != null) npcText2.text = NPCResponse2;
            if (activeContact != null)
            {
                activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = NPCResponse2 });
            }
            yield return new WaitForSeconds(.1f);
            firstPath.SetActive(true);

            RandomizeChoice();
        }
        else if (questionCount >= 7 && happinessLevel >= 5 || questionCount >= 7 && spicinessLevel >= 5 || questionCount >= 7 && romancenessLevel >= 5)
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            GameObject npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text npcText2 = npcMessage2.GetComponentInChildren<TMP_Text>();
            if (npcText2 != null) npcText2.text = "Let's meet up!";
            if (activeContact != null) activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = "Let's meet up!" });
            CheckEnding();
        }
        else
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            GameObject npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text npcText2 = npcMessage2.GetComponentInChildren<TMP_Text>();
            if (npcText2 != null) npcText2.text = "I don't think we're a match.";
            if (activeContact != null) activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = "I don't think we're a match." });
            CheckEnding();

        }
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        //yield return new WaitForSeconds(1f);
    }
    private IEnumerator MessageCooldown3()
    {
        yield return new WaitForSeconds(1f);
        GameObject npcMessage = Instantiate(NPCTextPrefab, chatContentParent);
        TMP_Text npcText = npcMessage.GetComponentInChildren<TMP_Text>();
        thunderAudioSource.PlayOneShot(messageAppear);
        if (npcText != null) npcText.text = "...";
        StartCoroutine(ScrollToBottomAnimated(0.18f));
        //yield return new WaitForSeconds(Random.Range(1f,4f));
        yield return new WaitForSeconds(1f);
        Destroy(npcMessage);
        //yield return new WaitForSeconds(Random.Range(0.2f,1.5f));
        yield return new WaitForSeconds(1f);

        currentResponse = randomIndex3;
        if (currentResponse >= 0 && currentResponse < poolMainText.Count)
        {
            poolPlayerResponses.RemoveAt(currentResponse);
            poolMainText.RemoveAt(currentResponse);
            poolNpcResponses.RemoveAt(currentResponse);
        }
        if (IsAnyStatAbove75Percent())
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            GameObject npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text npcText2 = npcMessage2.GetComponentInChildren<TMP_Text>();
            if (npcText2 != null) npcText2.text = "Let's meet up!";
            if (activeContact != null)
            {
                activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = "Let's meet up!" });
                activeContact.isMatchedSuccess = true;
                activeContact.firstPathActive = false;
            }
            StartCoroutine(MatchedAnimation());
        }
        else if (questionCount < 7)
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            GameObject npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text npcText2 = npcMessage2.GetComponentInChildren<TMP_Text>();
            if (npcText2 != null) npcText2.text = NPCResponse3;
            if (activeContact != null)
            {
                activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = NPCResponse3 });
            }
            yield return new WaitForSeconds(.1f);
            firstPath.SetActive(true);

            RandomizeChoice();
        }
        else if (questionCount >= 7 && happinessLevel >= 5 || questionCount >= 7 && spicinessLevel >= 5 || questionCount >= 7 && romancenessLevel >= 5)
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            GameObject npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text npcText2 = npcMessage2.GetComponentInChildren<TMP_Text>();
            if (npcText2 != null) npcText2.text = "Let's meet up!";
            if (activeContact != null) activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = "Let's meet up!" });
            CheckEnding();
        }
        else
        {
            thunderAudioSource.PlayOneShot(messageAppear);
            firstPath.SetActive(false);
            GameObject npcMessage2 = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text npcText2 = npcMessage2.GetComponentInChildren<TMP_Text>();
            if (npcText2 != null) npcText2.text = "I don't think we're a match.";
            if (activeContact != null) activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = "I don't think we're a match." });
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

    public void EnsureProfileImageReferences()
    {
        if (additionalBgImage == null || bodyImage == null || headImage == null || faceImage == null || hairImage == null)
        {
            Transform root = null;
            if (profilePicture != null)
            {
                if (profilePicture.transform.parent != null && profilePicture.transform.parent.name == "ProfilePicture")
                    root = profilePicture.transform.parent;
                else if (profilePicture.name == "ProfilePicture")
                    root = profilePicture.transform;
                else if (profilePicture.transform.parent != null)
                {
                    Transform pp = profilePicture.transform.parent.Find("ProfilePicture");
                    if (pp != null) root = pp;
                    else root = profilePicture.transform.parent;
                }
            }

            if (root == null)
            {
                GameObject ppObj = GameObject.Find("ProfilePicture");
                if (ppObj != null) root = ppObj.transform;
            }

            if (root != null)
            {
                if (additionalBgImage == null)
                {
                    Transform t = root.Find("AddBackground") ?? root.Find("AdditionalBg");
                    if (t != null) additionalBgImage = t.GetComponent<Image>();
                }
                if (bodyImage == null)
                {
                    Transform t = root.Find("Body");
                    if (t != null) bodyImage = t.GetComponent<Image>();
                }
                if (headImage == null)
                {
                    Transform t = root.Find("Head");
                    if (t != null) headImage = t.GetComponent<Image>();
                }
                if (faceImage == null)
                {
                    Transform t = root.Find("Face");
                    if (t != null) faceImage = t.GetComponent<Image>();
                }
                if (hairImage == null)
                {
                    Transform t = root.Find("Hair");
                    if (t != null) hairImage = t.GetComponent<Image>();
                }
            }
        }
    }

    public void RandomizeProfileParts()
    {
        EnsureProfileImageReferences();

        if (additionalBgSprites != null && additionalBgSprites.Length > 0 && additionalBgImage != null)
        {
            currentAdditionalBg = additionalBgSprites[Random.Range(0, additionalBgSprites.Length)];
            additionalBgImage.sprite = currentAdditionalBg;
        }

        if (bodySprites != null && bodySprites.Length > 0 && bodyImage != null)
        {
            currentBody = bodySprites[Random.Range(0, bodySprites.Length)];
            bodyImage.sprite = currentBody;
        }

        if (headSprites != null && headSprites.Length > 0 && headImage != null)
        {
            currentHead = headSprites[Random.Range(0, headSprites.Length)];
            headImage.sprite = currentHead;
        }

        if (faceSprites != null && faceSprites.Length > 0 && faceImage != null)
        {
            currentFace = faceSprites[Random.Range(0, faceSprites.Length)];
            faceImage.sprite = currentFace;
        }

        if (hairSprites != null && hairSprites.Length > 0 && hairImage != null)
        {
            currentHair = hairSprites[Random.Range(0, hairSprites.Length)];
            hairImage.sprite = currentHair;
        }

        if (headImage != null && headImage.sprite != null)
        {
            currentProfilePicture = headImage.sprite;
        }
        else if (profilePicture != null && profilePicture.sprite != null)
        {
            currentProfilePicture = profilePicture.sprite;
        }
    }

    private IEnumerator RandomizeProfile()
    {
        int randomPictureIndex = (profilePictures != null && profilePictures.Count > 0) ? Random.Range(0, profilePictures.Count) : 0;
        int randomNameIndex = names != null && names.Count > 0 ? Random.Range(0, names.Count) : 0;
        int randomAgeIndex = ages != null && ages.Count > 0 ? Random.Range(0, ages.Count) : 0;
        int randomGenderIndex = genders != null && genders.Count > 0 ? Random.Range(0, genders.Count) : 0;
        int randomDescriptionIndex = descriptions != null && descriptions.Count > 0 ? Random.Range(0, descriptions.Count) : 0;
        int randomInterest1Index = interest1 != null && interest1.Count > 0 ? Random.Range(0, interest1.Count) : 0;
        int randomInterest2Index = interest2 != null && interest2.Count > 0 ? Random.Range(0, interest2.Count) : 0;
        int randomInterest3Index = interest3 != null && interest3.Count > 0 ? Random.Range(0, interest3.Count) : 0;
        yield return new WaitForSeconds(.7f);
        if (names != null && names.Count > 0 && randomName != null) randomName.text = names[randomNameIndex];
        if (ages != null && ages.Count > 0 && randomAge != null) randomAge.text = ages[randomAgeIndex];
        if (genders != null && genders.Count > 0 && randomGender != null) randomGender.text = genders[randomGenderIndex];
        if (descriptions != null && descriptions.Count > 0 && randomDescription != null) randomDescription.text = descriptions[randomDescriptionIndex];
        if (interest1 != null && interest1.Count > 0 && randomInterest1 != null) randomInterest1.text = interest1[randomInterest1Index];
        if (interest2 != null && interest2.Count > 0 && randomInterest2 != null) randomInterest2.text = interest2[randomInterest2Index];
        if (interest3 != null && interest3.Count > 0 && randomInterest3 != null) randomInterest3.text = interest3[randomInterest3Index];
        if (profilePictures != null && profilePictures.Count > 0 && profilePicture != null)
        {
            profilePicture.sprite = profilePictures[randomPictureIndex];
        }

        RandomizeProfileParts();
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

        // Create or reset contact in database
        ThunderContact existing = contacts.Find(c => c.name == currentProfileName);
        if (existing != null)
        {
            activeContact = existing;
            existing.age = currentProfileAge;
            existing.profilePicture = currentProfilePicture;
            existing.additionalBgSprite = currentAdditionalBg;
            existing.bodySprite = currentBody;
            existing.headSprite = currentHead;
            existing.faceSprite = currentFace;
            existing.hairSprite = currentHair;
            existing.description = currentProfileDescription;
            existing.happinessLevel = 0;
            existing.spicinessLevel = 0;
            existing.romancenessLevel = 0;
            existing.questionCount = 0;
            existing.isAbandonedOrRejected = false;
            existing.isMatchedSuccess = false;
            existing.firstPathActive = true;
            existing.chatHistory.Clear();
            existing.currentChoice1 = responseDescription1 != null ? responseDescription1.text : "";
            existing.currentChoice2 = responseDescription2 != null ? responseDescription2.text : "";
            existing.currentChoice3 = responseDescription3 != null ? responseDescription3.text : "";
            existing.currentPlayerResp1 = playerResponse1;
            existing.currentPlayerResp2 = playerResponse2;
            existing.currentPlayerResp3 = playerResponse3;
            existing.currentNpcResp1 = NPCResponse1;
            existing.currentNpcResp2 = NPCResponse2;
            existing.currentNpcResp3 = NPCResponse3;
        }
        else
        {
            ThunderContact newContact = new ThunderContact
            {
                id = System.Guid.NewGuid().ToString(),
                name = currentProfileName,
                age = currentProfileAge,
                profilePicture = currentProfilePicture,
                additionalBgSprite = currentAdditionalBg,
                bodySprite = currentBody,
                headSprite = currentHead,
                faceSprite = currentFace,
                hairSprite = currentHair,
                description = currentProfileDescription,
                happinessLevel = 0,
                spicinessLevel = 0,
                romancenessLevel = 0,
                questionCount = 0,
                firstPathActive = true,
                currentChoice1 = responseDescription1 != null ? responseDescription1.text : "",
                currentChoice2 = responseDescription2 != null ? responseDescription2.text : "",
                currentChoice3 = responseDescription3 != null ? responseDescription3.text : "",
                currentPlayerResp1 = playerResponse1,
                currentPlayerResp2 = playerResponse2,
                currentPlayerResp3 = playerResponse3,
                currentNpcResp1 = NPCResponse1,
                currentNpcResp2 = NPCResponse2,
                currentNpcResp3 = NPCResponse3
            };
            contacts.Add(newContact);
            activeContact = newContact;
        }

        isDonatingMoney = false;
        if (sendMoneyButton != null) sendMoneyButton.interactable = true;
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
        if (activeContact != null)
        {
            activeContact.happinessLevel = happinessLevel;
            activeContact.spicinessLevel = spicinessLevel;
            activeContact.romancenessLevel = romancenessLevel;
            activeContact.questionCount = questionCount;
        }
        yield return new WaitForSeconds(selectDelay);
        

    }

    public bool IsAnyStatAbove75Percent()
    {
        float happyRatio = (happinessSlider != null && happinessSlider.maxValue > 0)
            ? (happinessSlider.value / happinessSlider.maxValue)
            : (happinessLevel / 10f);
        float spiceRatio = (spicinessSlider != null && spicinessSlider.maxValue > 0)
            ? (spicinessSlider.value / spicinessSlider.maxValue)
            : (spicinessLevel / 10f);
        float romanceRatio = (romancenessSlider != null && romancenessSlider.maxValue > 0)
            ? (romancenessSlider.value / romancenessSlider.maxValue)
            : (romancenessLevel / 10f);

        return happyRatio >= 0.75f || spiceRatio >= 0.75f || romanceRatio >= 0.75f;
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
        isDonatingMoney = false;
        if (sendMoneyButton != null) sendMoneyButton.interactable = true;
        if (activeContact != null)
        {
            activeContact.isAbandonedOrRejected = true;
            activeContact.firstPathActive = false;
        }
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
        isDonatingMoney = false;
        if (sendMoneyButton != null) sendMoneyButton.interactable = true;
        if (activeContact != null)
        {
            activeContact.isMatchedSuccess = true;
            activeContact.firstPathActive = false;
            contacts.Remove(activeContact);
            activeContact = null;
        }
        contacts.RemoveAll(c => c == null || c.isMatchedSuccess);
        currentProfileName = "";
        if (chatListWindow != null && chatListWindow.activeInHierarchy)
        {
            RefreshChatListUI();
        }
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


    // ==================== NEW CHAT & BUTTON FUNCTIONS ====================

    public void AbandonMatch()
    {
        OnAbandonButtonClicked();
    }

    public void OnAbandonButtonClicked()
    {
        isDonatingMoney = false;
        if (sendMoneyButton != null) sendMoneyButton.interactable = true;
        EnsureActiveContact();
        if (activeContact != null)
        {
            activeContact.isAbandonedOrRejected = true;
            activeContact.firstPathActive = false;
        }
        if (firstPath != null) firstPath.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(RejectionAnimation());
    }

    public void SendMoney()
    {
        OnSendMoneyButtonClicked();
    }

    public void OnSendMoneyButtonClicked()
    {
        if (isDonatingMoney) return;

        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null || gameManager.playerMoney < 10f)
        {
            if (thunderAudioSource != null && rejected != null)
                thunderAudioSource.PlayOneShot(rejected);
            return;
        }

        isDonatingMoney = true;
        if (sendMoneyButton != null) sendMoneyButton.interactable = false;

        // Deduct money
        gameManager.playerMoney -= 10f;
        gameManager.expensesAmount += 10f;
        PurchaseHistoryManager.RecordPurchase(10f, "Thunder donation");

        if (thunderAudioSource != null && sendMessage != null)
            thunderAudioSource.PlayOneShot(sendMessage);

        EnsureActiveContact();

        // Instantiate player message
        GameObject playerMessage = Instantiate(playerTextPrefab, chatContentParent);
        TMP_Text playerText = playerMessage.GetComponentInChildren<TMP_Text>();
        string donationMsg = "You donated $10!";
        if (playerText != null) playerText.text = donationMsg;
        if (activeContact != null)
        {
            activeContact.chatHistory.Add(new ChatMessageData { isPlayer = true, messageText = donationMsg });
        }

        // Randomly boost all 3 sliders
        int happyBoost = Random.Range(1, 3);
        int spiceBoost = Random.Range(1, 3);
        int romanceBoost = Random.Range(1, 3);

        happinessLevel += happyBoost;
        spicinessLevel += spiceBoost;
        romancenessLevel += romanceBoost;

        if (happinessSlider != null) happinessSlider.value = happinessLevel;
        if (spicinessSlider != null) spicinessSlider.value = spicinessLevel;
        if (romancenessSlider != null) romancenessSlider.value = romancenessLevel;

        if (activeContact != null)
        {
            activeContact.happinessLevel = happinessLevel;
            activeContact.spicinessLevel = spicinessLevel;
            activeContact.romancenessLevel = romancenessLevel;
        }

        StartCoroutine(ScrollToBottomAnimated(0.18f));

        // NPC response
        StartCoroutine(SendMoneyNPCResponseCoroutine());
    }

    private IEnumerator SendMoneyNPCResponseCoroutine()
    {
        yield return new WaitForSeconds(1f);
        GameObject npcTyping = Instantiate(NPCTextPrefab, chatContentParent);
        TMP_Text typingText = npcTyping.GetComponentInChildren<TMP_Text>();
        if (typingText != null) typingText.text = "...";
        if (thunderAudioSource != null && messageAppear != null)
            thunderAudioSource.PlayOneShot(messageAppear);
        StartCoroutine(ScrollToBottomAnimated(0.18f));

        yield return new WaitForSeconds(1f);
        Destroy(npcTyping);

        yield return new WaitForSeconds(0.4f);
        if (thunderAudioSource != null && messageAppear != null)
            thunderAudioSource.PlayOneShot(messageAppear);

        GameObject npcMsg = Instantiate(NPCTextPrefab, chatContentParent);
        TMP_Text npcText = npcMsg.GetComponentInChildren<TMP_Text>();

        string[] responses = new string[]
        {
            "Oh my, thank you!",
            "Oh my, thank you so much! <3",
            "Aww, thank you! You're so sweet!",
            "Thank you! You really didn't have to <3"
        };
        string npcResponse = responses[Random.Range(0, responses.Length)];
        if (npcText != null) npcText.text = npcResponse;

        if (activeContact != null)
        {
            activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = npcResponse });
        }

        StartCoroutine(ScrollToBottomAnimated(0.18f));

        isDonatingMoney = false;
        if (sendMoneyButton != null) sendMoneyButton.interactable = true;

        if (IsAnyStatAbove75Percent() && (activeContact == null || !activeContact.isMatchedSuccess))
        {
            yield return new WaitForSeconds(0.6f);
            if (firstPath != null) firstPath.SetActive(false);
            if (thunderAudioSource != null && messageAppear != null)
                thunderAudioSource.PlayOneShot(messageAppear);
            GameObject meetMsg = Instantiate(NPCTextPrefab, chatContentParent);
            TMP_Text meetText = meetMsg.GetComponentInChildren<TMP_Text>();
            if (meetText != null) meetText.text = "Let's meet up!";
            if (activeContact != null)
            {
                activeContact.chatHistory.Add(new ChatMessageData { isPlayer = false, messageText = "Let's meet up!" });
                activeContact.isMatchedSuccess = true;
                activeContact.firstPathActive = false;
            }
            StartCoroutine(ScrollToBottomAnimated(0.18f));
            StartCoroutine(MatchedAnimation());
        }
    }

    public void GoToMatches()
    {
        OnMatchesButtonClicked();
    }

    public void OpenMatches()
    {
        OnMatchesButtonClicked();
    }

    public void OnMatchesButtonClicked()
    {
        isDonatingMoney = false;
        if (sendMoneyButton != null) sendMoneyButton.interactable = true;
        SaveCurrentConversationState();
        if (chatWindow != null) chatWindow.SetActive(false);
        if (chatListWindow != null) chatListWindow.SetActive(false);
        if (swipeWindow != null) swipeWindow.SetActive(true);
    }

    public void GoToChats()
    {
        OnChatsButtonClicked();
    }

    public void OpenChats()
    {
        OnChatsButtonClicked();
    }

    public void OnChatsButtonClicked()
    {
        isDonatingMoney = false;
        if (sendMoneyButton != null) sendMoneyButton.interactable = true;
        SaveCurrentConversationState();
        if (chatWindow != null) chatWindow.SetActive(false);
        if (swipeWindow != null) swipeWindow.SetActive(false);
        if (chatListWindow != null)
        {
            chatListWindow.SetActive(true);
            RefreshChatListUI();
        }
    }

    public void BackToSwipeFromChatList()
    {
        if (chatListWindow != null) chatListWindow.SetActive(false);
        if (swipeWindow != null) swipeWindow.SetActive(true);
    }

    public void ResumeChat(ThunderContact contact)
    {
        isDonatingMoney = false;
        if (sendMoneyButton != null) sendMoneyButton.interactable = true;
        if (contact == null) return;

        SaveCurrentConversationState();
        activeContact = contact;

        // Restore chat headers
        if (chat_name != null) chat_name.text = contact.name;
        if (chat_profilePicture != null) chat_profilePicture.sprite = contact.profilePicture;

        // Restore sliders
        happinessLevel = contact.happinessLevel;
        spicinessLevel = contact.spicinessLevel;
        romancenessLevel = contact.romancenessLevel;
        if (happinessSlider != null) happinessSlider.value = happinessLevel;
        if (spicinessSlider != null) spicinessSlider.value = spicinessLevel;
        if (romancenessSlider != null) romancenessSlider.value = romancenessLevel;

        questionCount = contact.questionCount;

        // Rebuild chat messages
        if (chatContentParent != null)
        {
            for (int i = chatContentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(chatContentParent.GetChild(i).gameObject);
            }

            foreach (var msg in contact.chatHistory)
            {
                GameObject prefab = msg.isPlayer ? playerTextPrefab : NPCTextPrefab;
                if (prefab != null)
                {
                    GameObject go = Instantiate(prefab, chatContentParent);
                    TMP_Text txt = go.GetComponentInChildren<TMP_Text>();
                    if (txt != null) txt.text = msg.messageText;
                }
            }
        }

        // Restore choices / active path
        bool canChat = !contact.isAbandonedOrRejected && !contact.isMatchedSuccess;
        if (firstPath != null)
        {
            firstPath.SetActive(canChat && contact.firstPathActive);
        }

        if (canChat && contact.firstPathActive)
        {
            if (responseDescription1 != null) responseDescription1.text = contact.currentChoice1;
            if (responseDescription2 != null) responseDescription2.text = contact.currentChoice2;
            if (responseDescription3 != null) responseDescription3.text = contact.currentChoice3;
            playerResponse1 = contact.currentPlayerResp1;
            playerResponse2 = contact.currentPlayerResp2;
            playerResponse3 = contact.currentPlayerResp3;
            NPCResponse1 = contact.currentNpcResp1;
            NPCResponse2 = contact.currentNpcResp2;
            NPCResponse3 = contact.currentNpcResp3;
        }

        // Switch windows
        if (chatListWindow != null) chatListWindow.SetActive(false);
        if (swipeWindow != null) swipeWindow.SetActive(false);
        if (chatWindow != null) chatWindow.SetActive(true);

        StartCoroutine(ScrollToBottomAnimated(0.18f));
    }

    private void SaveCurrentConversationState()
    {
        if (activeContact == null) return;
        activeContact.happinessLevel = happinessLevel;
        activeContact.spicinessLevel = spicinessLevel;
        activeContact.romancenessLevel = romancenessLevel;
        activeContact.questionCount = questionCount;
        if (firstPath != null) activeContact.firstPathActive = firstPath.activeSelf;
        if (responseDescription1 != null) activeContact.currentChoice1 = responseDescription1.text;
        if (responseDescription2 != null) activeContact.currentChoice2 = responseDescription2.text;
        if (responseDescription3 != null) activeContact.currentChoice3 = responseDescription3.text;
        activeContact.currentPlayerResp1 = playerResponse1;
        activeContact.currentPlayerResp2 = playerResponse2;
        activeContact.currentPlayerResp3 = playerResponse3;
        activeContact.currentNpcResp1 = NPCResponse1;
        activeContact.currentNpcResp2 = NPCResponse2;
        activeContact.currentNpcResp3 = NPCResponse3;
    }

    private void EnsureActiveContact()
    {
        if (activeContact == null)
        {
            string pName = !string.IsNullOrEmpty(currentProfileName) ? currentProfileName : (chat_name != null ? chat_name.text : "Match");
            ThunderContact found = contacts.Find(c => c.name == pName);
            if (found != null)
            {
                activeContact = found;
            }
            else
            {
                activeContact = new ThunderContact
                {
                    id = System.Guid.NewGuid().ToString(),
                    name = pName,
                    age = !string.IsNullOrEmpty(currentProfileAge) ? currentProfileAge : "21",
                    profilePicture = currentProfilePicture != null ? currentProfilePicture : (chat_profilePicture != null ? chat_profilePicture.sprite : null),
                    additionalBgSprite = currentAdditionalBg,
                    bodySprite = currentBody,
                    headSprite = currentHead,
                    faceSprite = currentFace,
                    hairSprite = currentHair,
                    description = currentProfileDescription,
                    happinessLevel = happinessLevel,
                    spicinessLevel = spicinessLevel,
                    romancenessLevel = romancenessLevel,
                    questionCount = questionCount,
                    firstPathActive = firstPath != null && firstPath.activeSelf
                };
                contacts.Add(activeContact);
            }
        }
    }

    public void RefreshChatListUI()
    {
        if (contactListParent == null && chatListWindow != null)
        {
            // Auto-find Leftpanel or Content in chatListWindow
            foreach (Transform t in chatListWindow.GetComponentsInChildren<Transform>(true))
            {
                string tName = t.name.ToLower();
                if (tName.Contains("leftpanel") || tName.Contains("contactlist") || tName == "content")
                {
                    contactListParent = t;
                    break;
                }
            }
        }

        if (contactListParent == null) return;

        // Clean up previously spawned items
        for (int i = spawnedContactItems.Count - 1; i >= 0; i--)
        {
            if (spawnedContactItems[i] != null) Destroy(spawnedContactItems[i]);
        }
        spawnedContactItems.Clear();

        // If no contactItemPrefab, look for a prototype child in contactListParent
        if (contactItemPrefab == null && contactTemplate == null && contactListParent.childCount > 0)
        {
            contactTemplate = contactListParent.GetChild(0).gameObject;
            contactTemplate.SetActive(false);
        }

        GameObject prefabToUse = contactItemPrefab != null ? contactItemPrefab : contactTemplate;
        if (prefabToUse == null) return;

        contacts.RemoveAll(c => c == null || c.isMatchedSuccess);

        foreach (var contact in contacts)
        {
            if (contact.isMatchedSuccess) continue;
            GameObject itemObj = Instantiate(prefabToUse, contactListParent);
            itemObj.SetActive(true);
            BindContactItemUI(itemObj, contact);
            spawnedContactItems.Add(itemObj);
        }
    }

    private void BindContactItemUI(GameObject itemObj, ThunderContact contact)
    {
        if (itemObj == null || contact == null) return;

        // Ensure Bg has raycastTarget enabled
        Image bgImg = itemObj.transform.Find("Bg")?.GetComponent<Image>();
        if (bgImg != null)
        {
            bgImg.raycastTarget = true;
        }

        // Profile picture Image
        Image profileImg = null;
        Transform picTr = itemObj.transform.Find("ProfilePic") ?? itemObj.transform.Find("Image") ?? itemObj.transform.Find("Avatar");
        if (picTr != null) profileImg = picTr.GetComponent<Image>();
        if (profileImg == null)
        {
            Image[] images = itemObj.GetComponentsInChildren<Image>(true);
            foreach (var img in images)
            {
                if (img.gameObject != itemObj && img != bgImg)
                {
                    profileImg = img;
                    break;
                }
            }
        }
        if (profileImg != null)
        {
            profileImg.raycastTarget = false;
            if (contact.profilePicture != null)
                profileImg.sprite = contact.profilePicture;
        }

        // Texts for Name & Age
        TMP_Text nameText = null;
        TMP_Text ageText = null;
        Transform nameTr = itemObj.transform.Find("Con_name") ?? itemObj.transform.Find("Name") ?? itemObj.transform.Find("name");
        Transform ageTr = itemObj.transform.Find("Con_age") ?? itemObj.transform.Find("Age") ?? itemObj.transform.Find("age");
        if (nameTr != null) nameText = nameTr.GetComponent<TMP_Text>();
        if (ageTr != null) ageText = ageTr.GetComponent<TMP_Text>();

        TMP_Text[] allTexts = itemObj.GetComponentsInChildren<TMP_Text>(true);
        if (nameText == null && allTexts.Length > 0) nameText = allTexts[0];
        if (ageText == null && allTexts.Length > 1) ageText = allTexts[1];

        if (nameText != null)
        {
            nameText.raycastTarget = false;
            if (ageText != null)
            {
                ageText.raycastTarget = false;
                nameText.text = contact.name;
                ageText.text = string.IsNullOrEmpty(contact.age) ? "" : contact.age;
            }
            else
            {
                nameText.text = string.IsNullOrEmpty(contact.age)
                    ? contact.name
                    : $"{contact.name}, {contact.age}";
            }
        }

        // Button click
        Button btn = itemObj.GetComponent<Button>() ?? itemObj.GetComponentInChildren<Button>();
        if (btn == null)
        {
            btn = itemObj.AddComponent<Button>();
            if (bgImg != null) btn.targetGraphic = bgImg;
        }
        btn.interactable = true;
        ThunderContact targetContact = contact;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => ResumeChat(targetContact));
    }
}

[System.Serializable]
public class ChatMessageData
{
    public bool isPlayer;
    public string messageText;
}

[System.Serializable]
public class ThunderContact
{
    public string id;
    public string name;
    public string age;
    public Sprite profilePicture;
    public Sprite additionalBgSprite;
    public Sprite bodySprite;
    public Sprite headSprite;
    public Sprite faceSprite;
    public Sprite hairSprite;
    public string description;

    public int happinessLevel;
    public int spicinessLevel;
    public int romancenessLevel;
    public int questionCount;
    public bool isMatchedSuccess;
    public bool isAbandonedOrRejected;

    public List<ChatMessageData> chatHistory = new List<ChatMessageData>();

    public string currentChoice1;
    public string currentChoice2;
    public string currentChoice3;
    public string currentPlayerResp1;
    public string currentPlayerResp2;
    public string currentPlayerResp3;
    public string currentNpcResp1;
    public string currentNpcResp2;
    public string currentNpcResp3;
    public bool firstPathActive;
}
