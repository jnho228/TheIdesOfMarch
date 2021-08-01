using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshotter : MonoBehaviour
{
#if UNITY_EDITOR
    private void Update()
    {
        // For debug use only, so check if it's the editor or not.
        // Actually should probably do a start function and if it isn't the editor then I should auto delete this gameobject....... but meh. We'll see! 
        if (Input.GetKeyDown(KeyCode.G) && Application.isEditor)
        {
            string fileName = "Screen Shot " + System.DateTime.Now.Year + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Day + " at " + System.DateTime.Now.Hour + "." + System.DateTime.Now.Minute + "." + System.DateTime.Now.Second + ".png";
            ScreenCapture.CaptureScreenshot(fileName);

            Debug.Log("Saving " + fileName);
        }
    }
#endif
}
