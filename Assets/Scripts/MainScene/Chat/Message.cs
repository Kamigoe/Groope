using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    private const float BaseSizeHeight = 170f;
    
    [SerializeField] private Image _userIcon = default;
    [SerializeField] private Text _timeText = default;
    [SerializeField] private Text _text = default;
    [SerializeField] private Image _image = default;

    private RectTransform _myRectTransform = default;

    public void Initialize(Sprite iconImage, string time, string message)
    {
        _myRectTransform = GetComponent<RectTransform>();
        _userIcon.sprite = iconImage ? iconImage : _userIcon.sprite;
        _timeText.text = time;
        _text.text = message;
        Destroy(_image.transform.parent.gameObject);
        _image = null;
        Redraw();
    }

    public void Initialize(Sprite iconImage, string time, Sprite receiveImage)
    {
        _myRectTransform = GetComponent<RectTransform>();
        _userIcon.sprite = iconImage ? iconImage : _userIcon.sprite;
        _timeText.text = time;
        _image.sprite = receiveImage;
        Destroy(_text.gameObject);
        _text = null;
        Redraw();
    }

    private void Redraw()
    {
        if (_text != null)
        {
            var textRect = _text.GetComponent<RectTransform>();
            var textHeight = textRect.sizeDelta.y - textRect.localPosition.y + 10;

            _myRectTransform.sizeDelta = new Vector2(_myRectTransform.sizeDelta.x,
                textHeight > BaseSizeHeight ? textHeight : BaseSizeHeight);
        }

        if (_image == null) return;
        _image.SetNativeSize();
        var imageRect = _image.GetComponent<RectTransform>();
        var size = imageRect.sizeDelta;
        var ratioX = 240f / size.x;
        var ratioY = 135f / size.y;
        if (ratioX < ratioY)
            imageRect.sizeDelta = size * ratioX;
        else
            imageRect.sizeDelta = size * ratioY;
    }
}
