using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Eitrum.EiNet;

namespace Eitrum.Movement
{
	[AddComponentMenu ("Eitrum/Movement/Basic Movement")]
	public class EiBasicMovement : EiComponent, EiNetInterface
	{
		#region Variables

		#region Speed

		[Header ("Speed")]
		[SerializeField]
		protected float baseNormalSpeed = 5f;
		[SerializeField]
		protected float normalSpeedMultiplier = 1f;
		[Space (8f)]
		[SerializeField]
		protected float baseWalkingSpeed = 2f;
		[SerializeField]
		protected float walkingSpeedMultiplier = 1f;
		[Space (8f)]
		[SerializeField]
		protected float baseRunningSpeed = 7.5f;
		[SerializeField]
		protected float runningSpeedMultiplier = 1f;
		[Space (8f)]
		[SerializeField]
		protected float baseCrouchSpeed = 2f;
		[SerializeField]
		protected float crouchSpeedMultiplier = 1f;
		[Space (8f)]
		[SerializeField]
		protected float baseCrouchRunSpeed = 3.5f;
		[SerializeField]
		protected float crouchRunSpeedMultiplier = 1f;
		[Space (8f)]
		[SerializeField]
		protected float baseProneSpeed = 0.5f;
		[SerializeField]
		protected float proneSpeedMultiplier = 1f;
		[Space (8f)]
		[SerializeField]
		protected float extraSpeed = 0f;
		[SerializeField]
		protected float speedMultiplier = 1f;

		#endregion

		#region Acceleration

		[Header ("Acceleration")]
		[SerializeField]
		protected bool hasAcceleration = false;

		[SerializeField]
		protected float accelerationSpeed = 10f;

		[SerializeField]
		protected float accelerationMultiplier = 1f;

		#endregion

		#region Stamina

		[Header ("Stamina Config")]
		[SerializeField]
		protected bool useStamina = false;
		[SerializeField]
		protected EiStamina staminaComponent;
		[SerializeField]
		protected float staminaCostMultiplier = 1f;

		[Space (8f)]
		[SerializeField]
		protected float baseNormalStaminaCost = 5f;
		[SerializeField]
		protected float normalStaminaCostMultiplier = 1f;
		[Space (8f)]
		[SerializeField]
		protected float baseWalkingStaminaCost = 0f;
		[SerializeField]
		protected float walkingStaminaCostMuliplier = 1f;
		[Space (8f)]
		[SerializeField]
		protected float baseRunningStaminaCost = 7.5f;
		[SerializeField]
		protected float runningStaminaCostMultiplier = 1f;

		#endregion

		#region Prone Config

		[Header ("Prone Config")]
		[SerializeField]
		protected bool canEnterProne = true;
		[SerializeField]
		protected float proneDelay = 0.5f;
		[SerializeField]
		protected float proneColliderHeight = 1f;
		[SerializeField]
		protected bool standupAfterFallingProne = true;

		#endregion

		#region Input Config

		[Header ("Input Config")]
		[SerializeField]
		protected string horizontalAxis = "Horizontal";

		[SerializeField]
		protected string verticalAxis = "Vertical";

		[SerializeField]
		protected KeyCode walkKeyCode = KeyCode.LeftControl;

		[SerializeField]
		protected KeyCode runKeyCode = KeyCode.LeftShift;


		[SerializeField]
		protected KeyCode proneKeyCode = KeyCode.Z;

		#endregion

		#region Ground Check

		[Header ("Ground Check Config")]
		[SerializeField]
		protected Transform rayCasterTransform;

		[SerializeField]
		protected Vector3 rayCastDirection;

		[SerializeField]
		protected float rayCastDistance;

		[SerializeField]
		protected float sphereCastSize;

		protected RaycastHit lastGroundHit;
		protected bool? isGrounded;

		#endregion

		#region Sound

		[Header ("Sound Settings")]
		[SerializeField]
		protected bool useStepSound = false;

		[SerializeField]
		protected AudioClip basicStepSound;

