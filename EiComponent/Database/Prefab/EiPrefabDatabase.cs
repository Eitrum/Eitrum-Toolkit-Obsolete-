using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Eitrum.Database
{
	public class EiPrefabDatabase : EiComponentSingleton<EiPrefabDatabase>
	{
		#region Singleton

		public override bool KeepInResources()
		{
			return true;
		}

		#endregion

		#region Variables

#pragma warning disable
		[SerializeField]
		private List<EiPrefabSubCategory> subCategories = new List<EiPrefabSubCategory>();

		[SerializeField]
		private List<EiPrefab> cachedItems = new List<EiPrefab>();

		#endregion

		#region Core

		public EiPrefab _Get(int index)
		{
			return cachedItems[index];
		}

		#endregion

		#region Static Getters

		public EiPrefab Get(int index)
		{
			return Instance._Get(index);
		}

		#endregion
	}
}
