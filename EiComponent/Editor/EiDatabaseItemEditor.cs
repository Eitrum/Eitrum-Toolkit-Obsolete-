using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum
{
	[CustomPropertyDrawer (typeof(EiDatabaseItem))]
	public class EiDatabaseItemEditor : PropertyDrawer
	{

		static string path = "Assets/Eitrum/EiComponent/Database/EiDatabase.prefab";

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			List<string> items = new List<string> ();
			List<EiDatabaseItem> references = new List<EiDatabaseItem> ();
			items.Add ("None");
			references.Add (null);

			var go = AssetDatabase.LoadAssetAtPath<GameObject> (path);
			if (!go) {
				var tempObj = new GameObject ("EiDatabase", typeof(EiDatabaseResource));
				go = PrefabUtility.CreatePrefab (path, tempObj);
				UnityEngine.MonoBehaviour.DestroyImmediate (tempObj);
			}
			EiDatabaseResource database = go.GetComponent<EiDatabaseResource> ();
			var currentSelectedObject = property.objectReferenceValue;
			var index = 0;

			var categories = database._Length;
			for (int i = 0; i < categories; i++) {
				var category = database [i];
				var entries = category.Length;
				for (int e = 0; e < entries; e++) {
					var entry = category [e];
					if (entry.Item && entry.Item.name != entry.ItemName) {
						entry.GetType ().GetField ("itemName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue (entry, entry.Item.name);
					}
					string path = string.Format ("{0} / {1}", category.CategoryName, entry.ItemName);
					if (entry == currentSelectedObject) {
						index = items.Count;
					}
					int iterations = 0;
					while (items.Contains (path)) {
						path = string.Format ("{0} / {1} ({2})", category.CategoryName, entry.ItemName, iterations++);
					}
					items.Add (path);
					references.Add (entry);
				}
			}

			property.objectReferenceValue = references [EditorGUI.Popup (position, property.displayName, index, items.ToArray ())];
		}
	}
}