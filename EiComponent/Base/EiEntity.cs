using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum
{
	[AddComponentMenu("Eitrum/Core/Entity")]
	public class EiEntity : EiComponent
	{
		#region Variables

		[Header("Entity Settings")]
		[SerializeField]
		protected string entityName = "default-entity";

		[SerializeField]
		[Tooltip("Will Generate a bit of overhead but makes it possible to search for any entity")]
		protected bool subscribeToEntityList = false;

		[SerializeField]
		[Readonly]
		protected int entityTypeId = 0;

		[SerializeField]
		[Readonly]
		protected int entityId = 0;

		private static int uniqueIdGenerator = 1;

		[Header("Components")]
		[SerializeField]
		protected Rigidbody rigidbodyComponent;

		[SerializeField]
		protected Collider colliderComponent;

		[SerializeField]
		protected AudioSource audioComponent;

		[SerializeField]
		protected EiInput input;

		[Header("Pool Settings")]
		[SerializeField]
		private EiPoolableInterface[] poolableInterfaces;
		private EiPoolData poolTarget;

		#endregion

		#region Properties

		public virtual string EntityName
		{
			get
			{
				return entityName;
			}
			set
			{
				entityName = value;
			}
		}

		public virtual int EntityTypeId
		{
			get
			{
				if (entityTypeId == 0)
				{
					entityTypeId = entityName.GetHashCode();
				}
				return entityTypeId;
			}
		}

		public virtual int EntityId
		{
			get
			{
				if (entityId == 0)
				{
					entityId = AllocateEntityId;
				}
				return entityId;
			}
		}

		public static int AllocateEntityId
		{
			get
			{
				return uniqueIdGenerator++;
			}
		}

		public virtual Rigidbody Body
		{
			get
			{
				return rigidbodyComponent;
			}
			set
			{
				rigidbodyComponent = value;
			}
		}

		public virtual Collider Collider
		{
			get
			{
				return colliderComponent;
			}
			set
			{
				colliderComponent = value;
			}
		}

		public virtual AudioSource Audio
		{
			get
			{
				return audioComponent;
			}
			set
			{
				audioComponent = value;
			}
		}

		public EiInput Input
		{
			get
			{
				return input;
			}
		}

		public EiPoolableInterface[] PoolableInterfaces
		{
			get
			{
				return poolableInterfaces;
			}
		}

		public EiPoolData PoolTarget
		{
			get
			{
				return poolTarget;
			}
		}

		#endregion

		#region Core

		void Awake()
		{

		}

		public void FreezePhysics()
		{
			if (rigidbodyComponent)
			{
				rigidbodyComponent.velocity = Vector3.zero;
				rigidbodyComponent.isKinematic = true;
			}
		}

		public void UnfreezePhysics()
		{
			if (rigidbodyComponent)
			{
				rigidbodyComponent.isKinematic = false;
			}
		}

		public void AssignPoolTarget(EiPoolData item)
		{
			poolTarget = item;
		}
		
		#endregion

#if UNITY_EDITOR
		[ContextMenu("Add Missing Components")]
		public virtual void AddMissingComponents()
		{
			if (!rigidbodyComponent)
				rigidbodyComponent = this.GetOrAddComponent<Rigidbody>();
			if (!colliderComponent)
				Debug.LogWarning("Can't add 'collider'. Manually add collider if you want it");
			if (!audioComponent)
				audioComponent = this.GetOrAddComponent<AudioSource>();
			if (!input)
			{
				input = this.GetComponent<EiInput>();
				if (!input)
					Debug.LogWarning("Input Component Will not be created, make sure you do not need it or add it manually");
			}
		}
		protected override void AttachComponents()
		{
			base.AttachComponents();
			rigidbodyComponent = GetComponentInChildren<Rigidbody>();
			colliderComponent = GetComponentInChildren<Collider>();
			audioComponent = GetComponentInChildren<AudioSource>();
			input = GetComponentInChildren<EiInput>();
			poolableInterfaces = GetComponentsInChildren<EiPoolableInterface>();
		}

#endif
	}
}