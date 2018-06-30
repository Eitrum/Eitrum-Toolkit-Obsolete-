using System;
using UnityEngine;
using Eitrum.EiNet;

namespace Eitrum.Movement
{
	[AddComponentMenu ("Eitrum/Movement/Basic Rotation")]	
	public class EiBasicRotation : EiComponent, EiNetworkObservableInterface
	{
		#region Variables

		[Header ("Speed")]
		[SerializeField]
		protected float baseRotationSpeed = 1f;

		[SerializeField]
		protected float rotationSpeedMultiplier = 1f;

		[SerializeField]
		protected EiPropertyEventFloat neckRotationVerticalLimit = new EiPropertyEventFloat (80f);

		[SerializeField]
		protected Transform neck;

		[Header ("Input Settings")]
		[SerializeField]
		protected string horizontalAxis = "Mouse X";

		[SerializeField]
		protected string verticalAxis = "Mouse Y";


		protected Vector2 rotationInput;

		#endregion

		#region Properties

		#endregion

		#region Core

		void Awake ()
		{
			SubscribeUpdate ();
		}

		public override void UpdateComponent (float time)
		{
			rotationInput = new Vector2 (Input.GetAxisRaw (horizontalAxis), Input.GetAxisRaw (verticalAxis));

			Entity.Body.MoveRotation (Entity.Body.rotation * Quaternion.Euler (0, rotationInput.x, 0));

			var newRotation = neck.localEulerAngles.x + rotationInput.y;
			if (newRotation > 180)
				newRotation -= 360;
			neck.localEulerAngles = new Vector3 (Mathf.Clamp (newRotation, -neckRotationVerticalLimit.Value, neckRotationVerticalLimit.Value), 0, 0);
		}

		#endregion

		#region Set/Get

		public void SetNeck (Transform transform)
		{
			this.neck = transform;
		}

		public Transform GetNeck ()
		{
			return this.neck;
		}

		#endregion

		#region EiNetInterface implementation


		public void OnNetworkSerialize (EiBuffer buffer, bool isWriting)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

