using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource mouseClick;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayMouseClickSound();
        }
    }

    private void PlayMouseClickSound()
    {
        mouseClick.Play();
    }
}
