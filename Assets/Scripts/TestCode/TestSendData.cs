using Mirror;

[System.Serializable]
public class TestSendData:MessageBase {
    public MessageType type;
    public string value;
}

public enum MessageType {
    Connection,
    Text,
    Image,
    Voice
}