using System;

namespace Eitrum
{
	[Serializable]
	public class EiScene : EiScriptableObject<EiScene>
	{
		public UnityEngine.Object sceneObject;

		public string SceneName
		{
			get
			{
				return sceneObject.name;
			}
		}

		public void Load()
		{
			Loading.EiLoadingScreen.Instance.LoadLevel(SceneName);
		}
	}
}
