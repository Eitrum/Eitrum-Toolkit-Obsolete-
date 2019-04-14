using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Eitrum.Engine.EditorUtil {
    [InitializeOnLoad]
    public static class Autosave {

        private static EditorSettingWindow.Config configLostFocus;
        private static EditorSettingWindow.Config configTimer;

        private static bool hasSavedLostFocus = false;
        private static double timeSinceStartup;
        private const double fiveMin = 60d * 5d;

        static Autosave() {
            configLostFocus = EditorSettingWindow.AddConfiguration("Autosave (Lost Focus)");
            configTimer = EditorSettingWindow.AddConfiguration("Autosave (5min timer)");
            EditorApplication.update += Update;
        }

        static void Update() {
            if (configTimer != null && configTimer.Enabled && EditorApplication.timeSinceStartup - timeSinceStartup >= fiveMin) {
                timeSinceStartup = EditorApplication.timeSinceStartup;
                if (!hasSavedLostFocus) {
                    Save();
                }
            }

            if (configLostFocus != null && configLostFocus.Enabled) {
                var hasFocus = ApplicationIsActivated();
                if (!hasSavedLostFocus && !hasFocus) {
                    hasSavedLostFocus = true;
                    Save();
                }
                if (hasFocus) {
                    hasSavedLostFocus = false;
                }
            }
        }

        private static void Save() {
            AssetDatabase.SaveAssets();
            UnityEngine.Debug.Log(EditorColorConfiguration.TagText("Autosave") + " - Saving...");
        }

#if UNITY_EDITOR_WIN

        public static bool ApplicationIsActivated() {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero) {
                return false;
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

#else
        public static bool ApplicationIsActivated() {
            return true;
        }
#endif
    }
}