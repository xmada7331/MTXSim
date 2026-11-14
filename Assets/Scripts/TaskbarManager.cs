using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Windows 95 style taskbar:
///  - Adds an <see cref="AppWindow"/> (with a Minimise button) to every app window at startup.
///  - Creates a taskbar button for every opened window, removes it when the window is closed.
///  - Clicking a taskbar button restores / focuses / minimises its window, just like Windows.
///  - Clicking anywhere inside a window brings it to the front.
///  - Animates windows flying into / out of their taskbar button.
/// </summary>
[DefaultExecutionOrder(-50)]
public class TaskbarManager : MonoBehaviour
{
    public static TaskbarManager Instance { get; private set; }

    [Header("References (auto-found by name if empty)")]
    [Tooltip("Parent that holds all app windows (Window_parent).")]
    public RectTransform windowParent;
    [Tooltip("Optional: parent that holds the ad popups (Ads_parent). Only used if Include Ads is on.")]
    public RectTransform adsParent;
    [Tooltip("The taskbar at the bottom of the screen (Toolbar).")]
    public RectTransform toolbar;
    [Tooltip("The Start button on the taskbar. Taskbar buttons start right after it.")]
    public RectTransform startButton;

    [Header("Sprites & Fonts")]
    public Sprite minimiseSprite;
    [Tooltip("Font for the taskbar button labels. If empty, the window's title font is used.")]
    public TMP_FontAsset labelFont;

    [Header("Which windows get a taskbar button")]
    public bool includeAds = false;
    [Tooltip("Window names that should NOT get a Minimise button / taskbar entry.")]
    public List<string> excludedWindows = new List<string> { "Unavailable_window" };

    [Header("Minimise button")]
    [Tooltip("Gap (in window units) between the Minimise and Close buttons.")]
    public float minimiseButtonSpacing = 4f;

    [Header("Taskbar buttons layout (Toolbar local units)")]
    [Tooltip("Gap between the Start button and the first taskbar button.")]
    public float startButtonGap = 10f;
    [Tooltip("Right-most X (Toolbar local space) the taskbar buttons may reach. Keeps them away from the tray icons/clock.")]
    public float buttonsAreaRight = 600f;
    public float maxButtonWidth = 230f;
    public float buttonHeight = 36f;
    public float buttonSpacing = 4f;
    public float labelFontSize = 18f;
    public float bevelThickness = 2f;

    [Header("Taskbar button colours")]
    public Color buttonFace = new Color(0.26f, 0.50f, 0.70f, 1f);
    public Color buttonFacePressed = new Color(0.13f, 0.29f, 0.44f, 1f);
    public Color bevelLight = new Color(1f, 1f, 1f, 0.9f);
    public Color bevelDark = new Color(0.05f, 0.05f, 0.05f, 0.95f);
    public Color labelColor = Color.white;

    [Header("Animation")]
    public float minimiseDuration = 0.22f;
    public float restoreDuration = 0.22f;
    [Tooltip("Scale (relative to the window's normal scale) the window shrinks to when it reaches the taskbar.")]
    public float minimisedScale = 0.08f;

    private readonly List<AppWindow> windows = new List<AppWindow>();
    private readonly List<AppWindow> openOrder = new List<AppWindow>();
    private readonly Dictionary<AppWindow, TaskbarButton> buttons = new Dictionary<AppWindow, TaskbarButton>();
    private readonly Dictionary<AppWindow, Coroutine> running = new Dictionary<AppWindow, Coroutine>();
    private readonly List<RaycastResult> raycastResults = new List<RaycastResult>();
    private RectTransform buttonContainer;
    private AppWindow focused;

