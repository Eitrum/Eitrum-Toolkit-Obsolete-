using System;
using UnityEngine;

namespace Eitrum.Inventory
{
	public class EiWeight : EiComponent
	{
		[Header ("Settings")]
		public EiStat weight = new EiStat (1f);

		public bool multiplyWeightWithScale = false;
		public bool setBodyWeightOnStartup = true;

		void Awake ()
		{
			var scale = transform.localScale;
			var result = scale.x * scale.y * scale.z;

			weight.SetMultiplyMultiplier (result);
		}

		public virtual float TotalWeight {
			get {
				return weight.TotalStat;
			}
		}
	}
}