using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kakera;
using MessagePack;
using MessagePack.Resolvers;

public class TestChat : MonoBehaviour
{
    private const int MSG_MAX_LENGTH = 10000;

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
    private List<TestReceiveData> receiveDataList;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitMessagePack()
    {
        StaticCompositeResolver.Instance.Register( new IFormatterResolver[]
        {
            GeneratedResolver.Instance, 
            StandardResolver.Instance
        });

        var option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

        MessagePackSerializer.DefaultOptions = option;
    }

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    static void EditorInitMessagePack()
    {
        InitMessagePack();
    }
#endif

    void Start()
    {
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
            var pack = new TexturePack {data = TestUtils.LoadFile(path)};
            SendData(MessageSendType.Image, TestUtils.SerializeMessage(pack));
        };
        
        _connectRoomButton.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(_inputRoomName.text))
            {
                _start.SetActive(false);
                _connect.SetActive(true);
                VoiceChat.Instance.Connect(_inputRoomName.text);
            }
        });
        
        VoiceChat.Instance.AddReceiveEvent(ReceiveInfo);

        receiveDataList = new List<TestReceiveData>();

        _start.SetActive(true);
        _stay.SetActive(false);
        _connect.SetActive(false);
        _complete.SetActive(false);
        _disconnect.SetActive(false);
    }

    void Update()
    {
        if (!_isConnect && PhotonNetwork.inRoom)
        {
            _isConnect = true;
            _complete.SetActive(true);
            _connect.SetActive(false);
            _stay.SetActive(false);
        }

        if (_isConnect && !PhotonNetwork.inRoom)
        {
            _disconnect.SetActive(true);
            _complete.SetActive(false);
            DisConnect();
        }
    }

    void SendData(MessageSendType type, byte[] data)
    {
        var msgID = "hiodajhoiap641hui1";
        var msgDataList = new List<List<byte>>();
        var msgData = new List<byte>();

        var length = 0;
        foreach (var bytedata in data)
        {
            length++;

            msgData.Add(bytedata);

            if (length < MSG_MAX_LENGTH) continue;
            msgDataList.Add(msgData);
            msgData = new List<byte>();
            length = 0;
        }

        if (length != 0)
            msgDataList.Add(msgData);
        
        Debug.Log("SendCount :" + msgDataList.Count);
        for (var i = 0; i < msgDataList.Count; i++)
        {
            var sendData = new TestSendData();
            sendData.msgID = msgID;
            sendData.type = type;
            sendData.msgCnt = msgDataList.Count;
            sendData.msgNo = i + 1;
            sendData.value = msgDataList[i].ToArray();
            
            VoiceChat.Instance.Send(sendData);
        }
    }

    private void ReceiveInfo (TestSendData data)
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
        if (_isConnect)
            VoiceChat.Instance.Disconnect();

        _isConnect = false;
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