    private void Awake()
    {
        Instance = this;
        ResolveReferences();
        CreateButtonContainer();
        RegisterWindows();
        SetupWidgetHoverEffects();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void SetupWidgetHoverEffects()
    {
        // 1. App banner widgets (Bank, FreeCoin, Stocks)
        if (windowParent != null)
        {
            Transform bankWidget = windowParent.Find("Bank_window/Main_window/Widget_bt");
            if (bankWidget != null)
            {
                UIHoverEffect h = bankWidget.GetComponent<UIHoverEffect>() ?? bankWidget.gameObject.AddComponent<UIHoverEffect>();
                h.useExplicitHoverColor = true;
                h.hoverColor = new Color(0.88f, 0.94f, 1f, 1f);
                h.hoverAlphaOverride = 0.98f;
                h.useExplicitPressedColor = true;
                h.pressedColor = new Color(0.75f, 0.85f, 0.95f, 1f);
            }

            Transform freecoinWidget = windowParent.Find("FreeCoin_window/Main_window/Widget");
            if (freecoinWidget != null)
            {
                UIHoverEffect h = freecoinWidget.GetComponent<UIHoverEffect>() ?? freecoinWidget.gameObject.AddComponent<UIHoverEffect>();
                h.useExplicitHoverColor = true;
                h.hoverColor = new Color(0.88f, 0.94f, 1f, 1f);
                h.useExplicitPressedColor = true;
                h.pressedColor = new Color(0.75f, 0.85f, 0.95f, 1f);
            }

            Transform stocksWidget = windowParent.Find("Stocks_window/Main_window/BuyWidget");
            if (stocksWidget != null)
            {
                UIHoverEffect h = stocksWidget.GetComponent<UIHoverEffect>() ?? stocksWidget.gameObject.AddComponent<UIHoverEffect>();
                h.useExplicitHoverColor = true;
                h.hoverColor = new Color(0.88f, 0.94f, 1f, 1f);
                h.useExplicitPressedColor = true;
                h.pressedColor = new Color(0.75f, 0.85f, 0.95f, 1f);
            }
        }

        // 2. Desktop widgets (Banking_widget, Stock_widget, FreeCoinWidget)
        RectTransform widgetsParent = FindRect("Widgets_parent");
        if (widgetsParent != null)
        {
            string[] widgetMinPaths = new string[]
            {
                "Banking_widget/WidgetMin/Image (2)",
                "Banking_widget/MainBankWidget/Image",
                "Stock_widget/WidgetMin/Image (2)",
                "Stock_widget/MainStockWidget/Image",
                "FreeCoinWidget/WidgetMin/Image (2)",
                "FreeCoinWidget/MainFreeCoinWidget/Image",
                "FreeCoinWidget/MainFreeCoinWidget/Flip_bt (1)",
                "FreeCoinWidget/MainFreeCoinWidget/Stake_up_bt",
                "FreeCoinWidget/MainFreeCoinWidget/Stake_down_bt"
            };

            foreach (string p in widgetMinPaths)
            {
                Transform t = widgetsParent.Find(p);
                if (t != null)
                {
                    UIHoverEffect h = t.GetComponent<UIHoverEffect>() ?? t.gameObject.AddComponent<UIHoverEffect>();
                    h.useExplicitHoverColor = true;
                    h.hoverColor = new Color(0.88f, 0.94f, 1f, 1f);
                    h.useExplicitPressedColor = true;
                    h.pressedColor = new Color(0.75f, 0.85f, 0.95f, 1f);
                }
            }
        }
    }

    // ------------------------------------------------------------------ setup

    private void ResolveReferences()
    {
        if (windowParent == null) windowParent = FindRect("Window_parent");
        if (adsParent == null) adsParent = FindRect("Ads_parent");
        if (toolbar == null) toolbar = FindRect("Toolbar");
        if (startButton == null && toolbar != null) startButton = toolbar.Find("WindowsButton") as RectTransform;
    }

    private static RectTransform FindRect(string objectName)
    {
        foreach (RectTransform rt in Resources.FindObjectsOfTypeAll<RectTransform>())
        {
            if (rt.name == objectName && rt.gameObject.scene.IsValid())
                return rt;
        }
        return null;
    }

    private void CreateButtonContainer()
    {
        if (toolbar == null)
        {
            Debug.LogWarning("[TaskbarManager] Toolbar not found - taskbar buttons are disabled.");
            return;
        }

        var go = new GameObject("TaskbarButtons", typeof(RectTransform));
        buttonContainer = go.GetComponent<RectTransform>();
        buttonContainer.SetParent(toolbar, false);
        buttonContainer.anchorMin = buttonContainer.anchorMax = new Vector2(0.5f, 0.5f);
        buttonContainer.pivot = new Vector2(0.5f, 0.5f);
        buttonContainer.anchoredPosition = Vector2.zero;
        buttonContainer.sizeDelta = Vector2.zero;
        // Keep it behind pop-up menus (Start menu, notifications...) that live on the toolbar
        buttonContainer.SetAsFirstSibling();
    }

    private void RegisterWindows()
    {
        var found = new HashSet<Transform>();
        DragWindow[] dragHandles = FindObjectsByType<DragWindow>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (DragWindow handle in dragHandles)
        {
            Transform window = handle.transform.parent;
            if (window == null || found.Contains(window)) continue;

            bool isApp = windowParent != null && window.parent == windowParent;
            bool isAd = includeAds && adsParent != null && window.parent == adsParent;
            if (!isApp && !isAd) continue;
            if (excludedWindows.Contains(window.name)) continue;

            found.Add(window);
            AppWindow app = window.GetComponent<AppWindow>();
            if (app == null) app = window.gameObject.AddComponent<AppWindow>();
            windows.Add(app);
        }

        foreach (AppWindow app in windows)
            app.Initialize(this);
    }

    // ------------------------------------------------------------------ window events (called by AppWindow)

    public void OnWindowOpened(AppWindow window)
    {
        if (!openOrder.Contains(window)) openOrder.Add(window);
        GetOrCreateButton(window);
        RefreshButtons();
    }

    public void OnWindowMinimised(AppWindow window)
    {
        if (focused == window) focused = null;
        RefreshButtons();
    }

    public void OnWindowClosed(AppWindow window)
    {
        StopAnimation(window, true);
        openOrder.Remove(window);
        if (buttons.TryGetValue(window, out TaskbarButton btn))
        {
            buttons.Remove(window);
            if (btn != null) Destroy(btn.gameObject);
        }
        if (focused == window) focused = null;
        if (this != null && isActiveAndEnabled) RefreshButtons();
    }

    // ------------------------------------------------------------------ focus

    public void Focus(AppWindow window)
    {
        if (window == null || window.IsMinimised || !window.gameObject.activeInHierarchy) return;
        window.transform.SetAsLastSibling();
        focused = window;
        RefreshButtonStates();
    }

    /// <summary>Taskbar button behaviour, same as Windows.</summary>
    public void OnTaskbarButtonClicked(AppWindow window)
    {
        if (window == null || window.IsAnimating) return;

        if (window.IsMinimised)
            window.Restore();                 // minimised -> restore + focus
        else if (focused == window)
            window.Minimise();                // already in front -> minimise
        else
            Focus(window);                    // behind other windows -> bring to front
    }

    private void Update()
    {
        // Clicking inside any window brings it to the front (clicking the desktop un-focuses).
        if (!Input.GetMouseButtonDown(0) || EventSystem.current == null) return;

        var data = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        raycastResults.Clear();
        EventSystem.current.RaycastAll(data, raycastResults);
        if (raycastResults.Count == 0) return;

        GameObject top = raycastResults[0].gameObject;
        AppWindow hitWindow = top.GetComponentInParent<AppWindow>();
        if (hitWindow != null)
        {
            if (hitWindow != focused) Focus(hitWindow);
        }
        else if (focused != null && (toolbar == null || !top.transform.IsChildOf(toolbar)))
        {
            // Clicked the desktop: no active window (like Win95).
            // Clicks on the taskbar itself keep the focus so a taskbar button can minimise its window.
            focused = null;
            RefreshButtonStates();
        }
    }

    // ------------------------------------------------------------------ taskbar buttons

    private TaskbarButton GetOrCreateButton(AppWindow window)
    {
        if (buttonContainer == null) return null;
        if (buttons.TryGetValue(window, out TaskbarButton existing) && existing != null) return existing;

        TaskbarButton btn = TaskbarButton.Create(this, window, buttonContainer);
        buttons[window] = btn;
        return btn;
    }

    private void RefreshButtons()
    {
        if (buttonContainer == null) return;

        float left = -toolbar.rect.width * 0.5f + 10f;
        if (startButton != null)
        {
            // Right edge of the Start button in toolbar local space
            Vector3[] corners = new Vector3[4];
            startButton.GetWorldCorners(corners);
            left = toolbar.InverseTransformPoint(corners[2]).x;
        }
        left += startButtonGap;

        int count = 0;
        foreach (AppWindow w in openOrder) if (buttons.ContainsKey(w)) count++;
        if (count == 0) return;

        float available = buttonsAreaRight - left;
        float width = Mathf.Min(maxButtonWidth, (available - buttonSpacing * (count - 1)) / count);
        width = Mathf.Max(width, buttonHeight);

        int i = 0;
        foreach (AppWindow w in openOrder)
        {
            if (!buttons.TryGetValue(w, out TaskbarButton btn) || btn == null) continue;
            float x = left + i * (width + buttonSpacing) + width * 0.5f;
            btn.SetLayout(new Vector2(x, 0f), new Vector2(width, buttonHeight));
            i++;
        }
        RefreshButtonStates();
    }

    private void RefreshButtonStates()
    {
        foreach (var pair in buttons)
        {
            if (pair.Value != null)
                pair.Value.SetPressed(pair.Key == focused && !pair.Key.IsMinimised);
        }
    }

    // ------------------------------------------------------------------ animations

    public void PlayMinimiseAnimation(AppWindow window)
    {
        StopAnimation(window, false);
        running[window] = StartCoroutine(MinimiseRoutine(window));
    }

    public void PlayRestoreAnimation(AppWindow window)
    {
        StopAnimation(window, false);
        running[window] = StartCoroutine(RestoreRoutine(window));
    }

    private void StopAnimation(AppWindow window, bool resetVisuals)
    {
        if (running.TryGetValue(window, out Coroutine c) && c != null && this != null)
            StopCoroutine(c);
        running.Remove(window);
        if (resetVisuals && window.IsAnimating)
        {
            // Interrupted (e.g. closed by game logic mid-animation): snap back to the resting state
            window.IsAnimating = false;
            window.transform.position = window.RestPosition;
            window.transform.localScale = window.RestScale;
            if (window.Group != null) { window.Group.alpha = 1f; window.Group.interactable = true; window.Group.blocksRaycasts = true; }
        }
    }

    private Vector3 TaskbarTargetWorld(AppWindow window)
    {
        if (buttons.TryGetValue(window, out TaskbarButton btn) && btn != null)
            return btn.transform.position;
        if (toolbar != null) return toolbar.position;
        return window.transform.position + Vector3.down * 500f;
    }

    private IEnumerator MinimiseRoutine(AppWindow window)
    {
        window.IsAnimating = true;
        Transform t = window.transform;
        Vector3 startPos = t.position;
        Vector3 startScale = t.localScale;
        window.RestPosition = startPos;
        window.RestScale = startScale;
        Vector3 endPos = TaskbarTargetWorld(window);
        Vector3 endScale = startScale * minimisedScale;
        CanvasGroup g = window.Group;
        g.interactable = false;
        g.blocksRaycasts = false;

        if (focused == window) focused = null;
        RefreshButtonStates();

        float time = 0f;
        while (time < minimiseDuration)
        {
            time += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(time / minimiseDuration);
            float e = k * k * k;                      // ease-in: accelerates into the taskbar
            t.position = Vector3.LerpUnclamped(startPos, endPos, e);
            t.localScale = Vector3.LerpUnclamped(startScale, endScale, e);
            g.alpha = 1f - e * 0.85f;
            yield return null;
        }

        // Put everything back so the window re-opens exactly where it was
        t.position = startPos;
        t.localScale = startScale;
        g.alpha = 1f;
        g.interactable = true;
        g.blocksRaycasts = true;
        window.IsAnimating = false;
        running.Remove(window);
        window.DeactivateAsMinimised();
    }

    private IEnumerator RestoreRoutine(AppWindow window)
    {
        window.IsAnimating = true;
        Transform t = window.transform;
        Vector3 endPos = t.position;
        Vector3 endScale = t.localScale;
        window.RestPosition = endPos;
        window.RestScale = endScale;
        Vector3 startPos = TaskbarTargetWorld(window);
        Vector3 startScale = endScale * minimisedScale;
        CanvasGroup g = window.Group;
        g.interactable = false;
        g.blocksRaycasts = false;

        t.position = startPos;
        t.localScale = startScale;

        float time = 0f;
        while (time < restoreDuration)
        {
            time += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(time / restoreDuration);
            float e = 1f - Mathf.Pow(1f - k, 3f);     // ease-out: pops out of the taskbar
            t.position = Vector3.LerpUnclamped(startPos, endPos, e);
            t.localScale = Vector3.LerpUnclamped(startScale, endScale, e);
            g.alpha = 0.15f + e * 0.85f;
            yield return null;
        }

        t.position = endPos;
        t.localScale = endScale;
        g.alpha = 1f;
        g.interactable = true;
        g.blocksRaycasts = true;
        window.IsAnimating = false;
        running.Remove(window);
        RefreshButtonStates();
    }

#if UNITY_EDITOR
    private void Reset() => AutoAssignEditorDefaults();
    private void OnValidate() => AutoAssignEditorDefaults();

    private void AutoAssignEditorDefaults()
    {
        if (minimiseSprite == null)
            minimiseSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Minimise_icon.png");
        if (labelFont == null)
            labelFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Fonts/W95FA SDF.asset");
    }
#endif
}
