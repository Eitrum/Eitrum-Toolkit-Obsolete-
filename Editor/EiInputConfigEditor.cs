using System;
using UnityEngine;
using UnityEditor;

namespace Eitrum
{
	[CustomEditor (typeof(EiInputConfig))]
	public class EiInputConfigEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			var config = (EiInputConfig)target;
			GUILayout.Space (12f);
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Input Key");
			GUILayout.Label ("Output Key");
			GUILayout.EndHorizontal ();

			for (int i = 0; i < config.mappedKeys.Count; i++) {
				GUILayout.BeginHorizontal ();

				config.mappedKeys [i].inputKey = (KeyCode)EditorGUILayout.EnumPopup (config.mappedKeys [i].inputKey);
				config.mappedKeys [i].outputKey = (KeyCode)EditorGUILayout.EnumPopup (config.mappedKeys [i].outputKey);
				if (GUILayout.Button ("-")) {
					config.mappedKeys.RemoveAt (i);
					i--;
				}
				GUILayout.EndHorizontal ();
			}

			if (GUILayout.Button ("Add KeyCode Map")) {
				config.mappedKeys.Add (new EiInputConfig.EiInputMapKey ());
			}


			GUILayout.Space (18f);
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Input Axis");
			GUILayout.Label ("Output Axis");
			GUILayout.EndHorizontal ();

			for (int i = 0; i < config.mappedAxises.Count; i++) {
				GUILayout.BeginHorizontal ();

				config.mappedAxises [i].inputAxis = EditorGUILayout.TextField (config.mappedAxises [i].inputAxis);
				config.mappedAxises [i].outputAxis = EditorGUILayout.TextField (config.mappedAxises [i].outputAxis);
				if (GUILayout.Button ("-")) {
					config.mappedAxises.RemoveAt (i);
					i--;
				}
				GUILayout.EndHorizontal ();
			}

			if (GUILayout.Button ("Add Axis Map")) {
				config.mappedAxises.Add (new EiInputConfig.EiInputMapAxis ());
			}
		}
	}
}

