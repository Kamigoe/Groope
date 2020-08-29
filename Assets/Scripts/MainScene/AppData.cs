using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppData
{
    public static bool isTalking;
    public static bool cameraActive;
    public static bool micActive;

    public static void Initialize()
    {
        isTalking = false;
        cameraActive = false;
        micActive = true;
    }
}
