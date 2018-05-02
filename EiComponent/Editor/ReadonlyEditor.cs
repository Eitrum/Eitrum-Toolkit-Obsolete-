using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer (typeof(Readonly))]
public class ReadonlyEditor : PropertyDrawer
{
	public override void OnGUI (UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
	{
		GUI.enabled = false;
		EditorGUI.PropertyField (position, property, label, true);
		GUI.enabled = true;
	}
}
