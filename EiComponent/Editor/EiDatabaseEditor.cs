using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum {

    [CustomEditor(typeof(EiDatabase))]
    public class EiDatabaseEditor : Editor {

        static List<bool> categoriesFolded = new List<bool>();

        public override void OnInspectorGUI() {

            var database = (EiDatabase)target;
            DrawDatabase(database);
        }

        private void DrawDatabase(EiDatabase db) {
            EditorGUILayout.LabelField(string.Format("Categories ({0})", db.categories.Count));
            var categories = db.categories;
            while(categories.Count > categoriesFolded.Count) {
                categoriesFolded.Add(false);
            }

            for(int i = 0; i < categories.Count; i++) {
                var cat = categories[i];
                if(!DrawCategory(db, cat, i)) {
                    db.categories.RemoveAt(i);
                    i--;
                }
            }

            EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("Add Category", GUILayout.Width(100f))) {
                categories.Add(new EiCategory());
            }
            EditorGUILayout.EndHorizontal();
        }

        private bool DrawCategory(EiDatabase database, EiCategory category, int index) {
            EditorGUILayout.BeginHorizontal();
            categoriesFolded[index] = EditorGUILayout.Foldout(categoriesFolded[index], "Entries (" + category.entries.Count + ")", true);
            category.categoryName = EditorGUILayout.TextField(category.categoryName);
            if(GUILayout.Button("X", GUILayout.Width(24f))) {
                EditorGUILayout.EndHorizontal();
                return false;
            }
            EditorGUILayout.EndHorizontal();

            if(categoriesFolded[index]) {
                BeginSubCategory();
                var entries = category.entries;
                for(int i = 0; i < entries.Count; i++) {
                    var ent = entries[i];
                    if(!DrawEntry(ent, i)) {
                        category.entries.RemoveAt(i);
                        i--;
                    }
                }

                EditorGUILayout.BeginHorizontal();

                if(GUILayout.Button("Add Item", GUILayout.Width(100f))) {
                    category.entries.Add(new EiEntry()
                        {
                            uniqueId = database.uniqueIdGeneration++
                        });
                }
                var obj = EditorGUILayout.ObjectField(null, typeof(UnityEngine.Object), false, GUILayout.Width(100f));
                if(obj) {
                    category.entries.Add(new EiEntry()
                        {
                            item = obj,
                            uniqueId = database.uniqueIdGeneration++
                        });
                }

                EditorGUILayout.EndHorizontal();

                EndSubCategory();
            }
            return true;
        }

        private bool DrawEntry(EiEntry entry, int index) {
            EditorGUILayout.BeginHorizontal();
            entry.itemName = entry.item ? entry.item.name : "empty";
            entry.item = EditorGUILayout.ObjectField(entry.item, typeof(UnityEngine.Object), false); 
            if(GUILayout.Button("X", GUILayout.Width(24f))) {
                EditorGUILayout.EndHorizontal();
                return false;
            }
            EditorGUILayout.EndHorizontal();
            return true;
        }

        private void BeginSubCategory() {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(12f);
            EditorGUILayout.BeginVertical();
        }

        private void EndSubCategory() {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}