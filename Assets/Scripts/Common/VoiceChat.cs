using UnityEngine;

public class VoiceChat : MonoBehaviour
{
    // Photonプレハブのパス
    private string _prefabPath = "PlayerVoice";

    void Start ()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void OnJoinedLobby() {
        RoomOptions options = new RoomOptions() {
            isVisible = false
        };
        PhotonNetwork.JoinOrCreateRoom("room", options, TypedLobby.Default);
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
