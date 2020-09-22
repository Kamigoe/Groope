using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhotonSend : MonoBehaviour
{
    private List<TestSendData> _dataBuffer = new List<TestSendData> ();
    private IEnumerator _coroutine;

    public void Initialize ()
    {
        _dataBuffer = new List<TestSendData> ();
        _coroutine = SendCoroutine ();
        StartCoroutine (_coroutine);
    }

    public void End ()
    {
        StopCoroutine (_coroutine);
    }

    public void Send (TestSendData data)
    {
        _dataBuffer.Insert (0, data);
    }

    private void SendData (TestSendData data)
    {
        var msgId = data.msgID;
        var type = (int) data.type;
        var cnt = data.msgCnt;
        var msgNo = data.msgNo;
        
        PhotonView.Get(this).RPC ("ReceiveData", PhotonTargets.Others, msgId, type, cnt, msgNo, data.value);
    }

    [PunRPC]
    private void ReceiveData (string msgId, int type, int count, int no, byte[] value)
    {
        var sendData = new TestSendData
        {
            msgID = msgId,
            type = (MessageSendType) type,
            msgCnt = count,
            msgNo = no,
            value = value
        };
        foreach (var callback in VoiceChat.Instance.GetReceiveEvent())
        {
            callback.Invoke(sendData);
        }
    }

    private IEnumerator SendCoroutine ()
    {
        while (true)
        {
            var cnt = _dataBuffer.Count - 1;
            for (var i = cnt; i >= 0 && i >= cnt - 10; i--)
            {
                SendData (_dataBuffer[i]);
                _dataBuffer.RemoveAt (i);
            }

            yield return new WaitForSeconds (0.5f);
        }
    }
}