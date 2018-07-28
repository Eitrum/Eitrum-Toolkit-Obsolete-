using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Utility.Rotation
{
	[AddComponentMenu("Eitrum/Utility/Rotation/Basic")]
	public class EiRotation : EiComponent
	{
		#region Variables

		[SerializeField]
		private Vector3 rotation = Vector3.zero;
		[SerializeField]
		private Space rotationSpace = Space.Self;

		#endregion

		#region Core

		private void Awake()
		{
			SubscribeUpdate();
		}

		private void OnEnable()
		{
			SubscribeUpdate();
		}

		private void OnDisable()
		{
			UnsubscribeUpdate();
		}

		public override void UpdateComponent(float time)
		{
			this.transform.Rotate(rotation * time, rotationSpace);
		}

		#endregion
	}
}