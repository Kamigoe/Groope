using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[ExecuteAlways]
public class SafeAreaPadding : MonoBehaviour
{
    private DeviceOrientation postOrientation;

    public float width
    {
        get
        {
            var rect = GetComponent<RectTransform>();
            return rect.anchorMax.x * Screen.width - rect.anchorMin.x * Screen.width;
        }
    }
    public float height
    {
        get
        {
            var rect = GetComponent<RectTransform>();
            return rect.anchorMax.y * Screen.height - rect.anchorMin.y * Screen.height;
        }
    }
    public float padding_Bottom { get { return GetComponent<RectTransform>().anchorMin.y * Screen.height; } }
    
    void Update()
    {
#if UNITY_IOS || UNITY_ANDROID
        if (Input.deviceOrientation != DeviceOrientation.Unknown && postOrientation == Input.deviceOrientation)
            return;

        postOrientation = Input.deviceOrientation;

        var rect = GetComponent<RectTransform>();
        var area = Screen.safeArea;
        var resolution = Screen.currentResolution;

        var keyArea = TouchScreenKeyboard.area;

        rect.sizeDelta = Vector2.zero;
        rect.anchorMax = new Vector2(area.xMax / resolution.width, area.yMax / resolution.height);
        rect.anchorMin = new Vector2(area.xMin / resolution.width, (area.yMin + keyArea.height) / resolution.height);
#endif
    }
}