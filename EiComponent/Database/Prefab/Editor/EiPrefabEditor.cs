using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Eitrum.Database.Prefab
{
	[CustomPropertyDrawer(typeof(EiPrefab))]
	public class EiPrefabEditor : PropertyDrawer
	{
		static string path = "Assets/Eitrum/Configuration/EiPrefabDatabase.prefab";
		static GameObject objectPicker = null;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var attributes = fieldInfo.GetCustomAttributes(false);

			EiDatabaseFilter filter = null;

			for (int i = 0; i < attributes.Length; i++)
			{
				if (attributes[i] is EiDatabaseFilter)
				{
					filter = attributes[i] as EiDatabaseFilter;
					break;
				}
			}

			if (Event.current.commandName == "ObjectSelectorClosed")
				objectPicker = EditorGUIUtility.GetObjectPickerObject() as GameObject;

			List<string> items = new List<string>();
			List<EiPrefab> references = new List<EiPrefab>();
			items.Add("None");
			references.Add(null);

			var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
			if (!go)
			{
				var tempObj = new GameObject("EiPrefabDatabase", typeof(EiPrefabDatabase));
				go = PrefabUtility.CreatePrefab(path, tempObj);
				UnityEngine.MonoBehaviour.DestroyImmediate(tempObj);
			}
			EiPrefabDatabase database = go.GetComponent<EiPrefabDatabase>();
			var currentSelectedObject = property.objectReferenceValue as EiPrefab;
			var index = 0;

			var categories = EiPrefabDatabaseEditor.GetSubCategoryList(database);
			for (int i = 0; i < categories.Count; i++)
			{
				var category = categories[i];
				LoadCategory("", category, items, references, ref currentSelectedObject, ref index, filter);
			}

			Rect popupPosition = new Rect(position);
			popupPosition.width -= 40f;
			property.objectReferenceValue = references[EditorGUI.Popup(popupPosition, property.displayName, index, items.ToArray())];

			if (objectPicker)
			{
				objectPicker = null;
				var complex = EditorUtility.DisplayDialog("Error", "Selected item does not exists in the prefab database", "Ok", "Database");
				if (!complex)
					Selection.activeObject = database.gameObject;
			}

			Rect quickSearch = new Rect(position);
			quickSearch.x += position.width - 40f;
			quickSearch.width = 20f;
			if (GUI.Button(quickSearch, "s"))
				EditorGUIUtility.ShowObjectPicker<GameObject>(currentSelectedObject ? currentSelectedObject.GameObject : null, false, "", 0);

			Rect databaseReferencePosition = new Rect(position);
			databaseReferencePosition.x += position.width - 20f;
			databaseReferencePosition.width = 20f;
			if (GUI.Button(databaseReferencePosition, "~"))
				Selection.activeObject = database.gameObject;
		}

		void ObjectSelectorUpdated()
		{
			objectPicker = EditorGUIUtility.GetObjectPickerObject() as GameObject;
		}

		void LoadCategory(string path, EiPrefabSubCategory category, List<string> paths, List<EiPrefab> references, ref EiPrefab currentSelectedObject, ref int index, EiDatabaseFilter filter)
		{
			var subCatList = category.subCategories;
			var subPath = "";
			if (path == "")
				subPath = category.CategoryName;
			else
				subPath = string.Format("{0} / {1}", path, category.CategoryName);
			for (int i = 0; i < subCatList.Count; i++)
			{
				var subCategory = subCatList[i];
				LoadCategory(subPath, subCategory, paths, references, ref currentSelectedObject, ref index, filter);
			}

			var itemList = category.items;
			for (int e = 0; e < itemList.Count; e++)
			{
				var item = itemList[e];
				if (item)
				{
					if (item.Item && item.Item.name != item.ItemName)
					{
						Undo.RecordObject(item, "Database Item Name Change");
						item.GetType().GetField("itemName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(item, item.Item.name);
					}
					string itemPath = string.Format("{0} / {1}", subPath, item.ItemName);

					if (filter != null && !filter.IsCorrect(item, itemPath))
						continue;


					if (objectPicker && item.GameObject == objectPicker)
					{
						currentSelectedObject = item;
						objectPicker = null;
					}

					if (item == currentSelectedObject)
						index = paths.Count;

					int iterations = 0;
					while (paths.Contains(path))
						itemPath = string.Format("{0} / {1} ({2})", category.CategoryName, item.ItemName, iterations++);

					paths.Add(itemPath);
					references.Add(item);
				}
			}
		}
	}
}
