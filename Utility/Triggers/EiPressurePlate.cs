using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Eitrum.Utility.Trigger
{
	[AddComponentMenu ("Eitrum/Utility/Triggers/Pressure Plate (Physics)")]
	public class EiPressurePlate : EiComponent
	{
		#region Variables

		[Header ("Settings")]
		[SerializeField]
		private float minForce = 15f;
		[SerializeField]
		private float maxForce = 80f;

		[Header ("Position Settings")]
		[SerializeField]
		private float pressurePlateDepth = 0.1f;

		private float currentForce = 0f;
		private EiPropertyEventFloat percentage = new EiPropertyEventFloat (0f);

		private float totalImpulseMagnitude = 0;

		private float pressureSpeed = 10f;
		private Vector3 noPressurePosition;
		private Vector3 fullPressurePosition;
		private List<Rigidbody> bodies = new List<Rigidbody> ();
		private bool hasBeenPressed = false;

		[Header ("Events")]
		public UnityEventFloat onPressureChange;
		public UnityEvent onPressed;
		public UnityEvent onReleased;

		#endregion

		#region Properties

		public bool IsUnderPressure {
			get {
				return bodies.Count > 0;
			}
		}

		public float MaxForce {
			get {
				return maxForce;
			}
		}

		public float MinForce {
			get {
				return minForce;
			}
		}



		public float Percentage {
			get {
				return percentage.Value;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			pressureSpeed = 1f / pressurePlateDepth;
			noPressurePosition = this.transform.localPosition;
			fullPressurePosition = noPressurePosition + Vector3.down * pressurePlateDepth;
			SubscribeFixedUpdate ();
			percentage.Subscribe (OnPressureChange);
		}

		void OnPressureChange (float value)
		{
			if (hasBeenPressed && value < 0.05f) {
				hasBeenPressed = false;
				onReleased.Invoke ();
			}

			if (!hasBeenPressed && value > 0.95) {
				hasBeenPressed = true;
				onPressed.Invoke ();
			}

			onPressureChange.Invoke (value);
		}

		public override void FixedUpdateComponent (float time)
		{
			if (IsUnderPressure) {

				var newForce = totalImpulseMagnitude / time;
				currentForce = newForce < currentForce ? Mathf.Lerp (currentForce, newForce, time * pressureSpeed) : newForce;

				var targetPercentage = 0f;
				if (minForce >= maxForce) {
					percentage.Value = currentForce >= maxForce - 5f ? 1f : 0f;
				} else {
					targetPercentage = Mathf.Lerp (0f, 1f, (currentForce - minForce) / (maxForce - minForce));
				
					if (targetPercentage > percentage.Value) {
						percentage.Value = Mathf.Min (targetPercentage, percentage.Value + time * pressureSpeed);
					} else if (targetPercentage < percentage.Value) {
						percentage.Value = Mathf.Max (targetPercentage, percentage.Value - time * pressureSpeed);
					}
				}

				this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, Vector3.Lerp (noPressurePosition, fullPressurePosition, percentage.Value), time * pressureSpeed);
				totalImpulseMagnitude = 0f;
			} else {
				if (percentage.Value > 0f)
					percentage.Value = Mathf.Clamp01 (percentage.Value - time * pressureSpeed);
				this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, Vector3.Lerp (noPressurePosition, fullPressurePosition, percentage.Value), time * pressureSpeed);
			}
		}

		#endregion

		#region Collision Callbacks

		void OnCollisionEnter (Collision collision)
		{
			collision.rigidbody.sleepThreshold = 0f;
			if (!bodies.Contains (collision.rigidbody))
				bodies.Add (collision.rigidbody);
		}

		void OnCollisionStay (Collision collision)
		{
			totalImpulseMagnitude += collision.impulse.magnitude;
		}

		void OnCollisionExit (Collision collision)
		{
			bodies.Remove (collision.rigidbody);
			collision.rigidbody.sleepThreshold = Physics.sleepThreshold;
		}

		#endregion
	}
}

