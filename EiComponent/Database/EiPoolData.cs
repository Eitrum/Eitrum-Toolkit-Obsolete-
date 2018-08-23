using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Eitrum {
	[Serializable]
	public class EiPoolData : EiUpdateInterface {
		#region Variables

		[SerializeField]
		private int poolSize = 0;
		[SerializeField]
		private bool keepPoolAlive = false;
		[SerializeField]
		private EiPrefab prefab;
		private Queue<EiEntity> pooledObjects = new Queue<EiEntity>();

		private Transform parentContainer;

		private int objectsToInstantiate = 0;
		private EiLLNode<EiUpdateInterface> updateNode;

		#endregion

		#region Properties

		public bool HasPooling {
			get {
				return poolSize > 0;
			}
		}

		public int PoolSize {
			get {
				return poolSize;
			}
		}

		public int PooledAmount {
			get {
				return pooledObjects.Count;
			}
		}

		public bool KeepPoolAlive {
			get {
				return keepPoolAlive;
			}
		}

		public EiPrefab Prefab {
			get {
				return prefab;
			}
		}

		#endregion

		#region Core

		// TODO: Move Static Helper into these methods in core region
		public EiEntity Get() {
			if (pooledObjects.Count == 0) {
				return null;
			}
			return pooledObjects.Dequeue();
		}

		public bool Set(EiEntity entity) {
			if (pooledObjects.Count < poolSize) {
				if (parentContainer == null) {
					parentContainer = new GameObject(entity.EntityName + " Pool").transform;
					parentContainer.SetActive(false);
				}
				entity.SleepPhysics();
				entity.transform.SetParent(parentContainer);
				pooledObjects.Enqueue(entity);
				return true;
			}
			return false;
		}

		#endregion

		#region Fill API

		/// <summary>
		/// Fills the pool with objects until its full, 1 object per frame
		/// </summary>
		public void Fill() {
			PreLoadObjects(poolSize - pooledObjects.Count);
		}

		/// <summary>
		/// Pre loads the pool with 1 object during this frame
		/// </summary>
		public void PreLoadObject() {
			if (parentContainer == null) {
				parentContainer = new GameObject(prefab.ItemName + " Pool").transform;
				parentContainer.SetActive(false);
				if (keepPoolAlive)
					MonoBehaviour.DontDestroyOnLoad(parentContainer.gameObject);
			}
			var gameObject = MonoBehaviour.Instantiate(prefab.GameObject, parentContainer);
			var entity = gameObject.GetComponent<EiEntity>();
			if (entity)
				pooledObjects.Enqueue(entity);
		}

		/// <summary>
		/// Pre loads the pool with objects over a set amount of time
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="time"></param>
		public void PreLoadObjects(int amount, float time) {
			amount -= pooledObjects.Count;
			EiTimer.Repeat(time / (float)amount, amount, PreLoadObject);
		}

		/// <summary>
		/// Pre loads the pool with objects 1 at a frame
		/// </summary>
		/// <param name="amount"></param>
		public void PreLoadObjects(int amount) {
			objectsToInstantiate += amount - pooledObjects.Count;
			if (updateNode == null)
				updateNode = EiUpdateSystem.Instance.SubscribeUpdate(this);
		}

		#endregion

		#region Update system Implementations

		EiComponent EiBaseInterface.Component {
			get {
				return null;
			}
		}

		EiCore EiBaseInterface.Core {
			get {
				return null;
			}
		}

		bool EiBaseInterface.IsNull {
			get {
				return this == null;
			}
		}

		void EiUpdateInterface.PreUpdateComponent(float time) {
			throw new NotImplementedException();
		}

		void EiUpdateInterface.UpdateComponent(float time) {
			PreLoadObject();
			objectsToInstantiate--;
			if (objectsToInstantiate <= 0) {
				EiUpdateSystem.Instance.UnsubscribeUpdate(updateNode);
				updateNode = null;
			}
		}

		void EiUpdateInterface.LateUpdateComponent(float time) {
			throw new NotImplementedException();
		}

		void EiUpdateInterface.FixedUpdateComponent(float time) {
			throw new NotImplementedException();
		}

		void EiUpdateInterface.ThreadedUpdateComponent(float time) {
			throw new NotImplementedException();
		}

		#endregion

		#region Static Helper Methods

		public static void OnPoolInstantiateHelper(EiEntity entity) {
			var transform = entity.transform;
			entity.ReleaseParent();
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
#if EITRUM_POOLING
			var interfaces = entity.PoolableInterfaces;
			for (int i = 0; i < interfaces.Length; i++) {
				interfaces[i].Get.OnPoolInstantiate();
			}
#endif
		}

		public static void OnPoolInstantiateHelper(EiEntity entity, Vector3 position, Quaternion rotation, Transform parent) {
			var transform = entity.transform;
			entity.ReleaseParent();
			transform.localPosition = position;
			transform.localRotation = rotation;
#if EITRUM_POOLING
			var interfaces = entity.PoolableInterfaces;
			for (int i = 0; i < interfaces.Length; i++) {
				interfaces[i].Get.OnPoolInstantiate();
			}
#endif
		}

		public static void OnPoolInstantiateHelper(EiEntity entity, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent) {
			var transform = entity.transform;
			entity.ReleaseParent();
			transform.localPosition = position;
			transform.localRotation = rotation;

			var goScale = transform.localScale;
			goScale.Scale(scale);
			transform.localScale = goScale;
#if EITRUM_POOLING
			var interfaces = entity.PoolableInterfaces;
			for (int i = 0; i < interfaces.Length; i++) {
				interfaces[i].Get.OnPoolInstantiate();
			}
#endif
		}

		public static void OnPoolDestroyHelper(EiEntity entity) {
#if EITRUM_POOLING
			if (entity.PoolTarget != null) {
				var interfaces = entity.PoolableInterfaces;
				for (int i = 0; i < interfaces.Length; i++) {
					interfaces[i].Get.OnPoolDestroy();
				}
				var poolTarget = entity.PoolTarget;
				if (!poolTarget.Set(entity)) {
					MonoBehaviour.Destroy(entity.gameObject);
				}
			}
			else {
				MonoBehaviour.Destroy(entity.gameObject);
			}
#else
			MonoBehaviour.Destroy(entity.gameObject);
#endif
		}

		#endregion
	}
}