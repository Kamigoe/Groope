using System.Security.Cryptography;

public static class CommonUtils
{
    public static string SHA256(string key, string data)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] planeBytes = ue.GetBytes(data);
        byte[] keyBytes = ue.GetBytes(key);
	 
        HMACSHA256 sha256 = new HMACSHA256(keyBytes);
        byte[] hashBytes = sha256.ComputeHash(planeBytes);
        string hashStr = "";
        foreach(byte b in hashBytes) {
            hashStr += string.Format("{0,0:x2}", b);
        }
        return hashStr;
    }
}