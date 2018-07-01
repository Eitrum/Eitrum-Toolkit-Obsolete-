using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum
{
	[Serializable]
	public class EiCategory
	{
		#region Variables

		[SerializeField]
		private string categoryName = "";
		[SerializeField]
		private List<EiEntry> entries = new List<EiEntry> ();

		#endregion

		#region Properties

		public string CategoryName {
			get {
				return categoryName;
			}
		}

		public int Length {
			get {
				return entries.Count;
			}
		}

		public EiEntry this [int index] {
			get {
				return entries [index];
			}
		}

		#endregion

		#region Core

		public int EntriesLength ()
		{
			return entries.Count;
		}

		public EiEntry GetEntry (int index)
		{
			return entries [index];
		}

		#endregion
	}
}