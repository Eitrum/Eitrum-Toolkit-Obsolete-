using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum.Engine.EditorUtil {
    [InitializeOnLoad]
    public static class AssemblyReloadLock {

        private static EditorSettingWindow.Config config;
        private static bool locked = false;

        static AssemblyReloadLock() {
            config = EditorSettingWindow.AddConfiguration("In Game Assembly Lock");
            EditorApplication.update += LockCheck;
        }

        private static void LockCheck() {
            if (!locked && config != null && config.Enabled && EditorApplication.isPlaying && EditorApplication.isCompiling) {
                locked = true;
                EditorApplication.LockReloadAssemblies();
                EditorApplication.playModeStateChanged += PlaymodeChanged;
                Debug.Log(EditorColorConfiguration.TagText("Assembly Lock") + " Preventing assembly reload");
            }
        }

        private static void PlaymodeChanged(PlayModeStateChange state) {
            if (!EditorApplication.isPlaying) {
                locked = false;
                Debug.Log(EditorColorConfiguration.TagText("Assembly Lock") + " Unlocking assemblies to reload");
                EditorApplication.UnlockReloadAssemblies();
                EditorApplication.playModeStateChanged -= PlaymodeChanged;
            }
        }

    }
}