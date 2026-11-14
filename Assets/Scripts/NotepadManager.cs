using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Manages the Notepad app in MTXSim.
/// Ensures the TMP_InputField behaves like a classic desktop notepad:
/// 1. Unlimited vertical text entry (lineLimit = 0).
/// 2. Smooth scrolling down when text goes offscreen (via caret tracking and mouse wheel).
/// 3. A vertical scrollbar on the right that appears when text overflows and allows click/drag scrolling.
/// </summary>
[DisallowMultipleComponent]
public class NotepadManager : MonoBehaviour
{
    [Header("Input Field")]
    [Tooltip("The TMP_InputField for typing notes. If null, automatically found on or under this GameObject.")]
    public TMP_InputField inputField;

    [Header("Scrollbar Settings")]
    [Tooltip("Vertical scrollbar for the notepad. Created automatically if not assigned.")]
    public Scrollbar scrollbar;

    [Tooltip("Whether the scrollbar automatically hides when text fits on screen and appears when it overflows.")]
    public bool autoHideScrollbar = true;

    [Tooltip("Width of the scrollbar in pixels.")]
    public float scrollbarWidth = 18f;

    [Tooltip("Mouse wheel scrolling sensitivity.")]
    public float scrollSensitivity = 3f;

    [Header("Styling")]
    public Color trackColor = new Color(0f, 0f, 0f, 0.15f);
    public Color handleColor = new Color(0.78f, 0.78f, 0.8f, 1f);
    public Color handleHighlightColor = new Color(0.92f, 0.92f, 0.94f, 1f);
    public Color handlePressedColor = new Color(0.6f, 0.6f, 0.62f, 1f);

    private RectTransform textViewportRect;
    private bool isConfigured = false;
    private bool lastOverflowState = false;

    private void Awake()
    {
        EnsureSetup();
    }

    private void OnEnable()
    {
        EnsureSetup();
        UpdateScrollbarState(force: true);
    }

    private void Start()
    {
        EnsureSetup();
        UpdateScrollbarState(force: true);
    }

    /// <summary>
    /// Configures the input field and scrollbar components.
    /// </summary>
    public void EnsureSetup()
    {
        if (inputField == null)
        {
            inputField = GetComponent<TMP_InputField>();
            if (inputField == null)
            {
                inputField = GetComponentInChildren<TMP_InputField>(true);
            }
        }

        if (inputField == null) return;

        // 1. Remove line limits and enable multiline newline
        inputField.lineLimit = 0;
        inputField.characterLimit = 0;
        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
        inputField.scrollSensitivity = scrollSensitivity;

        if (inputField.textViewport != null)
        {
            textViewportRect = inputField.textViewport;
        }

        // 2. Ensure Scrollbar exists
        EnsureScrollbar();

        // 3. Connect scrollbar to TMP_InputField
        if (scrollbar != null && inputField.verticalScrollbar != scrollbar)
        {
            inputField.verticalScrollbar = scrollbar;
        }

        // 4. Hook text change listener
        inputField.onValueChanged.RemoveListener(OnTextChanged);
        inputField.onValueChanged.AddListener(OnTextChanged);

        isConfigured = true;
    }

