using UnityEngine;
using UnityEditor;

namespace Eitrum.Health
{
	[CustomPropertyDrawer(typeof(EiCombatData))]
	public class EiCombatDataEditor : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (property.isExpanded)
			{
				return EditorGUI.GetPropertyHeight(property, true);
			}
			return base.GetPropertyHeight(property, label);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var name = label.text;
			if (property.isExpanded)
			{
				EditorGUI.PropertyField(position, property, label, true);

				Rect foldoutRect = new Rect(position);
				foldoutRect.height = base.GetPropertyHeight(property, label);
				property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, name, true);
				if (!property.isExpanded)
					EditorUtility.SetDirty(property.serializedObject.targetObject);
			}
			else
			{
				Rect foldoutRect = new Rect(position);
				foldoutRect.height = base.GetPropertyHeight(property, label);
				foldoutRect.width = 55f;
				property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, name, true);
				if (property.isExpanded)
					EditorUtility.SetDirty(property.serializedObject.targetObject);

				var flatProperty = property.FindPropertyRelative("flatAmount");
				position.x += 55f;
				flatProperty.floatValue = EditorGUI.FloatField(position, " (Flat Amount)", flatProperty.floatValue);
			}
		}
	}
}
