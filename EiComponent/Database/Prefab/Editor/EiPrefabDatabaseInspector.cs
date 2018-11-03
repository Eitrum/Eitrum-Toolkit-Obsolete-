using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Eitrum.Database.Prefab
{
	[CustomEditor (typeof(EiPrefabDatabase))]
	public class EiPrefabDatabaseInspector : Editor
	{
		static bool[] folded;

		EiPrefab objPicker;

		public override void OnInspectorGUI ()
		{
			var db = (EiPrefabDatabase)target;
			var list = GetPrefabList (db);

			if (EditorGUIUtility.GetObjectPickerControlID () == 129) {
				objPicker = EditorGUIUtility.GetObjectPickerObject () as EiPrefab;
			} else {
				if (objPicker) {
					if (!list.Contains (objPicker) || EditorUtility.DisplayDialog ("Add Prefab Warning", "You are about to add a prefab that is already in the database, do you want to proceed?", "Yes", "Cancel")) {
						typeof(EiPrefab).GetField ("database", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue (objPicker, db);
						list.Add (objPicker);
					}
					objPicker = null;
				}
			}
			if (folded == null || folded.Length != list.Count) {
				folded = new bool[list.Count];
			}
			for (int i = 0; i < list.Count; i++) {
				var prefab = list [i];
				var toRemove = !Render (prefab, i, folded [i]);
				if (toRemove) {
					list.RemoveAt (i);
					i--;
				}
			}
			if (GUILayout.Button ("Add Item", GUILayout.Width (100))) {
				EditorGUIUtility.ShowObjectPicker<EiPrefab> (null, false, "", 129);
			}
		}

		private bool Render (EiPrefab prefab, int i, bool folded)
		{
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Select", GUILayout.Width (50))) {
				Selection.activeObject = prefab;
			}
			if (GUILayout.Button ("Remove", GUILayout.Width (60))) {
				if (EditorUtility.DisplayDialog ("Remove", "You sure you want to remove prefab from the database list?", "Yes", "Cancel")) {
					EditorGUILayout.EndHorizontal ();
					return false;
				}
			}
			if (prefab == null) {
				EditorGUILayout.LabelField ("Null Object");
			} else {
				ApplyUniqueId (prefab, i);
				var text = new GUIContent (prefab.editorPathName.Replace ("/", " / ") + " /");
				var size = EditorStyles.label.CalcSize (text);
				EditorGUILayout.LabelField (text, GUILayout.Width (size.x));
				EditorGUILayout.LabelField (prefab.ItemName, EditorStyles.boldLabel, GUILayout.Width (EditorStyles.boldLabel.CalcSize (new GUIContent (prefab.ItemName)).x));
			}
			EditorGUILayout.EndHorizontal ();
			return true;
		}

		public static List<EiPrefab> GetPrefabList (EiPrefabDatabase db)
		{
			return (List<EiPrefab>)typeof(EiPrefabDatabase).GetField ("cachedItems", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue (db);
		}

		public static void ApplyUniqueId (EiPrefab prefab, int id)
		{
			typeof(EiPrefab).GetField ("uniqueId", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue (prefab, id);
		}
	}
}