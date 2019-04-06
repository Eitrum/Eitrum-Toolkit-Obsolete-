using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Eitrum {
    public class EiComponentSettingsEditor : EditorWindow {
        #region Variables

        const string mcs = "Assets/mcs.rsp";
        const string csc = "Assets/csc.rsp";
        private static string reImportPath { get { return AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:MonoBehaviour")[0]); } }

        const string definePrefix = "-define:";

        string[] defines = new string[]{
            "EITRUM_PERFORMANCE_MODE",
            "EITRUM_POOLING",
            "EITRUM_NETWORKING",
            "EITRUM_ADVANCED_HEALTH"
        };

        bool[] enabledDefines = new bool[0];


        public bool useBuiltInPooling = true;

        #endregion

        #region Menu Item

        [MenuItem("Eitrum/Settings/Define Symbols")]
        public static void OpenSettings() {
            var win = GetWindow<EiComponentSettingsEditor>();
            win.titleContent = new GUIContent("Eitrum Settings");
            win.Show();
        }

        #endregion

        #region Helper

        void LoadFromFile() {
            enabledDefines = new bool[defines.Length];

            if (!File.Exists(mcs))
                return;

            List<string> existing = new List<string>(File.ReadAllLines(mcs));
            for (int i = 0; i < defines.Length; i++) {
                if (existing.Contains(definePrefix + defines[i]))
                    enabledDefines[i] = true;
            }
        }

        #endregion

        #region Core

        private void OnGUI() {
            if (defines.Length != enabledDefines.Length)
                LoadFromFile();

            for (int i = 0; i < defines.Length; i++)
                enabledDefines[i] = EditorGUILayout.ToggleLeft(defines[i], enabledDefines[i]);

            if (GUILayout.Button("Update", GUILayout.MaxWidth(200f))) {
                List<string> usedDefines = new List<string>();
                usedDefines.Add("-unsafe");
                for (int i = 0; i < defines.Length; i++)
                    if (enabledDefines[i])
                        usedDefines.Add(definePrefix + defines[i]);

                var array = usedDefines.ToArray();

                File.WriteAllLines(mcs, array);
                File.WriteAllLines(csc, array);
                PlayerSettings.allowUnsafeCode = true;

                AssetDatabase.ImportAsset(reImportPath, ImportAssetOptions.ForceUpdate);
                if (EditorUtility.DisplayDialog("Settings Updated", "You might need to restart scripting editor for it to take effect.\n\nSerialized objects and data might change as code gets removed.", "Ok"))
                    Close();
            }
        }

        #endregion
    }
}