    private void EnsureScrollbar()
    {
        if (scrollbar != null) return;

        Transform existingSB = inputField.transform.Find("Scrollbar");
        if (existingSB != null)
        {
            scrollbar = existingSB.GetComponent<Scrollbar>();
            if (scrollbar != null)
            {
                ConfigureScrollbarComponent(scrollbar);
                return;
            }
        }

        // Create Scrollbar GameObject under InputField (TMP)
        GameObject sbGO = new GameObject("Scrollbar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Scrollbar));
        sbGO.transform.SetParent(inputField.transform, false);

        RectTransform sbRect = sbGO.GetComponent<RectTransform>();
        sbRect.anchorMin = new Vector2(1f, 0f);
        sbRect.anchorMax = new Vector2(1f, 1f);
        sbRect.pivot = new Vector2(1f, 0.5f);
        sbRect.anchoredPosition = new Vector2(-2f, 0f);
        sbRect.sizeDelta = new Vector2(scrollbarWidth, -6f);

        Image trackImg = sbGO.GetComponent<Image>();
        trackImg.color = trackColor;

        scrollbar = sbGO.GetComponent<Scrollbar>();
        ConfigureScrollbarComponent(scrollbar);

        // Sliding Area
        GameObject slidingArea = new GameObject("Sliding Area", typeof(RectTransform));
        slidingArea.transform.SetParent(sbGO.transform, false);
        RectTransform slidingAreaRect = slidingArea.GetComponent<RectTransform>();
        slidingAreaRect.anchorMin = Vector2.zero;
        slidingAreaRect.anchorMax = Vector2.one;
        slidingAreaRect.sizeDelta = new Vector2(0f, -8f);
        slidingAreaRect.anchoredPosition = Vector2.zero;

        // Handle
        GameObject handle = new GameObject("Handle", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        handle.transform.SetParent(slidingArea.transform, false);
        RectTransform handleRect = handle.GetComponent<RectTransform>();
        handleRect.anchorMin = new Vector2(0f, 0.8f);
        handleRect.anchorMax = Vector2.one;
        handleRect.sizeDelta = Vector2.zero;
        handleRect.anchoredPosition = Vector2.zero;

        Image handleImg = handle.GetComponent<Image>();
        handleImg.color = handleColor;

        scrollbar.handleRect = handleRect;
        scrollbar.targetGraphic = handleImg;

        // Add scroll wheel forwarder so scrolling over the scrollbar also scrolls text
        if (sbGO.GetComponent<ScrollEventForwarder>() == null)
        {
            var forwarder = sbGO.AddComponent<ScrollEventForwarder>();
            forwarder.targetInputField = inputField;
        }

        if (handle.GetComponent<ScrollEventForwarder>() == null)
        {
            var forwarder = handle.AddComponent<ScrollEventForwarder>();
            forwarder.targetInputField = inputField;
        }
    }

    private void ConfigureScrollbarComponent(Scrollbar sb)
    {
        if (sb == null) return;
        sb.direction = Scrollbar.Direction.TopToBottom;

        ColorBlock cb = sb.colors;
        cb.normalColor = handleColor;
        cb.highlightedColor = handleHighlightColor;
        cb.pressedColor = handlePressedColor;
        cb.selectedColor = handleHighlightColor;
        cb.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        cb.colorMultiplier = 1f;
        cb.fadeDuration = 0.1f;
        sb.colors = cb;
    }

    private void OnTextChanged(string newText)
    {
        UpdateScrollbarState(force: false);
    }

    private void LateUpdate()
    {
        if (!isConfigured || inputField == null) return;
        UpdateScrollbarState(force: false);
    }

    /// <summary>
    /// Checks if text overflows the viewport and updates scrollbar visibility and text padding.
    /// </summary>
    public void UpdateScrollbarState(bool force = false)
    {
        if (inputField == null || inputField.textComponent == null || textViewportRect == null) return;

        float textHeight = inputField.textComponent.preferredHeight;
        float viewportHeight = textViewportRect.rect.height;

        bool isOverflowing = textHeight > (viewportHeight + 2f);

        if (scrollbar != null)
        {
            if (autoHideScrollbar)
            {
                if (scrollbar.gameObject.activeSelf != isOverflowing || force)
                {
                    scrollbar.gameObject.SetActive(isOverflowing);

                    // Adjust right padding on the text viewport so text wraps neatly before the scrollbar
                    float rightPadding = isOverflowing ? -(scrollbarWidth + 6f) : -10f;
                    textViewportRect.offsetMax = new Vector2(rightPadding, textViewportRect.offsetMax.y);
                }
            }
            else
            {
                if (!scrollbar.gameObject.activeSelf)
                {
                    scrollbar.gameObject.SetActive(true);
                }
                float rightPadding = -(scrollbarWidth + 6f);
                if (Mathf.Abs(textViewportRect.offsetMax.x - rightPadding) > 0.5f)
                {
                    textViewportRect.offsetMax = new Vector2(rightPadding, textViewportRect.offsetMax.y);
                }
            }
        }

        lastOverflowState = isOverflowing;
    }
}

/// <summary>
/// Forwards IScrollHandler events (e.g. mouse wheel) to the target TMP_InputField
/// so scrolling over the scrollbar track or handle scrolls the text smoothly.
/// </summary>
public class ScrollEventForwarder : MonoBehaviour, IScrollHandler
{
    public TMP_InputField targetInputField;

    public void OnScroll(PointerEventData eventData)
    {
        if (targetInputField != null)
        {
            targetInputField.OnScroll(eventData);
        }
    }
}
