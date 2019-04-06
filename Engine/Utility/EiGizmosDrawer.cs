using UnityEngine;

namespace Eitrum.EditorUtil {
    public class EiGizmosDrawer {

        #region Draw Mesh

        public static void DrawGameObject(GameObject gameObject) {
            var meshes = gameObject.GetComponentsInChildren<MeshFilter>();
            for (int i = 0; i < meshes.Length; i++) {
                var mesh = meshes[i];
                Gizmos.DrawMesh(mesh.sharedMesh, mesh.transform.position, mesh.transform.rotation, mesh.transform.lossyScale);
            }
        }

        public static void DrawGameObject(GameObject gameObject, Vector3 position, Quaternion rotation) {
            var meshes = gameObject.GetComponentsInChildren<MeshFilter>();
            for (int i = 0; i < meshes.Length; i++) {
                var mesh = meshes[i];
                var pos = mesh.transform.position + position;
                var rot = mesh.transform.rotation * rotation;
                Gizmos.DrawMesh(mesh.sharedMesh, pos, rot, mesh.transform.lossyScale);
            }
        }

        public static void DrawPrefab(EiPrefab prefab) {
            DrawGameObject(prefab.GameObject);
        }

        public static void DrawPrefab(EiPrefab prefab, Vector3 position, Quaternion rotation) {
            DrawGameObject(prefab.GameObject, position, rotation);
        }

        #endregion

        #region Draw Wire Mesh

        public static void DrawWireGameObject(GameObject gameObject) {
            var meshes = gameObject.GetComponentsInChildren<MeshFilter>();
            for (int i = 0; i < meshes.Length; i++) {
                var mesh = meshes[i];
                Gizmos.DrawWireMesh(mesh.sharedMesh, mesh.transform.position, mesh.transform.rotation, mesh.transform.lossyScale);
            }
        }

        public static void DrawWireGameObject(GameObject gameObject, Vector3 position, Quaternion rotation) {
            var meshes = gameObject.GetComponentsInChildren<MeshFilter>();
            for (int i = 0; i < meshes.Length; i++) {
                var mesh = meshes[i];
                var pos = mesh.transform.position + position;
                var rot = mesh.transform.rotation * rotation;
                Gizmos.DrawWireMesh(mesh.sharedMesh, pos, rot, mesh.transform.lossyScale);
            }
        }

        public static void DrawWirePrefab(EiPrefab prefab) {
            DrawWireGameObject(prefab.GameObject);
        }

        public static void DrawWirePrefab(EiPrefab prefab, Vector3 position, Quaternion rotation) {
            DrawWireGameObject(prefab.GameObject, position, rotation);
        }

        #endregion

        #region Draw Wire Cube Rotation Included

        public static void DrawWireCube(Vector3 point, Quaternion rotation, Vector3 size) {
            size /= 2f;
            var lbc = point + rotation * new Vector3(-size.x, -size.y, -size.z);
            var rbc = point + rotation * new Vector3(size.x, -size.y, -size.z);

            var luc = point + rotation * new Vector3(-size.x, size.y, -size.z);
            var ruc = point + rotation * new Vector3(size.x, size.y, -size.z);

            var lbf = point + rotation * new Vector3(-size.x, -size.y, size.z);
            var rbf = point + rotation * new Vector3(size.x, -size.y, size.z);

            var luf = point + rotation * new Vector3(-size.x, size.y, size.z);
            var ruf = point + rotation * new Vector3(size.x, size.y, size.z);

            Gizmos.DrawLine(lbc, rbc);
            Gizmos.DrawLine(rbc, ruc);
            Gizmos.DrawLine(ruc, luc);
            Gizmos.DrawLine(luc, lbc);

            Gizmos.DrawLine(lbf, rbf);
            Gizmos.DrawLine(rbf, ruf);
            Gizmos.DrawLine(ruf, luf);
            Gizmos.DrawLine(luf, lbf);

            Gizmos.DrawLine(lbc, lbf);
            Gizmos.DrawLine(rbc, rbf);
            Gizmos.DrawLine(luc, luf);
            Gizmos.DrawLine(ruc, ruf);
        }

        #endregion

        #region Draw Line

        public static void DrawLine(Eitrum.Mathematics.Line line) {
            Gizmos.DrawLine(line.StartPoint, line.EndPoint);
        }

        #endregion

        #region Draw Plane

        public static void DrawPlane(Plane plane) {
            DrawPlane(plane, Vector3.zero, 1f, 1);
        }

        public static void DrawPlane(Plane plane, float size, int iterations) {
            DrawPlane(plane, Vector3.zero, size, iterations);
        }

        public static void DrawPlane(Plane plane, Vector3 offset, float size, int iterations) {
            var pointFromOrigin = plane.normal * plane.distance + offset;

            float minSize = size / (float)iterations;

            for (int i = 0; i < iterations; i++) {
                DrawPlaneInternal(pointFromOrigin, Quaternion.LookRotation(plane.normal, Vector3.up), Mathf.Lerp(minSize, size, (float)i / (float)iterations));
            }
        }

        private static void DrawPlaneInternal(Vector3 position, Quaternion rotation, float size) {
            var lb = position + rotation * new Vector3(-size, -size, 0f);
            var rb = position + rotation * new Vector3(size, -size, 0f);
            var lu = position + rotation * new Vector3(-size, size, 0f);
            var ru = position + rotation * new Vector3(size, size, 0f);
            Gizmos.DrawLine(lb, rb);
            Gizmos.DrawLine(rb, ru);
            Gizmos.DrawLine(ru, lu);
            Gizmos.DrawLine(lu, lb);
        }

        #endregion

        #region Draw Spline

        public static void DrawSpline(Mathematics.EiSpline spline) {
            for (int i = 0; i < spline.CurveCount; i++) {
                DrawBezier(spline[i]);
            }
        }

        public static void DrawSpline(Mathematics.EiSpline spline, int resolution) {
            for (int i = 0; i < spline.CurveCount; i++) {
                DrawBezier(spline[i], resolution);
            }
        }

        public static void DrawSpline(Mathematics.EiSpline spline, int resolution, Vector3 position, Quaternion rotation) {
            for (int i = 0; i < spline.CurveCount; i++) {
                DrawBezier(spline[i], resolution, position, rotation);
            }
        }

        #endregion

        #region Draw Bezier

        public static void DrawBezier(Mathematics.EiBezier bezier) {
            DrawBezier(bezier, 16);
        }

        public static void DrawBezier(Mathematics.EiBezier bezier, int resolution) {
            var p = bezier.startPoint;
            resolution = Mathf.Max(resolution, 8);
            for (int i = 1; i <= resolution; i++) {
                Gizmos.DrawLine(p, p = bezier.Evaluate((float)i / (float)resolution));
            }
        }

        public static void DrawBezier(Mathematics.EiBezier bezier, int resolution, Vector3 position, Quaternion rotation) {
            var p = position + rotation * bezier.startPoint;
            resolution = Mathf.Max(resolution, 8);
            for (int i = 1; i <= resolution; i++) {
                Gizmos.DrawLine(p, p = position + (rotation * bezier.Evaluate((float)i / (float)resolution)));
            }
        }

        #endregion
    }
}