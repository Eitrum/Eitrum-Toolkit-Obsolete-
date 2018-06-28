using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Eitrum
{
	[CustomPropertyDrawer (typeof(Eitrum.Database))]
	public class EiDatabaseAttributeEditor : PropertyDrawer
	{
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var dbObj = attribute as Database;
			var go = property.objectReferenceValue;

			List<string> selection = new List<string> ();
			List<Vector2Int> vec2 = new List<Vector2Int> ();
			var cats = EiDatabase.Instance.categories;
			string category = "";
			string entryName = "";
			int selected = -1;
			for (int cat = 0; cat < cats.Count; cat++) {
				category = string.Format ("({0}) {1}", cat, cats [cat].name);
				var entries = cats [cat].entries;
				for (int ent = 0; ent < entries.Count; ent++) {
					entryName = string.Format ("({0}) {1}", ent, entries [ent].name);
					selection.Add (category + "/" + entryName);
					vec2.Add (new Vector2Int (cat, ent));
					if (go && entries [ent].targetObject.name == go.name) {
						selected = vec2.Count - 1;
					}
				}
			}
			if (selected == -1) {
				if (selection.Count == 0) {
					EditorGUI.LabelField (position, label.text, "No Items In Database");
					return;
				}
				selected = 0;
			}
			var id = EditorGUI.Popup (position, label.text, selected, selection.ToArray ());
			property.objectReferenceValue = EiDatabase.Instance.categories [vec2 [id].x].entries [vec2 [id].y].targetObject;
		}
	}
}

