using System;
using UnityEngine;

namespace Eitrum
{
	[AddComponentMenu ("Eitrum/Core/Entity")]
	public class EiEntity : EiComponent
	{
		#region Variables

		[SerializeField]
		protected string entityName = "default-entity";

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

		public override EiInput Input {
			get {
				return input;
			}
		}

		#endregion

		#region Core

		public void FreezePhysics ()
		{
			if (body) {
				body.isKinematic = true;
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

