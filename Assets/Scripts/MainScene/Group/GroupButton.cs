using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupButton : MonoBehaviour
{
    [SerializeField] private Image _groupIcon = default;
    [SerializeField] private GroupName _groupName = default;
    [SerializeField] private NotificationIcon _notificationIcon = default;
    [SerializeField] private LayoutElement _talkingIcon = default;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void SetGroupIcon(Sprite sprite)
    {
        _groupIcon.sprite = sprite;
    }

    public void SetGroupName (string name)
    {
        _groupName.SetGroupName(name);
    }

    public void SetNotification(int count)
    {
        for(var i = 0; i < count; i++)
            _notificationIcon.AddNotificationCount();
    }

    public void ClearNotification()
    {
        _notificationIcon.ClearNotification();
    }
}
