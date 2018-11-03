using Eitrum.Database;
using UnityEngine;

namespace Eitrum
{
	[CreateAssetMenu (fileName = "Prefab", menuName = "Eitrum/Database/Prefab", order = 1000)]
	public class EiPrefab : EiScriptableObject<EiPrefab>
	{
		#region Variables

		[SerializeField] private string itemName = "empty";
		[SerializeField] private GameObject item = null;
		[SerializeField] private int uniqueId = 0;
		[SerializeField] private EiPrefabDatabase database = null;

		#if EITRUM_POOLING
		[SerializeField]
		private EiPoolData poolData = new EiPoolData ();
		#endif

		#endregion

		#region Properties

		public GameObject Item {
			get {
				return item;
			}
		}

		public GameObject GameObject {
			get {
				return item;
			}
		}

		public string ItemName {
			get {
				return itemName;
			}
		}

		public int UniqueId {
			get {
				return uniqueId;
			}
		}

		public EiPrefabDatabase Database {
			get {
				return database;
			}
		}

		#endregion

		#region GameObject Instantiate

		public GameObject Instantiate ()
		{
#if EITRUM_POOLING
			if (poolData.HasPooling) {
				var poolObj = poolData.Get ();
				if (poolObj) {
					EiPoolData.OnPoolInstantiateHelper (poolObj);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate (GameObject);
				var ent = entGo.GetComponent<EiEntity> ();
				if (ent)
					ent.AssignPoolTarget (poolData);
				return entGo;
			}
#endif
			return MonoBehaviour.Instantiate (GameObject);
		}

		public GameObject Instantiate (Transform parent)
		{
#if EITRUM_POOLING
			if (poolData.HasPooling) {
				var poolObj = poolData.Get ();
				if (poolObj) {
					EiPoolData.OnPoolInstantiateHelper (poolObj, Vector3.zero, Quaternion.identity, parent);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate (GameObject, parent);
				var ent = entGo.GetComponent<EiEntity> ();
				if (ent)
					ent.AssignPoolTarget (poolData);
				return entGo;
			}
#endif
			return MonoBehaviour.Instantiate (GameObject, parent);
		}

		public GameObject Instantiate (Vector3 position)
		{
#if EITRUM_POOLING
			if (poolData.HasPooling) {
				var poolObj = poolData.Get ();
				if (poolObj) {
					EiPoolData.OnPoolInstantiateHelper (poolObj, position, Quaternion.identity, null);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate (GameObject, position, Quaternion.identity);
				var ent = entGo.GetComponent<EiEntity> ();
				if (ent)
					ent.AssignPoolTarget (poolData);
				return entGo;
			}
#endif
			return MonoBehaviour.Instantiate (GameObject, position, Quaternion.identity);
		}

		public GameObject Instantiate (Vector3 position, Quaternion rotation)
		{
#if EITRUM_POOLING
			if (poolData.HasPooling) {
				var poolObj = poolData.Get ();
				if (poolObj) {
					EiPoolData.OnPoolInstantiateHelper (poolObj, position, rotation, null);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate (GameObject, position, rotation);
				var ent = entGo.GetComponent<EiEntity> ();
				if (ent)
					ent.AssignPoolTarget (poolData);
				return entGo;
			}
#endif
			return MonoBehaviour.Instantiate (GameObject, position, rotation);
		}

		public GameObject Instantiate (Vector3 position, Quaternion rotation, Transform parent)
		{
#if EITRUM_POOLING
			if (poolData.HasPooling) {
				var poolObj = poolData.Get ();
				if (poolObj) {
					EiPoolData.OnPoolInstantiateHelper (poolObj, position, rotation, parent);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate (GameObject, position, rotation, parent);
				var ent = entGo.GetComponent<EiEntity> ();
				if (ent)
					ent.AssignPoolTarget (poolData);
				return entGo;
			}
#endif

			return MonoBehaviour.Instantiate (GameObject, position, rotation, parent);
		}

		public GameObject Instantiate (Vector3 position, Quaternion rotation, Vector3 scale)
		{
#if EITRUM_POOLING
			if (poolData.HasPooling) {
				var poolObj = poolData.Get ();
				if (poolObj) {
					EiPoolData.OnPoolInstantiateHelper (poolObj, position, rotation, scale, null);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate (GameObject, position, rotation);
				var entGoScale = entGo.transform.localScale;
				entGoScale.Scale (scale);
				entGo.transform.localScale = entGoScale;
				var ent = entGo.GetComponent<EiEntity> ();
				if (ent)
					ent.AssignPoolTarget (poolData);
				return entGo;
			}
#endif

			var go = MonoBehaviour.Instantiate (GameObject, position, rotation);
			var goScale = go.transform.localScale;
			goScale.Scale (scale);
			go.transform.localScale = goScale;
			return go;
		}

		public GameObject Instantiate (Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
		{
#if EITRUM_POOLING
			if (poolData.HasPooling) {
				var poolObj = poolData.Get ();
				if (poolObj) {
					EiPoolData.OnPoolInstantiateHelper (poolObj, position, rotation, scale, parent);
					return poolObj.gameObject;
				}
				var entGo = MonoBehaviour.Instantiate (GameObject, position, rotation, parent);
				var entGoScale = entGo.transform.localScale;
				entGoScale.Scale (scale);
				entGo.transform.localScale = entGoScale;
				var ent = entGo.GetComponent<EiEntity> ();
				if (ent)
					ent.AssignPoolTarget (poolData);
				return entGo;
			}
#endif
			var go = MonoBehaviour.Instantiate (GameObject, position, rotation, parent);
			var goScale = go.transform.localScale;
			goScale.Scale (scale);
			go.transform.localScale = goScale;
			return go;
		}

		#endregion

		#region PoolData API

		/// <summary>
		/// Fills the pool to its maximum by 1 object a frame
		/// ONLY WORKS WHEN POOLING IS ENABLED
		/// </summary>
		[System.Diagnostics.Conditional ("EITRUM_POOLING")]
		public void PoolFill ()
		{
#if EITRUM_POOLING
			poolData.Fill ();
#endif
		}

		/// <summary>
		/// Pre loads the pool with 1 object during this frame
		/// ONLY WORKS WHEN POOLING IS ENABLED
		/// </summary>
		[System.Diagnostics.Conditional ("EITRUM_POOLING")]
		public void PoolPreLoadObject ()
		{
#if EITRUM_POOLING
			poolData.PreLoadObject ();
#endif
		}

		/// <summary>
		/// Pre loads the pool with objects over a set amount of time
		/// ONLY WORKS WHEN POOLING IS ENABLED
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="time"></param>
		[System.Diagnostics.Conditional ("EITRUM_POOLING")]
		public void PoolPreLoadObjects (int amount, float time)
		{
#if EITRUM_POOLING
			poolData.PreLoadObjects (amount, time);
#endif
		}

		/// <summary>
		/// Pre loads the pool with objects 1 at a frame
		/// ONLY WORKS WHEN POOLING IS ENABLED
		/// </summary>
		/// <param name="amount"></param>
		[System.Diagnostics.Conditional ("EITRUM_POOLING")]
		public void PoolPreLoadObjects (int amount)
		{
#if EITRUM_POOLING
			poolData.PreLoadObjects (amount);
#endif
		}

		/// <summary>
		/// Clears the pool of any objects not in use
		/// ONLY WORKS WHEN POOLING IS ENABLED
		/// </summary>
		/// <param name="amount"></param>
		[System.Diagnostics.Conditional ("EITRUM_POOLING")]
		public void PoolClear ()
		{
#if EITRUM_POOLING
			poolData.ClearObjects ();
#endif
		}

		#endregion

		#region Editor

		#if UNITY_EDITOR

		public string editorPathName = "null";

		#endif

		#endregion
	}
}
