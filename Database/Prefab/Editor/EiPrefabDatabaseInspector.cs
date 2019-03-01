using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.VersionControl;

namespace Eitrum.Database.Prefab {
    [CustomEditor(typeof(EiPrefabDatabase))]
    public class EiPrefabDatabaseInspector : Editor {
        static bool[] folded;
        static bool editMode = false;
        static string pathFilter = "";

        EiPrefab objPicker;

        public override void OnInspectorGUI() {
            var db = (EiPrefabDatabase)target;
            var list = GetPrefabList(db);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(editMode ? "List" : "Edit", GUILayout.Width(50))) {
                editMode = !editMode;
            }
            pathFilter = EditorGUILayout.TextField("Search Filter", pathFilter);

            EditorGUILayout.EndHorizontal();

            if (editMode) {
                EditorGUILayout.LabelField("Edit mode no longer supported");
                for (int i = 0; i < list.Count; i++) {
                    var prefab = list[i];
                    if (prefab.FullName.Contains(pathFilter)) {

                    }
                }
            }
            else {
                if (EditorGUIUtility.GetObjectPickerControlID() == 129) {
                    objPicker = EditorGUIUtility.GetObjectPickerObject() as EiPrefab;
                }
                else {
                    if (objPicker) {
                        if (!list.Contains(objPicker) || EditorUtility.DisplayDialog("Add Prefab Warning", "You are about to add a prefab that is already in the database, do you want to proceed?", "Yes", "Cancel")) {
                            list.Add(objPicker);
                        }
                        objPicker = null;
                    }
                }
                if (folded == null || folded.Length != list.Count) {
                    folded = new bool[list.Count];
                }
                for (int i = 0; i < list.Count; i++) {
                    var prefab = list[i];
                    if (prefab == null)
                        continue;
                    if (prefab.FullName.Contains(pathFilter)) {
                        var toRemove = !Render(prefab, i, folded[i]);
                        if (toRemove) {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }
                if (GUILayout.Button("Add Item", GUILayout.Width(100))) {
                    EditorGUIUtility.ShowObjectPicker<EiPrefab>(null, false, "", 129);
                }
            }
        }

        private bool Render(EiPrefab prefab, int i, bool folded) {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Select", GUILayout.Width(50))) {
                Selection.activeObject = prefab;
            }
            if (GUILayout.Button("X", GUILayout.Width(18))) {
                EditorGUILayout.EndHorizontal();
                return false;
            }
            if (prefab == null) {
                EditorGUILayout.LabelField("Null Object");
            }
            else {
                if (prefab.Id != i) {
                    if (Provider.enabled && Provider.isActive) {
                        var asset = Provider.GetAssetByPath(AssetDatabase.GetAssetPath(prefab));
                        if (asset.locked && Provider.CheckoutIsValid(asset)) {
                            Provider.Checkout(asset, CheckoutMode.Both);
                        }
                        if (!asset.locked) {
                            ApplyUniqueId(prefab, i);
                        }
                    }
                    else {
                        ApplyUniqueId(prefab, i);
                    }
                }
                var text = new GUIContent(prefab.Path.Replace("/", " / ") + " /");
                var size = EditorStyles.label.CalcSize(text);
                EditorGUILayout.LabelField(text, GUILayout.Width(size.x));
                EditorGUILayout.LabelField(prefab.ItemName, EditorStyles.boldLabel, GUILayout.Width(EditorStyles.boldLabel.CalcSize(new GUIContent(prefab.ItemName)).x));
            }
            EditorGUILayout.EndHorizontal();
            return true;
        }

        public static List<EiPrefab> GetPrefabList(EiPrefabDatabase db) {
            return (List<EiPrefab>)typeof(EiPrefabDatabase).GetField("cachedItems", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(db);
        }

        public static void ApplyUniqueId(EiPrefab prefab, int id) {
            typeof(EiPrefab).GetField("id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(prefab, id);
        }
    }
}