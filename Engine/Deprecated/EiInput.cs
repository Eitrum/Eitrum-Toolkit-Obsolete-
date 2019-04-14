using System;
using UnityEngine;

namespace Eitrum.Engine.Core.Deprecated
{
	[AddComponentMenu ("Eitrum/Core/Input")]
	public class EiInput : EiComponent
	{

		#region Variables

		[SerializeField]
		protected int joystick = 0;

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

		void Start ()
		{
			Config.Instantiate ();
		}

		public virtual void SetInputConfig (EiInputConfig config)
		{
			this.config = config;
			this.config.Instantiate ();
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

		public virtual float GetAxis (EiAxis axis)
		{
			if (enableInput)
				return Config.GetAxis ("");
			return 0f;
		}

		public virtual float GetAxisRaw (EiAxis axis)
		{
			if (enableInput)
				return Config.GetAxisRaw ("");
			return 0f;
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

	public enum EiAxis
	{
		Vertical,
		Horizontal,
		Submit,
		Cancel,
		Mouse_X,
		Mouse_Y,
		Mouse_Scroll_Wheel,
		Joystick_Axis_1,
		Joystick_Axis_2,
		Joystick_Axis_3,
		Joystick_Axis_4,
		Joystick_Axis_5,
		Joystick_Axis_6,
		Joystick_Axis_7,
		Joystick_Axis_8,
		Joystick_Axis_9,
		Joystick_Axis_10,
		Joystick_Axis_11,
		Joystick_Axis_12,
		Joystick_Axis_13,
		Joystick_Axis_14,
		Joystick_Axis_15,
		Joystick_Axis_16,
		Joystick_Axis_17,
		Joystick_Axis_18,
		Joystick_Axis_19,
		Joystick_Axis_20,
		Joystick_Axis_21,
		Joystick_Axis_22,
		Joystick_Axis_23,
		Joystick_Axis_24
	}

	public enum EiAxisAll
	{
		Vertical,
		Horizontal,
		Submit,
		Cancel,
		Mouse_X,
		Mouse_Y,
		Mouse_Scroll_Wheel,

		#region Normal Joystick

		Joystick_Axis_1,
		Joystick_Axis_2,
		Joystick_Axis_3,
		Joystick_Axis_4,
		Joystick_Axis_5,
		Joystick_Axis_6,
		Joystick_Axis_7,
		Joystick_Axis_8,
		Joystick_Axis_9,
		Joystick_Axis_10,
		Joystick_Axis_11,
		Joystick_Axis_12,
		Joystick_Axis_13,
		Joystick_Axis_14,
		Joystick_Axis_15,
		Joystick_Axis_16,
		Joystick_Axis_17,
		Joystick_Axis_18,
		Joystick_Axis_19,
		Joystick_Axis_20,
		Joystick_Axis_21,
		Joystick_Axis_22,
		Joystick_Axis_23,
		Joystick_Axis_24,

		#endregion

		#region Joystick 1

		Joystick_1_Axis_1,
		Joystick_1_Axis_2,
		Joystick_1_Axis_3,
		Joystick_1_Axis_4,
		Joystick_1_Axis_5,
		Joystick_1_Axis_6,
		Joystick_1_Axis_7,
		Joystick_1_Axis_8,
		Joystick_1_Axis_9,
		Joystick_1_Axis_10,
		Joystick_1_Axis_11,
		Joystick_1_Axis_12,
		Joystick_1_Axis_13,
		Joystick_1_Axis_14,
		Joystick_1_Axis_15,
		Joystick_1_Axis_16,
		Joystick_1_Axis_17,
		Joystick_1_Axis_18,
		Joystick_1_Axis_19,
		Joystick_1_Axis_20,
		Joystick_1_Axis_21,
		Joystick_1_Axis_22,
		Joystick_1_Axis_23,
		Joystick_1_Axis_24

		#endregion
	}

	public enum EiAxisXbox
	{
		Left_Stick_Horizontal_Axis = 1,
		Left_Stick_Vertical_Axis,
		Triggers,
		Right_Stick_Horizontal_Axis,
		Right_Stick_Verical_Axis,
		D_Pad_Horizontal_Axis,
		D_Pad_Verical_Axis,
		Left_Trigger = 9,
		Right_Trigger
	}

	public enum EiAxisOculusTouch
	{
		Left_Stick_Horizontal = 0,
		Left_Stick_Verical,
		Right_Stick_Horizontal = 3,
		Right_Stick_Vertical,

		Left_Pointy_Finger_Trigger = 8,
		Right_Pointy_Finger_Trigger
	}
}