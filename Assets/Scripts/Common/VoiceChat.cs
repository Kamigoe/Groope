using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceChat : MonoBehaviour
{
    public static VoiceChat instance;
    public static PhotonSend send = null;
    private RectTransform _parent = null;

    // Photonプレハブのパス
    private string _path = "PlayerVoice";
    private string _roomName = "room";

    void Awake () { instance = this; }

    void OnJoinedLobby() {
        RoomOptions options = new RoomOptions();
        PhotonNetwork.JoinOrCreateRoom(_roomName,options,TypedLobby.Default);

        Debug.Log ("JoinLobby");
    }

    void OnJoinedRoom() {
        GameObject obj = PhotonNetwork.Instantiate(
            _path,
            Vector3.zero,
            Quaternion.identity,
            0
        );
        
        obj.transform.SetParent(_parent);

        send = obj.GetComponent<PhotonSend> ();
        send.Initialize ();
        
        Debug.Log ("JoinRoom");
    }

    public void Connect (string roomName = "room")
    {
        if (!string.IsNullOrEmpty (roomName))
            _roomName = roomName;
        PhotonNetwork.AuthValues = new AuthenticationValues(NCMBController.GetUserID());
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public void Disconnect ()
    {
        send.End ();
        PhotonNetwork.Disconnect ();

        Destroy (this.gameObject);
    }

    public void SetPlayerParent(RectTransform parent)
    {
        _parent = parent;
    }
}
