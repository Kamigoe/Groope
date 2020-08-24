using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GroupMenuManager : MonoBehaviour
{
    public static GroupMenuManager Instance;

    [SerializeField] private GameObject _groupButtonPrefab = default;
    [SerializeField] private Button _configButton = default;

    [SerializeField] private Button _showHideButton_Group = default;
    [SerializeField] private Button _showHideButton_Friend = default;

    [SerializeField] private Text _groupListName = default;
    [SerializeField] private Text _friendListName = default;

    [SerializeField] private RectTransform _groupListParent = default;
    [SerializeField] private RectTransform _friendListParent = default;
    [SerializeField] private RectTransform _listMargin = default;

    private bool _visibleGroupList = default;
    private bool _visibleFriendList = default;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _visibleFriendList = false;
        _visibleGroupList = false;

        _configButton.onClick.AddListener(() =>
        {
            Debug.Log("こんふぐ");
        });
        
        _showHideButton_Group.onClick.AddListener(() =>
        {
            var buttonRect = _showHideButton_Group.GetComponent<RectTransform>();
            var element = _groupListParent.GetComponent<LayoutElement>();
            if (!_visibleGroupList)
            {
                buttonRect.DOLocalRotate(new Vector3(0, 0, 180), 1f / 6f, RotateMode.FastBeyond360).SetRelative()
                    .OnComplete(() =>
                    {
                        buttonRect.DOLocalMoveY(5, 1f / 3f).SetRelative();
                    });

                DOVirtual.Float(0, 90, 0.5f, value =>
                {
                    _listMargin.sizeDelta = new Vector2(375, value);
                }).SetEase(Ease.OutSine);

                DOVirtual.Float(0, 90 * _groupListParent.childCount, 0.5f, value =>
                {
                    _groupListParent.sizeDelta = new Vector2(375, value);
                    element.preferredHeight = value;
                });
            }
            else
            {
                buttonRect.DOLocalRotate(new Vector3(0, 0, -180), 1f / 6f, RotateMode.FastBeyond360).SetRelative()
                    .OnComplete(() =>
                    {
                        buttonRect.DOLocalMoveY(-5, 1f / 3f).SetRelative();
                    });

                DOVirtual.Float(90, 0, 0.5f, value =>
                {
                    _listMargin.sizeDelta = new Vector2(375, value);
                }).SetEase(Ease.InSine);

                DOVirtual.Float(90 * _groupListParent.childCount, 0, 0.5f, value =>
                {
                    _groupListParent.sizeDelta = new Vector2(375, value);
                    element.preferredHeight = value;
                });
            }

            _visibleGroupList = !_visibleGroupList;
        });
        
        DebugFunc();
    }

    private void Update()
    {
        _groupListName.text = "グループ(" + _groupListParent.childCount + ")";
        _friendListName.text = "フレンド(" + _friendListParent.childCount + ")";
    }

    private void DebugFunc()
    {
        for (var i = 0; i < 20; i++)
        {
            var obj = Instantiate(_groupButtonPrefab, _groupListParent, true);
        }
    }
}
