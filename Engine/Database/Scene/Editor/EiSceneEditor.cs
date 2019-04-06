using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Eitrum {
	[CustomPropertyDrawer(typeof(EiScene))]
	public class EiSceneEditor : PropertyDrawer {
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUI.GetPropertyHeight(property, true);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var scene = SceneManager.GetSceneByBuildIndex(0);
			if (scene == default(Scene)) {
			}
			EditorGUI.PropertyField(position, property, label, true);
		}
	}
}
