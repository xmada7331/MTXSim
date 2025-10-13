using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsSystem : MonoBehaviour
{
    public List<GameObject> ads;
    public float timer;
    public float checkInterval = 1f; // How often to check for ad appearance

    void Start()
    {
        StartCoroutine(AdsEngage());
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    private IEnumerator AdsEngage()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            // Chance increases with timer, maxing out at 50% after 50 seconds
            float chance = Mathf.Clamp01(timer * 0.002f); // 1% per second, max 100%

            if (Random.value < chance)
            {
                // Reveal a random ad
                int adIndex = Random.Range(0, ads.Count);
                ads[adIndex].SetActive(true);

                // Reset timer after showing an ad
                timer = 0f;
            }
        }
    }
}
