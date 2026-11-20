using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Provides an interactive, subtle hover and press color change and optional scale pop for UI buttons and widgets
/// (Close buttons, Minimise buttons, app widget banners, category cards, Start button, etc.).
/// Preserves the original alpha and scale, and automatically resets if the object is disabled or pointer exits.
/// </summary>
[DisallowMultipleComponent]
public class UIHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Target Graphic")]
    [Tooltip("Graphic to tint. If null, automatically uses Graphic on this GameObject.")]
    public Graphic targetGraphic;

    [Header("Hover Settings")]
    [Tooltip("If true, explicitly sets color to hoverColor on hover. If false, multiplies RGB or applies subtle highlight.")]
    public bool useExplicitHoverColor = false;

    [Tooltip("Explicit hover color tint.")]
    public Color hoverColor = new Color(0.88f, 0.94f, 1f, 1f);

    [Range(0.5f, 2f)]
    [Tooltip("Brightness multiplier when hovered (if not using explicit hover color).")]
    public float hoverBrightness = 1.15f;

    [Header("Press Settings")]
    [Tooltip("If true, explicitly sets color to pressedColor on press.")]
    public bool useExplicitPressedColor = false;

    public Color pressedColor = new Color(0.75f, 0.85f, 0.95f, 1f);

    [Range(0.2f, 1.5f)]
    public float pressedBrightness = 0.82f;

    [Header("Alpha Handling")]
    [Tooltip("If > 0, overrides the alpha when hovered (useful for semi-transparent buttons like Widget_bt).")]
    public float hoverAlphaOverride = -1f;

    [Header("Scale Hover Settings")]
    [Tooltip("If true, scales the object when hovered / pressed.")]
    public bool enableScaleEffect = false;

    [Tooltip("Scale multiplier when hovered (e.g. 1.05f to 1.1f for a nice subtle pop).")]
    public float hoverScaleMultiplier = 1.08f;

    [Tooltip("Scale multiplier when pressed (e.g. 1.0f to 1.02f).")]
    public float pressedScaleMultiplier = 1.02f;

    [Tooltip("Target transform to scale. If null, automatically uses this transform.")]
    public Transform targetTransform;

    [Tooltip("Duration of the smooth scale transition in seconds. 0 for instant.")]
    public float scaleAnimationDuration = 0.1f;

    [Header("Hover Callbacks")]
    public System.Action onHoverEnterAction;
    public System.Action onHoverExitAction;

    [Header("Purchasable / Selectable Support")]
    [Tooltip("Optional ShopPurchasable reference. If null, automatically detected.")]
    public ShopPurchasable shopPurchasable;

    [Tooltip("Optional Selectable reference (Button, etc.). If null, automatically detected.")]
    public Selectable targetSelectable;

    [Tooltip("Color to display when the item is purchased / disabled.")]
    public Color purchasedColor = new Color(1f, 1f, 1f, 0.2f);

    private Color normalColor;
    private Vector3 normalScale = Vector3.one;
    private bool isHovered;
    private bool isPressed;
    private bool initialized;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (initialized) return;
        if (targetGraphic == null) targetGraphic = GetComponent<Graphic>();
        if (targetTransform == null) targetTransform = transform;
        if (shopPurchasable == null) shopPurchasable = GetComponent<ShopPurchasable>();
        if (shopPurchasable == null) shopPurchasable = GetComponentInParent<ShopPurchasable>();
        if (targetSelectable == null) targetSelectable = GetComponent<Selectable>();

        if (targetGraphic != null)
        {
            normalColor = targetGraphic.color;
        }

        if (targetTransform != null)
        {
            normalScale = targetTransform.localScale;
            if (normalScale == Vector3.zero) normalScale = Vector3.one;
        }

        initialized = true;

        if (IsPurchased())
        {
            ApplyPurchasedVisuals();
        }
    }

    public bool IsPurchased()
    {
        return shopPurchasable != null && shopPurchasable.itemPurchased;
    }

    public bool CanInteract()
    {
        if (!enabled || !gameObject.activeInHierarchy) return false;
        if (targetSelectable != null && !targetSelectable.interactable) return false;
        if (IsPurchased()) return false;
        return true;
    }

    private void OnEnable()
    {
        if (!initialized) Initialize();
        if (IsPurchased())
        {
            ApplyPurchasedVisuals();
        }
        else
        {
            if (targetGraphic != null && !isHovered && !isPressed)
            {
                normalColor = targetGraphic.color;
            }
            ResetVisuals();
        }
    }

    private void OnDisable()
    {
        if (targetGraphic != null && initialized && !IsPurchased())
        {
            if (isHovered || isPressed)
            {
                targetGraphic.color = normalColor;
            }
            else
            {
                normalColor = targetGraphic.color;
            }
        }
        isHovered = false;
        isPressed = false;
        if (targetTransform != null && enableScaleEffect && initialized)
        {
            targetTransform.localScale = normalScale;
        }
        onHoverExitAction?.Invoke();
    }

    private void Update()
    {
        if (IsPurchased())
        {
            if (enableScaleEffect && targetTransform != null && initialized && targetTransform.localScale != normalScale)
            {
                targetTransform.localScale = normalScale;
            }
            return;
        }

        // Keep normalColor dynamically in sync with targetGraphic.color whenever resting
        if (!isHovered && !isPressed && targetGraphic != null)
        {
            normalColor = targetGraphic.color;
        }

        if (!enableScaleEffect || targetTransform == null) return;

        Vector3 targetScale = normalScale;
        if (isPressed)
        {
            targetScale = new Vector3(
                normalScale.x * pressedScaleMultiplier,
                normalScale.y * pressedScaleMultiplier,
                normalScale.z
            );
        }
        else if (isHovered)
        {
            targetScale = new Vector3(
                normalScale.x * hoverScaleMultiplier,
                normalScale.y * hoverScaleMultiplier,
                normalScale.z
            );
        }

        if (scaleAnimationDuration > 0f)
        {
            float step = Time.unscaledDeltaTime * (1f / Mathf.Max(0.01f, scaleAnimationDuration)) * 1.5f;
            targetTransform.localScale = Vector3.Lerp(targetTransform.localScale, targetScale, Mathf.Clamp01(step));

            if (Vector3.SqrMagnitude(targetTransform.localScale - targetScale) < 0.00001f)
            {
                targetTransform.localScale = targetScale;
            }
        }
        else
        {
            targetTransform.localScale = targetScale;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CanInteract()) return;
        if (!initialized) Initialize();

        if (!isHovered && !isPressed && !IsPurchased() && targetGraphic != null)
        {
            normalColor = targetGraphic.color;
        }

        isHovered = true;
        ApplyColor();
        onHoverEnterAction?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        isPressed = false;
        if (IsPurchased())
        {
            ApplyPurchasedVisuals();
            return;
        }
        ApplyColor();
        onHoverExitAction?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanInteract()) return;
        if (!initialized) Initialize();

        if (!isHovered && !isPressed && !IsPurchased() && targetGraphic != null)
        {
            normalColor = targetGraphic.color;
        }

        isPressed = true;
        ApplyColor();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        if (IsPurchased())
        {
            ApplyPurchasedVisuals();
            return;
        }
        ApplyColor();
    }

    private void ApplyColor()
    {
        if (targetGraphic == null) return;

        if (IsPurchased())
        {
            targetGraphic.color = purchasedColor;
            return;
        }

        float alpha = normalColor.a;

        if (isPressed)
        {
            if (useExplicitPressedColor)
            {
                targetGraphic.color = new Color(pressedColor.r, pressedColor.g, pressedColor.b, alpha);
            }
            else
            {
                float maxRgb = Mathf.Max(normalColor.r, normalColor.g, normalColor.b);
                if (maxRgb < 0.25f)
                {
                    // Dark button press: subtle lightening
                    float pressBoost = 0.10f * (pressedBrightness >= 0.5f ? pressedBrightness : 0.8f);
                    targetGraphic.color = new Color(
                        Mathf.Clamp01(normalColor.r + pressBoost),
                        Mathf.Clamp01(normalColor.g + pressBoost),
                        Mathf.Clamp01(normalColor.b + pressBoost),
                        alpha
                    );
                }
                else
                {
                    targetGraphic.color = new Color(
                        Mathf.Clamp01(normalColor.r * pressedBrightness),
                        Mathf.Clamp01(normalColor.g * pressedBrightness),
                        Mathf.Clamp01(normalColor.b * pressedBrightness),
                        alpha
                    );
                }
            }
        }
        else if (isHovered)
        {
            if (hoverAlphaOverride > 0f)
                alpha = Mathf.Clamp01(hoverAlphaOverride);

            bool shouldUseExplicit = useExplicitHoverColor;
            float maxNormal = Mathf.Max(normalColor.r, normalColor.g, normalColor.b);

            // If explicit hover color was configured (e.g. for a dark button) but the button
            // has become bright/white, or explicit hover color is noticeably darker than normalColor,
            // calculate the hover dynamically from the current bright color instead.
            if (shouldUseExplicit && maxNormal >= 0.7f)
            {
                float maxHover = Mathf.Max(hoverColor.r, hoverColor.g, hoverColor.b);
                if (maxHover < maxNormal - 0.15f)
                {
                    shouldUseExplicit = false;
                }
            }

            if (shouldUseExplicit)
            {
                targetGraphic.color = new Color(hoverColor.r, hoverColor.g, hoverColor.b, alpha);
            }
            else
            {
                // Dynamic hover based on the current color:
                if (maxNormal >= 0.85f && hoverBrightness >= 1f)
                {
                    // Light / white button: subtle Windows highlight tint (cool light-blue tint)
                    targetGraphic.color = new Color(
                        normalColor.r * 0.88f,
                        normalColor.g * 0.94f,
                        normalColor.b * 1.0f,
                        alpha
                    );
                }
                else if (maxNormal < 0.35f)
                {
                    // Dark / black button: multiplying by hoverBrightness won't brighten enough (0 * 1.15 = 0).
                    // Add an offset boost so dark/black buttons visibly brighten to grey on hover!
                    float darkBoost = 0.22f * (hoverBrightness >= 1f ? hoverBrightness : 1f);
                    targetGraphic.color = new Color(
                        Mathf.Clamp01(normalColor.r + darkBoost),
                        Mathf.Clamp01(normalColor.g + darkBoost),
                        Mathf.Clamp01(normalColor.b + darkBoost),
                        alpha
                    );
                }
                else
                {
                    // Mid-tone button
                    targetGraphic.color = new Color(
                        Mathf.Clamp01(normalColor.r * hoverBrightness),
                        Mathf.Clamp01(normalColor.g * hoverBrightness),
                        Mathf.Clamp01(normalColor.b * hoverBrightness),
                        alpha
                    );
                }
            }
        }
        else
        {
            targetGraphic.color = normalColor;
        }
    }

    public void ResetVisuals(bool invokeExitCallback = false)
    {
        if (targetGraphic != null && initialized)
        {
            if (IsPurchased())
            {
                targetGraphic.color = purchasedColor;
            }
            else if (isHovered || isPressed)
            {
                targetGraphic.color = normalColor;
            }
            else
            {
                normalColor = targetGraphic.color;
            }
        }
        isHovered = false;
        isPressed = false;
        if (enableScaleEffect && targetTransform != null && initialized)
        {
            targetTransform.localScale = normalScale;
        }
        if (invokeExitCallback)
        {
            onHoverExitAction?.Invoke();
        }
    }

    /// <summary>
    /// Explicitly refreshes the cached normal color from the target graphic's current color.
    /// Useful if an external script modifies the graphic's color while active.
    /// </summary>
    public void RefreshBaseColor()
    {
        if (targetGraphic != null && !isHovered && !isPressed && !IsPurchased())
        {
            normalColor = targetGraphic.color;
        }
    }

    public void ApplyPurchasedVisuals()
    {
        isHovered = false;
        isPressed = false;
        if (targetGraphic != null)
        {
            targetGraphic.color = purchasedColor;
        }
        if (enableScaleEffect && targetTransform != null && initialized)
        {
            targetTransform.localScale = normalScale;
        }
    }

    public void SetPurchased(Color? newPurchasedColor = null)
    {
        if (newPurchasedColor.HasValue)
        {
            purchasedColor = newPurchasedColor.Value;
        }
        ApplyPurchasedVisuals();
    }

    public void SetNormalColor(Color c)
    {
        normalColor = c;
        initialized = true;
        ApplyColor();
    }

    public void SetNormalScale(Vector3 s)
    {
        normalScale = s;
        if (targetTransform != null && !isHovered && !isPressed)
        {
            targetTransform.localScale = normalScale;
        }
    }
}
