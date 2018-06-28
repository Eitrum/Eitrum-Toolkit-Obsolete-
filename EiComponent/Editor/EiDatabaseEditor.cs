using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Eitrum
{
	[CustomEditor (typeof(EiDatabase))]
	public class EiDatabaseEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			EiDatabase db = (EiDatabase)target;
			DrawDatabase (db);
		}

		private void DrawCategory (EiCategory category, int index)
		{
			MoveRight ();
			EditorGUILayout.BeginHorizontal ();
			category.folded = EditorGUILayout.Foldout (category.folded, string.Format ("({0}) {1}", index, category.name));
			category.name = EditorGUILayout.TextField (category.name);
			EditorGUILayout.EndHorizontal ();

			if (category.folded) {
				
				var entries = category.entries;
				if (entries == null) {
					entries = new List<EiEntry> ();
				}

				if (entries.Count == 0) {
					EditorGUILayout.LabelField ("No Entries");
				}

				for (int i = 0; i < entries.Count; i++) {
					var entry = entries [i];
					if (entry == null) {
						entries.RemoveAt (i);
						i--;
						continue;
					}
					entry.category = index;
					entry.entry = i;
					DrawEntry (entries [i], i);
				}

				EditorGUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Add Entry", GUILayout.Width (100f))) {
					var entry = EiEntry.CreateAsset ("Eitrum/EiDatabaseItems/Entries");
					category.entries.Add (entry);
				}
				if (GUILayout.Button ("Remove Category", GUILayout.Width (140f))) {
					DestroyCategory (category);
				}
				if (GUILayout.Button ("Clear Empty", GUILayout.Width (100f))) {
					for (int i = 0; i < entries.Count; i++) {
						var entry = entries [i];
						if (entry != null && entry.targetObject == null) {
							entry.DestroyFile ();
						}
					}
				}
				EditorGUILayout.EndHorizontal ();
			}
			MoveLeft ();
			if (category && category.folded) {
				GUILayout.Space (8f);
			}
		}

		private void DrawEntry (EiEntry entry, int index)
		{
			MoveRight ();
			entry.name = entry.targetObject ? entry.targetObject.name : "Empty Entry";
			var pre = entry.targetObject;
			var preEntryObj = entry;
			entry.targetObject = EditorGUILayout.ObjectField (string.Format ("({0}) {1}", index, entry.name), entry.targetObject, typeof(UnityEngine.Object), false);
			if (pre != null) {
				if (entry.targetObject == null) {
					preEntryObj.DestroyFile ();
				}
			}
			MoveLeft ();
		}

		private void DrawDatabase (EiDatabase db)
		{
			var categories = db.categories;
			if (categories == null) {
				categories = new List<EiCategory> ();
			}

			if (categories.Count == 0) {
				EditorGUILayout.LabelField ("No Categories");
			}

			for (int i = 0; i < categories.Count; i++) {
				if (categories [i] == null) {
					categories.RemoveAt (i);
					i--;
					continue;
				}
				DrawCategory (categories [i], i);
			}

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add Category", GUILayout.Width (100f))) {
				var category = EiCategory.CreateAsset ("Eitrum/EiDatabaseItems/Categories");
				db.categories.Add (category);
			}
			if (GUILayout.Button ("Clean Empty Entries", GUILayout.Width (150f))) {
				for (int i = 0; i < categories.Count; i++) {
					if (categories [i] == null) {
						categories.RemoveAt (i);
						i--;
						continue;
					}
					var entries = categories [i].entries;
					for (int i2 = 0; i2 < entries.Count; i2++) {
						var entry = entries [i2];
						if (entry == null) {
							entries.RemoveAt (i2);
							i2--;
							continue;
						}
						if (entry != null && entry.targetObject == null) {
							entry.DestroyFile ();
						}
					}
				}
			}
			if (GUILayout.Button ("Save + Format", GUILayout.Width (110f))) {
				float totalFiles = 0;
				float filesChecked = 0;
				for (int i = 0; i < categories.Count; i++) {
					for (int i2 = 0; i2 < categories [i].entries.Count; i2++) {
						totalFiles++;
					}
				}

				for (int i = 0; i < categories.Count; i++) {
					var category = categories [i];
					if (category == null) {
						categories.RemoveAt (i);
						i--;
						continue;
					}
					if (!category.folded) {
						continue;
					}
					var pathName = AssetDatabase.GetAssetPath (category);
					var nameAttempt = AssetDatabase.RenameAsset (pathName, category.name);
					int nameIndex = 0;
					while (nameAttempt != "") {
						nameAttempt = AssetDatabase.RenameAsset (pathName, category.name + " " + nameIndex++);
					}

					var entries = categories [i].entries;
					for (int i2 = 0; i2 < entries.Count; i2++) {
						var entry = entries [i2];
						if (entry == null) {
							entries.RemoveAt (i2);
							i2--;
							continue;
						}
						if (entry != null && entry.targetObject != null) {
							entry.category = i;
							entry.entry = i2;
							pathName = AssetDatabase.GetAssetPath (entry);
							nameAttempt = AssetDatabase.RenameAsset (pathName, entry.name);
							nameIndex = 0;
							while (nameAttempt != "") {
								nameAttempt = AssetDatabase.RenameAsset (pathName, entry.name + " " + nameIndex++);
							}
						}
						EditorUtility.DisplayProgressBar ("Formatting Files", pathName, filesChecked / totalFiles);
						filesChecked++;
					}
				}
				EditorUtility.ClearProgressBar ();
			}
			EditorGUILayout.EndHorizontal ();
		}

		private void DestroyCategory (EiCategory category)
		{
			var entries = category.entries;
			for (int i = 0; i < entries.Count; i++) {
				if (entries [i] != null) {
					entries [i].DestroyFile ();
				}
			}
			category.DestroyFile ();
		}

		private void MoveRight ()
		{
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Space (12f);
			EditorGUILayout.BeginVertical ();
		}

		private void MoveLeft ()
		{
			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndHorizontal ();
		}
	}
}

