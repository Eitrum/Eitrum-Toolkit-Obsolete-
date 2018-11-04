using System;
using UnityEngine;

namespace Eitrum
{
	[Serializable]
	public class EiSceneObject
	{
		#region Variables

		[SerializeField]
		private UnityEngine.Object sceneAssetObject;
		[SerializeField]
		private string sceneName = "";

		#endregion

		#region Properties

		public string SceneName {
			get {
				return sceneName;
			}
		}

		#endregion

		#region Implicit Conversion

		public static implicit operator string (EiSceneObject obj)
		{
			return obj.sceneName;
		}

		#endregion
	}
}