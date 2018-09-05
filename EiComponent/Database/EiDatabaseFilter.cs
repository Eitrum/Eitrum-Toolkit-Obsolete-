using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace Eitrum
{
	public class EiDatabaseFilter : Attribute
	{
		public Type typeFilter;
		public string pathFilter;

		public EiDatabaseFilter(Type type)
		{
			this.typeFilter = type;
		}

		public EiDatabaseFilter(string pathFilter)
		{
			this.pathFilter = pathFilter;
		}

		public EiDatabaseFilter(Type type, string pathFilter)
		{
			this.typeFilter = type;
			this.pathFilter = pathFilter;
		}

		public bool IsCorrect(EiPrefab item, string path)
		{
			if (pathFilter != null && !path.Contains(pathFilter))
				return false;
			if (typeFilter == null)
				return true;
			return item.Item.GetComponent(typeFilter) != null;
		}
	}
}
