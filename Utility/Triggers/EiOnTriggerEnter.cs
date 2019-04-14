using Eitrum.Engine.Core;
using Eitrum.Engine.Utility;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Eitrum.Utility.Trigger
{
	[AddComponentMenu ("Eitrum/Utility/Trigger/On Trigger Enter")]
	public class EiOnTriggerEnter : EiComponent
	{
		public UnityEventEiEntity onTriggerEnter;

		void OnTriggerEnter (Collider collider)
		{
			var entity = collider.GetComponent<EiEntity> ();
			if (entity) {
				onTriggerEnter.Invoke (entity);
			}
		}
	}
}

