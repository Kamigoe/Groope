using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Kakera;

public class TestChat : MonoBehaviour
{
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

    private bool _isConnect = false;
    private bool _isServer = false;

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

            TestSendData sendData = new TestSendData();
            sendData.type = MessageSendType.Text;
            sendData.value = data;

            if (_isServer)
                NetworkServer.SendToAll(sendData);
            else
                NetworkClient.Send(sendData);
        });

        _sendImageButton.onClick.AddListener(() =>
        {
            _unImgPicker.Show("選択", "sendimage", 1024);
        });
        _unImgPicker.Completed += (path) =>
        {
            SendImage(path);
        };

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
    
    private void ReceiveInfo (NetworkConnection connection, TestSendData data)
    {
        if (data.type == MessageSendType.Text)
            _giveMessage.text = System.Text.Encoding.UTF8.GetString(data.value);
        else if (data.type == MessageSendType.Image)
            ShowImage(data.value, 500);
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

    void SendImage(string path)
    {
        TestSendData sendData = new TestSendData();
        sendData.type = MessageSendType.Image;
        sendData.value = TestUtils.LoadFile(path);

        if (_isServer)
            NetworkServer.SendToAll(sendData);
        else
            NetworkClient.Send(sendData);
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