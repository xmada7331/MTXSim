using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Cycles through all direct children of ShopItems using Next and Back buttons.
/// Wrapping is automatic: going past the last child returns to the first, and going before the first wraps to the last.
/// </summary>
public class ShopItemCycler : MonoBehaviour
{
    [Tooltip("Parent transform whose direct children will be cycled. Defaults to this GameObject's transform.")]
    public Transform shopItemsParent;

    [Tooltip("Button GameObject that advances to the next child.")]
    public GameObject nextButton;

    [Tooltip("Button GameObject that goes back to the previous child.")]
    public GameObject backButton;

    private int currentIndex = 0;

    private void Awake()
    {
        if (shopItemsParent == null)
        {
            shopItemsParent = transform;
        }

        BindClick(nextButton, ShowNext);
        BindClick(backButton, ShowPrevious);
    }

    private void Start()
    {
        InitializeActiveChild();
    }

    private void BindClick(GameObject buttonObj, UnityEngine.Events.UnityAction callback)
    {
        if (buttonObj == null || callback == null) return;

        Button btn = buttonObj.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(callback);
            return;
        }

        EventTrigger trigger = buttonObj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = buttonObj.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((_) => callback());
        trigger.triggers.Add(entry);
    }

    private void InitializeActiveChild()
    {
        if (shopItemsParent == null || shopItemsParent.childCount == 0) return;

        int foundActive = -1;
        for (int i = 0; i < shopItemsParent.childCount; i++)
        {
            if (shopItemsParent.GetChild(i).gameObject.activeSelf)
            {
                foundActive = i;
                break;
            }
        }

        currentIndex = foundActive >= 0 ? foundActive : 0;
        ApplyActiveIndex();
    }

    private void SyncCurrentIndex()
    {
        if (shopItemsParent == null || shopItemsParent.childCount == 0) return;

        for (int i = 0; i < shopItemsParent.childCount; i++)
        {
            if (shopItemsParent.GetChild(i).gameObject.activeSelf)
            {
                currentIndex = i;
                return;
            }
        }

        if (currentIndex < 0 || currentIndex >= shopItemsParent.childCount)
        {
            currentIndex = 0;
        }
    }

    private void ApplyActiveIndex()
    {
        if (shopItemsParent == null || shopItemsParent.childCount == 0) return;

        for (int i = 0; i < shopItemsParent.childCount; i++)
        {
            bool shouldBeActive = (i == currentIndex);
            GameObject childObj = shopItemsParent.GetChild(i).gameObject;
            if (childObj.activeSelf != shouldBeActive)
            {
                childObj.SetActive(shouldBeActive);
            }
        }
    }

    /// <summary>
    /// Deactivates the current child and activates the next child (wrapping around to the first).
    /// </summary>
    public void ShowNext()
    {
        if (shopItemsParent == null) shopItemsParent = transform;
        int count = shopItemsParent.childCount;
        if (count <= 1) return;

        SyncCurrentIndex();
        currentIndex = (currentIndex + 1) % count;
        ApplyActiveIndex();
    }

    /// <summary>
    /// Deactivates the current child and activates the previous child (wrapping around to the last).
    /// </summary>
    public void ShowPrevious()
    {
        if (shopItemsParent == null) shopItemsParent = transform;
        int count = shopItemsParent.childCount;
        if (count <= 1) return;

        SyncCurrentIndex();
        currentIndex = (currentIndex - 1 + count) % count;
        ApplyActiveIndex();
    }
}
