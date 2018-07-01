using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System;

namespace Eitrum
{
	[CustomEditor (typeof(EiDatabase))]
	public class EiDatabaseEditor : Editor
	{

		static List<bool> categoriesFolded = new List<bool> ();

		private FieldInfo categoryList = null;
		private FieldInfo entryList = null;

		public override void OnInspectorGUI ()
		{
			var database = (EiDatabase)target;
			if (categoryList == null) {
				categoryList = typeof(EiDatabase).GetField ("categories", BindingFlags.NonPublic | BindingFlags.Instance);
				if (categoryList == null)
					return;
			}
			if (entryList == null) {
				entryList = typeof(EiCategory).GetField ("entries", BindingFlags.NonPublic | BindingFlags.Instance);
				if (entryList == null)
					return;
			}
			DrawDatabase (database);
		}

		private void DrawDatabase (EiDatabase db)
		{
			var dbLength = db._Length;
			EditorGUILayout.LabelField (string.Format ("Categories ({0})", dbLength));
			while (dbLength > categoriesFolded.Count) {
				categoriesFolded.Add (false);
			}

			for (int i = 0; i < db._Length; i++) {
				var cat = db [i];
				if (!DrawCategory (db, cat, i)) {
					GetCategories (db).RemoveAt (i);
					i--;
				}
			}

			EditorGUILayout.BeginHorizontal ();

			if (GUILayout.Button ("Add Category", GUILayout.Width (100f))) {
				GetCategories (db).Add (new EiCategory ());
			}

			if (GUILayout.Button ("Clear", GUILayout.Width (100f))) {
				if (EditorUtility.DisplayDialog ("Clear Database", "Do you really wanna clear the database?", "Yes", "No")) {
					GetCategories (db).Clear ();
					db.GetType ().GetField ("uniqueIdGenerator", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (db, 0);
				}
			}

			EditorGUILayout.EndHorizontal ();
		}

		private bool DrawCategory (EiDatabase database, EiCategory category, int index)
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
					var entryToAdd = new EiEntry ();
					SetEntryUniqueId (entryToAdd, AllocateUniqueId (database));
					GetEntries (category).Add (entryToAdd);
				}
				var obj = EditorGUILayout.ObjectField (null, typeof(UnityEngine.Object), false, GUILayout.Width (100f));
				if (obj) {
					var entryToAdd = new EiEntry ();
					SetEntryObject (entryToAdd, obj);
					SetEntryUniqueId (entryToAdd, AllocateUniqueId (database));
					GetEntries (category).Add (entryToAdd);
				}

				EditorGUILayout.EndHorizontal ();

				EndSubCategory ();
			}
			return true;
		}

		private bool DrawEntry (EiEntry entry, int index)
		{
			EditorGUILayout.BeginHorizontal ();
			SetEntryName (entry, entry.Item ? entry.Item.name : "empty");
			SetEntryObject (entry, EditorGUILayout.ObjectField (entry.Item, typeof(UnityEngine.Object), false)); 
			if (GUILayout.Button ("X", GUILayout.Width (24f))) {
				EditorGUILayout.EndHorizontal ();
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

		public int AllocateUniqueId (EiDatabase database)
		{
			var field = database.GetType ().GetField ("uniqueIdGenerator", BindingFlags.NonPublic | BindingFlags.Instance);
			var id = (int)field.GetValue (database);
			field.SetValue (database, id + 1);
			return id;
		}

		public void SetCategoryName (EiCategory category, string name)
		{
			var field = typeof(EiCategory).GetField ("categoryName", BindingFlags.NonPublic | BindingFlags.Instance);
			field.SetValue (category, name);
		}

		public void SetEntryUniqueId (EiEntry entry, int uniqueId)
		{
			entry.GetType ().GetField ("uniqueId", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (entry, uniqueId);
		}

		public void SetEntryName (EiEntry entry, string name)
		{
			entry.GetType ().GetField ("itemName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (entry, name);
		}

		public void SetEntryObject (EiEntry entry, UnityEngine.Object item)
		{
			entry.GetType ().GetField ("item", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (entry, item);
		}

		public List<EiCategory> GetCategories (EiDatabase database)
		{
			return categoryList.GetValue (database) as List<EiCategory>;
		}

		public List<EiEntry> GetEntries (EiCategory category)
		{
			return entryList.GetValue (category) as List<EiEntry>;
		}
	}
}