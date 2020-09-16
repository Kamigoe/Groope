using Mirror;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TestSendData:MessageBase {
    public MessageSendType type;
    public byte[] value;
}

public enum MessageSendType {
    Text,
    Image
}