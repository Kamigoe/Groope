using System.Linq;
using System.Collections.Generic;
using MessagePack;

public class TestSendData {
    public string msgID;
    public int msgCnt;
    public int msgNo;
    public MessageSendType type;
    public byte[] value;
}

public class TestReceiveData{
    public string msgID;
    public int msgCnt;
    public int msgAddCount;
    public byte[] value;
    public bool complete;
    private Dictionary<int, byte[]> buffer;

    public TestReceiveData(string msgID, int msgCnt)
    {
        this.msgID = msgID;
        this.msgCnt = msgCnt;
        complete = false;
        msgAddCount = 0;
        buffer = new Dictionary<int, byte[]>();
    }

    public void AddBuffer(int msgNo, byte[] value)
    {
        if (!buffer.ContainsKey(msgNo))
        {
            buffer.Add(msgNo, value);
            msgAddCount++;
        }

        buffer.OrderBy(x => x.Key);
        if (msgAddCount == msgCnt)
        {
            List<byte> compData = new List<byte>();
            foreach(byte[] byteData in buffer.Values)
                compData.AddRange(byteData);

            this.value = compData.ToArray();
            complete = true;
        }
    }
}

[MessagePackObject]
public class TexturePack
{
    [Key(0)]
    public byte[] data;
}

public enum MessageSendType {
    Text,
    Image
}