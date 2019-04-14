using Eitrum.Engine.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Eitrum.Loading
{
	public class EiLoadingButton : EiComponent
	{
		private Button button;
		public EiLoadingScreen loadingScreen;

		private void Awake()
		{
			button = GetComponent<Button>();
			button.interactable = false;
			button.onClick.AddListener(OnButtonClick);
			loadingScreen.SubscribeOnStartLoading(OnSceneLoading);
		}

		void OnSceneLoading(string sceneName)
		{
			SubscribeUpdate();
		}

		public override void UpdateComponent(float time)
		{
			button.interactable = loadingScreen.Progress >= 1f;
		}

		void OnButtonClick()
		{
			UnsubscribeUpdate();
			loadingScreen.ActivateScene();
			button.interactable = false;
		}
	}
}