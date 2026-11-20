using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    ShopPurchasable shopPurchasable;
    GameManager gameManager;
    ContentChecker contentChecker;
    GameTabUnlocker gameTabUnlocker;
    private int currentCursorIndex = 0;

    [Header("Cursor Mode & Scaling")]
    [Tooltip("If true, renders the cursor inside a UI Canvas. This guarantees the cursor is ALWAYS the exact same proportional size as buttons and sliders at ANY resolution (720p, 1080p, 4K, and in the Editor Game View preview). It also receives CRT shader effects!")]
    public bool useSoftwareCursor = true;

    [Tooltip("If true and using Hardware Cursor, dynamically rescales the cursor texture and hotspot to match the UI resolution.")]
    public bool scaleHardwareCursor = true;

    [Tooltip("Reference screen height matching your Canvas Scaler (default 1080).")]
    public float referenceHeight = 1080f;

    [Header("Optional Software Cursor References (auto-created if empty)")]
    public Canvas softwareCursorCanvas;
    public Image softwareCursorImage;

    [Header("Cursor Textures")]
    public List<Texture2D> cursorSprites;

    public Texture2D cursorSpriteDefault;
    public Texture2D cursorSpriteHand;
    public Texture2D cursorSpriteBrown;
    public Texture2D cursorSpriteFuck;

    public Texture2D cursorSpriteShop1;
    public Texture2D cursorSpriteShop2;
    public Texture2D cursorSpriteShop3;

    private Texture2D currentCursorTexture;
    private int lastScreenWidth = -1;
    private int lastScreenHeight = -1;
    private Dictionary<Texture2D, Sprite> spriteCache = new Dictionary<Texture2D, Sprite>();
    private Dictionary<Texture2D, Dictionary<int, Texture2D>> scaledHardwareCache = new Dictionary<Texture2D, Dictionary<int, Texture2D>>();

    private void Awake()
    {
        contentChecker = FindObjectOfType<ContentChecker>();
        shopPurchasable = FindObjectOfType<ShopPurchasable>();
        gameManager = FindObjectOfType<GameManager>();
        gameTabUnlocker = FindObjectOfType<GameTabUnlocker>();
    }

    void Start()
    {
        if (cursorSpriteDefault == null && cursorSprites != null && cursorSprites.Count > 0)
        {
            cursorSpriteDefault = cursorSprites[0];
        }
        currentCursorTexture = cursorSpriteDefault;
        SetCursor(cursorSpriteDefault);
    }

    public bool IsMouseInsideWindow()
    {
        if (!Application.isFocused) return false;

        Vector3 mousePos = Input.mousePosition;
        return mousePos.x >= 0 && mousePos.x <= Screen.width &&
               mousePos.y >= 0 && mousePos.y <= Screen.height;
    }

    void Update()
    {
        if (useSoftwareCursor)
        {
            bool isInside = IsMouseInsideWindow();

            if (isInside)
            {
                if (Cursor.visible)
                    Cursor.visible = false;

                if (softwareCursorImage != null)
                {
                    if (!softwareCursorImage.gameObject.activeSelf)
                        softwareCursorImage.gameObject.SetActive(true);

                    softwareCursorImage.transform.position = Input.mousePosition;
                }
            }
            else
            {
                if (!Cursor.visible)
                    Cursor.visible = true;

                if (softwareCursorImage != null && softwareCursorImage.gameObject.activeSelf)
                {
                    softwareCursorImage.gameObject.SetActive(false);
                }
            }
        }
        else if (scaleHardwareCursor)
        {
            if (!Cursor.visible)
                Cursor.visible = true;

            if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
            {
                lastScreenWidth = Screen.width;
                lastScreenHeight = Screen.height;
                ApplyHardwareCursor(currentCursorTexture);
            }
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Cursor.visible = true;
            if (softwareCursorImage != null)
            {
                softwareCursorImage.gameObject.SetActive(false);
            }
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Cursor.visible = true;
            if (softwareCursorImage != null)
            {
                softwareCursorImage.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        if (softwareCursorCanvas != null)
        {
            softwareCursorCanvas.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }

    /// <summary>
    /// Gets the pixel-perfect hotspot for each cursor texture.
    /// In the default arrow cursor, the tip is located at (0, 0).
    /// In other stylized/shop cursors, the tip is located at y=0, and x=6 or 7.
    /// </summary>
    public Vector2 GetHotspot(Texture2D tex)
    {
        if (tex == null) return Vector2.zero;

        if (tex == cursorSpriteDefault)
            return Vector2.zero;

        if (tex == cursorSpriteFuck || tex == cursorSpriteShop1)
            return new Vector2(6, 0);

        if (tex == cursorSpriteHand || tex == cursorSpriteBrown || tex == cursorSpriteShop2 || tex == cursorSpriteShop3)
            return new Vector2(7, 0);

        // Fallback: auto-detect topmost non-transparent pixel if texture is readable
        if (tex.isReadable)
        {
            for (int y = tex.height - 1; y >= 0; y--)
            {
                for (int x = 0; x < tex.width; x++)
                {
                    if (tex.GetPixel(x, y).a > 0.3f)
                    {
                        return new Vector2(x, tex.height - 1 - y);
                    }
                }
            }
        }

        return Vector2.zero;
    }

    /// <summary>
    /// Sets the active cursor texture, applying either Software (Canvas) or Hardware cursor mode.
    /// </summary>
    public void SetCursor(Texture2D newCursor)
    {
        if (newCursor == null) return;
        currentCursorTexture = newCursor;

        if (useSoftwareCursor)
        {
            ApplySoftwareCursor(newCursor);
        }
        else
        {
            ApplyHardwareCursor(newCursor);
        }
    }

    private void ApplySoftwareCursor(Texture2D tex)
    {
        EnsureSoftwareCursorUI();
        bool isInside = IsMouseInsideWindow();
        Cursor.visible = !isInside;

        if (softwareCursorCanvas != null)
            softwareCursorCanvas.gameObject.SetActive(true);

        if (softwareCursorImage != null)
        {
            softwareCursorImage.gameObject.SetActive(isInside);

            Vector2 hotspot = GetHotspot(tex);
            Vector2 pivot = new Vector2(hotspot.x / (float)tex.width, (tex.height - hotspot.y) / (float)tex.height);

            if (!spriteCache.TryGetValue(tex, out Sprite sprite) || sprite == null)
            {
                sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), pivot, 100f);
                spriteCache[tex] = sprite;
            }

            softwareCursorImage.sprite = sprite;
            softwareCursorImage.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
            softwareCursorImage.rectTransform.pivot = pivot;
        }
    }

    private void ApplyHardwareCursor(Texture2D tex)
    {
        Cursor.visible = true;
        if (softwareCursorCanvas != null)
            softwareCursorCanvas.gameObject.SetActive(false);

        Vector2 baseHotspot = GetHotspot(tex);

        if (!scaleHardwareCursor)
        {
            Cursor.SetCursor(tex, baseHotspot, CursorMode.Auto);
            return;
        }

        float scale = (float)Screen.height / referenceHeight;
        if (Mathf.Abs(scale - 1f) < 0.05f)
        {
            Cursor.SetCursor(tex, baseHotspot, CursorMode.Auto);
            return;
        }

        int targetWidth = Mathf.Max(16, Mathf.RoundToInt(tex.width * scale));
        int targetHeight = Mathf.Max(16, Mathf.RoundToInt(tex.height * scale));

        if (!scaledHardwareCache.TryGetValue(tex, out var heightDict))
        {
            heightDict = new Dictionary<int, Texture2D>();
            scaledHardwareCache[tex] = heightDict;
        }

        if (!heightDict.TryGetValue(targetHeight, out Texture2D scaledTex) || scaledTex == null)
        {
            scaledTex = CreateScaledTexture(tex, targetWidth, targetHeight);
            heightDict[targetHeight] = scaledTex;
        }

        Vector2 scaledHotspot = new Vector2(baseHotspot.x * scale, baseHotspot.y * scale);
        Cursor.SetCursor(scaledTex, scaledHotspot, CursorMode.Auto);
    }

    private Texture2D CreateScaledTexture(Texture2D src, int targetWidth, int targetHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.ARGB32);
        rt.filterMode = FilterMode.Point;

        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;

        Graphics.Blit(src, rt);

        Texture2D result = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
        result.filterMode = FilterMode.Point;
        result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        result.Apply();

        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }

    private void EnsureSoftwareCursorUI()
    {
        if (softwareCursorCanvas != null && softwareCursorImage != null)
            return;

        GameObject existingCanvas = GameObject.Find("SoftwareCursorCanvas");
        if (existingCanvas != null)
        {
            softwareCursorCanvas = existingCanvas.GetComponent<Canvas>();
            softwareCursorImage = existingCanvas.GetComponentInChildren<Image>();
            if (softwareCursorCanvas != null && softwareCursorImage != null)
                return;
        }

        GameObject canvasGO = new GameObject("SoftwareCursorCanvas");
        softwareCursorCanvas = canvasGO.AddComponent<Canvas>();
        softwareCursorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        softwareCursorCanvas.sortingOrder = 32767;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0f;

        GameObject imageGO = new GameObject("CursorImage");
        imageGO.transform.SetParent(canvasGO.transform, false);

        softwareCursorImage = imageGO.AddComponent<Image>();
        softwareCursorImage.raycastTarget = false;

        DontDestroyOnLoad(canvasGO);
    }

    public void RotateCursor()
    {
        if (cursorSprites.Count == 0) return;

        currentCursorIndex = (currentCursorIndex + 1) % cursorSprites.Count;
        SetCursor(cursorSprites[currentCursorIndex]);
    }

    public void PurchaseHandCursor(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            if (!cursorSprites.Contains(cursorSpriteHand))
            {
                gameManager.playerMoney -= shopPurchasable.itemPrice;
                cursorSprites.Add(cursorSpriteHand);
                shopPurchasable.PurchasedItem();
                SetCursor(cursorSpriteHand);
                gameManager.expensesAmount += shopPurchasable.itemPrice;
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }

    public void PurchaseBrownCursor(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            if (!cursorSprites.Contains(cursorSpriteBrown))
            {
                gameManager.playerMoney -= shopPurchasable.itemPrice;
                cursorSprites.Add(cursorSpriteBrown);
                shopPurchasable.PurchasedItem();
                SetCursor(cursorSpriteBrown);
                gameManager.expensesAmount += shopPurchasable.itemPrice;
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }

    public void PurchaseFuckCursor(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice)
        {
            if (!cursorSprites.Contains(cursorSpriteFuck))
            {
                gameManager.playerMoney -= shopPurchasable.itemPrice;
                cursorSprites.Add(cursorSpriteFuck);
                shopPurchasable.PurchasedItem();
                SetCursor(cursorSpriteFuck);
                gameManager.expensesAmount += shopPurchasable.itemPrice;
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
        }
    }

    public void PurchaseShopCursor1(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerGems >= shopPurchasable.itemPrice && contentChecker.hasCurrencyGems)
        {
            if (!cursorSprites.Contains(cursorSpriteShop1))
            {
                cursorSprites.Add(cursorSpriteShop1);
                shopPurchasable.PurchaseItemGems();
                SetCursor(cursorSpriteShop1);
                gameTabUnlocker.purchaseSound.PlayOneShot(gameTabUnlocker.purchaseClip);
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                gameTabUnlocker.denyPurchaseSound.PlayOneShot(gameTabUnlocker.denyPurchaseClip);
            }
        }
    }

    public void PurchaseShopCursor2(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerGems >= shopPurchasable.itemPrice && contentChecker.hasCurrencyGems)
        {
            if (!cursorSprites.Contains(cursorSpriteShop2))
            {
                cursorSprites.Add(cursorSpriteShop2);
                shopPurchasable.PurchaseItemGems();
                SetCursor(cursorSpriteShop2);
                gameTabUnlocker.purchaseSound.PlayOneShot(gameTabUnlocker.purchaseClip);
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                gameTabUnlocker.denyPurchaseSound.PlayOneShot(gameTabUnlocker.denyPurchaseClip);
            }
        }
    }

    public void PurchaseShopCursor3(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerGems >= shopPurchasable.itemPrice && contentChecker.hasCurrencyGems)
        {
            if (!cursorSprites.Contains(cursorSpriteShop3))
            {
                cursorSprites.Add(cursorSpriteShop3);
                shopPurchasable.PurchaseItemGems();
                SetCursor(cursorSpriteShop3);
                gameTabUnlocker.purchaseSound.PlayOneShot(gameTabUnlocker.purchaseClip);
            }
        }
        else
        {
            shopPurchasable.WiggleItem();
            if (contentChecker.hasUISFX)
            {
                gameTabUnlocker.denyPurchaseSound.PlayOneShot(gameTabUnlocker.denyPurchaseClip);
            }
        }
    }
}
