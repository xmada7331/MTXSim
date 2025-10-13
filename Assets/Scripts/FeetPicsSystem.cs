using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeetPicsSystem : MonoBehaviour
{
    GameManager gameManager;
    ShopPurchasable shopPurchasable;
    CameraSystem cameraSystem;
    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorText;
    public TMP_Text successText;

    public TMP_Text accountName;
    public Image accountImage;

    public GameObject registerScreen;
    public GameObject successScreen;
    private string successUsername;

    public Image registerImage;
    public Image sellImage;

    public GameObject picLock1;
    public GameObject picLock2;

    private bool pic1Unlocked = false;
    private bool pic2Unlocked = false;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        cameraSystem = FindObjectOfType<CameraSystem>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterAccount()
    {
        if (nameInput.text.Length <= 0 || passwordInput.text.Length <= 0)
        {
            errorText.gameObject.SetActive(true);
            return;
        }
        else
        {
            CreateAccount();
        }
    }

    private void CreateAccount()
    {
        accountName.gameObject.SetActive(true);
        accountImage.gameObject.SetActive(true);
        accountName.text = nameInput.text;
        successUsername = nameInput.text;
        successText.text = "Account created! Welcome, " + successUsername + "!";
        registerScreen.SetActive(false);
        successScreen.SetActive(true);
        registerImage.gameObject.SetActive(false);
        sellImage.gameObject.SetActive(true);
    }

    public void UnlockPic1(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        target.GetComponent<Image>().color = Color.gray;
        if (!pic1Unlocked)
        {
            if (gameManager.playerMoney >= shopPurchasable.itemPrice)
            {
                pic1Unlocked = true;
                cameraSystem.has2ndSlot = true;
                picLock1.SetActive(false);
                gameManager.playerMoney -= shopPurchasable.itemPrice;
                shopPurchasable.itemPurchased = true;
                shopPurchasable.PurchasedItem();

            }
            else
            {
                shopPurchasable.WiggleItem();
            } 
        }
    }

    public void UnlockPic2(GameObject target)
    {
        shopPurchasable = target.GetComponent<ShopPurchasable>();
        target.GetComponent<Image>().color = Color.gray;
        if (!pic2Unlocked)
        {
            if (gameManager.playerMoney >= shopPurchasable.itemPrice)
            {
                cameraSystem.has3rdSlot = true;
                pic2Unlocked = true;
                picLock2.SetActive(false);
                gameManager.playerMoney -= shopPurchasable.itemPrice;
                shopPurchasable.itemPurchased = true;
                shopPurchasable.PurchasedItem();

            }
            else
            {
                shopPurchasable.WiggleItem();
            } 
        }
    }
}
