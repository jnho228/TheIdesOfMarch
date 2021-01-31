using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenSwitcher : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Screen.fullScreen = !Screen.fullScreen;

            Resolution screenSize = Screen.currentResolution;
            Screen.SetResolution(screenSize.width, screenSize.height, FullScreenMode.ExclusiveFullScreen);
        }
    }
}
