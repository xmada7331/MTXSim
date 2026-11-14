using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class Win95WindowFrame : MonoBehaviour
{
    [Header("Border & Bevel Settings")]
    [Tooltip("Thickness in pixels for the 3D beveled borders.")]
    public float borderThickness = 5f;

    [Tooltip("Margin between the window outer frame and the title bar (DragLine).")]
    public float titleBarMargin = 6f;

    [Tooltip("Color of the top and left highlight borders (light source from top-left).")]
    public Color highlightColor = new Color(1f, 1f, 1f, 0.95f);

    [Tooltip("Color of the bottom and right shadow borders.")]
    public Color shadowColor = new Color(0.08f, 0.08f, 0.08f, 0.95f);

    [Header("Options")]
    [Tooltip("Remove or disable the blurry UnityEngine.UI.Shadow components on background/DragLine to keep the clean, crisp Windows 95 aesthetic.")]
    public bool removeBlurryShadows = true;

    [Tooltip("Automatically apply style on Awake/Start.")]
    public bool autoApplyOnStart = true;

    private void Awake()
    {
        if (autoApplyOnStart && Application.isPlaying)
        {
            ApplyWin95Style();
        }
    }

    private void OnEnable()
    {
        if (autoApplyOnStart)
        {
            ApplyWin95Style();
        }
    }

    [ContextMenu("Apply Win95 Style")]
    public void ApplyWin95Style()
    {
        RectTransform windowRect = GetComponent<RectTransform>();
        if (windowRect == null) return;

        // 1. Locate DragLine (title bar) and Close button
        Transform dragLineTransform = null;
        Transform closeTransform = null;
        Transform backgroundTransform = null;

        // Search among immediate children
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            string childName = child.name.ToLower();

            if (childName.Contains("dragline") || child.GetComponent<DragWindow>() != null)
            {
                dragLineTransform = child;
            }
            else if (childName == "close" || childName.Contains("closeapp"))
            {
                closeTransform = child;
            }
            else if (childName.Contains("background"))
            {
                if (backgroundTransform == null || childName == "background")
                    backgroundTransform = child;
            }
        }

        // If background is present and has a custom size/pos (like in ad windows or popups),
        // use background's rect as the window frame reference.
        RectTransform referenceRect = windowRect;
        float refWidth = windowRect.rect.width;
        float refHeight = windowRect.rect.height;
        float refCenterX = 0f;
        float refCenterY = 0f;

        if (backgroundTransform != null)
        {
            RectTransform bgRect = backgroundTransform.GetComponent<RectTransform>();
            if (bgRect != null && bgRect.rect.width > 50f && bgRect.rect.height > 50f)
            {
                // In some popup/ad windows, the background defines the visible window bounds
                if (Mathf.Abs(bgRect.rect.width - windowRect.rect.width) > 5f || Mathf.Abs(bgRect.rect.height - windowRect.rect.height) > 5f)
                {
                    referenceRect = bgRect;
                    refWidth = bgRect.rect.width;
                    refHeight = bgRect.rect.height;
                    refCenterX = bgRect.anchoredPosition.x;
                    refCenterY = bgRect.anchoredPosition.y;
                }
            }
        }

        // 2. Adjust DragLine (Title Bar) layout
        if (dragLineTransform != null)
        {
            RectTransform dragRect = dragLineTransform.GetComponent<RectTransform>();
            if (dragRect != null)
            {
                float titleHeight = dragRect.rect.height > 10f ? dragRect.rect.height : 54f;
                float targetWidth = refWidth - (titleBarMargin * 2f);

                // Inset title bar width
                dragRect.sizeDelta = new Vector2(targetWidth, titleHeight);

                // Inset title bar from top edge
                float topY = refCenterY + (refHeight * 0.5f) - titleBarMargin - (titleHeight * 0.5f);
                dragRect.anchoredPosition = new Vector2(refCenterX, topY);

                // Position Close button inside/alongside title bar
                if (closeTransform != null)
                {
                    RectTransform closeRect = closeTransform.GetComponent<RectTransform>();
                    if (closeRect != null)
                    {
                        float closeW = closeRect.rect.width > 5f ? closeRect.rect.width : 38f;
                        float closeMargin = 4f;
                        float closeX = refCenterX + (targetWidth * 0.5f) - closeMargin - (closeW * 0.5f);
                        closeRect.anchoredPosition = new Vector2(closeX, topY);

                        UIHoverEffect closeHover = closeTransform.GetComponent<UIHoverEffect>() ?? closeTransform.gameObject.AddComponent<UIHoverEffect>();
                        closeHover.useExplicitHoverColor = true;
                        closeHover.hoverColor = new Color(1f, 0.70f, 0.70f, 1f);
                        closeHover.useExplicitPressedColor = true;
                        closeHover.pressedColor = new Color(0.85f, 0.50f, 0.50f, 1f);
                    }
                }
            }
        }

        // 3. Create or update the 3D Beveled Borders
        // Top & Left = Highlight (White)
        // Bottom & Right = Shadow (Dark)
        EnsureBorderImage("_W95_Border_Top", referenceRect,
            new Vector2(0f, 1f), new Vector2(1f, 1f), // Anchor Top-Stretch
            new Vector2(0f, 1f), // Pivot Top
            new Vector2(0f, borderThickness), // Size
            new Vector2(0f, 0f), // Pos
            highlightColor);

        EnsureBorderImage("_W95_Border_Left", referenceRect,
            new Vector2(0f, 0f), new Vector2(0f, 1f), // Anchor Left-Stretch
            new Vector2(0f, 0.5f), // Pivot Left
            new Vector2(borderThickness, 0f), // Size
            new Vector2(0f, 0f), // Pos
            highlightColor);

        EnsureBorderImage("_W95_Border_Right", referenceRect,
            new Vector2(1f, 0f), new Vector2(1f, 1f), // Anchor Right-Stretch
            new Vector2(1f, 0.5f), // Pivot Right
            new Vector2(borderThickness, 0f), // Size
            new Vector2(0f, 0f), // Pos
            shadowColor);

        EnsureBorderImage("_W95_Border_Bottom", referenceRect,
            new Vector2(0f, 0f), new Vector2(1f, 0f), // Anchor Bottom-Stretch
            new Vector2(0.5f, 0f), // Pivot Bottom
            new Vector2(0f, borderThickness), // Size
            new Vector2(0f, 0f), // Pos
            shadowColor);

        // 4. Clean up blurry UnityEngine.UI.Shadow components if requested
        if (removeBlurryShadows)
        {
            if (backgroundTransform != null)
            {
                Shadow bgShadow = backgroundTransform.GetComponent<Shadow>();
                if (bgShadow != null)
                    bgShadow.enabled = false;
            }

            if (dragLineTransform != null)
            {
                Shadow dragShadow = dragLineTransform.GetComponent<Shadow>();
                if (dragShadow != null)
                    dragShadow.enabled = false;
            }
        }
    }

    private void EnsureBorderImage(string borderName, RectTransform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPos, Color color)
    {
        Transform borderTransform = parent.Find(borderName);
        GameObject borderGO;

        if (borderTransform == null)
        {
            borderGO = new GameObject(borderName);
            borderGO.transform.SetParent(parent, false);
        }
        else
        {
            borderGO = borderTransform.gameObject;
        }

        RectTransform rt = borderGO.GetComponent<RectTransform>() ?? borderGO.AddComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.sizeDelta = sizeDelta;
        rt.anchoredPosition = anchoredPos;

        Image img = borderGO.GetComponent<Image>() ?? borderGO.AddComponent<Image>();
        img.color = color;
        img.raycastTarget = false;

        // Bring border to front of parent background so it's always visible
        borderGO.transform.SetAsLastSibling();
    }
}

#if UNITY_EDITOR
public static class Win95WindowStylerEditor
{
    [MenuItem("Tools/Apply Windows 95 Style to All Windows")]
    public static void ApplyToAllWindowsInScene()
    {
        DragWindow[] dragWindows = Object.FindObjectsByType<DragWindow>(FindObjectsSortMode.None);
        int count = 0;

        foreach (var dw in dragWindows)
        {
            if (dw.transform.parent == null) continue;
            GameObject windowGO = dw.transform.parent.gameObject;

            Win95WindowFrame frame = windowGO.GetComponent<Win95WindowFrame>();
            if (frame == null)
            {
                frame = windowGO.AddComponent<Win95WindowFrame>();
            }

            frame.ApplyWin95Style();
            EditorUtility.SetDirty(windowGO);
            count++;
        }

        Debug.Log($"Successfully applied Windows 95 style to {count} windows in the scene!");
    }
}
#endif
