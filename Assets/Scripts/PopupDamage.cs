using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupDamage : MonoBehaviour
{
    Game_behaviour game_Behaviour;
    public Rigidbody2D rb;
    public float lifetime;
    public TMP_Text damageText;
    private void Awake()
    {
        game_Behaviour = FindObjectOfType<Game_behaviour>();
    }
    void Start()
    {
        rb.linearVelocity = new Vector2(Random.Range(-2, 3), Random.Range(5, 10));
        Destroy(gameObject, lifetime);
        damageText.text = game_Behaviour.clickDmg.ToString("F0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
