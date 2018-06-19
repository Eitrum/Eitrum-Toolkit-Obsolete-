using System;
using UnityEngine;

namespace Eitrum.Movement
{
	[AddComponentMenu ("Eitrum/Movement/Basic Jump")]
	public class EiBasicJump : EiComponent
	{
		#region Variables

		[Header ("Settings")]
		[SerializeField]
		protected EiPropertyEventFloat baseJumpForce = new EiPropertyEventFloat (5f);
		[SerializeField]
		protected EiPropertyEventFloat jumpForceMultiplier = new EiPropertyEventFloat (1f);
		[Space (8f)]
		[SerializeField]
		protected float timeOfGroundAllowedToJump = 0.1f;
		[SerializeField]
		protected int jumps = 1;
		[SerializeField]
		protected ForceMode forceMode = ForceMode.Impulse;
		[SerializeField]
		protected bool calculateMaxForceForQuickJumps = true;

		[Header ("Components")]
		[SerializeField]
		protected EiBasicMovement movementComponent;
		[SerializeField]
		protected bool ignoreMovementComponentSettings = false;

		private int currentJumps = 0;
		private float timeNotGrounded = 0f;
		private float jumpDelay = 0.2f;

		#endregion

		#region Properties

		public virtual float JumpForce {
			get {
				return baseJumpForce.Value * jumpForceMultiplier.Value;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			SubscribeUpdate ();
		}

		public override void UpdateComponent (float time)
		{
			if (!ignoreMovementComponentSettings && movementComponent.IsFrozen)
				return;
			
			jumpDelay -= time;

			if (movementComponent.IsGrounded) {
				if (jumpDelay < 0f) {
					timeNotGrounded = 0f;
					currentJumps = 0;
				}
			} else {
				timeNotGrounded += time;
			}
			if (Input.GetKeyDown (KeyCode.Space) && jumpDelay < 0f) {
				if (timeOfGroundAllowedToJump >= timeNotGrounded) {
					currentJumps = 0;
					timeNotGrounded = timeOfGroundAllowedToJump + 1f;
				}
				if (currentJumps < jumps) {
					currentJumps++;
					jumpDelay = 0.2f;
					var forceReduction = calculateMaxForceForQuickJumps ? Mathf.Clamp (Entity.Body.velocity.y, 0f, JumpForce) : 0f;
					Entity.Body.AddForce (new Vector3 (0, JumpForce - forceReduction, 0), forceMode);
				}
			}
		}

		#endregion

		#if UNITY_EDITOR

		protected override void AttachComponents ()
		{
			movementComponent = GetComponent<EiBasicMovement> ();
			base.AttachComponents ();
		}

		#endif
	}
}