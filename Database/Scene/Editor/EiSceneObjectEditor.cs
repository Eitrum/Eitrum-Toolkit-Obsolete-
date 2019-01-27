using System;
using UnityEditor;
using UnityEngine;

namespace Eitrum
{
	[CustomPropertyDrawer (typeof(EiSceneObject))]
	public class EiSceneObjectEditor : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight (property);
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var sceneProp = property.FindPropertyRelative ("sceneAssetObject");
			var nameProp = property.FindPropertyRelative ("sceneName");
			EditorGUI.ObjectField (position, sceneProp, typeof(SceneAsset), label);
			nameProp.stringValue = sceneProp.objectReferenceValue ? sceneProp.objectReferenceValue.name : "";
		}
	}
}

