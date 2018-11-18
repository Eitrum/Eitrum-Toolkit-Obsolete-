using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Eitrum.Mathematics;

namespace Eitrum
{
	[CustomPropertyDrawer (typeof(AnimationCurve))]
	public class AnimationCurveEditorExtension : PropertyDrawer
	{
		static Rect target;
		static AnimationCurve curve;
		static bool change = false;

		public static void Apply (AnimationCurve curve)
		{
			AnimationCurveEditorExtension.curve = curve;
			change = true;
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight (property);
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var width = 65f;
			var curveField = new Rect (position);
			curveField.width -= width;
			property.animationCurveValue = EditorGUI.CurveField (curveField, label.text, property.animationCurveValue);
			var selectButton = new Rect (position);
			selectButton.x += curveField.width;
			selectButton.width = width;
			if (GUI.Button (selectButton, "Generate")) {
				target = position;
				change = false;
				PopupWindow.Show (position, new AnimationCurveGeneratorPopup (property));
			}
			if (target == position && change) {
				property.animationCurveValue = curve;
				change = false;
			}
		}
	}

	public class AnimationCurveGeneratorPopup : PopupWindowContent
	{
		public SerializedProperty property;

		private EaseFunction easeFunction;
		private EaseType easeType;
		private int keyFrames = 8;
		private bool invert = false;

		public AnimationCurveGeneratorPopup (SerializedProperty property)
		{
			this.property = property;
		}

		public override Vector2 GetWindowSize ()
		{
			return new Vector2 (250f, 112f);
		}

		public override void OnGUI (Rect rect)
		{
			GUILayout.BeginArea (rect);
			GUILayout.Label ("Animation Curve Generator", EditorStyles.boldLabel);

			easeFunction = (EaseFunction)EditorGUILayout.EnumPopup ("Ease Function", easeFunction);
			easeType = (EaseType)EditorGUILayout.EnumPopup ("Ease Type", easeType);
			keyFrames = Mathf.Clamp (EditorGUILayout.IntField ("Key Frames", keyFrames), 4, 100);
			invert = EditorGUILayout.ToggleLeft ("Invert", invert);

			if (GUILayout.Button ("Generate")) {
				AnimationCurveEditorExtension.Apply (EiEase.GetAnimationCurve (EiEase.GetEaseFunction (easeFunction, easeType), keyFrames, invert));
			}
			GUILayout.EndArea ();
		}
	}
}