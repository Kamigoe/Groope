using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using Kakera;
using MessagePack;
using MessagePack.Resolvers;

public class TestChat : MonoBehaviour
{
    public static TestChat instance = null;

    private const int MSG_MAX_LENGTH = 10000;

    [SerializeField] private InputField _inputIP = default;
    [SerializeField] private Button _connectButton = default;
    [SerializeField] private Button _disconnectButton = default;
    [SerializeField] private Button _backButton = default;
    [SerializeField] private Text _descriptionInfo = default;

    [SerializeField] private InputField _messageInputFIeld = default;
    [SerializeField] private Button _sendMessageButton = default;
    [SerializeField] private Text _giveMessage = default;

    [SerializeField] private GameObject _start = default;
    [SerializeField] private GameObject _connect = default;
    [SerializeField] private GameObject _complete = default;
    [SerializeField] private GameObject _disconnect = default;

    [SerializeField] private Image _sendImage = default;
    [SerializeField] private Image _receiveImage = default;
    [SerializeField] private Button _sendImageButton = default;
    [SerializeField] private Unimgpicker _unImgPicker = default;
    [SerializeField] private GameObject _voiceChat = default;

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
        instance = this;

        _connectButton.onClick.AddListener(() =>
        {
            _connect.SetActive(true);
            _start.SetActive(false);

            Instantiate (_voiceChat);
            VoiceChat.instance.Connect (_inputIP.text);
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
            StartCoroutine(LoadImage(path, _sendImage));
        };

        receiveDataList = new List<TestReceiveData>();

        _start.SetActive(true);
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
        }
        if (_isConnect && !PhotonNetwork.inRoom)
        {
            _disconnect.SetActive(true);
            _complete.SetActive(false);
            DisConnect();
        }
    }

    List<TestSendData> SendData(MessageSendType type, byte[] data)
    {
        string msgID = "hiodajhoiap641hui1";
        List<TestSendData> sendingData = new List<TestSendData>();
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

            VoiceChat.send.SendData(sendData);
            
            sendingData.Add(sendData);
        }

        return sendingData;
    }

    public void ReceiveInfo (TestSendData data)
    {
        int index = receiveDataList.FindIndex(x => x.msgID == data.msgID);
        if (index == -1)
        {
            TestReceiveData receiveData = new TestReceiveData(data.msgID, data.msgCnt);
            index = receiveDataList.Count;
            receiveDataList.Add(receiveData);
            Debug.Log("新規メッセージ");
        }

        receiveDataList[index].AddBuffer(data.msgNo, data.value);
        Debug.Log("復元中... " +  receiveDataList[index].msgAddCount + "/" + data.msgCnt);

        if (receiveDataList[index].complete)
        {
            if (data.type == MessageSendType.Text)
                _giveMessage.text = System.Text.Encoding.UTF8.GetString(receiveDataList[index].value);
            if (data.type == MessageSendType.Image)
            {
                Debug.Log("画像復元");
                Debug.Log ("ID :" + data.msgID + " type :" + data.type);
                Debug.Log (data.value.Length);
                ShowImage(receiveDataList[index].value, _receiveImage, 500);
            }

            receiveDataList.RemoveAt(index);
        }
    }

    private void TestReceiveInfo (TestSendData data)
    {
        int index = receiveDataList.FindIndex(x => x.msgID == data.msgID);
        if (index == -1)
        {
            TestReceiveData receiveData = new TestReceiveData(data.msgID, data.msgCnt);
            index = receiveDataList.Count;
            receiveDataList.Add(receiveData);
            Debug.Log("新規メッセージ");
        }

        Debug.Log("データ情報");
        Debug.Log("ID : " + data.msgID);
        Debug.Log("メッセージ番号 : " + data.msgNo);

        receiveDataList[index].AddBuffer(data.msgNo, data.value);
        Debug.Log("復元中... " +  receiveDataList[index].msgAddCount + "/" + data.msgCnt);

        if (receiveDataList[index].complete)
        {
            if (data.type == MessageSendType.Text)
                _giveMessage.text = System.Text.Encoding.UTF8.GetString(receiveDataList[index].value);
            if (data.type == MessageSendType.Image)
            {
                Debug.Log("画像復元");
                ShowImage(receiveDataList[index].value, _receiveImage, 500);
            }

            receiveDataList.RemoveAt(index);
        }   
    }

    protected virtual void OnApplicationQuit()
    {
        DisConnect();
    }

    bool IsConnect()
    {
        return PhotonNetwork.connected;
    }

    void DisConnect()
    {
        if (_isConnect)
            VoiceChat.instance.Disconnect ();

        _isConnect = false;
    }

    IEnumerator LoadImage (string path, Image image)
    {
        string url = "file://" + path;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
            Debug.Log(www.error);
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            ShowImage(texture, _sendImage, 100);
            byte[] data = TestUtils.Texture2DToByteArray(texture);
            List<TestSendData> sendDataList =  SendData(MessageSendType.Image, data);

            foreach (TestSendData receiveData in sendDataList)
            {
                TestReceiveInfo(receiveData);
            }
        }
    }

    void ShowImage (Texture2D texture, Image image, float showSize)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));
        image.sprite = sprite;
        image.SetNativeSize();

        Vector2 size = image.GetComponent<RectTransform>().sizeDelta;

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

        image.GetComponent<RectTransform>().sizeDelta = size;
    }

    void ShowImage (byte[] texture, Image image, float showSize)
    {
        Texture2D tex = TestUtils.ByteArrayToTexture2D(texture);
        ShowImage(tex, image, showSize);
    }
}
