using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonSend : MonoBehaviour
{
    private List<TestSendData> _dataBuffer;

    private IEnumerator coroutine;

    public void Initialize ()
    {
        _dataBuffer = new List<TestSendData> ();
        coroutine = SendCoroutine ();
        StartCoroutine (coroutine);
    }

    public void End ()
    {
        Debug.Log ("PtotonSend Destractor");
        StopCoroutine (coroutine);
    }

    public void SendData (TestSendData data)
    {
        _dataBuffer.Insert (0, data);
    }

    public void Send (TestSendData data)
    {
        string msgID = data.msgID;
        int type = (int) data.type;
        int cnt = data.msgCnt;
        int msgNo = data.msgNo;
        
        PhotonView.Get(this).RPC ("ReceiveData", PhotonTargets.All, msgID, type, cnt, msgNo, data.value);
    }

    [PunRPC]
    void ReceiveData (string msgID, int type, int count, int no, byte[] value)
    {
        TestSendData sendData = new TestSendData ();
        sendData.msgID = msgID;
        sendData.type = (MessageSendType) type;
        sendData.msgCnt = count;
        sendData.msgNo = no;
        sendData.value = value;
        TestChat.instance.ReceiveInfo (sendData);
    }

    IEnumerator SendCoroutine ()
    {
        while (true)
        {
            int cnt = _dataBuffer.Count - 1;
            for (int i = cnt; i >= 0 && i >= cnt - 10; i--)
            {
                Send (_dataBuffer[i]);
                _dataBuffer.RemoveAt (i);
            }

            yield return new WaitForSeconds (0.5f);
        }
    }
}
