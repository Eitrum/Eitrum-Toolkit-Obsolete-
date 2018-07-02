using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System;

namespace Eitrum
{
	[CustomEditor (typeof(EiDatabaseResource))]
	public class EiDatabaseResourceEditor : Editor
	{

		static List<bool> categoriesFolded = new List<bool> ();

		private FieldInfo categoryList = null;
		private FieldInfo entryList = null;

		public override void OnInspectorGUI ()
		{
			var database = (EiDatabaseResource)target;
			if (categoryList == null) {
				categoryList = typeof(EiDatabaseResource).GetField ("categories", BindingFlags.NonPublic | BindingFlags.Instance);
				if (categoryList == null)
					return;
			}
			if (entryList == null) {
				entryList = typeof(EiDatabaseCategory).GetField ("entries", BindingFlags.NonPublic | BindingFlags.Instance);
				if (entryList == null)
					return;
			}
			DrawDatabase (database);
		}

		private void DrawDatabase (EiDatabaseResource db)
		{
			var dbLength = db._Length;
			EditorGUILayout.LabelField (string.Format ("Categories ({0})", dbLength));
			while (dbLength > categoriesFolded.Count) {
				categoriesFolded.Add (false);
			}

			for (int i = 0; i < db._Length; i++) {
				var cat = db [i];
				if (!DrawCategory (db, cat, i)) {
					DeleteCategory (cat);
					GetCategories (db).RemoveAt (i);
					i--;
				}
			}

			EditorGUILayout.BeginHorizontal ();

			if (GUILayout.Button ("Add Category", GUILayout.Width (100f))) {
				GetCategories (db).Add (new EiDatabaseCategory ());
			}

			if (GUILayout.Button ("Clear", GUILayout.Width (100f))) {
				if (EditorUtility.DisplayDialog ("Clear Database", "Do you really wanna clear the database?", "Yes", "No")) {
					ClearDatabase ();
					GetCategories (db).Clear ();
				}
			}

			EditorGUILayout.EndHorizontal ();
		}

		private bool DrawCategory (EiDatabaseResource database, EiDatabaseCategory category, int index)
		{
			EditorGUILayout.BeginHorizontal ();
			categoriesFolded [index] = EditorGUILayout.Foldout (categoriesFolded [index], "Entries (" + category.Length + ")", true);
			SetCategoryName (category, EditorGUILayout.TextField (category.CategoryName));
			if (GUILayout.Button ("X", GUILayout.Width (24f))) {
				EditorGUILayout.EndHorizontal ();
				return false;
			}
			EditorGUILayout.EndHorizontal ();

			if (categoriesFolded [index]) {
				BeginSubCategory ();
				for (int i = 0; i < category.Length; i++) {
					var ent = category [i];
					if (!DrawEntry (ent, i)) {
						GetEntries (category).RemoveAt (i);
						i--;
					}
				}

				EditorGUILayout.BeginHorizontal ();

				if (GUILayout.Button ("Add Item", GUILayout.Width (100f))) {
					var entryToAdd = EiDatabaseItem.CreateAsset ("Eitrum/DatabaseItems");
					GetEntries (category).Add (entryToAdd);
				}
				var obj = EditorGUILayout.ObjectField (null, typeof(UnityEngine.Object), false, GUILayout.Width (100f));
				if (obj) {
					var entryToAdd = EiDatabaseItem.CreateAsset ("Eitrum/DatabaseItems");
					SetEntryObject (entryToAdd, obj);
					GetEntries (category).Add (entryToAdd);
				}

				EditorGUILayout.EndHorizontal ();

				EndSubCategory ();
			}
			return true;
		}

		private bool DrawEntry (EiDatabaseItem entry, int index)
		{
			EditorGUILayout.BeginHorizontal ();
			SetEntryName (entry, entry.Item ? entry.Item.name : "empty");
			SetEntryObject (entry, EditorGUILayout.ObjectField (entry.Item, typeof(UnityEngine.Object), false)); 
			if (GUILayout.Button ("X", GUILayout.Width (24f))) {
				EditorGUILayout.EndHorizontal ();
				entry.DestroyFile ();
				return false;
			}
			EditorGUILayout.EndHorizontal ();
			return true;
		}

		private void BeginSubCategory ()
		{
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Space (12f);
			EditorGUILayout.BeginVertical ();
		}

		private void EndSubCategory ()
		{
			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndHorizontal ();
		}

		public void ClearDatabase (EiDatabaseResource resource)
		{
			for (int i = resource._Length; i >= 0; i--) {
				DeleteCategory (resource [i]);
			}
		}

		public void DeleteCategory (EiDatabaseCategory category)
		{
			var entries = GetEntries (category);
			for (int i = 0; i < entries.Count; i++) {
				entries [i].DestroyFile ();
			}
		}

		public void SetCategoryName (EiDatabaseCategory category, string name)
		{
			var field = typeof(EiDatabaseCategory).GetField ("categoryName", BindingFlags.NonPublic | BindingFlags.Instance);
			field.SetValue (category, name);
		}

		public void SetEntryName (EiDatabaseItem entry, string name)
		{
			entry.GetType ().GetField ("itemName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (entry, name);
		}

		public void SetEntryObject (EiDatabaseItem entry, UnityEngine.Object item)
		{
			entry.GetType ().GetField ("item", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (entry, item);
		}

		public List<EiDatabaseCategory> GetCategories (EiDatabaseResource database)
		{
			return categoryList.GetValue (database) as List<EiDatabaseCategory>;
		}

		public List<EiDatabaseItem> GetEntries (EiDatabaseCategory category)
		{
			return entryList.GetValue (category) as List<EiDatabaseItem>;
		}
	}
}