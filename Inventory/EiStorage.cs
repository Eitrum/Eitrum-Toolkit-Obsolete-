using System;
using UnityEngine;

namespace Eitrum.Inventory
{
	[AddComponentMenu ("Eitrum/Inventory/Storage")]
	public class EiStorage : EiComponent
	{
		#region Variables

		[Header ("Settings")]
		public int storageSize = 24;
		[Space (8f)]
		public bool useWeight = false;
		public float maxWeight = 0f;

		[Space (8f)]
		public Transform dropLocation;

		private EiSyncronizedList<EiItem> itemList = new EiSyncronizedList<EiItem> ();
		private float cachedWeight = 0f;

		#endregion

		public bool AddItem (EiItem item)
		{
			if (item.MaxStacks > 1) {
				var size = itemList.Length;
				for (int i = 0; i < size; i++) {
					var tempItem = itemList [i];
					if (tempItem.ItemId == item.ItemId) {
						var missingForFullStack = tempItem.MaxStacks - tempItem.Amount;
						var itemsToAdd = Math.Min (missingForFullStack, item.Amount);
						tempItem.Amount += itemsToAdd;
						item.Amount -= itemsToAdd;
					}
					if (item.Amount <= 0) {
						EiTask.RunUnityTask (InternalDestroyItem, item);
						return true;
					}
				}
			} 
			// Default Add if possible
			if (itemList.Count < storageSize && (!useWeight || !item.UseWeightInStorage || (maxWeight >= cachedWeight + item.Weight))) {
				itemList.Add (item);
				EiTask.RunUnityTask (InternalHideItem, item);
				return true;
			}
			return false;
		}

		private void InternalDestroyItem (EiItem item)
		{
			Destroy (item.gameObject);
		}

		private void InternalHideItem (EiItem item)
		{
			item.gameObject.SetActive (false);
			item.transform.SetParent (this.transform);
			item.transform.localPosition = Vector3.zero;
		}

		public EiSyncronizedList<EiItem> GetItems ()
		{
			return itemList;
		}
	}
}