using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TouchEventTrigger : EventTrigger
{
    public static TouchEventTrigger Instance = null;

    public bool _isInstance;

    private UnityAction<PointerEventData> _onPointerDown;
    private UnityAction<PointerEventData> _onPointerUp;
    private UnityAction<PointerEventData> _onDrag;

    private void Awake()
    {
        if (Instance != null || !_isInstance) return;
        Instance = this;
        Debug.Log("Instance : " + gameObject.name);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        _onPointerDown?.Invoke(eventData);
    }
    
    public override void OnPointerUp(PointerEventData eventData)
    {
        _onPointerUp?.Invoke(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        _onDrag?.Invoke(eventData);
    }

    public void SetEventTrigger(EventTriggerType type, UnityAction<PointerEventData> eventFunc)
    {
        switch (type)
        {
            case EventTriggerType.PointerDown:
                _onPointerDown = eventFunc;
                break;
            case EventTriggerType.PointerEnter:
                break;
            case EventTriggerType.PointerExit:
                break;
            case EventTriggerType.PointerUp:
                _onPointerUp = eventFunc;
                break;
            case EventTriggerType.PointerClick:
                break;
            case EventTriggerType.Drag:
                _onDrag = eventFunc;
                break;
            case EventTriggerType.Drop:
                break;
            case EventTriggerType.Scroll:
                break;
            case EventTriggerType.UpdateSelected:
                break;
            case EventTriggerType.Select:
                break;
            case EventTriggerType.Deselect:
                break;
            case EventTriggerType.Move:
                break;
            case EventTriggerType.InitializePotentialDrag:
                break;
            case EventTriggerType.BeginDrag:
                break;
            case EventTriggerType.EndDrag:
                break;
            case EventTriggerType.Submit:
                break;
            case EventTriggerType.Cancel:
                break;
            default:
                break;
        }
    }
}
