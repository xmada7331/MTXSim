using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraSystem : MonoBehaviour
{
    GameManager gameManager;
    ShopPurchasable shopPurchasable;
    FeetPicsSystem feetPicsSystem;
    public List<Sprite> feetPics;
    public Image feetPicsImage1;
    public Image feetPicsImage2;
    public Image feetPicsImage3;
    public Slider timerSlider;
    public GameObject cameraCooldown;
    public GameObject uploadText;
    public TMP_Text soldForText;
    public TMP_Text noSpaceText;

    public GameObject sellPicHover1;
    public GameObject sellPicHover2;
    public GameObject sellPicHover3;

    public float pictureCooldown = 60f;

    public bool has2ndSlot = false;
    public bool has3rdSlot = false;

    private bool slot1Taken = false;
    private bool slot2Taken = false;
    private bool slot3Taken = false;
    private float moneyEarned1;
    private float moneyEarned2;
    private float moneyEarned3;


    public float soldPics = 0f;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        feetPicsSystem = FindObjectOfType<FeetPicsSystem>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PressCameraButton()
    {
        {
            if (!slot1Taken || !slot2Taken && has2ndSlot || !slot3Taken && has3rdSlot)
            {
                    cameraCooldown.SetActive(true);
                    StartCoroutine(CameraCooldown()); 
            }
            else
            {
                StartCoroutine(NoSpace());
            }
        }

    }
    public void TakeSnapshot()
    {
        if (!slot1Taken)
        {
            feetPicsImage1.sprite = feetPics[Random.Range(0, feetPics.Count)];
            slot1Taken = true;

        }
        else if (!slot2Taken && has2ndSlot)
        {
            feetPicsImage2.sprite = feetPics[Random.Range(0, feetPics.Count)];
            slot2Taken = true;
        }
        else if (!slot3Taken && has3rdSlot)
        {
            feetPicsImage3.sprite = feetPics[Random.Range(0, feetPics.Count)];
            slot3Taken = true;
        }
        else
        {
            Debug.Log("All slots full!");
        }
    }

    public void SellFirstPic()
    {
        if (slot1Taken)
        {
            moneyEarned1 = Random.Range(5, 16);
            soldForText.text = "Sold for $" + moneyEarned1.ToString();
            gameManager.playerMoney += moneyEarned1;
            feetPicsImage1.sprite = null;
            slot1Taken = false;
            StartCoroutine(SoldForText());
            soldPics += 1f;
            gameManager.incomeAmount += moneyEarned1;
        }
    }
    public void SellSecondPic()
    {
        if (slot2Taken)
        {
            moneyEarned2 = Random.Range(5, 16);
            gameManager.playerMoney += moneyEarned2;
            feetPicsImage2.sprite = null;
            slot2Taken = false;
            StartCoroutine(SoldForText());
            soldPics += 1f;
            gameManager.incomeAmount += moneyEarned2;

        }
    }
    public void SellThirdPic()
    {
        if (slot3Taken)
        {
            moneyEarned3 = Random.Range(5, 16);
            gameManager.playerMoney += moneyEarned3;
            feetPicsImage3.sprite = null;
            slot3Taken = false;
            StartCoroutine(SoldForText());
            soldPics += 1f;
            gameManager.incomeAmount += moneyEarned3;

        }
    }

    private IEnumerator CameraCooldown()
    {
        StartCoroutine(UploadText());
        float cooldownTime = pictureCooldown;
        float elapsedTime = 0f;
        while (elapsedTime < cooldownTime)
        {
            elapsedTime += Time.deltaTime;
            timerSlider.value = elapsedTime / cooldownTime;
            yield return null;
        }
        cameraCooldown.SetActive(false);
        TakeSnapshot();
    }
    private IEnumerator UploadText()
    {
        uploadText.SetActive(true);
        yield return new WaitForSeconds(2f);
        uploadText.SetActive(false);
    }

    public IEnumerator SoldForText()
    {
        soldForText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        soldForText.gameObject.SetActive(false);
    }
    public IEnumerator NoSpace()
    {
        noSpaceText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        noSpaceText.gameObject.SetActive(false);
    }

    public void UpgradeCameraCD1(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice && gameManager.hasCamera)
        {
            pictureCooldown = 30f;
            gameManager.playerMoney -= shopPurchasable.itemPrice;
            shopPurchasable.PurchaseItem();
            gameManager.expensesAmount += shopPurchasable.itemPrice;
        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }
    public void UpgradeCameraCD2(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        if (gameManager.playerMoney >= shopPurchasable.itemPrice && pictureCooldown <= 40)
        {
            pictureCooldown = 10f;
            gameManager.playerMoney -= shopPurchasable.itemPrice;
            shopPurchasable.PurchaseItem();
            gameManager.expensesAmount += shopPurchasable.itemPrice;

        }
        else
        {
            shopPurchasable.WiggleItem();
        }

    }

    public void HoverSell1()
    {
        if (slot1Taken)
        {
            sellPicHover1.SetActive(true);
        }
        else
        {
            sellPicHover1.SetActive(false);
        }
    }
    public void HoverSell2()
    {
        if (slot2Taken)
        {
            sellPicHover2.SetActive(true);
        }
        else
        {
            sellPicHover2.SetActive(false);
        }
    }
    public void HoverSell3()
    {
        if (slot3Taken)
        {
            sellPicHover3.SetActive(true);
        }
        else
        {
            sellPicHover3.SetActive(false);
        }
    }
}
