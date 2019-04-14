using Eitrum.Engine.Core;
using System;
using UnityEngine;

namespace Eitrum.Health
{
	[AddComponentMenu ("Eitrum/Health/Instantiate On Death")]
	public class EiInstantiateOnDeath : EiComponent
	{
		#region Variables

		[SerializeField]
		protected EiHealth healthComponent;

		[SerializeField]
		protected GameObject prefabToSpawn;

		[SerializeField]
		protected Transform spawnAtTransform;

		[SerializeField]
		protected bool spawnAsChild = false;

		[SerializeField]
		protected bool localOffsetToTransform = true;

		[SerializeField]
		protected Vector3 offset;

		[SerializeField]
		protected Vector3 rotationOffset;

		#endregion

		#region Core

		void Awake ()
		{
			healthComponent.SubscribeOnDeath (OnDeath);
		}

		void OnDestroy ()
		{
			healthComponent.UnsubscribeOnDeath (OnDeath);
		}

		void OnDeath ()
		{
			GameObject go;
			if (spawnAtTransform) {
				if (spawnAsChild) {
					go = Instantiate (prefabToSpawn, spawnAtTransform);
				} else {
					go = Instantiate (prefabToSpawn, spawnAtTransform.position, spawnAtTransform.rotation);
				}
			} else {
				go = Instantiate (prefabToSpawn);
			}

			if (localOffsetToTransform) {
				go.transform.localPosition += offset;
				go.transform.localRotation *= Quaternion.Euler (rotationOffset);
			} else {
				go.transform.position += offset;
				go.transform.rotation *= Quaternion.Euler (rotationOffset);
			}
		}

		#endregion
	}
}