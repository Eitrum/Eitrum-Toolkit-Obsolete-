using System;
using UnityEngine;

namespace Eitrum
{
	public class EiEntry : EiScriptableObject<EiEntry>
	{
		[Readonly]
		public new string name = "Entry";
		[Readonly]
		public UnityEngine.Object targetObject;

		[Header ("Network Database")]
		[Readonly]
		public int category = 0;
		[Readonly]
		public int entry = 0;
	}
}