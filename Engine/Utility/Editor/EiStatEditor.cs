using System;
using UnityEngine;
using UnityEditor;

namespace Eitrum {
	[CustomPropertyDrawer(typeof(EiStat), true)]
	public class EiStatEditor : PropertyDrawer {

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			if (property.isExpanded)
				return base.GetPropertyHeight(property, label) * 4f;
			return base.GetPropertyHeight(property, label);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var folded = property.isExpanded;
			var baseStat = property.FindPropertyRelative("baseStat");
			var statMultiplier = property.FindPropertyRelative("statMultiplier");
			var statMultiplierX = property.FindPropertyRelative("statMultiplierX");
			var floatValues = new float[] {
				baseStat.floatValue,
				statMultiplier.floatValue,
				statMultiplierX.floatValue
			};

			if (!folded) {


				var labels = new GUIContent[] {
					new GUIContent ("B"),
					new GUIContent ("X"),
					new GUIContent ("Y")
				};
				EditorGUI.MultiFloatField(position, label, labels, floatValues);

			}
			else {
				EditorGUI.LabelField(position, label);
				var tempPosition = new Rect(position);
				var height = base.GetPropertyHeight(property, label);
				tempPosition.x += 20f;
				tempPosition.height = height;
				tempPosition.width -= 20f;
				tempPosition.y += height;
				floatValues[0] = EditorGUI.FloatField(tempPosition, "Base Stat", floatValues[0]);
				tempPosition.y += height;
				floatValues[1] = EditorGUI.FloatField(tempPosition, "Stat Multiplier", floatValues[1]);
				tempPosition.y += height;
				floatValues[2] = EditorGUI.FloatField(tempPosition, "Stat MultiplierX", floatValues[2]);

			}
			var changed = EditorGUI.Foldout(position, folded, "", true);
			if (changed != folded) {
				property.isExpanded = changed;
				EditorUtility.SetDirty(property.serializedObject.targetObject);
			}

			baseStat.floatValue = floatValues[0];
			statMultiplier.floatValue = floatValues[1];
			statMultiplierX.floatValue = floatValues[2];
		}
	}
}

