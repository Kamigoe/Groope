using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenuManager : MonoBehaviour
{
    public static ActionMenuManager Instance;

    [SerializeField] private Button _addTeamButton = default;
    [SerializeField] private Button _removeTeamButton = default;

    [SerializeField] private Button _cameraButton = default;
    [SerializeField] private Sprite _cameraOn = default;
    [SerializeField] private Sprite _cameraOff = default;
    
    [SerializeField] private Button _micButton = default;
    [SerializeField] private Sprite _micOn = default;
    [SerializeField] private Sprite _micOff = default;

    [SerializeField] private Button _talkEndButton = default;
    [SerializeField] private Button _talkStartButton = default;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _addTeamButton.onClick.AddListener(() =>
        {
            // TODO チーム追加
        });
        _removeTeamButton.onClick.AddListener(() =>
        {
            // TODO チーム削除
        });
        
        _cameraButton.onClick.AddListener(() =>
        {
            // TODO カメラオンオフ切り替え
            AppData.cameraActive = !AppData.cameraActive;
            _cameraButton.GetComponent<Image>().sprite = AppData.cameraActive ? _cameraOn : _cameraOff;
        });
        
        _micButton.onClick.AddListener(() =>
        {
            // TODO マイクオンオフ切り替え
            AppData.micActive = !AppData.micActive;
            _micButton.GetComponent<Image>().sprite = AppData.micActive ? _micOn : _micOff;
        });
        
        _talkEndButton.onClick.AddListener(() =>
        {
            // TODO 通話終了
            AppData.isTalking = false;
        });
        
        _talkStartButton.onClick.AddListener(() =>
        {
            // TODO 通話開始
            AppData.isTalking = true;
        });
        
        _cameraButton.GetComponent<Image>().sprite = AppData.cameraActive ? _cameraOn : _cameraOff;
        _micButton.GetComponent<Image>().sprite = AppData.micActive ? _micOn : _micOff;
    }

    private void Update()
    {
        _talkEndButton.interactable = AppData.isTalking;
        _talkStartButton.interactable = !AppData.isTalking;
    }
}
