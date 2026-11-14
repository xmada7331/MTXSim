using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Monitors and displays purchase history in the BunkBank app under Account_window / AnalysisImg / PurchaseHistoryPanel.
/// Ensures the panel is scrollable with a dynamic scrollbar that automatically appears when records overflow.
/// Displays newest purchases at the top.
/// </summary>
public class PurchaseHistoryManager : MonoBehaviour
{
    public static PurchaseHistoryManager Instance { get; private set; }

    [System.Serializable]
    public class PurchaseRecord
    {
        public float amount;
        public string description;
    }

    private static readonly List<PurchaseRecord> records = new List<PurchaseRecord>();

    [Header("Prefab")]
    [Tooltip("The PurchaseHistoryText prefab to instantiate for each purchase.")]
    public GameObject purchaseHistoryTextPrefab;

    [Header("UI References")]
    [Tooltip("The Content container with VerticalLayoutGroup that holds the instantiated entries.")]
    public RectTransform contentContainer;
    public ScrollRect scrollRect;
    public RectMask2D rectMask;
    public Scrollbar scrollbar;

    [Header("Settings")]
    public float scrollSensitivity = 25f;
    public float scrollbarWidth = 10f;

    private void Awake()
    {
        Instance = this;
        EnsureSetup();
        PopulateAllRecords();
    }

    private void OnEnable()
    {
        Instance = this;
        EnsureSetup();
        PopulateAllRecords();
    }

    /// <summary>
    /// Records a purchase from anywhere in the game.
    /// If the bank window is open, it immediately creates the UI entry at the top;
    /// otherwise, it persists in history and is displayed when the bank window is opened.
    /// </summary>
    public static void RecordPurchase(float amount, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            description = "Item";

        var record = new PurchaseRecord { amount = amount, description = description };
        records.Add(record);

        if (Instance != null && Instance.isActiveAndEnabled)
        {
            Instance.AddEntryUI(record, atTop: true);
            Instance.UpdateScrollbarVisibility();
        }
    }

    /// <summary>
    /// Ensures PurchaseHistoryPanel has a ScrollRect, RectMask2D, a Content child with VerticalLayoutGroup and ContentSizeFitter,
    /// and a vertical Scrollbar on the right side.
    /// </summary>
    public void EnsureSetup()
    {
        // 1. Resolve prefab if not assigned
        if (purchaseHistoryTextPrefab == null)
        {
#if UNITY_EDITOR
            purchaseHistoryTextPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PurchaseHistoryText.prefab");
#endif
        }

        // 2. Ensure RectMask2D exists on this panel so records never overflow
        if (rectMask == null)
        {
            rectMask = GetComponent<RectMask2D>();
            if (rectMask == null && GetComponent<Mask>() == null)
            {
                rectMask = gameObject.AddComponent<RectMask2D>();
            }
        }

        // 3. Ensure child Content exists
        if (contentContainer == null)
        {
            Transform existingContent = transform.Find("Content");
            if (existingContent != null)
            {
                contentContainer = existingContent as RectTransform;
            }
            else
            {
                var contentGO = new GameObject("Content", typeof(RectTransform));
                contentContainer = contentGO.GetComponent<RectTransform>();
                contentContainer.SetParent(transform, false);

                // Top-stretch layout inside the panel
                contentContainer.anchorMin = new Vector2(0f, 1f);
                contentContainer.anchorMax = new Vector2(1f, 1f);
                contentContainer.pivot = new Vector2(0.5f, 1f);
                contentContainer.anchoredPosition = Vector2.zero;
                contentContainer.sizeDelta = Vector2.zero;
            }
        }

        // 4. Configure VerticalLayoutGroup on Content
        VerticalLayoutGroup vlg = contentContainer.GetComponent<VerticalLayoutGroup>();
        if (vlg == null)
        {
            vlg = contentContainer.gameObject.AddComponent<VerticalLayoutGroup>();
        }

        // Check if this panel itself has a VerticalLayoutGroup to inherit settings from
        VerticalLayoutGroup panelVlg = GetComponent<VerticalLayoutGroup>();
        if (panelVlg != null && panelVlg != vlg)
        {
            vlg.padding = new RectOffset(panelVlg.padding.left, 14, panelVlg.padding.top, panelVlg.padding.bottom);
            vlg.spacing = panelVlg.spacing;
            vlg.childAlignment = panelVlg.childAlignment;
            vlg.childControlWidth = true;
            vlg.childControlHeight = false;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;

            Destroy(panelVlg);
        }
        else
        {
            vlg.padding = new RectOffset(5, 14, 5, 5);
            vlg.spacing = 15f;
            vlg.childAlignment = TextAnchor.UpperLeft;
            vlg.childControlWidth = true;
            vlg.childControlHeight = false;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
        }

        // 5. Configure ContentSizeFitter on Content
        ContentSizeFitter csf = contentContainer.GetComponent<ContentSizeFitter>();
        if (csf == null)
        {
            csf = contentContainer.gameObject.AddComponent<ContentSizeFitter>();
        }
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        // 6. Ensure Scrollbar exists
        EnsureScrollbar();

        // 7. Ensure ScrollRect exists on this panel
        if (scrollRect == null)
        {
            scrollRect = GetComponent<ScrollRect>();
            if (scrollRect == null)
            {
                scrollRect = gameObject.AddComponent<ScrollRect>();
            }
        }

        scrollRect.content = contentContainer;
        scrollRect.viewport = transform as RectTransform;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
        scrollRect.scrollSensitivity = scrollSensitivity;

        if (scrollbar != null)
        {
            scrollRect.verticalScrollbar = scrollbar;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
            scrollRect.verticalScrollbarSpacing = 2f;
        }
    }

