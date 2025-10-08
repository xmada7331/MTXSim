using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPurchasable : MonoBehaviour
{
    GameManager gameManager;
    public Image itemImage;
    public float itemPrice;
    public Transform transformToWiggle;
    public bool itemPurchased = false;
    public bool isPurchasable = false;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        itemImage = GetComponent<Image>();
        transformToWiggle = GetComponent<Transform>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PurchaseItem()
    {
        if (gameManager.playerMoney >= itemPrice && !itemPurchased && isPurchasable)
        {
            Color purchasedColor = new Color(1, 1, 1, 0.2f);
            itemPurchased = true;
            itemImage.color = purchasedColor;
            gameManager.playerMoney -= itemPrice;
        }
        else if (!itemPurchased)
        {
          WiggleItem();
        }
    }
    public void WiggleItem()
    {
        StartCoroutine(WiggleRoutine());
    }
    private IEnumerator WiggleRoutine()
    {

        float duration = 0.5f;
        float speed = 50f;
        float distance = 5f;
        float time = 0f;

        Vector3 pos = transformToWiggle.localPosition;
        Vector3 startPos = transformToWiggle.localPosition;

        while (time <= duration)
        {
            pos.x = Mathf.Lerp(Mathf.Sin(time * speed) * distance, 0, time / duration);
            transformToWiggle.localPosition = pos;
            time += Time.deltaTime;
            //itemImage.color = new Color32(200,0,0,200);
            yield return null;
        }

        transformToWiggle.localPosition = startPos;
        //itemImage.color = new Color32(244, 244, 244, 255);

    }


}
