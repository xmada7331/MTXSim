using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public UltEvents.UltEvent myEvent;
    ShopPurchasable shopPurchasable;
    ContentChecker contentChecker;

    public List<GameObject> antiClickObject;

    public float playerMoney;
    public float playerGems;
    public float playerDiamonds;


    public GameObject LockMenu;
    public GameObject bankWidget;
    public GameObject stockWidget;
    public GameObject freeCoinWidget;
    private void Awake()
    {
        contentChecker = FindObjectOfType<ContentChecker>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisableAntiClickObjects()
    {
        foreach (GameObject obj in antiClickObject)
        {
            obj.SetActive(false);
        }
    }
    public void UnlockShop(float amount)
    {
        if (playerMoney >= amount)
        {
            LockMenu.SetActive(false);
            playerMoney -= amount;
            contentChecker.unlockedMainShop = true;
        }
        else
        {
            Debug.Log("not enough money");
        }
    }


}
