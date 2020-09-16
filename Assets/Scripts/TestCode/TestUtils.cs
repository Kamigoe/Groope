using UnityEngine;
using System.IO;

public static class TestUtils
{
    public static Texture2D ByteArrayToTexture2D(byte[] tex)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(tex);

        return texture;
    }
    public static byte[] LoadFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

        bin.Close();

        return values;
    }
}