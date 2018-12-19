using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum.PhysicsExtension
{
	[CustomEditor (typeof(PhysicsLayerSettings))]
	public class PhysicsLayerSettingsEditor : Editor
	{
		static Vector2 scrollValue = Vector2.zero;

		public override void OnInspectorGUI ()
		{
			var ls = (PhysicsLayerSettings)target;
			if (ls.layerMask == null || ls.layerMask.Length != 32) {
				System.Array.Resize (ref ls.layerMask, 32);
			}
			scrollValue = EditorGUILayout.BeginScrollView (scrollValue, GUILayout.ExpandHeight (false));

			bool[] enabled = new bool[32];
			for (int i = 0; i < ls.layerMask.Length; i++) {
				var layerName = ls.GetLayerName (i);
				enabled [i] = !string.IsNullOrEmpty (layerName);
			}

			EditorGUILayout.LabelField ("Layout Collision Matrix");
			var rotatePos = GUILayoutUtility.GetLastRect ().position + new Vector2 (156f, 136f);
			var style = GUIStyle.none;
			style.alignment = TextAnchor.MiddleRight;
			GUILayout.Space (120f);
			for (int i = 31; i >= 0; i--) {
				if (enabled [i]) {
					var str = string.Format ("{1} ({0})", i, ls.GetLayerName (i));
					EditorGUIUtility.RotateAroundPivot (45f, rotatePos);
					GUI.Label (new Rect (rotatePos - new Vector2 (140f, 0f), new Vector2 (120f, 16f)), str, style);
					EditorGUIUtility.RotateAroundPivot (-45f, rotatePos);
					rotatePos += new Vector2 (20f, 0f);
				}
			}

			for (int i = 0; i < ls.layerMask.Length; i++) {
				if (enabled [i]) {
					var layerName = ls.GetLayerName (i);
					EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField (string.Format ("({0}) {1}", i, layerName), GUILayout.Width (120f));
					var value = ls.layerMask [i];
					for (int j = 0; j < 32 - i; j++) {
						if (enabled [31 - j]) {
							var bo = (value & 1 << j) == 1 << j;
							value = EditorGUILayout.Toggle (bo, GUILayout.Width (16f)) ? (value | (1 << j)) : (value & ~(1 << j));
						}
					}
					ls.layerMask [i] = value;
					EditorGUILayout.EndHorizontal ();
				}
			}
			EditorGUILayout.EndScrollView ();
		}
	}
}