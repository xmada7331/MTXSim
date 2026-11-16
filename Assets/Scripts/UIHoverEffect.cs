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
            ResetVisuals();
        }
    }

    private void OnDisable()
    {
        if (IsPurchased())
        {
            isHovered = false;
            isPressed = false;
            if (targetTransform != null && enableScaleEffect)
            {
                targetTransform.localScale = normalScale;
            }
        }
        else
        {
            ResetVisuals(true);
        }
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
                targetGraphic.color = new Color(
                    Mathf.Clamp01(normalColor.r * pressedBrightness),
                    Mathf.Clamp01(normalColor.g * pressedBrightness),
                    Mathf.Clamp01(normalColor.b * pressedBrightness),
                    alpha
                );
            }
        }
        else if (isHovered)
        {
            if (hoverAlphaOverride > 0f)
                alpha = Mathf.Clamp01(hoverAlphaOverride);

            if (useExplicitHoverColor)
            {
                targetGraphic.color = new Color(hoverColor.r, hoverColor.g, hoverColor.b, alpha);
            }
            else
            {
                // If the normal color is already white/near-white, multiplying by >1 won't show!
                // So apply a subtle Windows highlight tint (cool light-blue tint).
                float maxRgb = Mathf.Max(normalColor.r, normalColor.g, normalColor.b);
                if (maxRgb >= 0.9f && hoverBrightness >= 1f)
                {
                    targetGraphic.color = new Color(
                        normalColor.r * 0.88f,
                        normalColor.g * 0.94f,
                        normalColor.b * 1.0f,
                        alpha
                    );
                }
                else
                {
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
        isHovered = false;
        isPressed = false;
        if (targetGraphic != null && initialized)
        {
            if (IsPurchased())
            {
                targetGraphic.color = purchasedColor;
            }
            else
            {
                targetGraphic.color = normalColor;
            }
        }
        if (enableScaleEffect && targetTransform != null && initialized)
        {
            targetTransform.localScale = normalScale;
        }
        if (invokeExitCallback)
        {
            onHoverExitAction?.Invoke();
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
