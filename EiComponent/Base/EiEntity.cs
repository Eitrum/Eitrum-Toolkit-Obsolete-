using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum
{
	[AddComponentMenu ("Eitrum/Core/Entity")]
	public class EiEntity : EiComponent
	{

		#region Variables

		[Header ("Entity Settings")]
		[SerializeField]
		private string entityName = "default-entity";

		[SerializeField]
		[Readonly]
		[Tooltip ("Only assigned when first called")]
		private int entityTypeId = 0;

		[SerializeField]
		[Readonly]
		[Tooltip ("Only assigned when first called")]
		private int entityId = 0;

		private static int uniqueIdGenerator = 1;

		#if !EITRUM_PERFORMANCE_MODE
		private static Dictionary<int, Transform> parentContainers = new Dictionary<int, Transform> ();
		#endif

		[Header ("Components")]
		[SerializeField]
		private Rigidbody rigidbodyComponent;

		[SerializeField]
		private Collider colliderComponent;

		[SerializeField]
		private AudioSource audioComponent;

		[SerializeField]
		private EiInput input;

		#if EITRUM_POOLING
		[Header ("Pool Settings")]
		[SerializeField]
		private EiPoolableComponent[] poolableComponents;
		private EiPoolData poolTarget;
		private bool isPooled = false;
		#endif
		private Action<EiEntity> onDestroy;

		#endregion

		#region Properties

		public string EntityName {
			get {
				return entityName;
			}
			set {
				entityName = value;
			}
		}

		public int EntityTypeId {
			get {
				if (entityTypeId == 0) {
					entityTypeId = entityName.GetHashCode ();
				}
				return entityTypeId;
			}
		}

		public int EntityId {
			get {
				if (entityId == 0) {
					entityId = AllocateEntityId;
				}
				return entityId;
			}
		}

		public static int AllocateEntityId {
			get {
				return uniqueIdGenerator++;
			}
		}

		public Rigidbody Body {
			get {
				return rigidbodyComponent;
			}
			set {
				rigidbodyComponent = value;
			}
		}

		public Collider Collider {
			get {
				return colliderComponent;
			}
			set {
				colliderComponent = value;
			}
		}

		public AudioSource Audio {
			get {
				return audioComponent;
			}
			set {
				audioComponent = value;
			}
		}

		public EiInput Input {
			get {
				return input;
			}
		}

		#if EITRUM_POOLING
		public EiPoolableComponent[] PoolableComponents {
			get {
				return poolableComponents;
			}
		}

		public EiPoolData PoolTarget {
			get {
				return poolTarget;
			}
		}

		public bool IsPooled {
			get {
				return isPooled;
			}
		}
		#endif

		#endregion

		#region Core

		#if !EITRUM_PERFORMANCE_MODE
		private void Awake ()
		{
			if (this.transform.parent == null) {
				AssignToParent ();
			}
		}

		private void AssignToParent ()
		{
			Transform parent = null;
			if (parentContainers.ContainsKey (EntityTypeId)) {
				parent = parentContainers [EntityTypeId];
				if (!parent)
					parent = parentContainers [EntityTypeId] = new GameObject (entityName).transform;
			} else
				parentContainers.Add (EntityTypeId, parent = new GameObject (entityName).transform);

			this.transform.SetParent (parent);
		}
		#endif

		public void SleepPhysics ()
		{
			if (rigidbodyComponent) {
				rigidbodyComponent.velocity = Vector3.zero;
				rigidbodyComponent.angularVelocity = Vector3.zero;
			}
		}

		public void FreezePhysics ()
		{
			if (rigidbodyComponent) {
				rigidbodyComponent.velocity = Vector3.zero;
				rigidbodyComponent.angularVelocity = Vector3.zero;
				rigidbodyComponent.isKinematic = true;
			}
		}

		public void UnfreezePhysics ()
		{
			if (rigidbodyComponent) {
				rigidbodyComponent.isKinematic = false;
			}
		}

		#if EITRUM_POOLING
		public void AssignPoolTarget (EiPoolData item)
		{
			poolTarget = item;
		}

		/// <summary>
		/// used by pooling system
		/// </summary>
		/// <param name="value"></param>
		public void SetPooled (bool value)
		{
			isPooled = value;
		}
		#endif


		public override void Destroy ()
		{
			if (onDestroy != null)
				onDestroy (this);
#if EITRUM_POOLING
			EiPoolData.OnPoolDestroyHelper (this);
#else
			MonoBehaviour.Destroy(gameObject);
#endif
		}

		#endregion

		#region SetRemove Parent

		public void SetParent (EiEntity entity)
		{
			this.transform.SetParent (entity.transform);
		}

		public void SetParent (EiComponent component)
		{
			this.transform.SetParent (component.transform);
		}

		public void SetParent (Transform transform)
		{
			this.transform.SetParent (transform);
		}

		public void ReleaseParent ()
		{
#if !EITRUM_PERFORMANCE_MODE
			AssignToParent ();
#else
			this.transform.SetParent(null);
#endif
		}

		#endregion

		#region Subscribe On Death

		public void SubscribeOnDeath (Action<EiEntity> method)
		{
			onDestroy += method;
		}

		public void UnsubscribeOnDeath (Action<EiEntity> method)
		{
			if (onDestroy != null)
				onDestroy -= method;
		}

		#endregion

		#region Editor

		[ContextMenu ("Add Missing Components")]
		public virtual void AddMissingComponents ()
		{
			if (!rigidbodyComponent)
				rigidbodyComponent = this.GetOrAddComponent<Rigidbody> ();
			if (!colliderComponent)
				Debug.LogWarning ("Can't add 'collider'. Manually add collider if you want it");
			if (!audioComponent)
				audioComponent = this.GetOrAddComponent<AudioSource> ();
			if (!input) {
				input = this.GetComponent<EiInput> ();
				if (!input)
					Debug.LogWarning ("Input Component Will not be created, make sure you do not need it or add it manually");
			}
		}

		protected override void AttachComponents ()
		{
			base.AttachComponents ();
			rigidbodyComponent = GetComponentInChildren<Rigidbody> ();
			colliderComponent = GetComponentInChildren<Collider> ();
			audioComponent = GetComponentInChildren<AudioSource> ();
			input = GetComponentInChildren<EiInput> ();
#if EITRUM_POOLING
			GetComponentsInChildren<EiPoolableInterface> ().ToSerializableArray (ref poolableComponents);
#endif
		}

		#endregion
	}
}