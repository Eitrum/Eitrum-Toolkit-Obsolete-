using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Utility.Spawner
{
	[AddComponentMenu ("Eitrum/Utility/Spawner/Prefab Spawner")]
	public class EiPrefabSpawner : EiComponent
	{
		#region Variables

		[Header ("Spawn Settings")]
		[SerializeField]
		private EiDatabaseItem prefabToSpawn;
		[SerializeField]
		private Transform spawnTarget;
		[SerializeField]
		private bool spawnOnAwake = false;
		[SerializeField]
		private bool waitUntilReferenceIsGone = false;
		[SerializeField]
		private bool destroyOldObject = false;
		[SerializeField]
		private bool respawnIfReferenceIsGone = false;
		[SerializeField]
		private bool scalePrefab = true;

		[Header ("Time Settings")]
		[SerializeField]
		private EiPropertyEventBool useCooldown = new EiPropertyEventBool (false);
		[SerializeField]
		private EiPropertyEventFloat minCooldown = new EiPropertyEventFloat (15f);
		[SerializeField]
		private EiPropertyEventFloat maxCooldown = new EiPropertyEventFloat (30f);
		[SerializeField]
		private EiPropertyEventFloat currentCooldown = new EiPropertyEventFloat (30f);

		[Header ("Reference")]
		[SerializeField]
		private bool loseReferenceByDistance = true;
		[SerializeField]
		private float distanceToLoseReference = 1f;
		[Readonly]
		[SerializeField]
		private UnityEngine.Object spawnedReference;

		private EiTrigger onSpawned = new EiTrigger ();
		private EiTrigger<UnityEngine.Object> onSpawnedObject = new EiTrigger<UnityEngine.Object> ();

		#endregion

		#region Properties

		public bool UsesCooldown {
			get {
				return useCooldown.Value;
			}
		}

		public float MinCooldown {
			get {
				return minCooldown.Value;
			}
		}

		public float MaxCooldown {
			get {
				return maxCooldown.Value;
			}
		}

		public float CurrentCooldown {
			get {
				return currentCooldown.Value;
			}
		}

		public bool HasTarget {
			get {
				return spawnTarget != null;
			}
		}

		public Vector3 SpawnPosition {
			get {
				return (HasTarget ? spawnTarget.position : transform.position);
			}
		}

		public Quaternion SpawnRotation {
			get {
				return (HasTarget ? spawnTarget.rotation : transform.rotation);
			}
		}

		public Vector3 SpawnScale {
			get {
				return (HasTarget ? spawnTarget.lossyScale : transform.lossyScale);
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			SubscribeUpdate ();
			if (spawnOnAwake)
				ForceSpawn ();
		}

		public override void UpdateComponent (float time)
		{
			if (loseReferenceByDistance && (spawnedReference is GameObject) && distanceToLoseReference < Vector3.Distance (SpawnPosition, (spawnedReference as GameObject).transform.position))
				spawnedReference = null;
			if (waitUntilReferenceIsGone && spawnedReference != null)
				return;
			if (respawnIfReferenceIsGone && spawnedReference == null)
				ForceSpawn ();

			if (useCooldown.Value && currentCooldown.Value > 0f)
				currentCooldown.Value -= time;
		}

		[ContextMenu ("Spawn")]
		public void Spawn ()
		{
			if (useCooldown.Value && currentCooldown.Value > 0f) {
				return;
			}
			if (waitUntilReferenceIsGone && spawnedReference != null) {
				if (loseReferenceByDistance && (spawnedReference is GameObject) && distanceToLoseReference < Vector3.Distance (SpawnPosition, (spawnedReference as GameObject).transform.position)) {
					spawnedReference = null;
				} else {
					return;
				}
			}
			InternalSpawn ();
		}

		[ContextMenu ("Force Spawn")]
		public void ForceSpawn ()
		{
			InternalSpawn ();
		}

		private void InternalSpawn ()
		{
			if (useCooldown.Value)
				currentCooldown.Value = EiRandom.Range (MinCooldown, MaxCooldown);

			if (destroyOldObject && spawnedReference != null) {
				Destroy (spawnedReference);
			}

			if (scalePrefab && prefabToSpawn.Is (typeof(GameObject))) {
				spawnedReference = prefabToSpawn.InstantiateAsGameObject (SpawnPosition, SpawnRotation, SpawnScale);
			} else {
				spawnedReference = prefabToSpawn.Instantiate (SpawnPosition, SpawnRotation);
			}

			onSpawned.Trigger ();
			onSpawnedObject.Trigger (spawnedReference);
		}

		#endregion

		#if UNITY_EDITOR

		void OnDrawGizmos ()
		{
			if (prefabToSpawn && prefabToSpawn.Object && prefabToSpawn.Is (typeof(GameObject))) {
				var mesh = prefabToSpawn.GameObject.GetComponentsInChildren<MeshFilter> ();
				for (int i = 0; i < mesh.Length; i++) {
					var go = mesh [i].gameObject;
					var scale = go.transform.lossyScale;
					if (scalePrefab)
						scale.Scale (SpawnScale);
					Gizmos.DrawWireMesh (
						mesh [i].sharedMesh, 
						SpawnPosition + go.transform.position, 
						SpawnRotation * go.transform.rotation,
						scale);
				}
			}
		}

		#endif
	}
}
