using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Eitrum {

	public class EiSerializeInterfaceEditor : Editor {

		[MenuItem("Config/Generate Serialized Interfaces")]
		public static void GenerateInterfaces() {

		}

	}

	[CustomPropertyDrawer(typeof(EiPoolableComponent))]
	public class EiSerializeInterfaceEditorPoolableComponent : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.PropertyField(position, property.FindPropertyRelative("component"), label, false);
		}
	}

	[CustomPropertyDrawer(typeof(Networking.EiNetworkObservable))]
	public class EiSerializeInterfaceEditorNetworkObservable : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.PropertyField(position, property.FindPropertyRelative("component"), label, false);
		}
	}
}
