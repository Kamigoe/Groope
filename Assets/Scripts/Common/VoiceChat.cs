using UnityEngine;

public class VoiceChat : MonoBehaviour
{
    public static VoiceChat Instance = null;
    
    // Photonプレハブのパス
    private string _prefabPath = "PlayerVoice";

    private string _roomName = default;

    void Awake ()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void Connect(string roomName = "room")
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
        _roomName = roomName;
    }

    void OnJoinedLobby() {
        RoomOptions options = new RoomOptions() {
            isVisible = false
        };
        PhotonNetwork.JoinOrCreateRoom(_roomName, options, TypedLobby.Default);
    }

    void OnJoinedRoom() {
        PhotonNetwork.Instantiate(
            _prefabPath,
            Vector3.zero,
            Quaternion.identity,
            0
        );
    }
}
