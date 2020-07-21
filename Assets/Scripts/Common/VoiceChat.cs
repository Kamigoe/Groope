using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceChat : MonoBehaviour
{
    public static VoiceChat instance;
    public static PhotonSend send = null;

    // Photonプレハブのパス
    private string _path = "PlayerVoice";
    private string _roomName = "room";

    void Awake () { instance = this; }

    void OnJoinedLobby() {
        RoomOptions options = new RoomOptions() {
            isVisible = false,
            maxPlayers = 4
        };
        PhotonNetwork.JoinOrCreateRoom("room",options,TypedLobby.Default);
        
        Debug.Log ("JoinLobby");
    }

    void OnJoinedRoom() {
        GameObject obj = PhotonNetwork.Instantiate(
            _path,
            Vector3.zero,
            Quaternion.identity,
            0
        );

        send = obj.GetComponent<PhotonSend> ();
        send.Initialize ();
        
        Debug.Log ("JoinRoom");
    }

    public void Connect (string roomName = "room")
    {
        if (!string.IsNullOrEmpty (roomName))
            _roomName = roomName;
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public void Disconnect ()
    {
        send.End ();
        PhotonNetwork.Disconnect ();

        Destroy (this.gameObject);
    }
}
