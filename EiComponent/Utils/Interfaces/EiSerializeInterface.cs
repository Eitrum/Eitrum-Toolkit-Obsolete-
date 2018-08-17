using System;
using UnityEngine;

namespace Eitrum {
	[Serializable]
	public class EiSerializeInterface<T> : ISerializationCallbackReceiver where T : EiBaseInterface {
		#region Variables

		[SerializeField]
		protected EiComponent component;
		protected T targetInterface;

		#endregion

		#region Properties

		public T Get {
			get {
				return targetInterface;
			}
		}

		public T Interface {
			get {
				return targetInterface;
			}
		}

		public EiComponent Component {
			get {
				return component;
			}
		}

		#endregion

		#region Constructors

		public EiSerializeInterface() {

		}

		public EiSerializeInterface(T interf) {
			component = interf.Component;
			targetInterface = interf;
		}

		#endregion

		#region ISerializationCallbackReceiver Implementation

		public void OnAfterDeserialize() {
			if (component && component.GetType().GetInterface(typeof(T).Name) == null)
				component = null;
			if (component)
				targetInterface = (T)((object)component);
		}

		public void OnBeforeSerialize() {
			if (component && component.GetType().GetInterface(typeof(T).Name) == null)
				component = null;
			if (component)
				targetInterface = (T)((object)component);
		}

		#endregion
	}
}
