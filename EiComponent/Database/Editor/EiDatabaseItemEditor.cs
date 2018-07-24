using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum
{
    [CustomPropertyDrawer(typeof(EiDatabaseItem))]
    public class EiDatabaseItemEditor : PropertyDrawer
    {

        static string path = "Assets/Eitrum/Configuration/EiDatabase.prefab";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attributes = property.GetType().GetCustomAttributes(true);
            for (int i = 0; i < attributes.Length; i++)
            {
                Debug.Log(attributes[i].ToString());
            }
            List<string> items = new List<string>();
            List<EiDatabaseItem> references = new List<EiDatabaseItem>();
            items.Add("None");
            references.Add(null);

            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (!go)
            {
                var tempObj = new GameObject("EiDatabase", typeof(EiDatabaseResource));
                go = PrefabUtility.CreatePrefab(path, tempObj);
                UnityEngine.MonoBehaviour.DestroyImmediate(tempObj);
            }
            EiDatabaseResource database = go.GetComponent<EiDatabaseResource>();
            var currentSelectedObject = property.objectReferenceValue;
            var index = 0;

            var categories = database._Length;
            for (int i = 0; i < categories; i++)
            {
                var category = database[i];
                LoadCategory("", category, items, references, currentSelectedObject, ref index);
            }

            Rect popupPosition = new Rect(position);
            popupPosition.width -= 20f;
            property.objectReferenceValue = references[EditorGUI.Popup(popupPosition, property.displayName, index, items.ToArray())];

            Rect databaseReferencePosition = new Rect(position);
            databaseReferencePosition.x += position.width - 20f;
            databaseReferencePosition.width = 20f;
            if (GUI.Button(databaseReferencePosition, "~"))
            {
                Selection.activeObject = database.gameObject;
            }
        }

        void LoadCategory(string path, EiDatabaseCategory category, List<string> items, List<EiDatabaseItem> references, UnityEngine.Object currentSelectedObject, ref int index)
        {
            var subCategoriesLength = category.GetSubCategoriesLength();
            var subPath = "";
            if (path == "")
                subPath = category.CategoryName;
            else
                subPath = string.Format("{0} / {1}", path, category.CategoryName);
            for (int i = 0; i < subCategoriesLength; i++)
            {
                var subCategory = category.GetSubCategory(i);
                LoadCategory(subPath, subCategory, items, references, currentSelectedObject, ref index);
            }

            var entries = category.GetEntriesLength();
            for (int e = 0; e < entries; e++)
            {
                var entry = category.GetEntry(e);
                if (entry)
                {
                    if (entry.Item && entry.Item.name != entry.ItemName)
                    {
                        Undo.RecordObject(entry, "Database Item Name Change");
                        entry.GetType().GetField("itemName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(entry, entry.Item.name);
                    }
                    string itemPath = string.Format("{0} / {1}", subPath, entry.ItemName);
                    if (entry == currentSelectedObject)
                    {
                        index = items.Count;
                    }
                    int iterations = 0;
                    while (items.Contains(path))
                    {
                        itemPath = string.Format("{0} / {1} ({2})", category.CategoryName, entry.ItemName, iterations++);
                    }
                    items.Add(itemPath);
                    references.Add(entry);
                }
            }
        }
    }
}