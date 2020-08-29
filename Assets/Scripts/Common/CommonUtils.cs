using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public static class CommonUtils
{
    public static string SHA256(string key, string data)
    {
        var ue = new System.Text.UTF8Encoding();
        var planeBytes = ue.GetBytes(data);
        var keyBytes = ue.GetBytes(key);
	 
        var sha256 = new HMACSHA256(keyBytes);
        var hashBytes = sha256.ComputeHash(planeBytes);
        return hashBytes.Aggregate("", (current, b) => current + $"{b,0:x2}");
    }

    public static void LoadImage(string path, UnityAction<Sprite> onComplete)
    {
        var enumerator = LoadImageASync(path, onComplete);
        CoroutineManager.instance.StartCoroutine(enumerator);
    }

    private static IEnumerator LoadImageASync(string path, UnityAction<Sprite> onComplete)
    {
        var url = "file://" + path;
        var www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
            Debug.Log(www.error);
        else
        {
            var texture = DownloadHandlerTexture.GetContent(www);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));
            onComplete(sprite);
        }
    }
}