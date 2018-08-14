using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Eitrum
{
	[Serializable]
	public class EiPoolData
	{
		#region Variables

		[SerializeField]
		private int poolSize = 0;

		private Queue<EiEntity> pooledObjects = new Queue<EiEntity>();

		private Transform parentContainer;

		#endregion

		#region Properties

		public bool HasPooling
		{
			get
			{
				return poolSize > 0;
			}
		}

		public int PoolSize
		{
			get
			{
				return poolSize;
			}
		}

		#endregion

		#region Core

		public EiEntity Get()
		{
			if (pooledObjects.Count == 0)
				return null;
			return pooledObjects.Dequeue();
		}

		public void Set(EiEntity entity)
		{
			if (pooledObjects.Count < poolSize)
			{
				if (parentContainer == null)
				{
					parentContainer = new GameObject(entity.EntityName + " Pool").transform;
					parentContainer.SetActive(false);
				}
				entity.transform.SetParent(parentContainer);
				pooledObjects.Enqueue(entity);
			}
		}

		#endregion

		#region Static Helper Methods

		public static void OnPoolInstantiateHelper(EiEntity entity)
		{
			var transform = entity.transform;
			transform.SetParent(null);
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;

			var interfaces = entity.PoolableInterfaces;
			for (int i = 0; i < interfaces.Length; i++)
			{
				interfaces[i].Get.OnPoolInstantiate();
			}
		}

		public static void OnPoolInstantiateHelper(EiEntity entity, Vector3 position, Quaternion rotation, Transform parent)
		{
			var transform = entity.transform;
			transform.SetParent(parent);
			transform.localPosition = position;
			transform.localRotation = rotation;

			var interfaces = entity.PoolableInterfaces;
			for (int i = 0; i < interfaces.Length; i++)
			{
				interfaces[i].Get.OnPoolInstantiate();
			}
		}

		public static void OnPoolInstantiateHelper(EiEntity entity, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
		{
			var transform = entity.transform;
			transform.SetParent(parent);
			transform.localPosition = position;
			transform.localRotation = rotation;

			var goScale = transform.localScale;
			goScale.Scale(scale);
			transform.localScale = goScale;

			var interfaces = entity.PoolableInterfaces;
			for (int i = 0; i < interfaces.Length; i++)
			{
				interfaces[i].Get.OnPoolInstantiate();
			}
		}

		public static void OnPoolDestroyHelper(EiEntity entity)
		{
			var interfaces = entity.PoolableInterfaces;
			for (int i = 0; i < interfaces.Length; i++)
			{
				interfaces[i].Get.OnPoolDestroy();
			}
		}

		#endregion
	}
}
