using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    ContentChecker contentChecker;
    FeetPicsSystem feetPicsSystem;
    CameraSystem cameraSystem;
    FreeCoinSystem freeCoinSystem;
    GameManager gameManager;
    public Image blackout;
    public Animator blackoutAnimator;

    private bool tiggeredFeetEnding = false;
    private bool triggeredCoinEnding = false;
    private bool triggeredRichEnding = false;

    private void Awake()
    {
        gameManager =   FindObjectOfType<GameManager>();
        contentChecker = FindObjectOfType<ContentChecker>();
        feetPicsSystem = FindObjectOfType<FeetPicsSystem>();
        cameraSystem = FindObjectOfType<CameraSystem>();
        freeCoinSystem = FindObjectOfType<FreeCoinSystem>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraSystem.soldPics >= 50f && !tiggeredFeetEnding)
        {
            tiggeredFeetEnding = true;
            FeetPicsEnding();
        }
        if (freeCoinSystem.failedCoinFlips >= 6 && !triggeredCoinEnding)
        {
            triggeredCoinEnding = true;
            CoinFlipEnding();
        }
        if (gameManager.playerMoney >= 1000f && !triggeredRichEnding)
        {
            triggeredRichEnding = true;
            RichEnding();
        }
    }

    public void DebtGameOver()
    {
        StartCoroutine(DebtAnimation());
    }

    private IEnumerator DebtAnimation()
    {
        blackoutAnimator.SetTrigger("GameOver");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("DebtGameOver");
    }

    public void DiamondGameOver()
    {
        StartCoroutine(DiamondEndGameAnimation());
    }

    private IEnumerator DiamondEndGameAnimation()
    {
        blackoutAnimator.SetTrigger("GameOver");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("DiamondGameOver");
    }

    public void FeetPicsEnding()
    {
        StartCoroutine(FeetPicsEndGameAnimation());
    }

    private IEnumerator FeetPicsEndGameAnimation()
    {
               blackoutAnimator.SetTrigger("GameOver");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("FeetPicsEnding");
    }

    public void CoinFlipEnding()
    {
        StartCoroutine(CoinFlipEndGameAnimation());
    }

    private IEnumerator CoinFlipEndGameAnimation()
    {
        blackoutAnimator.SetTrigger("GameOver");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("CoinFlipEnding");
    }

    public void ShutDownSystemEnding()
    {
        StartCoroutine(ShutDownSystemEndGameAnimation());
    }
    private IEnumerator ShutDownSystemEndGameAnimation()
    {
        blackoutAnimator.SetTrigger("GameOver");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("ShutDownSystemEnding");
    }
    public void RichEnding()
    {
        StartCoroutine(RichEndingAnimation());
    }
    private IEnumerator RichEndingAnimation()
    {
        blackoutAnimator.SetTrigger("GameOver");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("RichEnding");
    }
}
