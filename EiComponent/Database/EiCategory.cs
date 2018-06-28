using System;
using UnityEngine;
using System.Collections.Generic;

namespace Eitrum
{
	public class EiCategory : EiScriptableObject<EiCategory>
	{
		[Readonly]
		public new string name = "Category";
		[Readonly]
		public bool folded = false;

		[HideInInspector]
		public List<EiEntry> entries = new List<EiEntry> ();
	}
}