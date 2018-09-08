using System;
using UnityEngine;

namespace Eitrum.Health {
	[AddComponentMenu("Eitrum/Health/Destroy On Death")]
	public class EiDestroyOnDeath : EiComponent {
		#region Variables

		[Header("Settings")]
		[SerializeField]
		protected Transform targetRootToDestroy;
		[SerializeField]
		protected float timeBeforeDestroy = 0f;
		[SerializeField]
		protected EiHealth healthComponent;

		#endregion

		#region Core

		void Awake() {
			healthComponent.SubscribeOnDeath(OnDeathCallback);
		}

		void OnDestroy() {
			healthComponent.UnsubscribeOnDeath(OnDeathCallback);
		}

		void OnDeathCallback() {
			if (timeBeforeDestroy == 0f)
				DestroyThis();
			else
				EiTimer.Once(timeBeforeDestroy, DestroyThis);
		}

		void DestroyThis() {
			if (targetRootToDestroy)
				Destroy(targetRootToDestroy);
			else
				Destroy(this.gameObject);
		}

		#endregion

#if UNITY_EDITOR

		protected override void AttachComponents() {
			targetRootToDestroy = this.transform;
			healthComponent = GetComponent<EiHealth>();
			base.AttachComponents();
		}

#endif
	}
}

