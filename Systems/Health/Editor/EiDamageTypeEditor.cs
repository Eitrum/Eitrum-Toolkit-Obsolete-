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
		static string path = "Assets/Eitrum/Resources/DamageTypeResource.asset";

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.Integer) {
				List<string> items = new List<string> ();
				List<int> ids = new List<int> ();
				items.Add ("None");
				ids.Add (-1);

				var resources = AssetDatabase.LoadAssetAtPath<DamageTypeResource> (path);
				if (!resources) {
                    var tempObj = ScriptableObject.CreateInstance<DamageTypeResource>();
					EiDamageTypeResourceEditor.LoadDefaultValues ();
					var list = EiDamageTypeResourceEditor.defaultValues;
                    tempObj.GetType ().GetField ("damageCategories", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (tempObj, list);
                    AssetDatabase.CreateAsset(tempObj, path);
                    resources = tempObj;

                }

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

				Rect popupPosition = new Rect(position);
				popupPosition.width -= 20f;
				property.intValue = ids[EditorGUI.Popup(popupPosition, property.displayName, index, items.ToArray())];

				Rect databaseReferencePosition = new Rect(position);
				databaseReferencePosition.x += position.width - 20f;
				databaseReferencePosition.width = 20f;
				if (GUI.Button(databaseReferencePosition, "~"))
				{
					Selection.activeObject = resources;
				}

			} else {
				EditorGUI.PropertyField (position, property, label);
			}
		}
	}
}