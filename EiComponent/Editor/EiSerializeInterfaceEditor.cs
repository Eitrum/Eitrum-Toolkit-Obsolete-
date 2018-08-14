using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace Eitrum
{
	[CustomPropertyDrawer(typeof(EiPoolableComponent))]
	public class EiSerializeInterfaceEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField(position, property.FindPropertyRelative("component"), label, false);
		}
	}
}
