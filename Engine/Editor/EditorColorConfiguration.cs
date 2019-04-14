using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum.Engine.EditorUtil {
    public static class EditorColorConfiguration {
        public static string TagColor {
            get {
                if (EditorGUIUtility.isProSkin) {
                    return "aqua";
                }
                return "blue";
            }
        }

        public static string UtilityColor {
            get {
                return "magenta";
            }
        }

        public static string ActiveColor {
            get {
                return "green";
            }
        }

        public static string InactiveColor {
            get {
                return "red";
            }
        }

        public static string GetToggleText(bool isActive) {
            return string.Format("<color={0}>{1}</color>", isActive ? ActiveColor : InactiveColor, isActive ? "Active" : "Inactive");
        }

        public static string GetToggleText(string text, bool isActive) {
            return string.Format("<color={0}>{1}</color>", isActive ? ActiveColor : InactiveColor, text);
        }

        public static string TagText(string text) {
            return string.Format("<color={0}>[{1}]</color>", TagColor, text);
        }

        public static string UtilityText(string text) {
            return string.Format("<color={0}>{1}</color>", UtilityColor, text);
        }
    }
}