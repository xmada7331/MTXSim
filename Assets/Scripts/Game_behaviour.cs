using UnityEngine;
using UnityEngine.UI;

public class Game_behaviour : MonoBehaviour
{
    GameManager gameManager;
    GameTabUnlocker gameTabUnlocker;
    public float bossHp;
    public Slider bossHpSlider;
    public float clickDmg;
    public float defeatedEnemies = 0f;

    public float clicks;
    public float score;

    public Image bossImage;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameTabUnlocker = FindObjectOfType<GameTabUnlocker>();
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
    }

    public void TakeClickDamage()
    {
        bossHp -= clickDmg;
        clicks += 1;
        gameTabUnlocker.clicksCount.text = clicks.ToString("F0");
        
    }

    private void RotateNewBoss()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        bossHp = 100;
        bossImage.color = randomColor;
        GetBossMoney(); 
        defeatedEnemies += 1;
        score += Random.Range(25,100);
        gameTabUnlocker.scoreText.text = score.ToString("F0");
    }

    private void GetBossMoney()
    {
        //float coinToss = Random.Range(0, 1);
        gameManager.playerMoney += 10;
        gameManager.playerGems += 1;
        if (Random.value <= .2)
        {
            gameManager.playerDiamonds += 1; 
        }
    }
}
