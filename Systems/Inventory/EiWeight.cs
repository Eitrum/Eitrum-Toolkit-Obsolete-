using System;
using UnityEngine;

namespace Eitrum.Inventory
{
	public class EiWeight : EiComponent
	{
		#region Variables

		[Header ("Settings")]
		[SerializeField]
		private EiStat weight = new EiStat (1f);

		[SerializeField]
		private bool multiplyWeightWithScale = false;
		[SerializeField]
		private bool setBodyWeightOnStartup = true;

		#endregion

		#region Properties

		public float TotalWeight {
			get {
				return weight.TotalStat;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			if (multiplyWeightWithScale) {
				var scale = transform.localScale;
				var result = scale.x * scale.y * scale.z;

				weight.AddStatMultiplier (result);
			}

			if (setBodyWeightOnStartup)
				Entity.Body.mass = weight.TotalStat;
		}

		#endregion
	}
}