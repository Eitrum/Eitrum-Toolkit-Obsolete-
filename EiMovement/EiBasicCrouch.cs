using System;
using UnityEngine;

namespace Eitrum.Movement
{
	[AddComponentMenu ("Eitrum/Movement/Basic Crouch")]
	public class EiBasicCrouch : EiComponent
	{
		#region Variables

		public float normalBaseSpeed = 2f;
		public float normalSpeedMultiplier = 1f;

		public float runningBaseSpeed = 4f;
		public float runningSpeedMultiplier = 1f;

		[Header ("Crouch Config")]
		[SerializeField]
		protected bool canEnterCrouch = true;
		[SerializeField]
		protected bool canRunInCrouch = true;
		[SerializeField]
		protected float crouchDelay = 0.5f;
		[SerializeField]
		protected float crouchColliderHeight = 0.5f;
		[Tooltip ("If set to 0 or lower, it will not be used")]
		[SerializeField]
		protected float fallSpeedToStandup = 5;

		[Header ("Input")]
		[SerializeField]
		protected KeyCode crouchKeyCode = KeyCode.C;

		[Header ("Stamina")]
		[Space (8f)]
		[SerializeField]
		protected float baseCrouchStaminaCost = 5f;
		[SerializeField]
		protected float crouchStaminaCostMuliplier = 1f;

		[Header ("Components")]
		[Tooltip ("Reference to change capsule collider direction")]
		[SerializeField]
		protected CapsuleCollider capsuleTarget;
		[SerializeField]
		protected EiBasicMovement movementComponent;



		protected bool isCrouched = false;
		protected bool isRunning = false;
		protected float standingHeight = 0f;
		protected int crouchAnimationId = -1;
		protected Coroutine colliderAnimation;
		protected Rigidbody body;
		protected EiStamina stamina;

		#endregion

		#region Properties

		public virtual float NormalSpeed {
			get {
				return normalBaseSpeed * normalSpeedMultiplier * Movement.SpeedMultiplier + Movement.ExtraSpeed;
			}
		}

		public virtual float RunningSpeed {
			get {
				return runningBaseSpeed * runningSpeedMultiplier * Movement.SpeedMultiplier + Movement.ExtraSpeed;
			}
		}

		public virtual float NormalStaminaCost {
			get {
				return baseCrouchStaminaCost * crouchStaminaCostMuliplier;
			}
		}

		public virtual float RunningStaminaCost {
			get {
				return 0f;
			}
		}

		public virtual EiBasicMovement Movement {
			get {
				return movementComponent;
			}
		}

		public virtual bool StandupAfterFalling {
			get {
				return fallSpeedToStandup <= 0f;
			}
		}

		public virtual bool IsCrouched {
			get {
				return isCrouched;
			}
		}

		public virtual Rigidbody Body {
			get {
				if (!body) {
					body = Entity.Body;
				}
				return body;
			}
		}

		public virtual bool UsingStamina {
			get {
				return Movement.UsingStamina;
			}
		}

		public virtual EiStamina Stamina {
			get {
				if (!stamina) {
					stamina = Movement.Stamina;
				}
				return stamina;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			standingHeight = capsuleTarget.height;
			SubscribeUpdate ();
			Movement.SubscribeOnStateChange (OnChangeState);
		}

		protected virtual void OnChangeState (int i)
		{
			if (colliderAnimation != null) {
				EiTimer.Stop (colliderAnimation);
				colliderAnimation = null;
			}
			if (isCrouched && i == 0)
				Movement.SetFrozen (false);
			isCrouched = i == 1;
			if (isCrouched) {
				Movement.SetFrozen (true);
				colliderAnimation = EiTimer.Animate (crouchDelay, AnimateCrouch);
				SubscribeFixedUpdate ();
			} else {
				if (i == 0 && fixedUpdateNode != null)
					colliderAnimation = EiTimer.Animate (crouchDelay, AnimateStandup);
				UnsubscribeFixedUpdate ();
			}
		}

		public override void UpdateComponent (float time)
		{
			if (Input.GetKeyDown (crouchKeyCode)) {
				Crouch ();
			}
		}

		public override void FixedUpdateComponent (float time)
		{
			var currentDirection = Body.velocity;
			if (StandupAfterFalling && !Movement.IsGrounded && currentDirection.y < -fallSpeedToStandup) {
				Movement.SetState (0);
				return;
			}
			currentDirection.y = 0f;
			var input = Movement.InputDirection;
			var hasAcceleration = Movement.HasAcceleration;
			var isRunning = Movement.IsRunning;
			var newDirection = Vector3.zero;

			if (isRunning && (!UsingStamina || Stamina.UseStamina (RunningStaminaCost * time))) {
				newDirection = input * RunningSpeed;
			} else if (!UsingStamina || Stamina.UseStamina (NormalStaminaCost * time)) {
				newDirection = input * NormalSpeed;
				isRunning = false;
			}

			if (hasAcceleration) {
				newDirection = (currentDirection + newDirection * (time * Movement.Acceleration)).ClampMagnitudeXZ (RunningSpeed);
			}
			newDirection.y = Body.velocity.y;
			Body.velocity = newDirection;
		}

		public virtual void AnimateStandup (float value)
		{
			capsuleTarget.height = Mathf.Lerp (crouchColliderHeight, standingHeight, value);
		}

		public virtual void AnimateCrouch (float value)
		{
			capsuleTarget.height = Mathf.Lerp (standingHeight, crouchColliderHeight, value);
		}

		public virtual void Crouch ()
		{
			if (isCrouched) {
				Standup ();
			} else {
				//Check if can crouch
				if (Movement.IsGrounded) {
					//Success
					Debug.Log ("Setting Crouch");
					Movement.SetState (1);
				}
			}
		}

		public virtual void Standup ()
		{
			//Check if can stand up

			if (!Physics.SphereCast (new Ray (transform.position, Vector3.up), capsuleTarget.radius, standingHeight - (capsuleTarget.radius * 2f))) {
				//Successful, can stand up
				Movement.SetState (0);
			}
		}

		#endregion

		#region Set/Add/Remove

		public virtual void SetCrouchBaseSpeed (float value)
		{
			normalBaseSpeed = value;
		}

		public virtual void AddCrouchBaseSpeed (float amount)
		{
			normalBaseSpeed += amount;
		}

		public virtual void RemoveCrouchBaseSpeed (float amount)
		{
			normalBaseSpeed -= amount;
		}

		public virtual void SetCrouchSpeedMultiplier (float value)
		{
			normalSpeedMultiplier = value;
		}

		public virtual void AddCrouchSpeedMultiplier (float amount)
		{
			normalSpeedMultiplier += amount;
		}

		public virtual void RemoveCrouchSpeedMultiplier (float amount)
		{
			normalSpeedMultiplier -= amount;
		}

		#endregion

		#if UNITY_EDITOR

		protected override void AttachComponents ()
		{
			capsuleTarget = GetComponent<CapsuleCollider> ();
			movementComponent = GetComponent<EiBasicMovement> ();
			base.AttachComponents ();
		}

		#endif
	}
}

