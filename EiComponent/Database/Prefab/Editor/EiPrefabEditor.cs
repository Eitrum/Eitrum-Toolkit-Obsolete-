using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Eitrum.Database.Prefab
{
	[CustomPropertyDrawer (typeof(EiPrefab))]
	public class EiPrefabEditor : PropertyDrawer
	{
		static string path = "Assets/Eitrum/Configuration/EiPrefabDatabase.prefab";
		static GameObject objectPicker = null;

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var attributes = fieldInfo.GetCustomAttributes (false);

			EiPrefabFilter filter = null;

			for (int i = 0; i < attributes.Length; i++) {
				if (attributes [i] is EiPrefabFilter) {
					filter = attributes [i] as EiPrefabFilter;
					break;
				}
			}

			if (Event.current.commandName == "ObjectSelectorClosed")
				objectPicker = EditorGUIUtility.GetObjectPickerObject () as GameObject;


			var currentSelectedObject = property.objectReferenceValue as EiPrefab;
			var index = 0;

			List<string> items = new List<string> ();
			List<EiPrefab> references = new List<EiPrefab> ();
			items.Add ("None");
			references.Add (null);

			var go = AssetDatabase.LoadAssetAtPath<GameObject> (path);
			if (!go) {
				var tempObj = new GameObject ("EiPrefabDatabase", typeof(EiPrefabDatabase));
				go = PrefabUtility.CreatePrefab (path, tempObj);
				UnityEngine.MonoBehaviour.DestroyImmediate (tempObj);
			}
			EiPrefabDatabase database = go.GetComponent<EiPrefabDatabase> ();

			var itemList = database.Length;
			for (int e = 0; e < itemList; e++) {
				var item = database [e];
				if (item) {
					string itemPath = string.Format ("{0} / {1}", item.editorPathName, item.ItemName);

					if (filter != null && !filter.IsCorrect (item, itemPath))
						continue;

					if (objectPicker && item.GameObject == objectPicker) {
						currentSelectedObject = item;
						objectPicker = null;
					}

					if (currentSelectedObject == item)
						index = items.Count;

					int iterations = 0;
					while (items.Contains (path))
						itemPath = string.Format ("{0} / {1} ({2})", item.editorPathName, item.ItemName, iterations++);

					items.Add (itemPath);
					references.Add (item);
				}
			}

			Rect popupPosition = new Rect (position);
			popupPosition.width -= 40f;
			property.objectReferenceValue = references [EditorGUI.Popup (popupPosition, property.displayName, index, items.ToArray ())];

			if (objectPicker) {
				objectPicker = null;
				var complex = EditorUtility.DisplayDialog ("Error", "Selected item does not exists in the prefab database", "Ok", "Database");
				if (!complex)
					Selection.activeObject = database.gameObject;
			}

			Rect quickSearch = new Rect (position);
			quickSearch.x += position.width - 40f;
			quickSearch.width = 20f;
			if (GUI.Button (quickSearch, "s"))
				EditorGUIUtility.ShowObjectPicker<GameObject> (currentSelectedObject ? currentSelectedObject.GameObject : null, false, "", 0);

			Rect databaseReferencePosition = new Rect (position);
			databaseReferencePosition.x += position.width - 20f;
			databaseReferencePosition.width = 20f;
			if (GUI.Button (databaseReferencePosition, "~"))
				Selection.activeObject = database.gameObject;
		}

		void ObjectSelectorUpdated ()
		{
			objectPicker = EditorGUIUtility.GetObjectPickerObject () as GameObject;
		}
	}
}
