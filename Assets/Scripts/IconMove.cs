using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IconMove : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Canvas iconCanvas;

    private Vector3 previousWorldPosition;
    private Vector2 previousAnchoredPosition;
    private bool hadAnchoredPosition;

    // Track whether we've stored the previous position for the current drag
    private bool storedStartPosition = false;

    // Existing EventTrigger-compatible method kept for compatibility
    public void DragHandler(BaseEventData data)
    {
        OnDrag((PointerEventData)data);
    }

    // EventSystem interface implementation: store start position when pointer is pressed
    public void OnPointerDown(PointerEventData eventData)
    {
        StoreStartPosition();
    }

    // Also provide EventTrigger-compatible pointer-down signature
    public void PointerDownHandler(BaseEventData data)
    {
        OnPointerDown((PointerEventData)data);
    }

    // Store current position so we can restore if necessary
    private void StoreStartPosition()
    {
        if (storedStartPosition)
            return;

        storedStartPosition = true;

        var rt = transform as RectTransform;
        if (rt != null)
        {
            previousAnchoredPosition = rt.anchoredPosition;
            hadAnchoredPosition = true;
        }
        else
        {
            hadAnchoredPosition = false;
        }

        previousWorldPosition = transform.position;
    }

    // Drag implementation (keeps current behavior)
    public void OnDrag(PointerEventData eventData)
    {
        if (iconCanvas == null)
            return;

        RectTransform canvasRect = iconCanvas.transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, iconCanvas.worldCamera, out Vector2 pos);
        transform.position = iconCanvas.transform.TransformPoint(pos);
    }

    // EventTrigger-compatible end-drag signature
    public void EndDragHandler(BaseEventData data)
    {
        OnEndDrag((PointerEventData)data);
    }

    // When drag ends, check whether the object's local (0,0) is outside the canvas.
    // If so, revert to the stored start position.
    public void OnEndDrag(PointerEventData eventData)
    {
        if (iconCanvas == null)
        {
            storedStartPosition = false;
            return;
        }

        // Ensure we have a stored start position (if pointer-down wasn't fired for some reason, store now)
        if (!storedStartPosition)
            StoreStartPosition();

        RectTransform canvasRect = iconCanvas.transform as RectTransform;

        // Determine world position of this object's local (0,0) (the pivot point / middle)
        Vector3 originWorld = transform.TransformPoint(Vector3.zero);

        // Convert to screen point and then to canvas-local point
        Vector2 originScreen = RectTransformUtility.WorldToScreenPoint(iconCanvas.worldCamera, originWorld);
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, originScreen, iconCanvas.worldCamera, out Vector2 originLocal))
        {
            // Conversion failed — be conservative and restore
            RestoreStartPosition();
            storedStartPosition = false;
            return;
        }

        // If the origin is outside the canvas rect, restore previous position
        if (!canvasRect.rect.Contains(originLocal))
        {
            RestoreStartPosition();
        }

        storedStartPosition = false;
    }

    private void RestoreStartPosition()
    {
        var rt = transform as RectTransform;
        if (hadAnchoredPosition && rt != null)
        {
            rt.anchoredPosition = previousAnchoredPosition;
        }
        else
        {
            transform.position = previousWorldPosition;
        }
    }
}
