using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ShareContentManager : MonoBehaviour
{
    public void Share() 
    {
        StartCoroutine(TakeScreenshot());
    }

    public IEnumerator TakeScreenshot() 
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0,0, Screen.width, Screen.height),0,0);
        texture.Apply();

        string path = Path.Combine(Application.temporaryCachePath, "SharedImage.png");
        File.WriteAllBytes(path, texture.EncodeToPNG());

        Destroy(texture);

        new NativeShare()
            .AddFile(path)
            .SetSubject("This is my score")
            .SetText("Share your score with your friends")
            .Share();
    }
}
