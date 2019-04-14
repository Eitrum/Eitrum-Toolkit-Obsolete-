using Eitrum.Engine.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Utility.Trigger
{
	public class EiPressurePlateTrigger : EiComponent
	{
		private List<Rigidbody> bodies = new List<Rigidbody> ();

		void OnTriggerEnter (Collider collider)
		{
			var rb = collider.attachedRigidbody;
			if (rb) {
				if (!bodies.Contains (rb)) {
					bodies.Add (rb);
				}
			}
		}

		void OnColliderExit (Collider collider)
		{
			var rb = collider.attachedRigidbody;
			if (rb) {
				if (bodies.Contains (rb)) {
					bodies.Remove (rb);
				}
			}
		}
	}
}