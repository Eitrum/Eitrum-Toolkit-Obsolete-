using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Eitrum
{
	[CustomPropertyDrawer (typeof(TypeFilter))]
	public class TypeFilterEditor : PropertyDrawer
	{
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var typeFilter = attribute as TypeFilter;

			if (property.objectReferenceValue != null) {
				if (property.objectReferenceValue is GameObject) {
					property.objectReferenceValue = (property.objectReferenceValue as GameObject).GetComponent (typeFilter.type);
				} else {
					var typeOfObject = property.objectReferenceValue.GetType ();
					var interfaces = typeOfObject.FindInterfaces (new System.Reflection.TypeFilter (MyInterfaceFilter), typeFilter.type.FullName);
					if (interfaces.Length == 0) {
						property.objectReferenceValue = null;
					}
				}
			}

			property.objectReferenceValue = EditorGUI.ObjectField (position, label.text, property.objectReferenceValue, typeFilter.type, true);
		}

		public static bool MyInterfaceFilter (Type typeObj, object criteriaObj)
		{
			if (typeObj.ToString () == criteriaObj.ToString ())
				return true;
			else
				return false;
		}
	}

	[CustomPropertyDrawer (typeof(TypeFilterScene))]
	public class TypeFilterSceneEditor : PropertyDrawer
	{
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var typeFilter = typeof(SceneAsset);
			property.objectReferenceValue = EditorGUI.ObjectField (position, label.text, property.objectReferenceValue, typeFilter, false);
		}
	}
}