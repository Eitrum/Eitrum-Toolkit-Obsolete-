using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Eitrum.Database.Scene
{
	[CustomEditor (typeof(EiScene))]
	public class EiSceneInspector : Editor
	{
		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();
			var scene = serializedObject.FindProperty ("scene");
			var sceneAssetProperty = scene.FindPropertyRelative ("sceneAssetObject");
			EditorGUILayout.PropertyField (scene);

			if (sceneAssetProperty.objectReferenceValue != null) {
				var path = AssetDatabase.GetAssetPath (sceneAssetProperty.objectReferenceValue);
				var guid = AssetDatabase.AssetPathToGUID (path);
				var loadedScene = EditorSceneManager.GetSceneByPath (path);
				bool found = false;
				int index = 0;
				var buildScenes = EditorBuildSettings.scenes;
				for (int i = 0; i < buildScenes.Length; i++) {
					if (buildScenes [i].guid.ToString () == guid) {
						found = true;
						index = i;
					}
				}
				if (loadedScene.IsValid ()) {
					if (EditorSceneManager.sceneCount > 1) {
						EditorGUILayout.BeginHorizontal ();
						if (GUILayout.Button ("Close", GUILayout.Width (50))) {
							EditorSceneManager.CloseScene (loadedScene, true);
						}
						if (GUILayout.Button ("Set Active", GUILayout.Width (100))) {
							EditorSceneManager.SetActiveScene (loadedScene);
						}
						EditorGUILayout.EndHorizontal ();
					}
				} else {
					if (GUILayout.Button ("Open", GUILayout.Width (50))) {
						EditorSceneManager.OpenScene (path, OpenSceneMode.Additive);
					}
				}

				if (found) {
					if (GUILayout.Button ("Remove scene from build", GUILayout.Width (170)) && EditorUtility.DisplayDialog ("Remove Scene", "You sure you want to remove this scene from the build setting", "Yes", "Cancel")) {
						buildScenes = buildScenes.Remove (index);
						EditorBuildSettings.scenes = buildScenes;
					}
				} else {
					if (GUILayout.Button ("Add Scene to build settings", GUILayout.Width (200))) {
						buildScenes = buildScenes.Add (new EditorBuildSettingsScene (path, true));
						EditorBuildSettings.scenes = buildScenes;
					}
				}
			}
		}
	}
}