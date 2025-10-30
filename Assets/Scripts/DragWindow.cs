using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public Canvas mainWindowCanvas;

    // Offset between the window parent's world position and the pointer world position
    private Vector3 pointerOffset;
    private Transform windowParent;

    void Awake()
    {
        windowParent = transform.parent;
    }

    // Capture the offset when the pointer is pressed so the window won't snap
    public void OnPointerDown(PointerEventData eventData)
    {
        if (windowParent == null || mainWindowCanvas == null)
            return;

        RectTransform canvasRect = mainWindowCanvas.transform as RectTransform;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, mainWindowCanvas.worldCamera, out Vector2 localPoint))
        {
            Vector3 pointerWorld = mainWindowCanvas.transform.TransformPoint(localPoint);
            pointerOffset = windowParent.position - pointerWorld;
        }
    }

    // Move the parent using the saved offset so dragging feels like a normal window
    public void OnDrag(PointerEventData eventData)
    {
        if (windowParent == null || mainWindowCanvas == null)
            return;

        RectTransform canvasRect = mainWindowCanvas.transform as RectTransform;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, mainWindowCanvas.worldCamera, out Vector2 localPoint))
        {
            Vector3 pointerWorld = mainWindowCanvas.transform.TransformPoint(localPoint);
            windowParent.position = pointerWorld + pointerOffset;
        }
    }

    // When dragging ends, snap back to (0,0) only if the window's local (0,0) point OR
    // the draggable child's local (0,0) point is off the canvas/screen.
    public void OnEndDrag(PointerEventData eventData)
    {
        if (windowParent == null || mainWindowCanvas == null)
            return;

        var canvasRect = mainWindowCanvas.transform as RectTransform;
        var windowRect = windowParent as RectTransform;

        // Helper: returns true if the given world point is outside the canvas rect
        bool IsWorldPointOutsideCanvas(Vector3 worldPoint)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(mainWindowCanvas.worldCamera, worldPoint);
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, mainWindowCanvas.worldCamera, out Vector2 localPoint))
                return true; // conservative: if conversion fails treat as outside
            return !canvasRect.rect.Contains(localPoint);
        }

        // Check the window parent's local (0,0) origin
        Vector3 windowOriginWorld = (windowRect != null) ? windowRect.TransformPoint(Vector3.zero) : windowParent.TransformPoint(Vector3.zero);

        // Check the draggable child (this) local (0,0) origin (the "middle point" of the top line)
        Vector3 childOriginWorld = transform.TransformPoint(Vector3.zero);

        if (IsWorldPointOutsideCanvas(windowOriginWorld) || IsWorldPointOutsideCanvas(childOriginWorld))
        {
            // Snap back to (0,0)
            if (windowRect != null)
                windowRect.anchoredPosition = Vector2.zero;
            else
                windowParent.position = mainWindowCanvas.transform.TransformPoint(Vector3.zero);
        }
    }

    // Keep existing EventTrigger-compatible signatures if you used them previously
    public void DragHandler(BaseEventData data)
    {
        OnDrag((PointerEventData)data);
    }

    public void PointerDownHandler(BaseEventData data)
    {
        OnPointerDown((PointerEventData)data);
    }

    public void EndDragHandler(BaseEventData data)
    {
        OnEndDrag((PointerEventData)data);
    }
}
