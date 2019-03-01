using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System;

namespace Eitrum.Health
{
	[CustomEditor (typeof(DamageTypeResource))]
	public class EiDamageTypeResourceEditor : Editor
	{
		public static List<string> defaultValues = new List<string> ();

		public override void OnInspectorGUI ()
		{
			var damageTypes = (DamageTypeResource)target;
			Undo.RecordObject (damageTypes, "Damage Types Resource");
			if (defaultValues == null || defaultValues.Count == 0) {
				LoadDefaultValues ();
			}
			DrawDamageType (damageTypes);
			EditorUtility.SetDirty (damageTypes);
		}

		public static void LoadDefaultValues ()
		{
			defaultValues = new List<string> ();
			defaultValues.Add ("Normal / Bludgeoning");
			defaultValues.Add ("Normal / Piercing");
			defaultValues.Add ("Normal / Slashing");
			defaultValues.Add ("Magic / Acid");
			defaultValues.Add ("Magic / Cold");
			defaultValues.Add ("Magic / Fire");
			defaultValues.Add ("Magic / Force");
			defaultValues.Add ("Magic / Lightning");
			defaultValues.Add ("Magic / Necrotic");
			defaultValues.Add ("Magic / Poison");
			defaultValues.Add ("Magic / Psychic");
			defaultValues.Add ("Magic / Radiant");
			defaultValues.Add ("Magic / Thunder");
		}

		private void DrawDamageType (DamageTypeResource resource)
		{
			var categoryList = GetCategoryList (resource);
			if (categoryList.Count < defaultValues.Count) {
				categoryList.Clear ();
				categoryList.AddRange (defaultValues);
				while (categoryList.Count < 32) {
					categoryList.Add ("");
				}
			}
			EditorGUILayout.LabelField (string.Format ("Damage Types ({0})", categoryList.Count));

			for (int i = 0; i < categoryList.Count; i++) {
				var cat = categoryList [i];
				if (!DrawEntry (categoryList, cat, i)) {
					categoryList.RemoveAt (i);
					i--;
				}
			}

			EditorGUILayout.BeginHorizontal ();

			/*if (GUILayout.Button ("Add Damage Type", GUILayout.Width (130f))) {
				categoryList.Add ("");
			}*/

			if (GUILayout.Button ("Clear", GUILayout.Width (100f))) {
				if (EditorUtility.DisplayDialog ("Clear Damage Types", "Do you really wanna clear the damage types?", "Yes", "No")) {
					categoryList.Clear ();
					categoryList.AddRange (defaultValues);
					while (categoryList.Count < 32) {
						categoryList.Add ("");
					}
				}
			}

			EditorGUILayout.EndHorizontal ();
		}

		private bool DrawEntry (List<string> resource, string entry, int index)
		{
			EditorGUILayout.BeginHorizontal ();
			if (index >= defaultValues.Count) {
				resource [index] = EditorGUILayout.TextField ("Type (" + index + ")", entry);
				//if (GUILayout.Button ("X", GUILayout.Width (24f))) {
				//	EditorGUILayout.EndHorizontal ();
				//	return false;
				//}
			} else {
				EditorGUILayout.LabelField ("Type (" + index + ")", entry);
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

		//----------------------------------------------------------

		public List<string> GetCategoryList (DamageTypeResource resource)
		{
			return GetListFromType<string, DamageTypeResource> (resource, "damageCategories");
		}

		public List<TList> GetListFromType<TList, TResource> (TResource resource, string fieldName)
		{
			return resource.GetType ().GetField (fieldName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue (resource) as List<TList>;
		}
	}
}