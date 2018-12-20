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
		private GUIStyle style;

		public override void OnInspectorGUI ()
		{
			var ls = (PhysicsLayerSettings)target;
			ls.SafetySecureLayerMasks ();
			scrollValue = EditorGUILayout.BeginScrollView (scrollValue, GUILayout.ExpandHeight (false));

			bool[] enabled = new bool[32];
			for (int i = 0; i < 32; i++) {
				enabled [i] = ls.HasLayer (i);
			}

			EditorGUILayout.LabelField ("Layout Collision Matrix");
			var rotatePos = GUILayoutUtility.GetLastRect ().position + new Vector2 (156f, 136f);
			if (style == null) {
				style = GUIStyle.none;
				style.alignment = TextAnchor.MiddleRight;
			}
			GUILayout.Space (120f);
			for (int i = 31; i >= 0; i--) {
				if (enabled [i]) {
					var str = string.Format ("{1} ({0})", i, ls.LayerToName (i));
					EditorGUIUtility.RotateAroundPivot (45f, rotatePos);
					GUI.Label (new Rect (rotatePos - new Vector2 (140f, 0f), new Vector2 (120f, 16f)), str, style);
					EditorGUIUtility.RotateAroundPivot (-45f, rotatePos);
					rotatePos += new Vector2 (20f, 0f);
				}
			}

			for (int i = 0; i < 32; i++) {
				if (enabled [i]) {
					var layerName = ls.LayerToName (i);
					EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField (string.Format ("({0}) {1}", i, layerName), GUILayout.Width (120f));
					for (int j = 0; j < 32 - i; j++) {
						if (enabled [31 - j]) {
							if (j < 32 - i) {
								ls.IgnoreLayerCollision (i, 31 - j, !EditorGUILayout.Toggle (!ls.GetIgnoreLayerCollision (i, 31 - j), GUILayout.Width (16f)));
							}
						}
					}
					EditorGUILayout.EndHorizontal ();
				}
			}
			EditorGUILayout.EndScrollView ();
		}
	}
}