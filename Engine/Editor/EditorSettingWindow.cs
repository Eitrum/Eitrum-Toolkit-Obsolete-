using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum.Engine.EditorUtil {
    [InitializeOnLoad]
    public class EditorSettingWindow : EditorWindow {

        #region Private classes

        public class Config {
            private string name = "";
            private bool configuration = false;

            public string Name => name;

            public bool Enabled => Configuration;

            public bool Disabled => !Configuration;

            public bool Configuration { get { return configuration; } set { if (configuration == value) return; configuration = value; Save(); } }

            public Config(string name) {
                this.name = name;
                Load();
            }

            public void Save() {
                EditorPrefs.SetBool(name, configuration);
                Debug.LogFormat("{0} - {1} has changed to {2}",
                    EditorColorConfiguration.TagText("Configuration"),
                    name,
                    EditorColorConfiguration.GetToggleText(configuration));
            }

            public void Load() {
                configuration = EditorPrefs.GetBool(name, false);
                //Debug.LogFormat("{0} - {1} is {2}", EditorColorConfiguration.TagText("Configuration"), name, EditorColorConfiguration.GetToggleText(configuration));
            }
        }

        #endregion

        #region Variables

        private static List<Config> configurations = new List<Config>();

        private Vector2 configurationScroll = new Vector2();

        #endregion

        #region Menu Items

        [MenuItem("Eitrum/Editor/Settings", priority = 0)]
        public static void Open() {
            GetWindow<EditorSettingWindow>("Settings");
        }

        #endregion

        #region Public static API

        public static Config AddConfiguration(string name) {
            var config = GetConfiguration(name);
            if (config != null) {
                return config;
            }
            config = new Config(name);
            configurations.Add(config);
            return config;
        }

        public static void RemoveConfiguration(string name) {
            for (int i = 0, length = configurations.Count; i < length; i++) {
                if (configurations[i].Name == name) {
                    configurations.RemoveAt(i);
                    return;
                }
            }
        }

        public static Config GetConfiguration(string name) {
            for (int i = 0, length = configurations.Count; i < length; i++) {
                if (configurations[i].Name == name) {
                    return configurations[i];
                }
            }
            return null;
        }

        #endregion

        #region Drawing

        private void OnGUI() {
            DrawConfigurations(new Rect(Vector2.zero, position.size));
        }

        private void DrawConfigurations(Rect rect) {
            GUILayout.BeginArea(rect);
            configurationScroll = GUILayout.BeginScrollView(configurationScroll);
            for (int i = 0, length = configurations.Count; i < length; i++) {
                var config = configurations[i];
                config.Configuration = EditorGUILayout.ToggleLeft(config.Name, config.Configuration);
            }


            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        #endregion
    }
}