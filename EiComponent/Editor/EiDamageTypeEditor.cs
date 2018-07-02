using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

namespace Eitrum.Health
{
	[CustomPropertyDrawer (typeof(EiDamageType))]
	public class EiDamageTypeEditor : PropertyDrawer
	{
		static string path = "Assets/Eitrum/Health/DamageTypes/EiDamageTypeResource.prefab";

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.Integer) {
				List<string> items = new List<string> ();
				List<int> ids = new List<int> ();
				items.Add ("None");
				ids.Add (-1);

				var go = AssetDatabase.LoadAssetAtPath<GameObject> (path);
				if (!go) {
					var tempObj = new GameObject ("EiDamageTypeResource", typeof(EiDamageTypeResource));
					EiDamageTypeResourceEditor.LoadDefaultValues ();
					var list = EiDamageTypeResourceEditor.defaultValues;
					var tempResource = tempObj.GetComponent<EiDamageTypeResource> ();
					tempResource.GetType ().GetField ("damageCategories", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (tempResource, list);
					go = PrefabUtility.CreatePrefab (path, tempObj);
					UnityEngine.MonoBehaviour.DestroyImmediate (tempObj);
				}

				EiDamageTypeResource resources = go.GetComponent<EiDamageTypeResource> ();
				var currentSelectedId = property.intValue;
				var index = 0;

				var categories = resources.Length;

				for (int i = 0; i < categories; i++) {
					var name = resources [i];
					if (name.Length == 0) {
						name = string.Format ("Empty / DamageType ({0})", i);
					} else {
						var tempName = name.Replace (" ", "");
						if (tempName.Length == 0) {
							name = string.Format ("Empty / DamageType ({0})", i);
						}
					}
					if (currentSelectedId == i) {
						index = items.Count;
					}
					items.Add (name);
					ids.Add (i);
				}

				property.intValue = ids [EditorGUI.Popup (position, property.displayName, index, items.ToArray ())];
			} else {
				EditorGUI.PropertyField (position, property, label);
			}
		}
	}
}