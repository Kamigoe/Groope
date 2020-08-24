using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticicationIcon : MonoBehaviour
{
    private Text _text = default;
    private LayoutElement _element = default;

    private int _notificationCount = default;
    
    private bool _allReadyInit = false;

    private void Start()
    {
        Initialize();
    }

    public void Initialize(int notificationCount = 0)
    {
        if (_allReadyInit) return;
        
        _text = GetComponentInChildren<Text>();
        _element = GetComponent<LayoutElement>();

        _element.preferredWidth = notificationCount == 0 ? 0 : 48;
        _notificationCount = notificationCount;
        _allReadyInit = true;
    }

    public void AddNotificationCount()
    {
        _notificationCount++;
        if (_notificationCount >= 1)
        {
            _element.preferredWidth = 48;
        }
    }

    public void ClearNotification()
    {
        _notificationCount = 0;
        _element.preferredWidth = 0;
    }
}
