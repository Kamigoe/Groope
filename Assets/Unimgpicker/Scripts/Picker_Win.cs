#if UNITY_STANDALONE_WIN
using UnityEngine;
using System.IO;
using SFB;

namespace Kakera
{
	internal class Picker_Win : IPicker
	{
        private static string _filePath = "";
		public void Show(string title, string outputFileName, int maxSize)
		{            
			var path = StandaloneFileBrowser.OpenFilePanel(title, "","png", false);
			if (path.Length != 0) {
				string destination = Application.persistentDataPath + "/" + outputFileName;
				if (File.Exists(destination))
					File.Delete(destination);
				File.Copy(path[0], destination);
				Debug.Log ("PickerOSX:" + destination);
				var receiver = GameObject.Find("Unimgpicker");
				if (receiver != null)
				{
					receiver.SendMessage("OnComplete", Application.persistentDataPath + "/" + outputFileName);
				}
			}
		}
	}
}
#endif