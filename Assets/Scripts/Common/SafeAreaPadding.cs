using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[ExecuteAlways]
public class SafeAreaPadding : MonoBehaviour
{
    private DeviceOrientation postOrientation;
    
    void Update()
    {
#if UNITY_IOS || UNITY_ANDROID
        if (Input.deviceOrientation != DeviceOrientation.Unknown && postOrientation == Input.deviceOrientation)
            return;

        postOrientation = Input.deviceOrientation;

        RectTransform rect = GetComponent<RectTransform>();
        Rect area = Screen.safeArea;
        Resolution resolution = Screen.currentResolution;

        Rect keyArea = TouchScreenKeyboard.area;

        rect.sizeDelta = Vector2.zero;
        rect.anchorMax = new Vector2(area.xMax / resolution.width, area.yMax / resolution.height);
        rect.anchorMin = new Vector2(area.xMin / resolution.width, (area.yMin + keyArea.height) / resolution.height);
#endif
    }
}