    /// <summary>
    /// Creates or finds the vertical scrollbar on the right side of PurchaseHistoryPanel.
    /// </summary>
    private void EnsureScrollbar()
    {
        if (scrollbar != null) return;

        Transform existingSB = transform.Find("Scrollbar");
        if (existingSB != null)
        {
            scrollbar = existingSB.GetComponent<Scrollbar>();
            if (scrollbar != null) return;
        }

        // Root Scrollbar GameObject
        GameObject sbGO = new GameObject("Scrollbar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Scrollbar));
        sbGO.transform.SetParent(transform, false);

        RectTransform sbRect = sbGO.GetComponent<RectTransform>();
        sbRect.anchorMin = new Vector2(1f, 0f);
        sbRect.anchorMax = new Vector2(1f, 1f);
        sbRect.pivot = new Vector2(1f, 0.5f);
        sbRect.anchoredPosition = new Vector2(-2f, 0f);
        sbRect.sizeDelta = new Vector2(scrollbarWidth, -10f);

        Image trackImg = sbGO.GetComponent<Image>();
        trackImg.color = new Color(0f, 0f, 0f, 0.22f); // subtle semi-transparent track

        scrollbar = sbGO.GetComponent<Scrollbar>();
        scrollbar.direction = Scrollbar.Direction.BottomToTop;

        // Sliding Area
        GameObject slidingArea = new GameObject("Sliding Area", typeof(RectTransform));
        slidingArea.transform.SetParent(sbGO.transform, false);
        RectTransform slidingAreaRect = slidingArea.GetComponent<RectTransform>();
        slidingAreaRect.anchorMin = Vector2.zero;
        slidingAreaRect.anchorMax = Vector2.one;
        slidingAreaRect.sizeDelta = Vector2.zero;
        slidingAreaRect.anchoredPosition = Vector2.zero;

        // Handle
        GameObject handle = new GameObject("Handle", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        handle.transform.SetParent(slidingArea.transform, false);
        RectTransform handleRect = handle.GetComponent<RectTransform>();
        handleRect.anchorMin = Vector2.zero;
        handleRect.anchorMax = Vector2.one;
        handleRect.sizeDelta = Vector2.zero;
        handleRect.anchoredPosition = Vector2.zero;

        Image handleImg = handle.GetComponent<Image>();
        handleImg.color = new Color(0.85f, 0.85f, 0.85f, 0.85f);

        scrollbar.handleRect = handleRect;
        scrollbar.targetGraphic = handleImg;

        ColorBlock cb = scrollbar.colors;
        cb.normalColor = new Color(0.85f, 0.85f, 0.85f, 0.85f);
        cb.highlightedColor = Color.white;
        cb.pressedColor = new Color(0.65f, 0.65f, 0.65f, 1f);
        cb.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        scrollbar.colors = cb;

        // Start hidden until items overflow
        sbGO.SetActive(false);
    }

    /// <summary>
    /// Populates all recorded purchases with the newest at the top (sibling 0).
    /// </summary>
    public void PopulateAllRecords()
    {
        if (contentContainer == null || purchaseHistoryTextPrefab == null)
            return;

        // Clear existing spawned entries to rebuild clean list with newest at top
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        // Iterate backwards from newest (last) to oldest (first)
        for (int i = records.Count - 1; i >= 0; i--)
        {
            AddEntryUI(records[i], atTop: false);
        }

        // Position scroll at top to see newest purchases
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }

        UpdateScrollbarVisibility();
    }

    /// <summary>
    /// Instantiates and sets up a single PurchaseHistoryText entry in the content container.
    /// If atTop is true, sets it as the first sibling so it appears at the very top.
    /// Format: "$[x] - [description]"
    /// </summary>
    private void AddEntryUI(PurchaseRecord record, bool atTop)
    {
        if (contentContainer == null || purchaseHistoryTextPrefab == null)
            return;

        GameObject entry = Instantiate(purchaseHistoryTextPrefab, contentContainer);
        entry.name = $"PurchaseEntry_{record.description}";
        entry.transform.localScale = Vector3.one;

        if (atTop)
        {
            entry.transform.SetAsFirstSibling();
        }

        string formattedAmount = (Mathf.Approximately(record.amount, Mathf.Round(record.amount)))
            ? ((int)Mathf.Round(record.amount)).ToString()
            : record.amount.ToString("F2");

        string fullText = $"${formattedAmount} - {record.description}";

        TMP_Text textComp = entry.GetComponent<TMP_Text>();
        if (textComp != null)
        {
            textComp.text = fullText;
            textComp.raycastTarget = false;
            textComp.textWrappingMode = TextWrappingModes.NoWrap;
            textComp.overflowMode = TextOverflowModes.Ellipsis;
        }

        if (atTop && scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    /// <summary>
    /// Checks whether the content height exceeds the panel height and toggles the scrollbar visibility.
    /// </summary>
    public void UpdateScrollbarVisibility()
    {
        if (contentContainer == null) return;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentContainer);

        if (scrollbar != null)
        {
            RectTransform viewport = scrollRect != null && scrollRect.viewport != null
                ? scrollRect.viewport
                : (transform as RectTransform);

            bool isOverflowing = contentContainer.rect.height > viewport.rect.height;
            scrollbar.gameObject.SetActive(isOverflowing);
        }
    }

    private void LateUpdate()
    {
        // Periodically verify scrollbar visibility in case layout changes dynamically
        if (scrollbar != null && contentContainer != null && scrollRect != null)
        {
            RectTransform viewport = scrollRect.viewport != null ? scrollRect.viewport : (transform as RectTransform);
            bool isOverflowing = contentContainer.rect.height > viewport.rect.height;
            if (scrollbar.gameObject.activeSelf != isOverflowing)
            {
                scrollbar.gameObject.SetActive(isOverflowing);
            }
        }
    }

    public static IReadOnlyList<PurchaseRecord> GetHistory() => records;

    public static void ClearHistory()
    {
        records.Clear();
        if (Instance != null && Instance.contentContainer != null)
        {
            foreach (Transform child in Instance.contentContainer)
            {
                Destroy(child.gameObject);
            }
            Instance.UpdateScrollbarVisibility();
        }
    }
}
