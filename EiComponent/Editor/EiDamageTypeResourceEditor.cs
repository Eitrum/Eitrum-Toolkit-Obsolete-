using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System;

namespace Eitrum.Health
{
	[CustomEditor (typeof(EiDamageTypeResource))]
	public class EiDamageTypeResourceEditor : Editor
	{

		static List<bool> categoriesFolded = new List<bool> ();

		public override void OnInspectorGUI ()
		{
			var damageTypes = (EiDamageTypeResource)target;
			DrawDamageType (damageTypes);
		}

		private void DrawDamageType (EiDamageTypeResource resource)
		{
			var categoryList = GetCategoryList (resource);
			EditorGUILayout.LabelField (string.Format ("Categories ({0})", categoryList.Count));
			while (categoryList.Count > categoriesFolded.Count) {
				categoriesFolded.Add (false);
			}

			for (int i = 0; i < categoryList.Count; i++) {
				var cat = categoryList [i];
				if (!DrawCategory (resource, cat, i)) {
					categoryList.RemoveAt (i);
					i--;
				}
			}

			EditorGUILayout.BeginHorizontal ();

			if (GUILayout.Button ("Add Category", GUILayout.Width (100f))) {
				categoryList.Add (new EiDamageTypeCategory ());
			}

			if (GUILayout.Button ("Clear", GUILayout.Width (100f))) {
				if (EditorUtility.DisplayDialog ("Clear Damage Types", "Do you really wanna clear the damage types?", "Yes", "No")) {
					categoryList.Clear ();
					resource.GetType ().GetField ("uniqueIdGenerator", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (resource, 0);
				}
			}

			EditorGUILayout.EndHorizontal ();
		}

		private bool DrawCategory (EiDamageTypeResource resource, EiDamageTypeCategory category, int index)
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
				var entryList = GetEntryList (category);
				for (int i = 0; i < entryList.Count; i++) {
					var ent = entryList [i];
					if (!DrawEntry (ent, i)) {
						entryList.RemoveAt (i);
						i--;
					}
				}

				EditorGUILayout.BeginHorizontal ();

				if (GUILayout.Button ("Add Item", GUILayout.Width (100f))) {
					var entryToAdd = new EiDamageTypeEntry ();
					SetEntryUniqueId (entryToAdd, AllocateUniqueId (resource));
					entryList.Add (entryToAdd);
				}

				EditorGUILayout.EndHorizontal ();

				EndSubCategory ();
			}

			return true;
		}

		private bool DrawEntry (EiDamageTypeEntry entry, int index)
		{
			EditorGUILayout.BeginHorizontal ();
			SetEntryName (entry, EditorGUILayout.TextField ("Type (" + index + ")", entry.DamageTypeName));
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

		public int AllocateUniqueId (EiDamageTypeResource database)
		{
			var field = database.GetType ().GetField ("uniqueIdGenerator", BindingFlags.NonPublic | BindingFlags.Instance);
			var id = (int)field.GetValue (database);
			field.SetValue (database, id + 1);
			return id;
		}

		//----------------------------------------------------------

		public List<EiDamageTypeEntry> GetEntryList (EiDamageTypeCategory category)
		{
			return GetListFromType<EiDamageTypeEntry, EiDamageTypeCategory> (category, "damageTypes");
		}

		public List<EiDamageTypeCategory> GetCategoryList (EiDamageTypeResource resource)
		{
			return GetListFromType<EiDamageTypeCategory, EiDamageTypeResource> (resource, "damageCategories");
		}

		public List<TList> GetListFromType<TList, TResource> (TResource resource, string fieldName)
		{
			return resource.GetType ().GetField (fieldName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue (resource) as List<TList>;
		}

		public void SetEntryUniqueId (EiDamageTypeEntry entry, int uniqueId)
		{
			typeof(EiDamageTypeEntry).GetField ("uniqueDamageTypeId", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (entry, uniqueId);
		}

		public void SetEntryName (EiDamageTypeEntry entry, string newName)
		{
			typeof(EiDamageTypeEntry).GetField ("damageTypeName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (entry, newName);
		}

		public void SetCategoryName (EiDamageTypeCategory category, string newName)
		{
			typeof(EiDamageTypeCategory).GetField ("categoryName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (category, newName);
		}
			
		/*	[SerializeField]
		private string damageTypeName = "";
		[SerializeField]
		private int uniqueDamageTypeId = 0;*/
	}
}