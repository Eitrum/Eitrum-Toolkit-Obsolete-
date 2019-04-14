using Eitrum.Engine.Core;
using Eitrum.Engine.Utility;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Eitrum.Utility.Trigger
{
	[AddComponentMenu ("Eitrum/Utility/Trigger/On Trigger Exit")]
	public class EiOnTriggerExit : EiComponent
	{
		public UnityEventEiEntity onTriggerExit;

		void OnTriggerEnter (Collider collider)
		{
			var entity = collider.GetComponent<EiEntity> ();
			if (entity) {
				onTriggerExit.Invoke (entity);
			}
		}
	}
}

