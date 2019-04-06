using System;
using UnityEngine;
using UnityEditor;

namespace Eitrum.Mathematics {
    [CustomPropertyDrawer(typeof(EiSpline))]
    public class SplineEditor : PropertyDrawer {
        #region Variables

        bool isEdit = false;
        public bool isLocalView = true;
        public EiSpline spline;

        private UnityEngine.Object targetObject;
        private Transform offset;

        #endregion

        #region Property Height

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, false);
        }

        #endregion

        #region Setup Callback

        private void GetSpline(SerializedProperty property) {
            targetObject = property.serializedObject.targetObject;
            offset = ((Component)targetObject).transform;
            spline = (EiSpline)targetObject.GetType().GetField(property.name).GetValue(targetObject);
        }

        #endregion

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            GetSpline(property);

            GUI.Label(position, label.text);
            // Rect Generation
            var editButton = new Rect(position);
            editButton.width = 80f;
            editButton.x = position.x + position.width - 80f;

            var localButton = new Rect(editButton);
            localButton.x -= 80f;

            var freeHandleButton = new Rect(localButton);
            freeHandleButton.x -= 100f;
            freeHandleButton.width = 100f;

            var loopButton = new Rect(freeHandleButton);
            loopButton.x -= 60f;
            loopButton.width = 60f;


            if (!isEdit && GUI.Button(editButton, "Edit")) {
                OnEditStart();
            }
            if (isEdit) {
                if (GUI.Button(editButton, "Save")) {
                    OnEditEnd();
                }
                if (GUI.Button(localButton, isLocalView ? "To World" : "To Local")) {
                    isLocalView = !isLocalView;
                    SceneView.RepaintAll();
                }

                if (GUI.Button(freeHandleButton, IsFreeHandle ? "Free Handle" : "Constrained")) {
                    IsFreeHandle = !IsFreeHandle;
                    SceneView.RepaintAll();
                }

                if (!spline.IsLooping && GUI.Button(loopButton, "Loop")) {
                    spline.SetLooping();
                    SceneView.RepaintAll();
                }
            }
        }

        void OnSceneGUI(SceneView sceneView) {
            if (spline == null)
                return;
            if (targetObject == null || Selection.activeGameObject != ((Component)targetObject).gameObject) {
                OnEditEnd();
                return;
            }
            for (int i = 0; i < spline.CurveCount; i++) {
                var b = spline[i];
                // Start Point
                {
                    var newSp = EditPoint(b[0]);
                    var diff = newSp - b[0];
                    b[0] = newSp;
                    b[1] += diff;
                    if (i > 0) {
                        var oldB = spline[i - 1];
                        oldB[3] = newSp;
                        oldB[2] += diff;
                        spline[i - 1] = oldB;
                    }
                    if (i == 0 && spline.IsLooping) {
                        var oldB = spline[spline.CurveCount - 1];
                        oldB[3] = newSp;
                        oldB[2] += diff;
                        spline[spline.CurveCount - 1] = oldB;
                    }
                }
                // Start Handle
                {
                    var newSh = EditPoint(b[1]);
                    b[1] = newSh;
                    if (!IsFreeHandle && i > 0) {
                        var oldB = spline[i - 1];
                        oldB[2] = b[0] + (b[0] - b[1]);
                        spline[i - 1] = oldB;
                    }
                    if (!IsFreeHandle && i == 0 && spline.IsLooping) {
                        var oldB = spline[spline.CurveCount - 1];
                        oldB[2] = b[0] + (b[0] - b[1]);
                        spline[spline.CurveCount - 1] = oldB;
                    }
                }
                // End Handle
                {
                    var newEh = EditPoint(b[2]);
                    b[2] = newEh;
                    if (!IsFreeHandle && i < spline.CurveCount - 1) {
                        var nextB = spline[i + 1];
                        nextB[1] = b[3] + (b[3] - b[2]);
                        spline[i + 1] = nextB;
                    }
                    if (!IsFreeHandle && i == spline.CurveCount - 1 && spline.IsLooping) {
                        var nextB = spline[0];
                        nextB[1] = b[3] + (b[3] - b[2]);
                        spline[0] = nextB;
                    }
                }

                // End Point
                if (i == spline.CurveCount - 1 && !spline.IsLooping) {
                    var newEp = EditPoint(b[3]);
                    var diff = newEp - b[3];
                    b[3] = newEp;
                    b[2] += diff;
                }

                spline[i] = b;
                DrawBezierLine(b);
            }
        }

        private void DrawBezierLine(EiBezier b) {
            if (isLocalView) {
                var pos = offset.position;
                var rot = offset.rotation;
                var scale = offset.lossyScale;
                Handles.DrawBezier(
                    pos + rot * b[0].ScaleReturn(scale),
                    pos + rot * b[3].ScaleReturn(scale),
                    pos + rot * b[1].ScaleReturn(scale),
                    pos + rot * b[2].ScaleReturn(scale),
                    Color.white, null, 1f * scale.magnitude);
                Handles.DrawDottedLine(pos + rot * b[0], pos + rot * b[1], 10f * scale.magnitude);
                Handles.DrawDottedLine(pos + rot * b[2], pos + rot * b[3], 10f * scale.magnitude);
            }
            else {
                Handles.DrawBezier(b[0], b[3], b[1], b[2], Color.white, null, 1f);
                Handles.DrawDottedLine(b[0], b[1], 10f);
                Handles.DrawDottedLine(b[2], b[3], 10f);
            }
        }

        private Vector3 EditPoint(Vector3 position) {
            if (isLocalView) {
                var scale = offset.lossyScale;
                var cubePos = Handles.DoPositionHandle(offset.position + offset.rotation * position.ScaleReturn(scale), offset.rotation);
                cubePos = (Quaternion.Inverse(offset.rotation) * (cubePos - offset.position)).ScaleReturn(new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z));
                return cubePos;
            }
            else {
                var vec = Handles.DoPositionHandle(position, Quaternion.identity);
                return vec;
            }
        }

        #region Reflection

        public bool IsFreeHandle {
            get {
                if (spline == null)
                    return false;
                return (bool)spline.GetType().GetField("freeHandle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(spline);
            }
            set {
                if (spline != null)
                    spline.GetType().GetField("freeHandle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(spline, value);
            }
        }

        #endregion

        #region On Edit Callbacks

        void OnEditStart() {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            SceneView.onSceneGUIDelegate += OnSceneGUI;
            isEdit = true;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(targetObject);
        }

        void OnEditEnd() {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            isEdit = false;
            SceneView.RepaintAll();
            EditorUtility.SetDirty(targetObject);
        }

        #endregion
    }
}