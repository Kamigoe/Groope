using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    private readonly float ModalAlpha = 0.5f;
    
    [SerializeField] private RectTransform _groupMenu;
    [SerializeField] private RaycastDetector _modal;

    private Vector2 _screenRatio;
    private Vector2 _mousePosition;
    private Vector2 _touchPosRatio;

    private Vector2 _groupPos;

    private bool _onTouch;
    
    // Start is called before the first frame update
    void Start()
    {
        _onTouch = false;
        
        var trigger = TouchEventTrigger.Instance;
        var resolution = GetComponent<CanvasScaler>().referenceResolution;

        _screenRatio.x = resolution.x / Screen.width;
        _screenRatio.y = resolution.y / Screen.height;
        
        trigger.SetEventTrigger(EventTriggerType.PointerDown, (data) =>
        {
            _onTouch = true;
            _mousePosition = Input.mousePosition;
            _groupPos = _groupMenu.anchoredPosition;
        });
        
        trigger.SetEventTrigger(EventTriggerType.Drag, (data) =>
        {
            var pos = new Vector2(Input.mousePosition.x - _mousePosition.x, Input.mousePosition.y - _mousePosition.y);
            _touchPosRatio = new Vector2(pos.x / Screen.width, pos.y / Screen.height);

            var menuPosX = _groupPos.x + 365f * _touchPosRatio.x;
            if (menuPosX > -10f) menuPosX = -10f;
            if (menuPosX < -375f) menuPosX = -375f;
            
            _groupMenu.anchoredPosition = new Vector2(menuPosX, _groupPos.y);
            _modal.color = new Color(0, 0, 0, (menuPosX + 10f) / -365f * ModalAlpha);
        });
        
        trigger.SetEventTrigger(EventTriggerType.PointerUp, (data) =>
        {
            if (Mathf.Abs(_touchPosRatio.x) >= 0.15f)
            {
                DOVirtual.Float((float)_groupMenu.anchoredPosition.x, _touchPosRatio.x > 0 ? -10 : -375f, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f, (posX) =>
                {
                    _groupMenu.anchoredPosition = new Vector2(posX, _groupPos.y);
                });
                _modal.DOFade(_touchPosRatio.x > 0 ? 0 : ModalAlpha, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f);
            }
            else
            {
                DOVirtual.Float((float)_groupMenu.anchoredPosition.x, _groupPos.x, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f, (posX) =>
                {
                    _groupMenu.anchoredPosition = new Vector2(posX, _groupPos.y);
                });
                _modal.DOFade(_modal.color.a > ModalAlpha / 2f ? ModalAlpha : 0, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f);
            }

            _onTouch = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
