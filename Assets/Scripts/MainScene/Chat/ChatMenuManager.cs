using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMenuManager : MonoBehaviour
{
    public static ChatMenuManager Instance;

    [SerializeField] private InputField _inputFIeld = default;
    [SerializeField] private Button _sendButton = default;
    [SerializeField] private Button _contentButton = default;
    [SerializeField] private RectTransform _messageParent = default;

    [SerializeField] private GameObject _messagePrefab = default;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _sendButton.onClick.AddListener(() =>
        {
            var message = Instantiate(_messagePrefab, _messageParent, true);
            message.GetComponent<Message>().Initialize(null, "01/23\n04:56",_inputFIeld.text);
        });
        
        _contentButton.onClick.AddListener(() =>
        {
            
        });
    }

    private void Update()
    {
        _sendButton.interactable = !string.IsNullOrEmpty(_inputFIeld.text);
    }
}
