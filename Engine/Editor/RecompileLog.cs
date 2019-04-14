using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum.Engine.EditorUtil {
    [InitializeOnLoad]
    public static class RecompileLog {

        static EditorSettingWindow.Config config;

        static RecompileLog() {
            config = EditorSettingWindow.AddConfiguration("Recompile Log");
            AssemblyReloadEvents.beforeAssemblyReload += Log;
        }

        static void Log() {
            if (config != null && config.Configuration) {
                Debug.Log(EditorColorConfiguration.UtilityText("------------------ [ Recompile ] ------------------"));
            }
        }

        [MenuItem("Eitrum/Editor/Recompile")]
        public static void Recompile() {
            var assets = AssetDatabase.FindAssets("t:MonoScript");
            var path = AssetDatabase.GUIDToAssetPath(assets[0]);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
    }
}