﻿using System;
using UnityEngine;

namespace Eitrum
{
	[CreateAssetMenu (fileName = "Scene", menuName = "Eitrum/Database/Scene", order = 10)]
	public class EiScene : EiScriptableObject<EiScene>
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