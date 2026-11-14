using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the account analysis descriptions and bankruptcy chance calculation in the BunkBank app.
/// Randomly selects 5 unique funny/semi-funny descriptions each time the player logs into their account,
/// and dynamically calculates bankruptcy risk (0-99%) based on current money with diminishing returns.
/// </summary>
public class BankAnalysisManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The 5 TMP Text elements for Analysis(1) through Analysis(5).")]
    public TMP_Text[] analysisTexts;

    [Tooltip("The TMP Text element showing the bankruptcy chance percentage.")]
    public TMP_Text bankruptcyChanceText;

    [Header("Analysis Descriptions Pool")]
    [TextArea(2, 4)]
    public string[] analysisPool;

    private GameManager gameManager;

    private readonly string[] defaultDescriptions = new string[]
    {
        "-spend less money gambling",
        "-find a better paying job",
        "-soon you might banktupt",
        "-suspicious activity",
        "-unusual amount of money",
        "-don't forget to invest in bank!",
        "-financial advisor recommends crying",
        "-stop buying virtual pictures",
        "-your wallet is screaming for help",
        "-have you considered not spending money?",
        "-high expenditure on questionable assets",
        "-risky investments detected in your area",
        "-assets declining faster than expected",
        "-budget plan: hope and pray",
        "-avoid visiting the DigiStore today",
        "-portfolio diversity: 100% regret",
        "-we noticed an extreme lack of savings",
        "-cutting back on digital coffee might help",
        "-emergency fund: nonexistent",
        "-cryptocurrency is not a retirement plan",
        "-donating to random streamers won't pay rent",
        "-bank AI detected questionable financial maturity",
        "-pro tip: spending $0 costs $0",
        "-your net worth is currently loading...",
        "-consider eating instant ramen this week",
        "-investing in memes has yielded 0 dividends",
        "-credit score is currently hiding in fear",
        "-our algorithm has given up on your budget"
    };

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (analysisPool == null || analysisPool.Length == 0)
        {
            analysisPool = defaultDescriptions;
        }
    }

    private void OnEnable()
    {
        GenerateAnalysis();
    }

    /// <summary>
    /// Generates 5 unique random analysis descriptions and calculates bankruptcy chance based on current money.
    /// Called whenever the player logs in or views the account window.
    /// </summary>
    public void GenerateAnalysis()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        UpdateBankruptcyChance();
        UpdateAnalysisTexts();
    }

    public void UpdateBankruptcyChance()
    {
        if (bankruptcyChanceText == null) return;

        float money = (gameManager != null) ? gameManager.playerMoney : 0f;
        int chance = CalculateBankruptcyChance(money);
        bankruptcyChanceText.text = $"{chance}%";
    }

    public static int CalculateBankruptcyChance(float money)
    {
        if (money <= 0f)
        {
            return Random.Range(90, 100); // 90 to 99 inclusive
        }

        float baseChance;

        if (money < 100f)
        {
            // From $0 to $100: drops from 94.5% down to 64%
            float t = money / 100f;
            baseChance = 94.5f - t * (94.5f - 64f);
        }
        else if (money < 200f)
        {
            // From $100 to $200: settles around ~60% (64% down to 56%)
            float t = (money - 100f) / 100f;
            baseChance = 64f - t * (64f - 56f);
        }
        else if (money < 500f)
        {
            // From $200 to $500: drops down towards the 40% milestone
            float t = (money - 200f) / 300f;
            baseChance = 56f - t * (56f - 40f);
        }
        else
        {
            // Hard cap at $500 (40%), then lowers by 5% points for every $100
            baseChance = 40f - ((money - 500f) / 100f) * 5f;
            if (baseChance <= 0f)
            {
                return 0;
            }
        }

        // Maintain a natural random variance/range around the base chance
        float spread = Mathf.Min(4.5f, (baseChance / 94.5f) * 2.5f + 2f);
        if (baseChance < 5f)
        {
            spread = baseChance * 0.4f;
        }

        int low = Mathf.Max(0, Mathf.RoundToInt(baseChance - spread));
        int high = Mathf.Min(99, Mathf.RoundToInt(baseChance + spread));

        if (low >= high)
        {
            return Mathf.Clamp(low, 0, 99);
        }

        return Random.Range(low, high + 1);
    }

    public void UpdateAnalysisTexts()
    {
        if (analysisTexts == null || analysisTexts.Length == 0) return;

        string[] pool = (analysisPool != null && analysisPool.Length >= analysisTexts.Length)
            ? analysisPool
            : defaultDescriptions;

        List<string> available = new List<string>(pool);
        for (int i = 0; i < analysisTexts.Length; i++)
        {
            if (analysisTexts[i] == null) continue;

            if (available.Count > 0)
            {
                int randomIndex = Random.Range(0, available.Count);
                analysisTexts[i].text = available[randomIndex];
                available.RemoveAt(randomIndex);
            }
            else
            {
                analysisTexts[i].text = "";
            }
        }
    }
}
