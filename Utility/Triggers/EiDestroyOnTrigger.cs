using Eitrum.Engine.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Utility.Trigger
{
	public class EiDestroyOnTrigger : EiComponent
	{
		public float delay = 0f;

		public void DestroyOnTrigger (EiEntity entity)
		{
			Destroy (entity.gameObject, delay);
		}
	}
}