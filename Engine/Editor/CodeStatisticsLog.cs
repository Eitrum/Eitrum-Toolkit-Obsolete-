using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum.Engine.EditorUtil {
    [InitializeOnLoad]
    public static class CodeStatisticsLog {

        private static EditorSettingWindow.Config config;

        static CodeStatisticsLog() {
            config = EditorSettingWindow.AddConfiguration("Code Statistics");
            if (config != null && config.Enabled) {
                Print();
            }
        }

        private class Log {
            public int scripts = 0;
            public int linesOfCode = 0;
            public int commentLines = 0;
            public int emptyLines = 0;
            public int codeLines = 0;

            public void Fill(string assetGuid) {
                var path = AssetDatabase.GUIDToAssetPath(assetGuid);
                if (!path.StartsWith("Assets/"))
                    return;
                scripts++;
                var ta = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                var lines = ta.text.Split('\n');
                var length = lines.Length;
                linesOfCode += length;
                for (int i = 0; i < length; i++) {
                    var line = lines[i].Trim();
                    if (line.StartsWith("#") || line.StartsWith("//")) {
                        commentLines++;
                    }
                    else if (string.IsNullOrEmpty(line)) {
                        emptyLines++;
                    }
                    else {
                        codeLines++;
                    }
                }
            }

            public void Print() {
                var result = EditorColorConfiguration.TagText("Code Statistics");
                result += string.Format(" - Lines of code\t({0:N0})", linesOfCode);
                result += string.Format("\n - Scripts\t\t\t({0:N0})", scripts);
                result += string.Format("\n - Lines of actual code\t\t({0:N0})", codeLines);
                result += string.Format("\n - Lines of comments\t\t({0:N0})", commentLines);
                result += string.Format("\n - Empty Lines\t\t({0:N0})", emptyLines);
                result += string.Format("\n - Percentage Of Lines is code\t({0:P})", (double)codeLines / (double)linesOfCode);
                Debug.Log(result);
            }
        }

        [MenuItem("Eitrum/Editor/Print Code Statistics", priority = 1)]
        public static void Print() {
            var assets = AssetDatabase.FindAssets("t:MonoScript");
            var log = new Log();

            for (int i = 0, length = assets.Length; i < length; i++) {
                log.Fill(assets[i]);
            }

            log.Print();
        }
    }
}