using System;
using UnityEngine;
using UnityEditor;

namespace Eitrum
{
	public class InputManagerConfigEditor : Editor
	{
		#region Enums

		public enum AxisType
		{
			KeyOrMouseButton = 0,
			MouseMovement = 1,
			JoystickAxis = 2
		}

		#endregion

		public class InputAxis
		{
			public string name = "default-name";
			public string descriptiveName = "";
			public string descriptiveNegativeName = "";
			public string negativeButton = "";
			public string positiveButton = "";
			public string altNegativeButton = "";
			public string altPositiveButton = "";

			public float gravity = 0f;
			public float dead = 0.19f;
			public float sensitivity = 1f;

			public bool snap = false;
			public bool invert = false;

			public AxisType type = AxisType.JoystickAxis;

			public int axis = 0;
			public int joyNum = 0;
		}


		[MenuItem ("Eitrum/Configure/Input Manager")]
		public static void ConfigureStandardSettings ()
		{
			ClearAxis ();

			AddAxis (new InputAxis () {
				name = "Vertical",
				sensitivity = 1f,
				dead = 0f,
				type = AxisType.KeyOrMouseButton,
				axis = 1,
				positiveButton = "w",
				negativeButton = "s"
			});
			AddAxis (new InputAxis () {
				name = "Horizontal",
				sensitivity = 1f,
				dead = 0f,
				type = AxisType.KeyOrMouseButton,
				axis = 1,
				positiveButton = "d",
				negativeButton = "a"
			});
			AddAxis (new InputAxis () { name = "Mouse X", sensitivity = 1f, type = AxisType.MouseMovement, axis = 1 });
			AddAxis (new InputAxis () {
				name = "Mouse Y",
				sensitivity = 1f,
				type = AxisType.MouseMovement,
				axis = 2,
				invert = true
			});
			AddAxis (new InputAxis () { name = "Mouse ScrollWheel", sensitivity = 1f, type = AxisType.MouseMovement, axis = 3 });

			for (int joysticks = 0; joysticks < 8; joysticks++) {
				for (int axises = 0; axises < 24; axises++) {
					AddAxis (new InputAxis () {
						name = string.Format ("Joystick {0} A {1}", joysticks + 1, axises + 1),
						dead = 0.19f,
						sensitivity = 1f,
						type = AxisType.JoystickAxis,
						axis = axises + 1,
						joyNum = joysticks + 1
					});
				}
			}
		}

		private static void ClearAxis ()
		{
			SerializedObject serializedObject = new SerializedObject (AssetDatabase.LoadAllAssetsAtPath ("ProjectSettings/InputManager.asset") [0]);
			SerializedProperty axesProperty = serializedObject.FindProperty ("m_Axes");
			axesProperty.ClearArray ();
			serializedObject.ApplyModifiedProperties ();
		}

		private static void AddAxis (InputAxis axis)
		{
			if (AxisDefined (axis.name))
				return;

			SerializedObject serializedObject = new SerializedObject (AssetDatabase.LoadAllAssetsAtPath ("ProjectSettings/InputManager.asset") [0]);
			SerializedProperty axesProperty = serializedObject.FindProperty ("m_Axes");

			axesProperty.arraySize++;
			serializedObject.ApplyModifiedProperties ();

			SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex (axesProperty.arraySize - 1);

			GetChildProperty (axisProperty, "m_Name").stringValue = axis.name;
			GetChildProperty (axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
			GetChildProperty (axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
			GetChildProperty (axisProperty, "negativeButton").stringValue = axis.negativeButton;
			GetChildProperty (axisProperty, "positiveButton").stringValue = axis.positiveButton;
			GetChildProperty (axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
			GetChildProperty (axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
			GetChildProperty (axisProperty, "gravity").floatValue = axis.gravity;
			GetChildProperty (axisProperty, "dead").floatValue = axis.dead;
			GetChildProperty (axisProperty, "sensitivity").floatValue = axis.sensitivity;
			GetChildProperty (axisProperty, "snap").boolValue = axis.snap;
			GetChildProperty (axisProperty, "invert").boolValue = axis.invert;
			GetChildProperty (axisProperty, "type").intValue = (int)axis.type;
			GetChildProperty (axisProperty, "axis").intValue = axis.axis - 1;
			GetChildProperty (axisProperty, "joyNum").intValue = axis.joyNum;

			serializedObject.ApplyModifiedProperties ();
		}

		private static bool AxisDefined (string axisName)
		{
			SerializedObject serializedObject = new SerializedObject (AssetDatabase.LoadAllAssetsAtPath ("ProjectSettings/InputManager.asset") [0]);
			SerializedProperty axesProperty = serializedObject.FindProperty ("m_Axes");

			axesProperty.Next (true);
			axesProperty.Next (true);
			while (axesProperty.Next (false)) {
				SerializedProperty axis = axesProperty.Copy ();
				axis.Next (true);
				if (axis.stringValue == axisName)
					return true;
			}
			return false;
		}

		private static SerializedProperty GetChildProperty (SerializedProperty parent, string name)
		{
			SerializedProperty child = parent.Copy ();
			child.Next (true);
			do {
				if (child.name == name)
					return child;
			} while (child.Next (false));
			return null;
		}
	}
}

