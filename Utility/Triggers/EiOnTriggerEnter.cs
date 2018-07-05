using System;
using UnityEngine;
using UnityEngine.Events;

namespace Eitrum.Utility.Trigger
{
	[AddComponentMenu ("Eitrum/Utility/Trigger/OnTriggerEnter")]
	public class EiOnTriggerEnter : EiComponent
	{
		public bool useOnTriggerEnter = true;
		public UnityEvent onTriggerEnter;
		public bool useOnColliderEnter = false;
		public UnityEventCollider onColliderEnter;
		public bool useOnEntityEnter = false;
		public UnityEventEiEntity onEntityEnter;

		void OnTriggerEnter (Collider collider)
		{
			if (useOnTriggerEnter)
				onTriggerEnter.Invoke ();
			if (useOnColliderEnter)
				onColliderEnter.Invoke (collider);
			if (useOnEntityEnter) {
				var entity = collider.GetComponent<EiEntity> ();
				if (entity)
					onEntityEnter.Invoke (entity);
			}
		}
	}
}