		[SerializeField]
		protected float stepsPerMeter = 2.5f;

		#endregion

		#region Cache

		protected bool isFrozen = false;
		protected float currentStepTimer = 0f;
		protected bool isRunning = false;
		protected bool isWalking = false;
		protected Vector3 inputDirection;
		protected Vector3 currentDirection;
		protected Rigidbody body;
		// 0 == standing, 1 == crouch, 2 == prone
		protected EiPropertyEventInt currentState = new EiPropertyEventInt (0);

		#endregion

		#endregion

		#region Properties

		#region Speed

		public virtual float NormalSpeed {
			get {
				return baseNormalSpeed * normalSpeedMultiplier * speedMultiplier + extraSpeed;
			}
		}

		public virtual float WalkingSpeed {
			get {
				return baseWalkingSpeed * walkingSpeedMultiplier * speedMultiplier + extraSpeed;
			}
		}

		public virtual float RunningSpeed {
			get {
				return baseRunningSpeed * runningSpeedMultiplier * speedMultiplier + extraSpeed;
			}
		}

		public virtual float CrouchSpeed {
			get {
				return baseCrouchSpeed * crouchSpeedMultiplier * speedMultiplier + extraSpeed;
			}
		}

		public virtual float ProneSpeed {
			get {
				return baseProneSpeed * proneSpeedMultiplier * speedMultiplier + extraSpeed;
			}
		}

		public virtual float TargetSpeed {
			get {
				if (isWalking && isRunning)
					return NormalSpeed;
				if (isWalking)
					return WalkingSpeed;
				if (isRunning)
					return RunningSpeed;
				return NormalSpeed;
			}
		}

		public virtual float CurrentSpeed {
			get {
				return Body.velocity.magnitude;
			}
		}

		public virtual float ExtraSpeed {
			get {
				return extraSpeed;
			}
		}

		public virtual float SpeedMultiplier {
			get {
				return speedMultiplier;
			}
		}

		#endregion

		#region Ground Check

		public virtual bool IsGrounded {
			get {
				if (!isGrounded.HasValue) {
					if (!rayCasterTransform)
						return (isGrounded = false).Value;
					Ray ray = new Ray (rayCasterTransform.position, rayCastDirection);
					isGrounded = Physics.Raycast (ray, out lastGroundHit, rayCastDistance) || Physics.SphereCast (ray, sphereCastSize, out lastGroundHit, 0.01f);
				}
				return isGrounded.Value;
			}
		}

		public virtual RaycastHit LastGroundHit {
			get {
				return lastGroundHit;
			}
		}

		#endregion

		#region Acceleration

		public virtual bool HasAcceleration {
			get {
				return hasAcceleration;
			}
		}

		public virtual float Acceleration {
			get {
				return accelerationSpeed * accelerationMultiplier;
			}
		}

		#endregion

		#region Stamina

		public virtual EiStamina Stamina {
			get {
				return staminaComponent;
			}
		}

		public virtual bool UsingStamina {
			get {
				return useStamina;
			}
		}

		public virtual float NormalStaminaCost {
			get {
				return baseNormalStaminaCost * normalStaminaCostMultiplier * staminaCostMultiplier;
			}
		}

		public virtual float WalkingStaminaCost {
			get {
				return baseWalkingStaminaCost * walkingStaminaCostMuliplier * staminaCostMultiplier;
			}
		}

		public virtual float RunningStaminaCost {
			get {
				return baseRunningStaminaCost * runningStaminaCostMultiplier * staminaCostMultiplier;
			}
		}

		public virtual float CurrentStaminaCost {
			get {
				if (isWalking && isRunning)
					return NormalStaminaCost;
				if (isWalking)
					return WalkingStaminaCost;
				if (isRunning)
					return RunningStaminaCost;
				return NormalStaminaCost;
			}
		}

		#endregion

		#region Input

		public virtual bool HasMovementInput {
			get {
				return !(inputDirection.magnitude < 0.02f);
			}
		}

