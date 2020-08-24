using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupName : MonoBehaviour
{
    private Text _text = default;
    private RectTransform _textRect = default;
    private RectTransform _myRectTransform = default;
    
    private void Start()
    {
        _text = GetComponentInChildren<Text>();
        _textRect = GetComponentInChildren<RectTransform>();
        _myRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        _textRect.sizeDelta = _myRectTransform.sizeDelta;
    }

    public void SetGroupName(string name)
    {
        _text.text = name;
    }
}
