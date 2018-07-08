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
		protected string entityName = "default-entity";

		[SerializeField]
		[Tooltip ("Will Generate a bit of overhead but makes it possible to search for any entity")]
		protected bool subscribeToEntityList = false;

		[SerializeField]
		[Readonly]
		protected int entityTypeId = 0;

		[SerializeField]
		[Readonly]
		protected int entityId = 0;

		private static int uniqueIdGenerator = 1;

		[Header ("Components")]
		[SerializeField]
		protected Rigidbody body;

		[SerializeField]
		protected new Collider collider;

		[SerializeField]
		protected new AudioSource audio;

		[SerializeField]
		protected EiInput input;

		#endregion

		#region Properties

		public virtual string EntityName {
			get {
				return entityName;
			}set {
				entityName = value;
			}
		}

		public virtual int EntityTypeId {
			get {
				if (entityTypeId == 0) {
					entityTypeId = entityName.GetHashCode ();
				}
				return entityTypeId;
			}
		}

		public virtual int EntityId {
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

		public virtual Rigidbody Body {
			get {
				return body;
			}
			set {
				body = value;
			}
		}

		public virtual Collider Collider {
			get {
				return collider;
			}
			set {
				collider = value;
			}
		}

		public virtual AudioSource Audio {
			get {
				return audio;
			}
			set {
				audio = value;
			}
		}

		public EiInput Input {
			get {
				return input;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{

		}

		public void FreezePhysics ()
		{
			if (body) {
				body.isKinematic = true;
			}
		}

		public void UnfreezePhysics ()
		{
			if (body) {
				body.isKinematic = false;
			}
		}

		[ContextMenu ("Add Missing Components")]
		public virtual void AddMissingComponents ()
		{
			if (!body)
				body = this.GetOrAddComponent<Rigidbody> ();
			if (!collider)
				Debug.LogWarning ("Can't add 'collider'. Manually add collider if you want it");
			if (!audio)
				audio = this.GetOrAddComponent<AudioSource> ();
			if (!input) {
				input = this.GetComponent<EiInput> ();
				if (!input)
					Debug.LogWarning ("Input Component Will not be created, make sure you do not need it or add it manually");
			}
		}

		#endregion

		#if UNITY_EDITOR

		protected override void AttachComponents ()
		{
			AddMissingComponents ();
			base.AttachComponents ();
		}

		#endif
	}
}

