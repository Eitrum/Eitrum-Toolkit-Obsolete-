using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum
{
	[CustomPropertyDrawer (typeof(EiDatabaseItem))]
	public class EiDatabaseItemEditor : PropertyDrawer
	{

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			List<string> items = new List<string> ();
			List<EiDatabaseItem> references = new List<EiDatabaseItem> ();
			items.Add ("None");
			references.Add (null);
			EiDatabaseResource database = EiDatabaseResource.Instance;
			var currentSelectedObject = property.FindPropertyRelative ("item").objectReferenceValue;
			var index = 0;

			var categories = database._Length;
			for (int i = 0; i < categories; i++) {
				var category = database [i];
				var entries = category.Length;
				for (int e = 0; e < entries; e++) {
					var entry = category [e];
					string path = string.Format ("{0} / {1}", category.CategoryName, entry.ItemName);
					if (entry.Item == currentSelectedObject) {
						index = items.Count;
					}
					items.Add (path);
					references.Add (entry);
				}
			}

			property.objectReferenceValue = references [EditorGUI.Popup (position, property.displayName, index, items.ToArray ())];
		}
	}
}