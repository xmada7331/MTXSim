using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class Game_behaviour : MonoBehaviour
{
    GameManager gameManager;
    GameTabUnlocker gameTabUnlocker;
    ContentChecker contentChecker;
    public GameObject currencyMoney;
    public GameObject currencyGems;
    public GameObject currencyDiamonds;
    public float bossHp;
    public Slider bossHpSlider;
    public float clickDmg;
    public float defeatedEnemies = 0f;
    private int lastBossIndex = -1;
    private float dropRateMult = 1f;

    public float clicks;
    public float monstersSlain;

    public Image bossImage;
    public TMP_Text bossName;
    // Keep existing type (Image) so this change is non-breaking in inspector.
    public List<Sprite> bossSprites;
    public List<string> bossNames;

    public List<Sprite> playerSkin;
    public List<Sprite> weaponSkin;
    public List<Sprite> backgroundSkin;
    public Image defaultPlayerSkin;
    public Image defaultWeaponSkin;
    public Image defaultBackgroundSkin;

    public bool hasAutoClick = false;
    public bool isAutoClickRunning = false;
    public GameObject gameWindow;

    public GameObject PopupPrefab;
    public bool hasPopUpUpgrade = false;

    public Animator playerAnimator;
    public Animator bossAnimator;

    private bool firstTimePlaying = true;
    public GameObject introSequenceObject;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameTabUnlocker = FindObjectOfType<GameTabUnlocker>();
        contentChecker = FindObjectOfType<ContentChecker>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bossHpSlider.value = bossHp;
        if (bossHp <= 0)
        {
            RotateNewBoss();
        }
        if (hasAutoClick && !isAutoClickRunning && (gameWindow == null || gameWindow.activeSelf))
        {
            StartCoroutine(AutoClickDamage());
        }

        if (contentChecker.hasCardLinked)
        {
            currencyMoney.SetActive(true);
            currencyGems.SetActive(true);
            currencyDiamonds.SetActive(true);
        }
        else
        {
            currencyMoney.SetActive(false);
            currencyGems.SetActive(false);
            currencyDiamonds.SetActive(false);
        }
    }

    public void TakeClickDamage()
    {
        bossHp -= clickDmg;
        clicks += 1;
        gameTabUnlocker.clicksCount.text = clicks.ToString("F0");
        if (hasPopUpUpgrade)
        {
            GameObject popUp = Instantiate(PopupPrefab, new Vector3((Random.Range(1f, 2f)), 0, 0), Quaternion.identity, gameWindow != null ? gameWindow.transform : null);
            popUp.GetComponentInChildren<TMP_Text>().text = clickDmg.ToString("F0"); 
        }
        playerAnimator.SetTrigger("Click");
        bossAnimator.SetTrigger("Hit");

    }

    private void RotateNewBoss()
    {

        int available = Mathf.Min(
            bossSprites != null ? bossSprites.Count : 0,
            bossNames != null ? bossNames.Count : 0
        );

        int index;
        if (available == 1)
        {
            index = 0;
        }
        else
        {
            // Build a candidate list excluding the last chosen index to avoid repeats
            var candidates = new List<int>(available - 1);
            for (int i = 0; i < available; i++)
            {
                if (i != lastBossIndex) candidates.Add(i);
            }
            index = candidates[Random.Range(0, candidates.Count)];
        }

        lastBossIndex = index;

        var selectedImage = bossSprites[index];
        if (selectedImage != null)
        {
            bossImage.sprite = selectedImage;
        }
        if (selectedImage == bossSprites[0] || selectedImage == bossSprites[1] || selectedImage == bossSprites[2])
        {
            dropRateMult = 1f;
            bossHpSlider.maxValue = 100;
            bossHp = 100;
        }
        else
        {
            dropRateMult = 1.5f;
            bossHpSlider.maxValue = 200;
            bossHp = 200;
        }

            bossName.text = bossNames[index];

        GetBossMoney();
        defeatedEnemies += 1;
        monstersSlain++;
        gameTabUnlocker.scoreText.text = monstersSlain.ToString("F0");
    }

    private void GetBossMoney()
    {
        if (Random.value <= 0.35f * dropRateMult && contentChecker.hasCurrencyGems)
        {
            gameManager.playerGems += 1; 
        }
        if (Random.value <= .005f && contentChecker.hasCurrencyDiamonds)
        {
            gameManager.playerDiamonds += 1; 
        }
    }

    private IEnumerator AutoClickDamage()
    {
        isAutoClickRunning = true;
        while (hasAutoClick && (gameWindow == null || gameWindow.activeSelf))
        {
            yield return new WaitForSeconds(0.1f);
            bossHp -= clickDmg / 2f;
            playerAnimator.SetTrigger("Click");
            bossAnimator.SetTrigger("Hit");
            if (hasPopUpUpgrade)
            {
                GameObject popUp = Instantiate(PopupPrefab, new Vector3((Random.Range(1f, 2f)), 0, 0), Quaternion.identity, gameWindow != null ? gameWindow.transform : null);
                popUp.GetComponentInChildren<TMP_Text>().text = clickDmg.ToString("F0");
            }
        }
        isAutoClickRunning = false;
    }
    public void ChangePlayerSkin()
    {
        if (playerSkin == null || playerSkin.Count == 0 || defaultPlayerSkin == null)
        {
            return;
        }

        Sprite current = defaultPlayerSkin.sprite;
        int currentIndex = playerSkin.IndexOf(current); // -1 if not found
        int nextIndex = currentIndex + 1;

        // If nextIndex is out of bounds, rotate back to first index (0)
        if (nextIndex >= playerSkin.Count || nextIndex < 0)
        {
            nextIndex = 0;
        }

        // Find the next non-null sprite, scanning up to playerSkin.Count entries and wrapping around.
        for (int i = 0; i < playerSkin.Count; i++)
        {
            int idx = (nextIndex + i) % playerSkin.Count;
            Sprite candidate = playerSkin[idx];
            if (candidate != null)
            {
                defaultPlayerSkin.sprite = candidate;
                return;
            }
        }
    }
    public void ChangeWeaponSkin()
    {
        if (weaponSkin == null || weaponSkin.Count == 0 || defaultWeaponSkin == null)
        {
            return;
        }

        Sprite current = defaultWeaponSkin.sprite;
        int currentIndex = weaponSkin.IndexOf(current); // -1 if not found
        int nextIndex = currentIndex + 1;

        // If nextIndex is out of bounds, rotate back to first index (0)
        if (nextIndex >= weaponSkin.Count || nextIndex < 0)
        {
            nextIndex = 0;
        }

        // Find the next non-null sprite, scanning up to weaponSkin.Count entries and wrapping around.
        for (int i = 0; i < weaponSkin.Count; i++)
        {
            int idx = (nextIndex + i) % weaponSkin.Count;
            Sprite candidate = weaponSkin[idx];
            if (candidate != null)
            {
                defaultWeaponSkin.sprite = candidate;
                return;
            }
        }
    }

    public void ChangeBackgroundSkin()
    {
        if (backgroundSkin == null || backgroundSkin.Count == 0 || defaultBackgroundSkin == null)
            return;

        Sprite current = defaultBackgroundSkin.sprite;
        int currentIndex = backgroundSkin.IndexOf(current); // -1 if not found
        int nextIndex = currentIndex + 1;

        if (nextIndex >= backgroundSkin.Count || nextIndex < 0)
            nextIndex = 0;

        // Scan for the next valid choice. If index == 0 we treat it as the "none" option
        for (int i = 0; i < backgroundSkin.Count; i++)
        {
            int idx = (nextIndex + i) % backgroundSkin.Count;
            Sprite candidate = backgroundSkin[idx];

            if (idx == 0)
            {
                // index 0 = "no background" -> disable Image component
                defaultBackgroundSkin.enabled = false;
                defaultBackgroundSkin.sprite = candidate; // optional (may be null)
                return;
            }

            if (candidate != null)
            {
                // normal background -> enable Image and assign sprite
                defaultBackgroundSkin.enabled = true;
                defaultBackgroundSkin.sprite = candidate;
                return;
            }
        }
    }

    public void IntroSequence()
    {
        if (firstTimePlaying)
        {
            firstTimePlaying = false;
            StartCoroutine(IntroSequenceAnimation());
        }
    }

    private IEnumerator IntroSequenceAnimation()
    {
        introSequenceObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        introSequenceObject.SetActive(false);
    }
}
