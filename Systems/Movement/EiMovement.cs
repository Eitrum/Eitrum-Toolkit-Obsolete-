using Eitrum.Engine.Core;
using UnityEngine;

namespace Eitrum.Movement {
	public class EiMovement : EiComponent {

		#region Variables
		
		[Header("Settings")]
		public EiStat maxSpeed = new EiStat(5f, 1f, 1f);
		public EiStat acceleration = new EiStat(5f, 1f, 1f);

		[Header("Ground Settings")]
		public bool useSteepAngle = true;
		public float maxSteepAngle = 90f;
		public AnimationCurve steepAngleMultiplier = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		[Header("Air Settings")]
		[Range(0f, 2f)]
		public float airControl = 0.4f;


		[Readonly]
		[SerializeField]
		private Vector2 input;

		private EiBoolStack disabled = new EiBoolStack();
		private RaycastHit lastGroundHit;
		private bool isGrounded = false;
		private Rigidbody body;

		#endregion

		#region Properties

		public float MaxSpeed {
			get {
				return maxSpeed.TotalStat;
			}
		}

		public float CurrentSpeed {
			get {
				return body.velocity.ToVector2_XZ().magnitude;
			}
		}

		public float Acceleration {
			get {
				return acceleration.TotalStat;
			}
		}

		public bool IsDisabled {
			get {
				return disabled;
			}
			set {
				if (value)
					disabled++;
				else
					disabled--;
			}
		}

		public bool IsEnabled {
			get {
				return !disabled;
			}
		}

		public bool IsGrounded {
			get {
				return isGrounded;
			}
		}

		public Vector2 InputValue {
			get {
				return input;
			}
			set {
				AssignInput(value);
			}
		}

		#endregion

		#region Core

		public void AssignInput(Vector2 input) {
			this.input = Vector2.ClampMagnitude(input, 1f);
		}

		private void Awake() {
#if !EITRUM_PERFORMANCE_MODE
			if (Acceleration * Time.fixedDeltaTime > 1f)
				Debug.Log("Acceleration of the object is above fixed time rate, this might cause some unexpected behaviour");
#endif
			SubscribeFixedUpdate();
			body = Entity.Body;
		}

		public override void FixedUpdateComponent(float time) {
			isGrounded = body.IsGrounded(out lastGroundHit);

			input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

			if (IsGrounded)
				ApplyGroundForce();
			else
				ApplyAirForce();
		}

		private void ApplyGroundForce() {
			var direction = input.ToVector3_XZ() * MaxSpeed;
			var targetVelocity = direction.ToVector3_XZ();
			var forceMultiplier = body.mass * Acceleration;

			if (useSteepAngle) {
				var groundNormalRotation = Quaternion.FromToRotation(Vector3.up, lastGroundHit.normal);
				var angle = Quaternion.Angle(Quaternion.identity, groundNormalRotation);

				var percentage = angle / maxSteepAngle;
				targetVelocity = groundNormalRotation * targetVelocity;
				if (targetVelocity.y < 0)
					forceMultiplier *= 1f + this.steepAngleMultiplier.Evaluate(1f - percentage);
				else
					forceMultiplier *= this.steepAngleMultiplier.Evaluate(percentage);
			}

			var currentVelocity = body.velocity;
			var force = (targetVelocity - currentVelocity);
			if (force.sqrMagnitude > Mathf.Epsilon)
				body.AddForce(force * forceMultiplier);
		}

		private void ApplyAirForce() {
			var direction = input.ToVector3_XZ() * MaxSpeed;
			var forceMultiplier = (body.mass * Acceleration * airControl);

			var targetVelocity = direction.ToVector3_XZ();
			var currentVelocity = body.velocity.ToVector3_XZ();
			var force = (targetVelocity - currentVelocity);

			if (force.sqrMagnitude > Mathf.Epsilon)
				body.AddForce(force * forceMultiplier);
		}

		#endregion

		#region Disable API

		public void Disable() {
			disabled++;
		}

		public void Enable() {
			disabled--;
		}

		public EiBoolStack GetDisabled() {
			return disabled;
		}

		#endregion
	}
}
