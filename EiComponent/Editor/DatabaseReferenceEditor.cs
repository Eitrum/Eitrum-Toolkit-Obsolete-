using System;
using UnityEditor;
using UnityEngine;
using Eitrum;
using System.Collections.Generic;

[CustomPropertyDrawer (typeof(DatabaseReference))]
public class DatabaseReferenceEditor : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		var databaseAttribute = (DatabaseReference)attribute;
		List<string> items = new List<string> ();
		List<UnityEngine.Object> objs = new List<UnityEngine.Object> ();
		items.Add ("None");
		objs.Add (null);
		EiDatabaseResource database = EiDatabaseResource.Instance;
		var currentSelectedObject = property.objectReferenceValue;
		var index = 0;
		bool doTypeCheck = databaseAttribute.type != null;

		var categories = database._Length;
		for (int i = 0; i < categories; i++) {
			var category = database [i];
			var entries = category.Length;
			for (int e = 0; e < entries; e++) {
				var entry = category [e];
				if (!doTypeCheck || entry.Is (databaseAttribute.type)) {
					string path = string.Format ("{0} / {1}", category.CategoryName, entry.ItemName);
					if (entry.Item == currentSelectedObject) {
						index = items.Count;
					}
					items.Add (path);
					objs.Add (entry.Item);
				}
			}
		}

		if (index == 0) {
			if (currentSelectedObject != null) {
				items.Insert (1, currentSelectedObject.name + " (not in database)");
				objs.Insert (1, currentSelectedObject);
				index = 1;
			}
		}

		property.objectReferenceValue = objs [EditorGUI.Popup (position, property.displayName, index, items.ToArray ())];
	}
}