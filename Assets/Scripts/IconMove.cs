using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IconMove : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas iconCanvas;
    [SerializeField] private GameObject blueBg;

    public static readonly List<IconMove> AllIcons = new List<IconMove>();
    private static readonly List<RaycastResult> raycastResults = new List<RaycastResult>();

    private Vector3 previousWorldPosition;
    private Vector2 previousAnchoredPosition;
    private bool hadAnchoredPosition;
    private bool storedStartPosition = false;
    private bool isDragging = false;
    private Vector2 pointerDownScreenPos;

    private void Awake()
    {
        if (iconCanvas == null)
        {
            iconCanvas = GetComponentInParent<Canvas>();
        }

        FindBlueBg();
    }

    private void OnEnable()
    {
        if (!AllIcons.Contains(this))
            AllIcons.Add(this);
    }

    private void OnDisable()
    {
        AllIcons.Remove(this);
        SetSelected(false);
    }

    private void OnDestroy()
    {
        AllIcons.Remove(this);
    }

    public void FindBlueBg()
    {
        if (blueBg != null) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name.ToLower().Contains("blue_bg"))
            {
                blueBg = child.gameObject;
                break;
            }
        }
    }

    public void SetSelected(bool selected)
    {
        FindBlueBg();
        if (blueBg != null && blueBg.activeSelf != selected)
        {
            blueBg.SetActive(selected);
        }
    }

    public static void DeselectAllIcons()
    {
        for (int i = 0; i < AllIcons.Count; i++)
        {
            if (AllIcons[i] != null)
                AllIcons[i].SetSelected(false);
        }
    }

    public static void DeselectAllExcept(IconMove keepSelected)
    {
        for (int i = 0; i < AllIcons.Count; i++)
        {
            if (AllIcons[i] != null && AllIcons[i] != keepSelected)
                AllIcons[i].SetSelected(false);
        }
    }

    // Global click listener to deselect desktop icons when clicking anywhere else on screen
    private void Update()
    {
        // Run once per frame from the first active instance
        if (AllIcons.Count == 0 || AllIcons[0] != this) return;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            CheckGlobalClick();
        }
    }

    private static void CheckGlobalClick()
    {
        if (EventSystem.current == null)
        {
            DeselectAllIcons();
            return;
        }

        var pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        raycastResults.Clear();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        if (raycastResults.Count == 0)
        {
            // Clicked on empty desktop or outside canvas
            DeselectAllIcons();
            return;
        }

        // Check if any raycast hit belongs to an IconMove
        IconMove hitIcon = null;
        for (int i = 0; i < raycastResults.Count; i++)
        {
            hitIcon = raycastResults[i].gameObject.GetComponentInParent<IconMove>();
            if (hitIcon != null) break;
        }

        if (hitIcon == null)
        {
            // Clicked on something other than a desktop icon (window, taskbar, background, widget, etc.)
            DeselectAllIcons();
        }
        else
        {
            // Clicked on a desktop icon
            hitIcon.SetSelected(true);
            DeselectAllExcept(hitIcon);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDownScreenPos = eventData.position;
        isDragging = false;
        SetSelected(true);
        DeselectAllExcept(this);
        StoreStartPosition();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        eventData.dragging = true;
        StoreStartPosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (iconCanvas == null)
            return;

        if (!isDragging && Vector2.Distance(eventData.position, pointerDownScreenPos) > 4f)
        {
            isDragging = true;
            eventData.dragging = true;
        }

        RectTransform canvasRect = iconCanvas.transform as RectTransform;
        if (canvasRect != null && RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, iconCanvas.worldCamera, out Vector2 pos))
        {
            transform.position = iconCanvas.transform.TransformPoint(pos);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (iconCanvas == null)
        {
            storedStartPosition = false;
            isDragging = false;
            return;
        }

        if (!storedStartPosition)
            StoreStartPosition();

        RectTransform canvasRect = iconCanvas.transform as RectTransform;
        if (canvasRect != null)
        {
            Vector3 originWorld = transform.TransformPoint(Vector3.zero);
            Vector2 originScreen = RectTransformUtility.WorldToScreenPoint(iconCanvas.worldCamera, originWorld);
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, originScreen, iconCanvas.worldCamera, out Vector2 originLocal) || !canvasRect.rect.Contains(originLocal))
            {
                RestoreStartPosition();
            }
        }

        storedStartPosition = false;
        isDragging = false;
    }

    private void StoreStartPosition()
    {
        if (storedStartPosition) return;
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

    // EventTrigger-compatible methods
    public void DragHandler(BaseEventData data)
    {
        if (data is PointerEventData ped)
            OnDrag(ped);
    }

    public void PointerDownHandler(BaseEventData data)
    {
        if (data is PointerEventData ped)
            OnPointerDown(ped);
    }

    public void EndDragHandler(BaseEventData data)
    {
        if (data is PointerEventData ped)
            OnEndDrag(ped);
    }
}
