using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Added automatically (at runtime) by <see cref="TaskbarManager"/> to every app window.
/// Tracks the open / minimised / closed state of the window, creates the Minimise button
/// next to the Close button, and keeps the window's taskbar button in sync.
///
/// Windows are still opened/closed the same way as before (SetActive(true/false) from desktop
/// icons and Close buttons). Minimising also deactivates the window, but flags it as minimised
/// so its taskbar button stays alive and it can be restored.
/// </summary>
[DisallowMultipleComponent]
public class AppWindow : MonoBehaviour
{
    [Tooltip("Text shown on the taskbar button. Taken from DragLine/AppName if left empty.")]
    public string displayName;

    [Tooltip("Icon shown on the taskbar button. Taken from DragLine/Icon if left empty.")]
    public Sprite icon;

    public bool IsMinimised { get; private set; }
    public bool IsAnimating { get; set; }
    /// <summary>Position/scale the window returns to if an animation is interrupted.</summary>
    public Vector3 RestPosition { get; set; }
    public Vector3 RestScale { get; set; }
    public RectTransform Rect { get; private set; }
    public CanvasGroup Group { get; private set; }
    public TMP_FontAsset TitleFont { get; private set; }

    private TaskbarManager manager;
    private RectTransform closeRect;
    private RectTransform minimiseRect;
    private bool initialized;
    private bool layoutDirty;

    // When true the next OnDisable is a minimise (keep taskbar button), not a close.
    private bool minimiseInProgress;

    public void Initialize(TaskbarManager owner)
    {
        if (initialized) return;
        initialized = true;
        manager = owner;
        Rect = transform as RectTransform;

        Group = GetComponent<CanvasGroup>();
        if (Group == null) Group = gameObject.AddComponent<CanvasGroup>();

        ReadTitleBarInfo();
        CreateMinimiseButton();
        SetupButtonHoverEffects();

        // Window already open when the game starts
        if (gameObject.activeInHierarchy)
            OnEnable();
    }

    private void ReadTitleBarInfo()
    {
        Transform dragLine = null;
        foreach (Transform child in transform)
        {
            string n = child.name.ToLowerInvariant();
            if (dragLine == null && (n.Contains("dragline") || child.GetComponent<DragWindow>() != null))
                dragLine = child;
            if (closeRect == null && (n == "close" || n.Contains("closeapp")))
                closeRect = child as RectTransform;
        }

        if (dragLine != null)
        {
            TMP_Text title = dragLine.GetComponentInChildren<TMP_Text>(true);
            if (title != null)
            {
                if (string.IsNullOrWhiteSpace(displayName)) displayName = title.text;
                TitleFont = title.font;
            }

            if (icon == null)
            {
                Transform iconT = dragLine.Find("Icon");
                Image iconImg = iconT != null ? iconT.GetComponent<Image>() : null;
                if (iconImg != null) icon = iconImg.sprite;
            }
        }

        if (string.IsNullOrWhiteSpace(displayName))
            displayName = name.Replace("_window", "").Replace("_", " ");
    }

    private void CreateMinimiseButton()
    {
        if (closeRect == null || manager.minimiseSprite == null) return;

        Transform existing = closeRect.parent.Find("Minimise");
        GameObject go = existing != null ? existing.gameObject : new GameObject("Minimise", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        go.transform.SetParent(closeRect.parent, false);
        go.transform.SetSiblingIndex(closeRect.GetSiblingIndex() + 1);

        minimiseRect = go.GetComponent<RectTransform>();
        Image img = go.GetComponent<Image>() ?? go.AddComponent<Image>();
        img.sprite = manager.minimiseSprite;
        img.preserveAspect = true;

        Button btn = go.GetComponent<Button>() ?? go.AddComponent<Button>();
        btn.targetGraphic = img;
        btn.transition = Selectable.Transition.None;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(Minimise);

        PositionMinimiseButton();
    }

    private void PositionMinimiseButton()
    {
        if (minimiseRect == null || closeRect == null) return;
        minimiseRect.anchorMin = closeRect.anchorMin;
        minimiseRect.anchorMax = closeRect.anchorMax;
        minimiseRect.pivot = closeRect.pivot;
        minimiseRect.localScale = closeRect.localScale;
        minimiseRect.sizeDelta = closeRect.sizeDelta;
        float step = (closeRect.rect.width * closeRect.localScale.x) + manager.minimiseButtonSpacing;
        minimiseRect.anchoredPosition = closeRect.anchoredPosition - new Vector2(step, 0f);
    }


    private void SetupButtonHoverEffects()
    {
        float targetHoverBrightness = 0.75f;

        if (closeRect != null)
        {
            UIHoverEffect closeHover = closeRect.GetComponent<UIHoverEffect>();
            if (closeHover == null)
            {
                closeHover = closeRect.gameObject.AddComponent<UIHoverEffect>();
                closeHover.hoverBrightness = targetHoverBrightness;
            }
            closeHover.useExplicitHoverColor = false;
            closeHover.useExplicitPressedColor = false;
            targetHoverBrightness = closeHover.hoverBrightness;
        }

        if (minimiseRect != null)
        {
            UIHoverEffect minHover = minimiseRect.GetComponent<UIHoverEffect>();
            if (minHover == null)
            {
                minHover = minimiseRect.gameObject.AddComponent<UIHoverEffect>();
                minHover.hoverBrightness = targetHoverBrightness;
            }
            minHover.useExplicitHoverColor = false;
            minHover.useExplicitPressedColor = false;
        }
    }

    private void OnEnable()
    {
        if (!initialized) return;
        layoutDirty = true; // Close button may be moved by Win95WindowFrame this frame

        if (IsMinimised)
        {
            // Restored either from the taskbar or by re-opening the app from its desktop icon
            IsMinimised = false;
            manager.PlayRestoreAnimation(this);
        }
        else
        {
            manager.OnWindowOpened(this);
        }
        manager.Focus(this);
    }

    private void LateUpdate()
    {
        if (layoutDirty)
        {
            layoutDirty = false;
            PositionMinimiseButton();
        }
    }

    private void OnDisable()
    {
        if (!initialized || manager == null) return;

        if (minimiseInProgress)
        {
            minimiseInProgress = false;
            IsMinimised = true;
            manager.OnWindowMinimised(this);
        }
        else
        {
            // Closed (Close button, game logic, etc.)
            IsMinimised = false;
            manager.OnWindowClosed(this);
        }
    }

    /// <summary>Hooked to the Minimise button. Can also be called from UnityEvents.</summary>
    public void Minimise()
    {
        if (!gameObject.activeInHierarchy || IsAnimating) return;
        manager.PlayMinimiseAnimation(this);
    }

    /// <summary>Called by the manager at the end of the minimise animation.</summary>
    public void DeactivateAsMinimised()
    {
        minimiseInProgress = true;
        gameObject.SetActive(false);
        minimiseInProgress = false;
    }

    /// <summary>Re-opens a minimised window (taskbar click).</summary>
    public void Restore()
    {
        if (!IsMinimised) return;
        gameObject.SetActive(true); // OnEnable plays the restore animation
    }
}
