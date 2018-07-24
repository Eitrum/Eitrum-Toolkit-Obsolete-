using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Eitrum.Loading
{
	public class EiLoadingScreen : EiComponentSingleton<EiLoadingScreen>
	{
		#region Singleton

		public override void SingletonCreation()
		{
			KeepAlive();
		}

		#endregion

		#region Variables
		
		public bool autoActivateScene = false;
		private AsyncOperation async = null;
		private string currentLoadingSceneName = "";

		private EiTrigger<string> onStartLoading = new EiTrigger<string>();
		private EiTrigger<string> onDoneLoading = new EiTrigger<string>();

		#endregion

		#region Properties

		public bool IsLoading
		{
			get
			{
				return async != null;
			}
		}

		public float Progress
		{
			get
			{
				if (async == null)
					return 0f;

				if (IsAllowSceneActivation)
				{
					return async.progress / 0.9f;
				}
				return async.progress;
			}
		}

		public bool IsAllowSceneActivation
		{
			get
			{
				if (async == null)
					return autoActivateScene;
				return async.allowSceneActivation;
			}
		}

		public bool IsDone
		{
			get
			{
				if (async == null)
					return true;
				return async.isDone;
			}
		}

		#endregion

		#region Core

		public void LoadLevel(EiDatabaseItem item)
		{
			LoadLevel(item.SceneName);
		}

		public void LoadLevel(EiDatabaseItem item, bool unloadAllScenes)
		{
			LoadLevel(item.SceneName, unloadAllScenes);
		}

		public void LoadLevel(string sceneName)
		{
			LoadLevel(sceneName, true);
		}

		public void LoadLevel(string sceneName, bool unloadAllScenes)
		{
			if (async != null)
			{
				Debug.LogWarning("Can't start loading a level as its currently loading an other level");
				return;
			}

			this.gameObject.SetActive(true);

			if (unloadAllScenes)
				async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
			else
				async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

			async.allowSceneActivation = autoActivateScene;
			async.completed += OnComplete;
			currentLoadingSceneName = sceneName;

			onStartLoading.Trigger(currentLoadingSceneName);
		}

		public void ActivateScene()
		{
			if (async != null)
			{
				async.allowSceneActivation = true;
			}
		}

		private void OnComplete(AsyncOperation async)
		{
			onDoneLoading.Trigger(currentLoadingSceneName);
			this.async = null;
			this.gameObject.SetActive(false);
		}

		#endregion

		#region Unloading

		public void UnloadAllNonActiveScenes()
		{
			UnloadAllScenesExcept(SceneManager.GetActiveScene().name);
		}

		public void UnloadAllScenes()
		{
			var sceneCount = SceneManager.sceneCount;
			for (int i = sceneCount - 1; i >= 0; i--)
			{
				var scene = SceneManager.GetSceneAt(i);
				SceneManager.UnloadSceneAsync(scene);
			}
		}

		public void UnloadAllScenesExcept(string sceneName)
		{
			var sceneCount = SceneManager.sceneCount;
			for (int i = sceneCount - 1; i >= 0; i--)
			{
				var scene = SceneManager.GetSceneAt(i);
				if (scene.name != sceneName)
					SceneManager.UnloadSceneAsync(scene);
			}
		}

		#endregion
	}
}