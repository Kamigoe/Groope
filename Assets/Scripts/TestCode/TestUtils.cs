using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using MessagePack;

public static class TestUtils
{
    public static byte[] Texture2DToByteArray(Texture2D tex)
    {
        Texture2DPack texPack = new Texture2DPack();
        texPack.rawData = tex.GetRawTextureData();
        texPack.width = tex.width;
        texPack.height = tex.height;
        texPack.format = (int) tex.format;
        texPack.flags = tex.mipmapCount > 1;

        return MessagePackSerializer.Serialize(texPack);
    }

    public static Texture2D ByteArrayToTexture2D (byte[] tex)
    {
        Texture2DPack texPack = MessagePackSerializer.Deserialize<Texture2DPack>(tex);
        Texture2D texture = new Texture2D(texPack.width, texPack.height, (TextureFormat) texPack.format, texPack.flags);
        texture.LoadRawTextureData(texPack.rawData);
        texture.Apply();

        return texture;
    }
}


[MessagePackObject]
public class Texture2DPack
{
    [Key(0)]
    public byte[] rawData;
    [Key(1)]
    public int width;
    [Key(2)]
    public int height;
    [Key(3)]
    public int format;
    [Key(4)]
    public bool flags;
}