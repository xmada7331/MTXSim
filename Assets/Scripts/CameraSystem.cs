using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraSystem : MonoBehaviour
{
    GameManager gameManager;
    public List<Sprite> feetPics;
    public Image feetPicsImage1;
    public Image feetPicsImage2;
    public Image feetPicsImage3;
    public Slider timerSlider;
    public GameObject cameraCooldown;
    public GameObject uploadText;
    public TMP_Text soldForText;
    public TMP_Text noSpaceText;

    public float pictureCooldown = 60f;

    public bool has2ndSlot = false;
    public bool has3rdSlot = false;

    private bool slot1Taken = false;
    private bool slot2Taken = false;
    private bool slot3Taken = false;
    private float moneyEarned1;
    private float moneyEarned2;
    private float moneyEarned3;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
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
            if (!slot1Taken || !slot2Taken || !slot3Taken)
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
        yield return new WaitForSeconds(2f);
        soldForText.gameObject.SetActive(false);
    }
    public IEnumerator NoSpace()
    {
        noSpaceText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        noSpaceText.gameObject.SetActive(false);
    }
}
