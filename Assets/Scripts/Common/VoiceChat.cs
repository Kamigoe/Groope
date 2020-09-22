using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoiceChat : MonoBehaviour
{
    public static VoiceChat Instance = null;
    
    private List<UnityAction<TestSendData>> _callBackList;
    
    // Photonプレハブのパス
    private string _prefabPath = "PlayerVoice";
    private string _roomName = default;
    private PhotonSend _photonSend = null;

    void Awake ()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        
        _callBackList = new List<UnityAction<TestSendData>>();
    }

    public void Connect(string roomName = "room")
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
        _roomName = roomName;
        _photonSend = null;
    }
    
    public void Disconnect()
    {
        _photonSend.End();
        PhotonNetwork.Disconnect();
    }

    public void Send(TestSendData data)
    {
        _photonSend?.Send(data);
    }

    public void AddReceiveEvent(UnityAction<TestSendData> receiveEvent)
    {
        _callBackList.Add(receiveEvent);
    }

    public List<UnityAction<TestSendData>> GetReceiveEvent()
    {
        return _callBackList;
    }

    void OnJoinedLobby() {
        RoomOptions options = new RoomOptions() {
            IsVisible = false
        };
        PhotonNetwork.JoinOrCreateRoom(_roomName, options, TypedLobby.Default);
    }

    void OnJoinedRoom() {
        var player = PhotonNetwork.Instantiate(
            _prefabPath,
            Vector3.zero,
            Quaternion.identity,
            0
        );
        
        _photonSend = player.GetComponent<PhotonSend>();
        _photonSend.Initialize();
    }
}
