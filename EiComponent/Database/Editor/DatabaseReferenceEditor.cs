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
            LoadCategory("", category, items, objs, currentSelectedObject, doTypeCheck, databaseAttribute.type, ref index);
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

    void LoadCategory(string path, EiDatabaseCategory category, List<string> items, List<UnityEngine.Object> objs,UnityEngine.Object currentSelectedObject, bool doTypeCheck, Type type, ref int index)
    {
        var subCategoriesLength = category.GetSubCategoriesLength();
        var subPath = "";
        if(path == "")
            subPath = category.CategoryName;
        else
            subPath = string.Format("{0} / {1}",path, category.CategoryName);
        for (int i = 0; i < subCategoriesLength; i++)
        {
            var subCategory = category.GetSubCategory(i);
            LoadCategory(subPath, subCategory, items, objs, currentSelectedObject, doTypeCheck, type, ref index);
        }

        var entries = category.GetEntriesLength();
        for (int e = 0; e < entries; e++)
        {
            var entry = category.GetEntry(e);
            if (!doTypeCheck || entry.Is(type))
            {
                string itemPath = string.Format("{0} / {1}", subPath, entry.ItemName);
                if (entry.Item == currentSelectedObject)
                {
                    index = items.Count;
                }
                items.Add(itemPath);
                objs.Add(entry.Item);
            }
        }
    }
}