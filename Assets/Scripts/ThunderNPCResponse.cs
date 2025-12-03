using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThunderNPCResponse : MonoBehaviour
{
    public TMP_Text responseDescription;
    public List<string> mainText;
    public List<string> playerResponses;
    public string playerResponse;
    public List<string> npcResponses;
    public string NPCResponse;

    public bool triggersHappiness;
    public bool triggersSpiciness;
    public bool triggersRomanceness;

    public int happinessAmount;
    public int spicinessAmount;
    public int romancenessAmount;

    private void Start()
    {
        RandomizeChoice();
    }

    public void RandomizeChoice()
    {
        int randomIndexNumber = Random.Range(0, mainText.Count);

        responseDescription.text = mainText[randomIndexNumber];
        playerResponse = playerResponses[randomIndexNumber];
        NPCResponse = npcResponses[randomIndexNumber];
    }
}
