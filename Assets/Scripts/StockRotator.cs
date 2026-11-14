using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Animates the stock ticker items in the BunkBank app (StocksRotating / Texts).
/// Smoothly scrolls the stocks from right to left in an infinite, seamless loop
/// so that when the last stock is revealed, the first stock immediately follows it.
/// </summary>
public class StockRotator : MonoBehaviour
{
    [Header("Scroll Settings")]
    [Tooltip("Speed in pixels per second that the stocks move from right to left.")]
    public float scrollSpeed = 60f;

    [Tooltip("Whether to pause scrolling when the mouse cursor hovers over the ticker.")]
    public bool pauseOnHover = false;

    private RectTransform rectTransform;
    private HorizontalLayoutGroup layoutGroup;
    private float startX;
    private float sequenceWidth;
    private bool isInitialized = false;
    private bool isHovered = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        layoutGroup = GetComponent<HorizontalLayoutGroup>();
        if (rectTransform != null)
        {
            startX = rectTransform.anchoredPosition.x;
        }
    }

    private void Start()
    {
        if (!isInitialized)
        {
            Initialize();
        }
    }

    private void OnEnable()
    {
        // Re-ensure layout is built when window is opened
        if (isInitialized && layoutGroup != null && rectTransform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }

    private void Initialize()
    {
        if (isInitialized) return;
        isInitialized = true;

        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        if (layoutGroup == null)
        {
            layoutGroup = GetComponent<HorizontalLayoutGroup>();
        }

        startX = rectTransform.anchoredPosition.x;

        int originalCount = transform.childCount;
        if (originalCount == 0) return;

        // Collect all original children
        RectTransform[] originalChildren = new RectTransform[originalCount];
        for (int i = 0; i < originalCount; i++)
        {
            originalChildren[i] = transform.GetChild(i) as RectTransform;
        }

        float spacing = layoutGroup != null ? layoutGroup.spacing : 0f;

        // Calculate the base sequence width of one full loop of items
        sequenceWidth = 0f;
        for (int i = 0; i < originalCount; i++)
        {
            float w = originalChildren[i].rect.width;
            if (w <= 0f)
            {
                w = originalChildren[i].sizeDelta.x;
            }
            sequenceWidth += w + spacing;
        }

        if (sequenceWidth <= 0f) return;

        // Viewport width (StocksRotating parent)
        RectTransform parentRect = transform.parent as RectTransform;
        float viewportWidth = parentRect != null ? parentRect.rect.width : 1215f;
        if (viewportWidth <= 0f && parentRect != null)
        {
            viewportWidth = parentRect.sizeDelta.x;
        }

        // Ensure we duplicate enough sets so the viewport is always completely covered
        int setsNeeded = Mathf.CeilToInt(viewportWidth / sequenceWidth) + 1;
        setsNeeded = Mathf.Max(2, setsNeeded);

        for (int s = 1; s < setsNeeded; s++)
        {
            for (int i = 0; i < originalCount; i++)
            {
                GameObject clone = Instantiate(originalChildren[i].gameObject, transform);
                clone.name = $"{originalChildren[i].name}_LoopClone_{s}";
            }
        }

        // Force layout pass so clones are positioned immediately
        Canvas.ForceUpdateCanvases();
        if (layoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        // Measure actual distance between original child 0 and its first clone if available
        if (transform.childCount > originalCount)
        {
            RectTransform firstClone = transform.GetChild(originalCount) as RectTransform;
            if (firstClone != null && originalChildren[0] != null)
            {
                float measured = Mathf.Abs(firstClone.anchoredPosition.x - originalChildren[0].anchoredPosition.x);
                if (measured > 1f)
                {
                    sequenceWidth = measured;
                }
            }
        }
    }

    private void Update()
    {
        if (pauseOnHover && isHovered) return;
        if (sequenceWidth <= 0f) return;

        // Move container to the left
        rectTransform.anchoredPosition += Vector2.left * (scrollSpeed * Time.deltaTime);

        // Seamless wrap: when one complete sequence has moved past startX, shift forward by sequenceWidth
        if (scrollSpeed > 0f)
        {
            while (rectTransform.anchoredPosition.x <= startX - sequenceWidth)
            {
                rectTransform.anchoredPosition += new Vector2(sequenceWidth, 0f);
            }
        }
        else if (scrollSpeed < 0f)
        {
            while (rectTransform.anchoredPosition.x >= startX + sequenceWidth)
            {
                rectTransform.anchoredPosition -= new Vector2(sequenceWidth, 0f);
            }
        }
    }

    public void OnPointerEnter()
    {
        isHovered = true;
    }

    public void OnPointerExit()
    {
        isHovered = false;
    }

    public void SetSpeed(float newSpeed)
    {
        scrollSpeed = newSpeed;
    }

    public void ResetPosition()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(startX, rectTransform.anchoredPosition.y);
        }
    }
}
