using System;
using System.Collections;
using System.Collections.Generic;
using Kakera;
using UnityEngine;
using UnityEngine.UI;

public class ChatMenuManager : MonoBehaviour
{
    public static ChatMenuManager Instance;

    [SerializeField] private InputField _inputField = default;
    [SerializeField] private Button _sendButton = default;
    [SerializeField] private Button _contentButton = default;
    [SerializeField] private RectTransform _messageParent = default;

    [SerializeField] private GameObject _messagePrefab = default;

    [SerializeField] private Unimgpicker _picker = default;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _sendButton.onClick.AddListener(() =>
        {
            var message = Instantiate(_messagePrefab, _messageParent, true);
            message.transform.localScale = Vector3.one;
            message.GetComponent<Message>().Initialize(null, "01/23\n04:56",_inputField.text);
            _inputField.text = null;
        });
        
        _contentButton.onClick.AddListener(() =>
        {
            _picker.Show("選択", "image", 2048);
        });

        _picker.Completed += path =>
        {
            CommonUtils.LoadImage(path, sprite =>
            {
                var message = Instantiate(_messagePrefab, _messageParent, true);
                message.transform.localScale = Vector3.one;
                message.GetComponent<Message>().Initialize(null, "01/23\n04:56", sprite);
            });
        };
    }

    private void Update()
    {
        _sendButton.interactable = !string.IsNullOrEmpty(_inputField.text);
    }
}
