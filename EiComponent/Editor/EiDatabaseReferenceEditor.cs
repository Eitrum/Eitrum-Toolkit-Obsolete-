using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum
{
	[CustomPropertyDrawer (typeof(EiDatabaseReference))]
	public class EiDatabaseReferenceEditor : PropertyDrawer
	{

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			List<string> items = new List<string> ();
			List<int> ids = new List<int> ();
			items.Add ("None");
			ids.Add (-1);
			EiDatabase database = EiDatabase.Instance;
			var currentSelectedId = property.FindPropertyRelative ("uniqueIdReference").intValue;
			var index = 0;

			var categories = database._Length;
			for (int i = 0; i < categories; i++) {
				var category = database [i];
				var entries = category.Length;
				for (int e = 0; e < entries; e++) {
					var entry = category [e];
					string path = string.Format ("{0} / {1}", category.CategoryName, entry.ItemName);
					var uniqueId = entry.UniqueId;
					if (uniqueId == currentSelectedId) {
						index = items.Count;
					}
					items.Add (path);
					ids.Add (uniqueId);
				}
			}

			property.FindPropertyRelative ("uniqueIdReference").intValue = ids [EditorGUI.Popup (position, property.displayName, index, items.ToArray ())];
		}
	}
}