using System;
using UnityEditor;
using UnityEngine;

namespace Eitrum.Mathematics {
    [CustomPropertyDrawer(typeof(Line))]
    public class LineEditor : PropertyDrawer {

        static bool useDirection = false;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property) * 2f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var topRect = GetRect(position, 0);
            var botRect = GetRect(position, 1);
            var buttonRect = new Rect(botRect);
            buttonRect.width = 60;
            GUI.Box(position, "");
            var startPoint = GetVector3Value(property, 0);
            var endPoint = GetVector3Value(property, 1);

            var direction = endPoint - startPoint;

            var newStartPoint = EditorGUI.Vector3Field(topRect, new GUIContent(label.text + "\t(Start Point)", label.tooltip), startPoint);

            if (newStartPoint != startPoint) {
                SetVector3Value(property, 0, newStartPoint);
            }

            if (useDirection) {
                var newDirection = EditorGUI.Vector3Field(botRect, new GUIContent("\t(Direction)"), direction);
                var newEndPoint = newStartPoint + newDirection;
                if (endPoint != newEndPoint) {
                    SetVector3Value(property, 1, newEndPoint);
                }
            }
            else {
                var newEndPoint = EditorGUI.Vector3Field(botRect, new GUIContent("\t(End Point)"), endPoint);
                if (newEndPoint != endPoint) {
                    SetVector3Value(property, 1, newEndPoint);
                }
            }

            if (GUI.Button(buttonRect, "Switch")) {
                useDirection = !useDirection;
            }

        }

        void DrawDirectionField(Rect botRect, GUIContent conent, SerializedProperty property) {
            var start = GetVector3Value(property, 0);
            var end = GetVector3Value(property, 1);
            var dir = end - start;
            var output = EditorGUI.Vector3Field(botRect, conent, dir);
            if (output != dir) {
                SetVector3Value(property, 1, start + output);
            }
        }

        static Vector3 GetVector3Value(SerializedProperty property, int index) {
            var x = property.FindPropertyRelative($"p{index}x");
            var y = property.FindPropertyRelative($"p{index}y");
            var z = property.FindPropertyRelative($"p{index}z");

            return new Vector3(x.floatValue, y.floatValue, z.floatValue);
        }

        static void SetVector3Value(SerializedProperty property, int index, Vector3 value) {
            var x = property.FindPropertyRelative($"p{index}x");
            var y = property.FindPropertyRelative($"p{index}y");
            var z = property.FindPropertyRelative($"p{index}z");
            x.floatValue = value.x;
            y.floatValue = value.y;
            z.floatValue = value.z;
        }

        static Rect GetRect(Rect rect, int index) {
            var r = new Rect(rect);
            r.height /= 2f;
            r.y += r.height * index;
            return r;
        }

    }
}
