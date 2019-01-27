using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace Eitrum
{
	public class EiPrefabFilter : Attribute
	{
		#region Variables

		public Type typeFilter;
		public string pathFilter;

		#endregion

		#region Constructor

		public EiPrefabFilter (Type type)
		{
			this.typeFilter = type;
		}

		public EiPrefabFilter (string pathFilter)
		{
			this.pathFilter = pathFilter;
		}

		public EiPrefabFilter (Type type, string pathFilter)
		{
			this.typeFilter = type;
			this.pathFilter = pathFilter;
		}

		#endregion

		#region Helper

		public bool IsCorrect (EiPrefab item, string path)
		{
			if (pathFilter != null && !path.Contains (pathFilter))
				return false;
			if (typeFilter == null)
				return true;
			return item.Item.GetComponent (typeFilter) != null;
		}

		#endregion
	}
}