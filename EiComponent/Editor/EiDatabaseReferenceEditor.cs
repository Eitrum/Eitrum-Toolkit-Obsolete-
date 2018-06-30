using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum {
    [CustomPropertyDrawer(typeof(EiDatabaseReference))]
    public class EiDatabaseReferenceEditor : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            List<string> items = new List<string>();
            List<int> ids = new List<int>();
            items.Add("None");
            ids.Add(-1);
            EiDatabase database = EiDatabase.Instance;
            var currentSelectedId = property.FindPropertyRelative("uniqueIdReference").intValue;
            var index = 0;

            var categories = database.categories;
            for(int i = 0; i < categories.Count; i++) {
                var category = categories[i];
                var entries = category.entries;
                for(int e = 0; e < entries.Count; e++) {
                    var entry = entries[e];
                    string path = string.Format("{0} / {1}", category.categoryName, entry.itemName);
                    var uniqueId = entry.uniqueId;
                    if(uniqueId == currentSelectedId) {
                        index = items.Count;
                    }
                    items.Add(path);
                    ids.Add(uniqueId);
                }
            }

            property.FindPropertyRelative("uniqueIdReference").intValue = ids[EditorGUI.Popup(position, property.displayName, index, items.ToArray())];
        }
    }
}