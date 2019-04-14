using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum.Engine.Core
{
	[CustomPropertyDrawer (typeof(EiPropertyEventFloat))]
	public class EiPropertyEventEditor : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return 16f;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var field = property.FindPropertyRelative ("value");

			field.floatValue = EditorGUI.FloatField (position, property.displayName, field.floatValue);
		}
	}

	[CustomPropertyDrawer (typeof(EiPropertyEventVector3))]
	public class EiPropertyEventEditorVector3 : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return 16f;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var field = property.FindPropertyRelative ("value");
			field.vector3Value = EditorGUI.Vector3Field (position, property.displayName, field.vector3Value);
		}
	}

	[CustomPropertyDrawer (typeof(EiPropertyEventVector2))]
	public class EiPropertyEventEditorVector2 : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return 16f;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var field = property.FindPropertyRelative ("value");
			field.vector2Value = EditorGUI.Vector2Field (position, property.displayName, field.vector2Value);
		}
	}

	[CustomPropertyDrawer (typeof(EiPropertyEventVector4))]
	public class EiPropertyEventEditorVector4 : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return 16f;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var field = property.FindPropertyRelative ("value");
			field.vector4Value = EditorGUI.Vector4Field (position, property.displayName, field.vector4Value);
		}
	}

	[CustomPropertyDrawer (typeof(EiPropertyEventQuaternion))]
	public class EiPropertyEventEditorQuaternion : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return 16f;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var field = property.FindPropertyRelative ("value");
			field.quaternionValue = EditorGUI.Vector4Field (position, property.displayName, field.quaternionValue.ToVector4 ()).ToQuaternion ();
		}
	}

	[CustomPropertyDrawer (typeof(EiPropertyEventInt))]
	public class EiPropertyEventEditorInt : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return 16f;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var field = property.FindPropertyRelative ("value");
			field.intValue = EditorGUI.IntField (position, property.displayName, field.intValue);
		}
	}

	[CustomPropertyDrawer (typeof(EiPropertyEventBool))]
	public class EiPropertyEventEditorBool : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return 16f;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var propEvent = fieldInfo.GetValue (property.serializedObject.targetObject) as EiPropertyEventBool;
			propEvent.Value = EditorGUI.Toggle (position, property.displayName, propEvent.Value);
		}
	}
}