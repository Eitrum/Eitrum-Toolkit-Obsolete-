using System;
using UnityEngine;

namespace Eitrum.Inventory
{
	public class EiItemDatabase : EiComponentSingleton<EiItemDatabase>
	{
		public override bool KeepInResources ()
		{
			return true;
		}

		[SerializeField]
		protected EiItem[] itemList;
	}
}

