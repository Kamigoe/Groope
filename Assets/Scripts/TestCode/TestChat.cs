using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Kakera;

public class TestChat : MonoBehaviour
{
    private const int MSG_MAX_LENGTH = 10000;

    [SerializeField] private NetworkManager manager = default;

    [SerializeField] private InputField _inputIP = default;
    [SerializeField] private Button _stayButton = default;
    [SerializeField] private Button _connectButton = default;
    [SerializeField] private Button _disconnectButton = default;
    [SerializeField] private Button _backButton = default;

    [SerializeField] private InputField _messageInputFIeld = default;
    [SerializeField] private Button _sendMessageButton = default;
    [SerializeField] private Text _giveMessage = default;

    [SerializeField] private GameObject _start = default;
    [SerializeField] private GameObject _stay = default;
    [SerializeField] private GameObject _connect = default;
    [SerializeField] private GameObject _complete = default;
    [SerializeField] private GameObject _disconnect = default;

    [SerializeField] private Image _receiveImage = default;
    [SerializeField] private Button _sendImageButton = default;
    [SerializeField] private Unimgpicker _unImgPicker = default;

    [SerializeField] private InputField _inputRoomName = default;
    [SerializeField] private Button _connectRoomButton = default;

    private bool _isConnect = false;
    private bool _isServer = false;
    private List<TestReceiveData> receiveDataList;

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
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

            SendData(MessageSendType.Text, data);
        });

        _sendImageButton.onClick.AddListener(() =>
        {
            _unImgPicker.Show("選択", "sendimage", 1024);
        });
        _unImgPicker.Completed += (path) =>
        {
            TexturePack pack = new TexturePack();
            pack.data = TestUtils.LoadFile(path);
            SendData(MessageSendType.Image, TestUtils.SerializeMessage(pack));
        };
        
        _connectRoomButton.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(_inputRoomName.text))
                VoiceChat.Instance.Connect(_inputRoomName.text);
        });

        receiveDataList = new List<TestReceiveData>();

        _start.SetActive(true);
        _stay.SetActive(false);
        _connect.SetActive(false);
        _complete.SetActive(false);
        _disconnect.SetActive(false);
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

    void SendData(MessageSendType type, byte[] data)
    {
        string msgID = "hiodajhoiap641hui1";
        List<List<byte>> msgDataList = new List<List<byte>>();
        List<byte> msgData = new List<byte>();

        int length = 0;
        foreach (byte bytedata in data)
        {
            length++;

            msgData.Add(bytedata);

            if (length >= MSG_MAX_LENGTH)
            {
                msgDataList.Add(msgData);
                msgData = new List<byte>();
                length = 0;
            }
        }

        if (length != 0)
            msgDataList.Add(msgData);
        
        Debug.Log("SendCount :" + msgDataList.Count);
        for (int i = 0; i < msgDataList.Count; i++)
        {
            TestSendData sendData = new TestSendData();
            sendData.msgID = msgID;
            sendData.type = type;
            sendData.msgCnt = msgDataList.Count;
            sendData.msgNo = i + 1;
            sendData.value = msgDataList[i].ToArray();
            if (_isServer)
                NetworkServer.SendToAll(sendData);
            else
                NetworkClient.Send(sendData);
        }
    }

    private void ReceiveInfo (NetworkConnection connection, TestSendData data)
    {
        int index = receiveDataList.FindIndex(x => x.msgID == data.msgID);
        if (index == -1)
        {
            TestReceiveData receiveData = new TestReceiveData(data.msgID, data.msgCnt);
            index = receiveDataList.Count;
            receiveDataList.Add(receiveData);
        }

        receiveDataList[index].AddBuffer(data.msgNo, data.value);

        if (receiveDataList[index].complete)
        {
            if (data.type == MessageSendType.Text)
                _giveMessage.text = System.Text.Encoding.UTF8.GetString(receiveDataList[index].value);
            if (data.type == MessageSendType.Image)
            {
                byte[] texture = TestUtils.DeserializeMessage<TexturePack>(receiveDataList[index].value).data;
                ShowImage(texture, 500);
            }

            receiveDataList.RemoveAt(index);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        DisConnect();
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

    void ShowImage (Texture2D texture, float showSize)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));
        _receiveImage.sprite = sprite;
        _receiveImage.SetNativeSize();

        Vector2 size = _receiveImage.GetComponent<RectTransform>().sizeDelta;

        if (size.x > showSize)
        {
            float ratio = size.x / showSize;
            size.x /= ratio;
            size.y /= ratio;
        }

        if (size.y > showSize)
        {
            float ratio = size.y / showSize;
            size.x /= ratio;
            size.y /= ratio;
        }

        _receiveImage.GetComponent<RectTransform>().sizeDelta = size;
    }

    void ShowImage (byte[] texture, float showSize)
    {
        Texture2D tex = TestUtils.ByteArrayToTexture2D(texture);
        ShowImage(tex, showSize);
    }
}