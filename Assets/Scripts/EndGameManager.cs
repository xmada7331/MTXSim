using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public Image blackout;
    public Animator blackoutAnimator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
