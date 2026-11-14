using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A single Windows 95 style taskbar button (raised when the window is in the background or
/// minimised, pushed-in when the window is the active one). Built entirely from code by
/// <see cref="TaskbarManager"/>.
/// </summary>
public class TaskbarButton : MonoBehaviour, IPointerClickHandler
{
    private TaskbarManager manager;
    private AppWindow window;
    private RectTransform rect;
    private Image face;
    private Image top, left, right, bottom;
    private RectTransform content;
    private bool pressed;

    public static TaskbarButton Create(TaskbarManager manager, AppWindow window, RectTransform parent)
    {
        var go = new GameObject("TaskbarButton_" + window.name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        go.transform.SetParent(parent, false);
        var btn = go.AddComponent<TaskbarButton>();
        btn.Build(manager, window);
        return btn;
    }

    private void Build(TaskbarManager owner, AppWindow target)
    {
        manager = owner;
        window = target;
        rect = (RectTransform)transform;
        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);

        face = GetComponent<Image>();
        face.color = manager.buttonFace;
        face.raycastTarget = true;

        // Content (icon + label) - nudged by 1px when pressed, like real Win95 buttons
        content = NewRect("Content", rect);
        content.anchorMin = Vector2.zero;
        content.anchorMax = Vector2.one;
        content.offsetMin = new Vector2(6f, 3f);
        content.offsetMax = new Vector2(-6f, -3f);

        float iconSize = manager.buttonHeight - 12f;
        float labelLeft = 0f;
        if (window.icon != null)
        {
            RectTransform iconRect = NewRect("Icon", content);
            iconRect.anchorMin = new Vector2(0f, 0.5f);
            iconRect.anchorMax = new Vector2(0f, 0.5f);
            iconRect.pivot = new Vector2(0f, 0.5f);
            iconRect.sizeDelta = new Vector2(iconSize, iconSize);
            iconRect.anchoredPosition = Vector2.zero;
            Image iconImg = iconRect.gameObject.AddComponent<Image>();
            iconImg.sprite = window.icon;
            iconImg.preserveAspect = true;
            iconImg.raycastTarget = false;
            labelLeft = iconSize + 6f;
        }

        RectTransform labelRect = NewRect("Label", content);
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = new Vector2(labelLeft, 0f);
        labelRect.offsetMax = Vector2.zero;
        TextMeshProUGUI label = labelRect.gameObject.AddComponent<TextMeshProUGUI>();
        TMP_FontAsset font = manager.labelFont != null ? manager.labelFont : window.TitleFont;
        if (font != null) label.font = font;
        label.text = window.displayName;
        label.fontSize = manager.labelFontSize;
        label.color = manager.labelColor;
        label.alignment = TextAlignmentOptions.MidlineLeft;
        label.textWrappingMode = TextWrappingModes.NoWrap;
        label.overflowMode = TextOverflowModes.Ellipsis;
        label.raycastTarget = false;

        // 3D bevel
        float b = manager.bevelThickness;
        top = Edge("Top", new Vector2(0, 1), new Vector2(1, 1), new Vector2(0.5f, 1), new Vector2(0, b));
        left = Edge("Left", new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 0.5f), new Vector2(b, 0));
        right = Edge("Right", new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, 0.5f), new Vector2(b, 0));
        bottom = Edge("Bottom", new Vector2(0, 0), new Vector2(1, 0), new Vector2(0.5f, 0), new Vector2(0, b));

        SetPressed(false);
    }

    private static RectTransform NewRect(string objectName, Transform parent)
    {
        var go = new GameObject(objectName, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return (RectTransform)go.transform;
    }

    private Image Edge(string objectName, Vector2 aMin, Vector2 aMax, Vector2 pivot, Vector2 size)
    {
        RectTransform r = NewRect(objectName, rect);
        r.anchorMin = aMin;
        r.anchorMax = aMax;
        r.pivot = pivot;
        r.sizeDelta = size;
        r.anchoredPosition = Vector2.zero;
        Image img = r.gameObject.AddComponent<Image>();
        img.raycastTarget = false;
        return img;
    }

    public void SetLayout(Vector2 anchoredPosition, Vector2 size)
    {
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = size;
    }

    public void SetPressed(bool isPressed)
    {
        pressed = isPressed;
        face.color = pressed ? manager.buttonFacePressed : manager.buttonFace;
        Color tl = pressed ? manager.bevelDark : manager.bevelLight;
        Color br = pressed ? manager.bevelLight : manager.bevelDark;
        top.color = tl; left.color = tl;
        right.color = br; bottom.color = br;
        content.anchoredPosition = pressed ? new Vector2(1f, -1f) : Vector2.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        manager.OnTaskbarButtonClicked(window);
    }
}