		public virtual bool IsStanding {
			get {
				return currentState.Value == 0;
			}
		}

		public virtual Vector3 InputDirection {
			get {
				return inputDirection;
			}
		}

		#endregion

		#region Cache

		protected virtual Rigidbody Body {
			get {
				if (!body)
					body = Entity.Body;
				return body;
			}
		}

		public virtual bool IsFrozen {
			get {
				return isFrozen;
			}
			set {
				isFrozen = value;
			}
		}

		public virtual bool IsRunning {
			get {
				return isRunning;
			}
		}

		public virtual bool IsWalking {
			get {
				return isWalking && IsStanding;
			}
		}

		#endregion

		#endregion

		#region Core

		void Awake ()
		{
			SubscribeUpdate ();
			SubscribeFixedUpdate ();
		}

		public override void UpdateComponent (float time)
		{
			inputDirection = Body.transform.localRotation * new Vector3 (Input.GetAxisRaw (horizontalAxis), 0f, Input.GetAxisRaw (verticalAxis)).normalized;
			isRunning = Input.GetKey (runKeyCode);
			isWalking = Input.GetKey (walkKeyCode);
			if (isRunning && isWalking) {
				isRunning = isWalking = false;
			}
		}

		public override void FixedUpdateComponent (float time)
		{
			isGrounded = null;
			
			if (isFrozen)
				return;

			var currentDirection = Body.velocity.ToVector3_XZ ();
			var newDirection = Vector3.zero;


			if (isRunning && (!useStamina || Stamina.UseStamina (RunningStaminaCost * time))) {
				newDirection = inputDirection * RunningSpeed;
			} else if (isWalking && (!useStamina || Stamina.UseStamina (WalkingStaminaCost * time))) {
				newDirection = inputDirection * WalkingSpeed;
			} else if ((!useStamina || Stamina.UseStamina (NormalStaminaCost * time))) {
				newDirection = inputDirection * NormalSpeed;
				isRunning = isWalking = false;
			}

			if (HasAcceleration) {
				newDirection = (currentDirection + newDirection * (time * Acceleration)).ClampMagnitudeXZ (TargetSpeed);
			}

			if (useStepSound && IsGrounded) {
				currentStepTimer += time * stepsPerMeter;
				if (currentStepTimer >= 1f) {
					currentStepTimer -= 1f;
					Entity.Audio.PlayOneShot (basicStepSound);
				}
			}
			newDirection.y = Body.velocity.y;
			Body.velocity = newDirection;
		}

		#endregion

		#region Setters

		#region Others



		public virtual void SetFrozen (bool frozen)
		{
			isFrozen = frozen;
		}

		public virtual void SetFrozen (bool frozen, bool resetVelocity)
		{
			isFrozen = frozen;
			if (resetVelocity)
				Body.velocity = Vector3.zero;
		}

		public virtual void SetAcceleration (bool active)
		{
			hasAcceleration = active;
		}

		public virtual void UseStamina (bool active)
		{
			useStamina = active;
		}

		public virtual void UseStepSound (bool active)
		{
			useStepSound = active;
		}

		/// <summary>
		/// Sets the state. Should only be used in Crouch and or Prone
		/// </summary>
		/// <param name="state">State.</param>
		public virtual void SetState (int state)
		{
			currentState.Value = state;
		}

		#endregion

		#region Normal Speed

		public virtual void SetNormalBaseSpeed (float value)
		{
			baseNormalSpeed = value;
		}

		public virtual void AddNormalBaseSpeed (float amount)
		{
			baseNormalSpeed += amount;
		}

		public virtual void RemoveNormalBaseSpeed (float amount)
		{
			baseNormalSpeed -= amount;
		}

		public virtual void SetNormalSpeedMultiplier (float value)
		{
			normalSpeedMultiplier = value;
		}

		public virtual void AddNormalSpeedMultiplier (float amount)
		{
			normalSpeedMultiplier += amount;
		}

		public virtual void RemoveNormalSpeedMultiplier (float amount)
		{
			normalSpeedMultiplier -= amount;
		}

