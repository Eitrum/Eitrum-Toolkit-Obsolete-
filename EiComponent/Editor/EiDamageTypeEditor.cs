using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Eitrum.Health
{
	[CustomPropertyDrawer (typeof(EiDamageType))]
	public class EiDamageTypeEditor : PropertyDrawer
	{
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.Integer) {
				List<string> items = new List<string> ();
				List<int> ids = new List<int> ();
				items.Add ("None");
				ids.Add (-1);

				EiDamageTypeResource resources = EiDamageTypeResource.Instance;
				var currentSelectedId = property.intValue;
				var index = 0;

				var categories = resources._Length;
				for (int i = 0; i < categories; i++) {
					var category = resources [i];
					var entries = category.Length;
					for (int e = 0; e < entries; e++) {
						var entry = category [e];
						string path = string.Format ("{0} / {1}", category.CategoryName, entry.DamageTypeName);
						var uniqueId = entry.UniqueDamageTypeId;
						if (uniqueId == currentSelectedId) {
							index = items.Count;
						}
						items.Add (path);
						ids.Add (uniqueId);
					}
				}

				property.intValue = ids [EditorGUI.Popup (position, property.displayName, index, items.ToArray ())];
			} else {
				EditorGUI.PropertyField (position, property, label);
			}
		}
	}
}