using Eitrum.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Eitrum
{
	public class EiPrefab : EiScriptableObject<EiPrefab>
	{
		#region Variables

		[Readonly]
		[SerializeField]
		private string itemName = "empty";
		[SerializeField]
		private GameObject item = null;
		[SerializeField]
		[Readonly]
		private int uniqueId = 0;
		[SerializeField]
		private EiPrefabDatabase database = null;

		[SerializeField]
		private EiPoolData poolData = new EiPoolData();

		#endregion

		#region Properties

		public GameObject Item
		{
			get
			{
				return item;
			}
		}

		public GameObject GameObject
		{
			get
			{
				return item;
			}
		}

		public string ItemName
		{
			get
			{
				return itemName;
			}
		}

		public int UniqueId
		{
			get
			{
				return uniqueId;
			}
		}

		public EiPrefabDatabase Database
		{
			get
			{
				return database;
			}
		}

		#endregion

		#region GameObject Instantiate

		public GameObject Instantiate()
		{
			if (poolData.HasPooling)
			{
				var poolObj = poolData.Get();
				if (poolObj)
				{
					EiPoolData.OnPoolInstantiateHelper(poolObj);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate(GameObject);
				var ent = entGo.GetComponent<EiEntity>();
				if (ent)
					ent.AssignPoolTarget(poolData);
				return entGo;
			}
			return MonoBehaviour.Instantiate(GameObject);
		}

		public GameObject Instantiate(Transform parent)
		{
			if (poolData.HasPooling)
			{
				var poolObj = poolData.Get();
				if (poolObj)
				{
					EiPoolData.OnPoolInstantiateHelper(poolObj, Vector3.zero, Quaternion.identity, parent);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate(GameObject, parent);
				var ent = entGo.GetComponent<EiEntity>();
				if (ent) ent.AssignPoolTarget(poolData);
				return entGo;
			}
			return MonoBehaviour.Instantiate(GameObject, parent);
		}

		public GameObject Instantiate(Vector3 position)
		{
			if (poolData.HasPooling)
			{
				var poolObj = poolData.Get();
				if (poolObj)
				{
					EiPoolData.OnPoolInstantiateHelper(poolObj, position, Quaternion.identity, null);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate(GameObject, position, Quaternion.identity);
				var ent = entGo.GetComponent<EiEntity>();
				if (ent) ent.AssignPoolTarget(poolData);
				return entGo;
			}
			return MonoBehaviour.Instantiate(GameObject, position, Quaternion.identity);
		}

		public GameObject Instantiate(Vector3 position, Quaternion rotation)
		{
			if (poolData.HasPooling)
			{
				var poolObj = poolData.Get();
				if (poolObj)
				{
					EiPoolData.OnPoolInstantiateHelper(poolObj, position, rotation, null);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate(GameObject, position, rotation);
				var ent = entGo.GetComponent<EiEntity>();
				if (ent) ent.AssignPoolTarget(poolData);
				return entGo;
			}
			return MonoBehaviour.Instantiate(GameObject, position, rotation);
		}

		public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent)
		{
			if (poolData.HasPooling)
			{
				var poolObj = poolData.Get();
				if (poolObj)
				{
					EiPoolData.OnPoolInstantiateHelper(poolObj, position, rotation, parent);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate(GameObject, position, rotation, parent);
				var ent = entGo.GetComponent<EiEntity>();
				if (ent) ent.AssignPoolTarget(poolData);
				return entGo;
			}

			return MonoBehaviour.Instantiate(GameObject, position, rotation, parent);
		}

		public GameObject Instantiate(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			if (poolData.HasPooling)
			{
				var poolObj = poolData.Get();
				if (poolObj)
				{
					EiPoolData.OnPoolInstantiateHelper(poolObj, position, rotation, scale, null);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate(GameObject, position, rotation);
				var entGoScale = entGo.transform.localScale;
				entGoScale.Scale(scale);
				entGo.transform.localScale = entGoScale;
				var ent = entGo.GetComponent<EiEntity>();
				if (ent) ent.AssignPoolTarget(poolData);
				return entGo;
			}

			var go = MonoBehaviour.Instantiate(GameObject, position, rotation);
			var goScale = go.transform.localScale;
			goScale.Scale(scale);
			go.transform.localScale = goScale;
			return go;
		}

		public GameObject Instantiate(Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
		{
			if (poolData.HasPooling)
			{
				var poolObj = poolData.Get();
				if (poolObj)
				{
					EiPoolData.OnPoolInstantiateHelper(poolObj, position, rotation, scale, parent);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate(GameObject, position, rotation, parent);
				var entGoScale = entGo.transform.localScale;
				entGoScale.Scale(scale);
				entGo.transform.localScale = entGoScale;
				var ent = entGo.GetComponent<EiEntity>();
				if (ent) ent.AssignPoolTarget(poolData);
				return entGo;
			}

			var go = MonoBehaviour.Instantiate(GameObject, position, rotation, parent);
			var goScale = go.transform.localScale;
			goScale.Scale(scale);
			go.transform.localScale = goScale;
			return go;
		}

		#endregion
	}
}