		#endregion

		#region Walking Speed

		public virtual void SetWalkingBaseSpeed (float value)
		{
			baseWalkingSpeed = value;
		}

		public virtual void AddWalkingBaseSpeed (float amount)
		{
			baseWalkingSpeed += amount;
		}

		public virtual void RemoveWalkingBaseSpeed (float amount)
		{
			baseWalkingSpeed -= amount;
		}

		public virtual void SetWalkingSpeedMultiplier (float value)
		{
			walkingSpeedMultiplier = value;
		}

		public virtual void AddWalkingSpeedMultiplier (float amount)
		{
			walkingSpeedMultiplier += amount;
		}

		public virtual void RemoveWalkingSpeedMultiplier (float amount)
		{
			walkingSpeedMultiplier -= amount;
		}

		#endregion

		#region Running Speed

		public virtual void SetRunningBaseSpeed (float value)
		{
			baseRunningSpeed = value;
		}

		public virtual void AddRunningBaseSpeed (float amount)
		{
			baseRunningSpeed += amount;
		}

		public virtual void RemoveRunningBaseSpeed (float amount)
		{
			baseRunningSpeed -= amount;
		}

		public virtual void SetRunningSpeedMultiplier (float value)
		{
			runningSpeedMultiplier = value;
		}

		public virtual void AddRunningSpeedMultiplier (float amount)
		{
			runningSpeedMultiplier += amount;
		}

		public virtual void RemoveRunningSpeedMultiplier (float amount)
		{
			runningSpeedMultiplier -= amount;
		}

		#endregion

		#region Prone Speed

		public virtual void SetProneBaseSpeed (float value)
		{
			baseProneSpeed = value;
		}

		public virtual void AddProneBaseSpeed (float amount)
		{
			baseProneSpeed += amount;
		}

		public virtual void RemoveProneBaseSpeed (float amount)
		{
			baseProneSpeed -= amount;
		}

		public virtual void SetProneSpeedMultiplier (float value)
		{
			proneSpeedMultiplier = value;
		}

		public virtual void AddProneSpeedMultiplier (float amount)
		{
			proneSpeedMultiplier += amount;
		}

		public virtual void RemoveProneSpeedMultiplier (float amount)
		{
			proneSpeedMultiplier -= amount;
		}

		#endregion

		#region Stamina Normal



		#endregion

		#endregion

		#region Settings

		public void SetRayCastTransform (Transform transform)
		{
			rayCasterTransform = transform;
		}

		public Transform GetRayCastTransform ()
		{
			return rayCasterTransform;
		}

		public void SetRayCastDirection (Vector3 direction)
		{
			rayCastDirection = direction;
		}

		public Vector3 GetRayCastDirection ()
		{
			return rayCastDirection;
		}

		public void SetRayCastDistance (float distance)
		{
			rayCastDistance = distance;
		}

		public float GetRayCastDistance ()
		{
			return rayCastDistance;
		}

		#endregion

		#region Subscribe

		public virtual void SubscribeOnStateChange (Action<int> method)
		{
			currentState.SubscribeThreadSafe (method);
		}

		public virtual void UnsubscribeOnStateChange (Action<int> method)
		{
			currentState.UnsubscribeThreadSafe (method);
		}

		#endregion

		#region EiNetInterface implementation

		public void NetWriteTo (EiBuffer buffer)
		{
			buffer.Write (inputDirection.ToVector2_XZ ());
			buffer.Write (Body.position);
			buffer.Write (isRunning);
			buffer.Write (isWalking);
		}

		public void NetReadFrom (EiBuffer buffer)
		{
			inputDirection = buffer.ReadVector2 ().ToVector3_XZ ();
			Body.MovePosition (buffer.ReadVector3 ());
			isRunning = buffer.ReadBoolean ();
			isWalking = buffer.ReadBoolean ();
		}

		public int NetPackageSize {
			get {
				return 22;
			}
		}

		#endregion
	}
}