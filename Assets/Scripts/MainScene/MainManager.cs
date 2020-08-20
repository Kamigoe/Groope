using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    private float ChangeStateMoveRatioX = 0.05f;
    private float ChangeStateMoveRatioY = 0.05f;
    private float ChangeMenuMoveRatioX = 0.15f;
    private float ChangeMenuMoveRatioY = 0.15f;
    private float ModalAlpha = 0.5f;

    private float GroupMinX = -375f;
    private float GroupMaxX = -10f;
    private float ChatMinX = 10f;
    private float ChatMaxX = 375f;
    private float ActionMinY = 0;
    private float ActionMaxY = 545f;
    
    [SerializeField] private SafeAreaPadding _padding = default;
    [SerializeField] private RectTransform _groupMenu = default;
    [SerializeField] private RectTransform _chatMenu = default;
    [SerializeField] private RectTransform _actionMenu = default;
    [SerializeField] private TouchEventTrigger _modalAction = default;
    [SerializeField] private TouchEventTrigger _modal = default;

    [SerializeField] private Button _groupButton = default;
    [SerializeField] private Button _chatButton = default;
    [SerializeField] private Button _actionButton = default;

    private RaycastDetector _modalAction_Graph;
    private RaycastDetector _modal_Graph;
    private Image _action_Graph;

    private MainSceneState _beforeState;
    private MainSceneState _state;
    private GraphicRaycaster _raycaster;

    private Vector2 _screenRatio;
    private Vector2 _touchPosition;
    private Vector2 _touchPosRatio;

    private RectTransform _menuRect;
    private Vector2 _menuPos;
    private Vector2 _minPos;
    private Vector2 _maxPos;

    private float _bottomPaddingSize;
    private bool _onTouch;
    private bool _isShowingAction;
    
    void Start()
    {
        ChangeMenuMoveRatioY = ChangeMenuMoveRatioX * _padding.width / _padding.height;
        ChangeStateMoveRatioY = ChangeStateMoveRatioX * _padding.width / _padding.height;

        _onTouch = false;
        _isShowingAction = false;
        _state = MainSceneState.ShowGroup;
        _raycaster = GetComponent<GraphicRaycaster>();
        
        var trigger = TouchEventTrigger.Instance;
        var resolution = GetComponent<CanvasScaler>().referenceResolution;

        _screenRatio.x = resolution.x / Screen.width;
        _screenRatio.y = resolution.y / Screen.height;

        _bottomPaddingSize = _padding.padding_Bottom;
        ActionMinY -= _bottomPaddingSize;

        _actionMenu.anchoredPosition = new Vector2(_actionMenu.anchoredPosition.x, ActionMinY);
        _actionButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, _bottomPaddingSize);

        _modalAction_Graph = _modalAction.GetComponent<RaycastDetector>();
        _modal_Graph = _modal.GetComponent<RaycastDetector>();
        _action_Graph = _actionButton.GetComponent<Image>();

        _modal.SetEventTrigger(EventTriggerType.PointerClick, (data) =>
        {
            if (_state == MainSceneState.ShowGroup)
            {
                DOVirtual.Float(GroupMinX, GroupMaxX, 0.2f, (posX) =>
                {
                    _groupMenu.anchoredPosition = new Vector2(posX, _groupMenu.anchoredPosition.y);
                });
                _modal_Graph.DOFade(0, 0.2f);
                _state = _isShowingAction ? MainSceneState.ShowAction : MainSceneState.ShowMain;
            }
            else if (_state == MainSceneState.ShowChat)
            {
                DOVirtual.Float(ChatMaxX, ChatMinX, 0.2f, (posX) =>
                {
                    _chatMenu.anchoredPosition = new Vector2(posX, _chatMenu.anchoredPosition.y);
                });
                _modal_Graph.DOFade(0, 0.2f);
                _state = _isShowingAction ? MainSceneState.ShowAction : MainSceneState.ShowMain;
            }
        });
        _modalAction.SetEventTrigger(EventTriggerType.PointerClick, (data) =>
        {
            if (_state == MainSceneState.ShowAction)
            {
                DOVirtual.Float(ActionMaxY, ActionMinY, 0.2f, (posY) =>
                {
                    _actionMenu.anchoredPosition = new Vector2(_actionMenu.anchoredPosition.x, posY);
                });
                _modalAction_Graph.DOFade(0, 0.2f);
                _action_Graph.DOFade(1, 0.2f);
                _isShowingAction = false;
                _state = MainSceneState.ShowMain;
            }
        });

        _groupButton.onClick.AddListener(() => 
        {
            if (_state == MainSceneState.ShowMain)
            {
                DOVirtual.Float(GroupMaxX, GroupMinX, 0.2f, (posX) =>
                {
                    _groupMenu.anchoredPosition = new Vector2(posX, _groupMenu.anchoredPosition.y);
                });
                _modal_Graph.DOFade(ModalAlpha, 0.2f);
                _state = MainSceneState.ShowGroup;
            }
        });
        
        _chatButton.onClick.AddListener(() => 
        {
            if (_state == MainSceneState.ShowMain)
            {
                DOVirtual.Float(ChatMinX, ChatMaxX, 0.2f, (posX) =>
                {
                    _chatMenu.anchoredPosition = new Vector2(posX, _chatMenu.anchoredPosition.y);
                });
                _modal_Graph.DOFade(ModalAlpha, 0.2f);
                _state = MainSceneState.ShowChat;
            }
        });

        _actionButton.onClick.AddListener(() =>
        {
            if (_state == MainSceneState.ShowMain)
            {
                DOVirtual.Float(ActionMinY, ActionMaxY, 0.2f, (posY) =>
                {
                    _actionMenu.anchoredPosition = new Vector2(_actionMenu.anchoredPosition.x, posY);
                });
                _modalAction_Graph.DOFade(ModalAlpha, 0.2f);
                _action_Graph.DOFade(0, 0.2f);
                _state = MainSceneState.ShowAction;

                _isShowingAction = true;
            }
        });
        
        // trigger.SetEventTrigger(EventTriggerType.PointerDown, (data) =>
        // {
        //     _onTouch = true;
        //     _touchPosition = Input.mousePosition;
        //     _menuPos = _groupMenu.anchoredPosition;
        // });
        
        // trigger.SetEventTrigger(EventTriggerType.Drag, (data) =>
        // {
        //     var pos = new Vector2(Input.mousePosition.x - _touchPosition.x, Input.mousePosition.y - _touchPosition.y);
        //     _touchPosRatio = new Vector2(pos.x / Screen.width, pos.y / Screen.height);

        //     var menuPosX = _menuPos.x + 365f * _touchPosRatio.x;
        //     if (menuPosX > -10f) menuPosX = -10f;
        //     if (menuPosX < -375f) menuPosX = -375f;
            
        //     _groupMenu.anchoredPosition = new Vector2(menuPosX, _menuPos.y);
        //     _modal.color = new Color(0, 0, 0, (menuPosX + 10f) / -365f * ModalAlpha);
        // });
        
        // trigger.SetEventTrigger(EventTriggerType.PointerUp, (data) =>
        // {
        //     if (Mathf.Abs(_touchPosRatio.x) >= 0.15f)
        //     {
        //         DOVirtual.Float((float)_groupMenu.anchoredPosition.x, _touchPosRatio.x > 0 ? -10 : -375f, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f, (posX) =>
        //         {
        //             _groupMenu.anchoredPosition = new Vector2(posX, _menuPos.y);
        //         });
        //         _modal.DOFade(_touchPosRatio.x > 0 ? 0 : ModalAlpha, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f);
        //     }
        //     else
        //     {
        //         DOVirtual.Float((float)_groupMenu.anchoredPosition.x, _menuPos.x, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f, (posX) =>
        //         {
        //             _groupMenu.anchoredPosition = new Vector2(posX, _menuPos.y);
        //         });
        //         _modal.DOFade(_modal.color.a > ModalAlpha / 2f ? ModalAlpha : 0, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f);
        //     }

        //     _onTouch = false;
        // });
    }

    void Update()
    {
        var touchInfo = TouchInput.GetTouch();

        if (touchInfo == TouchInfo.Began)
        {
            Debug.Log("TouchBegan");
            _onTouch = true;
            _touchPosition = TouchInput.GetTouchPosition();
            
            Debug.Log(_state);
        }
        else if (touchInfo == TouchInfo.Ended)
        {
            Debug.Log("TouchEnded");
            _onTouch = false;
            _raycaster.ignoreReversedGraphics = true;

            if (_menuRect != null)
            {
                var menuRect = _menuRect;
                if (Mathf.Abs(_touchPosRatio.x) >= ChangeMenuMoveRatioX && _state != MainSceneState.MoveAction)
                {
                    DOVirtual.Float(menuRect.anchoredPosition.x, _touchPosRatio.x > 0 ? _maxPos.x : _minPos.x, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f, (posX) =>
                    {
                        menuRect.anchoredPosition = new Vector2(posX, menuRect.anchoredPosition.y);
                    });

                    if (_state == MainSceneState.MoveGroup)
                        _modal_Graph.DOFade(_touchPosRatio.x > 0 ? 0 : ModalAlpha, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f);
                    else if (_state == MainSceneState.MoveChat)
                        _modal_Graph.DOFade(_touchPosRatio.x > 0 ? ModalAlpha : 0, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f);
                }
                else if (Mathf.Abs(_touchPosRatio.y) >= ChangeMenuMoveRatioY && _state == MainSceneState.MoveAction)
                {
                    DOVirtual.Float(menuRect.anchoredPosition.y, _touchPosRatio.y > 0 ? _maxPos.y : _minPos.y, (1f - Mathf.Abs(_touchPosRatio.y)) * 0.2f, (posY) =>
                    {
                        menuRect.anchoredPosition = new Vector2(menuRect.anchoredPosition.x, posY);
                    });
                    
                    _modalAction_Graph.DOFade(_touchPosRatio.y > 0 ? ModalAlpha : 0, (1f - Mathf.Abs(_touchPosRatio.y)) * 0.2f);
                    _action_Graph.DOFade(_touchPosRatio.y > 0 ? 0 : 1, (1f - Mathf.Abs(_touchPosRatio.y)) * 0.2f);
                    _isShowingAction = _touchPosRatio.y > 0 ? true : false;
                }
                else
                {
                    DOVirtual.Float(menuRect.anchoredPosition.x, _menuPos.x, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f, (posX) =>
                    {
                        menuRect.anchoredPosition = new Vector2(posX, menuRect.anchoredPosition.y);
                    });
                    DOVirtual.Float(menuRect.anchoredPosition.y, _menuPos.y, (1f - Mathf.Abs(_touchPosRatio.y)) * 0.2f, (posY) =>
                    {
                        menuRect.anchoredPosition = new Vector2(menuRect.anchoredPosition.x, posY);
                    });
                    if (_state == MainSceneState.MoveAction)
                        _modalAction_Graph.DOFade(_modalAction_Graph.color.a > ModalAlpha / 2f ? ModalAlpha : 0, (1f - Mathf.Abs(_touchPosRatio.y)) * 0.2f);
                    else
                        _modal_Graph.DOFade(_modal_Graph.color.a > ModalAlpha / 2f ? ModalAlpha : 0, (1f - Mathf.Abs(_touchPosRatio.x)) * 0.2f);
                }

                _menuRect = null;
                var nextState = GetNextState();
                _beforeState = _state;
                _state = nextState;
                Debug.Log(nextState);
            }
        }
        
        if (_onTouch)
        {
            var touchPos = TouchInput.GetTouchPosition();
            var moveX = touchPos.x - _touchPosition.x;
            var moveY = touchPos.y - _touchPosition.y;
            _touchPosRatio = new Vector2(moveX / Screen.width, moveY / Screen.height);

            if (_state != MainSceneState.MoveAction &&
                _state != MainSceneState.MoveChat &&
                _state != MainSceneState.MoveGroup)
            {
                var nextState = GetNextState();
                if (_state != nextState)
                {
                    _raycaster.ignoreReversedGraphics = false;
                    _beforeState = _state;
                    _state = nextState;
                    Debug.Log(nextState);
                    if (_state == MainSceneState.MoveGroup)
                    {
                        _menuPos = _groupMenu.anchoredPosition;
                        _menuRect = _groupMenu;
                        _minPos = new Vector2(GroupMinX, _menuPos.y);
                        _maxPos = new Vector2(GroupMaxX, _menuPos.y);
                    }
                    else if (_state == MainSceneState.MoveChat)
                    {
                        _menuPos = _chatMenu.anchoredPosition;
                        _menuRect = _chatMenu;
                        _minPos = new Vector2(ChatMinX, _menuPos.y);
                        _maxPos = new Vector2(ChatMaxX, _menuPos.y);
                    }
                    else if (_state == MainSceneState.MoveAction)
                    {
                        _menuPos = _actionMenu.anchoredPosition;
                        _menuRect = _actionMenu;
                        _minPos = new Vector2(_menuPos.x, ActionMinY);
                        _maxPos = new Vector2(_menuPos.x, ActionMaxY);
                    }
                }
            }
            else if (_menuRect != null)
            {
                var distX = Mathf.Abs(_maxPos.x - _minPos.x);
                var distY = Mathf.Abs(_maxPos.y - _minPos.y);
                var menuPosX = _menuPos.x + distX * (_touchPosRatio.x - ChangeStateMoveRatioX) / (1 - ChangeStateMoveRatioX);
                var menuPosY = _menuPos.y + distY * (_touchPosRatio.y - ChangeStateMoveRatioY) / (1 - ChangeStateMoveRatioY);
                if (menuPosX > _maxPos.x) menuPosX = _maxPos.x;
                if (menuPosX < _minPos.x) menuPosX = _minPos.x;
                if (menuPosY > _maxPos.y) menuPosY = _maxPos.y;
                if (menuPosY < _minPos.y) menuPosY = _minPos.y;
                
                _menuRect.anchoredPosition = new Vector2(menuPosX, menuPosY);
                if (_state == MainSceneState.MoveAction)
                {
                    var alpha = Mathf.Abs(menuPosY - _minPos.y) / distY;
                    _modalAction_Graph.color = new Color(0, 0, 0, alpha * ModalAlpha);
                    _action_Graph.color = new Color(1, 1, 1, 1 - alpha);
                }
                else
                {
                    var minX = Mathf.Min(Mathf.Abs(_minPos.x), Mathf.Abs(_maxPos.x)) == Mathf.Abs(_minPos.x) ? _minPos.x : _maxPos.x;
                    var alpha = Mathf.Abs(menuPosX - minX) / distX;
                    _modal_Graph.color = new Color(0, 0, 0, alpha * ModalAlpha);
                }
            }
        }

        _modal_Graph.gameObject.SetActive(false);
        _modalAction_Graph.gameObject.SetActive(false);
        _action_Graph.gameObject.SetActive(false);
        if (_modal_Graph.color.a > 0)
            _modal_Graph.gameObject.SetActive(true);
        if (_modalAction_Graph.color.a > 0)
            _modalAction_Graph.gameObject.SetActive(true);
        if (_action_Graph.color.a > 0)
            _action_Graph.gameObject.SetActive(true);
    }

    private MainSceneState GetNextState ()
    {
        if (_state == MainSceneState.MoveGroup)
        {
            if (_touchPosRatio.x >= ChangeMenuMoveRatioX)
                return _isShowingAction ? MainSceneState.ShowAction : MainSceneState.ShowMain;
            if (_touchPosRatio.x <= -ChangeMenuMoveRatioX)
                return MainSceneState.ShowGroup;
            return _beforeState;
        }
        else if (_state == MainSceneState.MoveChat)
        {
            if (_touchPosRatio.x <= -ChangeMenuMoveRatioX)
                return _isShowingAction ? MainSceneState.ShowAction : MainSceneState.ShowMain;
            if (_touchPosRatio.x >= ChangeMenuMoveRatioX)
                return MainSceneState.ShowChat;
            return _beforeState;
        }
        else if (_state == MainSceneState.MoveAction)
        {
            if (_touchPosRatio.y <= -ChangeMenuMoveRatioY)
                return MainSceneState.ShowMain;
            if (_touchPosRatio.y >= ChangeMenuMoveRatioY)
                return MainSceneState.ShowAction;
            return _beforeState;
        }
        else
        {
            var touchPos = TouchInput.GetTouchPosition();

            if (_touchPosRatio.x >= ChangeStateMoveRatioX)
            {
                if (_state == MainSceneState.ShowMain || _state == MainSceneState.ShowAction)
                    return MainSceneState.MoveChat;
                else if (_state == MainSceneState.ShowGroup)
                    return MainSceneState.MoveGroup;
                else
                    _touchPosition.x = touchPos.x;
            }
            else if (_touchPosRatio.x <= -ChangeStateMoveRatioX)
            {
                if (_state == MainSceneState.ShowMain || _state == MainSceneState.ShowAction)
                    return MainSceneState.MoveGroup;
                else if (_state == MainSceneState.ShowChat)
                    return MainSceneState.MoveChat;
                else
                    _touchPosition.x = touchPos.x;
            }
            else if (_touchPosRatio.y >= ChangeStateMoveRatioY)
            {
                if (_state == MainSceneState.ShowMain)
                    return MainSceneState.MoveAction;
                else
                    _touchPosition.y = touchPos.y;
            }
            else if (_touchPosRatio.y <= -ChangeStateMoveRatioY)
            {
                if (_state == MainSceneState.ShowAction)
                    return MainSceneState.MoveAction;
                else
                    _touchPosition.y = touchPos.y;
            }
        }

        return _state;
    }
}

public enum MainSceneState
{
    ShowMain,
    ShowGroup,
    ShowChat,
    ShowAction,
    MoveGroup,
    MoveChat,
    MoveAction
}