using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum
{
	[Serializable]
	public class EiDatabaseCategory
	{
		#region Variables

		[SerializeField]
		private string categoryName = "";
		[SerializeField]
		private List<EiDatabaseItem> entries = new List<EiDatabaseItem> ();

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

		public EiDatabaseItem this [int index] {
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

		public EiDatabaseItem GetEntry (int index)
		{
			return entries [index];
		}

		#endregion
	}
}