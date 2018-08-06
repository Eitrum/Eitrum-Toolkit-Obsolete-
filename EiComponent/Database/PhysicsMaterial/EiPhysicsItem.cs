using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum
{
	public class EiPhysicsItem : EiScriptableObject<EiPhysicsItem>
	{
		#region Variables

		public PhysicMaterial physicMaterial = null;
		public float materialStrength = 1f;

		#endregion

		#region Properties

		public PhysicMaterial PhysicMaterial
		{
			get
			{
				return physicMaterial;
			}
		}

		public float MaterialStrength
		{
			get
			{
				return materialStrength;
			}
		}

		#endregion
	}
}