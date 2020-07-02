using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class TestChat : MonoBehaviour
{
    [SerializeField] private NetworkManager manager = default;

    [SerializeField] private InputField _inputIP = default;
    [SerializeField] private Button _stayButton = default;
    [SerializeField] private Button _connectButton = default;
    [SerializeField] private Button _disconnectButton = default;
    [SerializeField] private Button _backButton = default;
    [SerializeField] private Text _descriptionInfo = default;

    [SerializeField] private InputField _messageInputFIeld = default;
    [SerializeField] private Button _sendMessageButton = default;
    [SerializeField] private Text _giveMessage = default;

    [SerializeField] private GameObject _start = default;
    [SerializeField] private GameObject _stay = default;
    [SerializeField] private GameObject _connect = default;
    [SerializeField] private GameObject _complete = default;
    [SerializeField] private GameObject _disconnect = default;

    private bool _isConnect = false;
    private bool _isServer = false;
    private ConnectType _type = ConnectType.None;

    void Start()
    {
        _stayButton.onClick.AddListener(() =>
        {
            _stay.SetActive(true);
            _start.SetActive(false);
            _isServer = true;

            string ipAdress = _inputIP.text;

            manager.networkAddress = ipAdress;
            manager.StartHost();
            NetworkServer.RegisterHandler<TestSendData>(ReceiveInfo);
        });

        _connectButton.onClick.AddListener(() =>
        {
            _connect.SetActive(true);
            _start.SetActive(false);
            
            string ipAdress = _inputIP.text;

            manager.networkAddress = ipAdress;
            manager.StartClient();
            NetworkClient.RegisterHandler<TestSendData>(ReceiveInfo);
        });

        _disconnectButton.onClick.AddListener(() =>
        {
            _disconnect.SetActive(true);
            _complete.SetActive(false);
            DisConnect();
        });

        _backButton.onClick.AddListener(()=>
        {
            _start.SetActive(true);
            _disconnect.SetActive(false);
        });

        _sendMessageButton.onClick.AddListener(() =>
        {
            string message = _messageInputFIeld.text;

            TestSendData data = new TestSendData();
            data.value = message;
            data.type = MessageType.Text;

            if (_isServer)
                NetworkServer.SendToAll(data);
            else
                NetworkClient.Send(data);
        });

        _start.SetActive(true);
    }

    void Update()
    {
        if (!_isConnect && NetworkClient.isConnected)
        {
            _isConnect = true;
            _complete.SetActive(true);
            _connect.SetActive(false);
            _stay.SetActive(false);
        }

        if (_isConnect && !NetworkClient.isConnected)
        {
            _disconnect.SetActive(true);
            _complete.SetActive(false);
            DisConnect();
        }
    }

    private void ReceiveInfo (NetworkConnection connection, TestSendData data)
    {
        if (data.type == MessageType.Text)
            _giveMessage.text = data.value;
    }

    protected virtual void OnApplicationQuit()
    {
        DisConnect();
    }

    bool IsConnect()
    {
        if (_isServer)
            return NetworkServer.connections.Count != 0;
        
        return NetworkClient.isConnected;
    }

    void DisConnect()
    {
        if (NetworkClient.isConnected)
        {
            if (NetworkServer.active)
                manager.StopHost();
            else
                manager.StopClient();
        }

        _isConnect = false;
        _isServer = false;
    }
}

public enum ConnectType
{
    Host,
    Client,
    Server,
    None
}