using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarSystemIcon : MonoBehaviour
{
    public Image desktopWallpaper;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WallpaperChange()
    {
        Color newColor = new Color(Random.value, Random.value, Random.value, .25f);
        desktopWallpaper.color = newColor;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
