using System;
using UnityEngine;

namespace Eitrum
{
	[CreateAssetMenu (fileName = "Scene", menuName = "Eitrum/Database/Scene", order = 20)]
	public class EiScene : EiScriptableObject
	{
		#region Variables

		public EiSceneObject scene;

		#endregion

		#region Properties

		public string SceneName {
			get {
				return scene;
			}
		}

		#endregion
	}
}