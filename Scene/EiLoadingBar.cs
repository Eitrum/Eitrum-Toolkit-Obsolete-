using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eitrum;

namespace Eitrum.Loading
{
	public class EiLoadingBar : EiComponent
	{
		public Image image;
		public EiLoadingScreen loadingScreen;

		private void Awake()
		{
			loadingScreen.SubscribeOnStartLoading(OnSceneLoading);
			loadingScreen.SubscribeOnDoneLoading(OnSceneDone);
		}

		void OnSceneLoading(string sceneName)
		{
			SubscribeUpdate();
		}

		void OnSceneDone(string sceneName)
		{
			UnsubscribeUpdate();
		}

		public override void UpdateComponent(float time)
		{
			if (image)
				image.fillAmount = loadingScreen.Progress;
		}
	}
}