using System;
using UnityEngine;
using Eitrum.Threading;

namespace Eitrum.Utility.Spawner
{
	public class EiTimedPrefabSpawner : EiComponent
	{
		#region Variables

		[Header ("Time Settings")]
		[SerializeField]
		private EiPropertyEventFloat minCooldown = new EiPropertyEventFloat (15f);
		[SerializeField]
		private EiPropertyEventFloat maxCooldown = new EiPropertyEventFloat (30f);
		[SerializeField]
		private EiPropertyEventFloat currentCooldown = new EiPropertyEventFloat (30f);

		[Header ("Spawn Settings")]
		[SerializeField]
		private EiPrefabSpawnMode spawnMode = EiPrefabSpawnMode.None;
		[SerializeField]
		private EiDatabaseItem prefabToSpawn;
		[SerializeField]
		private Transform spawnTarget;
		[SerializeField]
		private bool scalePrefab = true;

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
			currentCooldown.Subscribe (OnCooldownChanged);
			SubscribeUpdate ();
		}

		public override void UpdateComponent (float time)
		{
			if (spawnMode == EiPrefabSpawnMode.None)
				return;
			if (spawnMode == EiPrefabSpawnMode.WaitUntilReferenceGone && spawnedReference != null) {
				if (loseReferenceByDistance && (spawnedReference is GameObject) && distanceToLoseReference < Vector3.Distance (SpawnPosition, (spawnedReference as GameObject).transform.position)) {
					spawnedReference = null;
				} else {
					return;
				}
			}
			
			if (currentCooldown.Value > 0f)
				currentCooldown.Value -= time;
		}

		void OnCooldownChanged (float value)
		{
			if (value <= 0f) {
				Spawn ();
			}
		}

		void Spawn ()
		{
			if (spawnMode == EiPrefabSpawnMode.None)
				return;
			if (spawnMode == EiPrefabSpawnMode.WaitUntilReferenceGone && spawnedReference != null) {
				if (loseReferenceByDistance && (spawnedReference is GameObject) && distanceToLoseReference < Vector3.Distance (SpawnPosition, (spawnedReference as GameObject).transform.position)) {
					spawnedReference = null;
				} else {
					return;
				}
			}
			if (spawnMode == EiPrefabSpawnMode.DestroyOldObject && spawnedReference != null) {
				Destroy (spawnedReference);
			}

			currentCooldown.Value = EiRandom.Range (MinCooldown, MaxCooldown);

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