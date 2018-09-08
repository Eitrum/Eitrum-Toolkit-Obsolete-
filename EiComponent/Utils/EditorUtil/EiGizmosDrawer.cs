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
	}
}
