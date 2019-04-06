using System;
using UnityEditor;
using UnityEngine;

namespace Eitrum {
	[CustomPropertyDrawer(typeof(Optional), true)]
	public class OptionalEditor : PropertyDrawer {

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			label.text += " (Optional) ";
			EditorGUI.PropertyField(position, property, label, true);
		}
	}
}