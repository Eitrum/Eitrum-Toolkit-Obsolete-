using System;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Eitrum
{
	public class EiSOCreator
	{
		public static void CreateAsset<T> (T asset, string path) where T : ScriptableObject
		{
			#if UNITY_EDITOR
			var longPath = Application.dataPath + "\\" + path;
			if (!Directory.Exists (longPath)) {
				Directory.CreateDirectory (longPath);
				AssetDatabase.Refresh ();
			}
			AssetDatabase.CreateAsset (asset, AssetDatabase.GenerateUniqueAssetPath (string.Format ("{0}/{1}.asset", "Assets/" + path, typeof(T).Name)));
			AssetDatabase.SaveAssets ();
			#endif
		}

		public static void CreateAsset<T> (T asset) where T : ScriptableObject
		{
			#if UNITY_EDITOR
			string path = AssetDatabase.GetAssetPath (Selection.activeObject);
			if (path == "") {
				path = "Assets";
			} else if (Path.GetExtension (path) != "") {
				path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
			}
			AssetDatabase.CreateAsset (asset, AssetDatabase.GenerateUniqueAssetPath (string.Format ("{0}/New {1}.asset", path, typeof(T).Name)));
			AssetDatabase.SaveAssets ();
			#endif
		}

		public static void DestroyAsset<T> (T asset) where T: ScriptableObject
		{
			#if UNITY_EDITOR
			string path = AssetDatabase.GetAssetPath (asset);
            if (path == "")
                return;
			AssetDatabase.DeleteAsset (path);
			AssetDatabase.SaveAssets ();
			#endif
		}
	}
}