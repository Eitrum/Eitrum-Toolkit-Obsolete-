using System;
using UnityEngine;

namespace Eitrum
{
	[AddComponentMenu ("Eitrum/Core/Input")]
	public class EiInput : EiComponent
	{

		#region Variables

		[SerializeField]
		protected bool enableInput = true;

		[SerializeField]
		protected EiInputConfig config;

		#endregion

		#region Property

		public virtual EiInputConfig Config {
			get {
				if (!config)
					config = EiInputConfig.Default;
				return config;
			}
		}

		#endregion

		#region Core

		public virtual void SetInputConfig (EiInputConfig config)
		{
			this.config = config;
		}

		public virtual void SetInputActive (bool active)
		{
			enableInput = active;
		}

		#endregion

		#region KeyInput

		public virtual bool GetKeyDown (KeyCode key)
		{
			if (enableInput)
				return Config.GetKeyDown (key);
			return false;
		}

		public virtual bool GetKeyUp (KeyCode key)
		{
			if (enableInput)
				return Config.GetKeyUp (key);
			return false;
		}

		public virtual bool GetKey (KeyCode key)
		{
			if (enableInput)
				return Config.GetKey (key);
			return false;
		}

		public virtual float GetAxis (string axis)
		{
			if (enableInput)
				return Config.GetAxis (axis);
			return 0f;
		}

		public virtual float GetAxisRaw (string axis)
		{
			if (enableInput)
				return Config.GetAxisRaw (axis);
			return 0f;
		}

		#endregion
	}
}

