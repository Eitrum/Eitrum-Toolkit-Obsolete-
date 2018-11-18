﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Eitrum.EditorUtil
{
	[CustomPropertyDrawer (typeof(KeyCode))]
	public class KeyCodeEditor : PropertyDrawer
	{
		#region Dictianaries

		/*static Dictionary<KeyCode, string> keyCodeToPath = new Dictionary<KeyCode, string> () {
			{ KeyCode.None, "None" },
			{ KeyCode.Backspace, "Keyboard/Not Used/Backspace" },
			{ KeyCode.Delete, "Keyboard/Not Used/Delete" },
			{ KeyCode.Tab, "Keyboard/Tab" },
			{ KeyCode.Clear, "Keyboard/Not Used/Clear" },
			{ KeyCode.Return, "Keyboard/Not Used/Return" },
			{ KeyCode.Pause, "Keyboard/Not Used/Pause" },
			{ KeyCode.Escape, "Keyboard/Escape" },
			{ KeyCode.Space, "Keyboard/Space" },
			{ KeyCode.Exclaim, "Keyboard/Not Used/Exclaim" },
			{ KeyCode.DoubleQuote, "Keyboard/Not Used/DoubleQuote" },
			{ KeyCode.Hash, "Keyboard/Not Used/Hash" },
			{ KeyCode.Dollar, "Keyboard/Not Used/Dollar" },
			{ KeyCode.Ampersand, "Keyboard/Not Used/Ampersand" },
			{ KeyCode.Quote, "Keyboard/Not Used/Quote" },
			{ KeyCode.LeftParen, "Keyboard/Left/Paren" },
			{ KeyCode.RightParen, "Keyboard/Right/Paren" },
			{ KeyCode.Asterisk, "Keyboard/Not Used/Asterisk" },
			{ KeyCode.Plus, "Keyboard/Not Used/Plus" },
			{ KeyCode.Comma, "Keyboard/Not Used/Comma" },
			{ KeyCode.Minus, "Keyboard/Not Used/Minus" },
			{ KeyCode.Period, "Keyboard/Not Used/Period" },
			{ KeyCode.Slash, "Keyboard/Not Used/Slash" },
			{ KeyCode.Alpha0, "Numbers/Alpha0" },
			{ KeyCode.Alpha1, "Numbers/Alpha1" },
			{ KeyCode.Alpha2, "Numbers/Alpha2" },
			{ KeyCode.Alpha3, "Numbers/Alpha3" },
			{ KeyCode.Alpha4, "Numbers/Alpha4" },
			{ KeyCode.Alpha5, "Numbers/Alpha5" },
			{ KeyCode.Alpha6, "Numbers/Alpha6" },
			{ KeyCode.Alpha7, "Numbers/Alpha7" },
			{ KeyCode.Alpha8, "Numbers/Alpha8" },
			{ KeyCode.Alpha9, "Numbers/Alpha9" },
			{ KeyCode.Colon, "Keyboard/Not Used/Colon" },
			{ KeyCode.Semicolon, "Keyboard/Not Used/Semicolon" },
			{ KeyCode.Less, "Keyboard/Not Used/Less" },
			{ KeyCode.Equals, "Keyboard/Not Used/Equals" },
			{ KeyCode.Greater, "Keyboard/Not Used/Greater" },
			{ KeyCode.Question, "Keyboard/Not Used/Question" },
			{ KeyCode.At, "Keyboard/Not Used/At" },
			{ KeyCode.LeftBracket, "Keyboard/Left/Bracket" },
			{ KeyCode.Backslash, "Keyboard/Not Used/Backslash" },
			{ KeyCode.RightBracket, "Keyboard/Right/Bracket" },
			{ KeyCode.Caret, "Keyboard/Not Used/Caret" },
			{ KeyCode.Underscore, "Keyboard/Not Used/Underscore" },
			{ KeyCode.BackQuote, "Keyboard/Not Used/BackQuote" },
			{ KeyCode.A, "Alphabet/A" },
			{ KeyCode.B, "Alphabet/B" },
			{ KeyCode.C, "Alphabet/C" },
			{ KeyCode.D, "Alphabet/D" },
			{ KeyCode.E, "Alphabet/E" },
			{ KeyCode.F, "Alphabet/F" },
			{ KeyCode.G, "Alphabet/G" },
			{ KeyCode.H, "Alphabet/H" },
			{ KeyCode.I, "Alphabet/I" },
			{ KeyCode.J, "Alphabet/J" },
			{ KeyCode.K, "Alphabet/K" },
			{ KeyCode.L, "Alphabet/L" },
			{ KeyCode.M, "Alphabet/M" },
			{ KeyCode.N, "Alphabet/N" },
			{ KeyCode.O, "Alphabet/O" },
			{ KeyCode.P, "Alphabet/P" },
			{ KeyCode.Q, "Alphabet/Q" },
			{ KeyCode.R, "Alphabet/R" },
			{ KeyCode.S, "Alphabet/S" },
			{ KeyCode.T, "Alphabet/T" },
			{ KeyCode.U, "Alphabet/U" },
			{ KeyCode.V, "Alphabet/V" },
			{ KeyCode.W, "Alphabet/W" },
			{ KeyCode.X, "Alphabet/X" },
			{ KeyCode.Y, "Alphabet/Y" },
			{ KeyCode.Z, "Alphabet/Z" },
			{ KeyCode.Keypad0, "Keypad/Keypad0" },
			{ KeyCode.Keypad1, "Keypad/Keypad1" },
			{ KeyCode.Keypad2, "Keypad/Keypad2" },
			{ KeyCode.Keypad3, "Keypad/Keypad3" },
			{ KeyCode.Keypad4, "Keypad/Keypad4" },
			{ KeyCode.Keypad5, "Keypad/Keypad5" },
			{ KeyCode.Keypad6, "Keypad/Keypad6" },
			{ KeyCode.Keypad7, "Keypad/Keypad7" },
			{ KeyCode.Keypad8, "Keypad/Keypad8" },
			{ KeyCode.Keypad9, "Keypad/Keypad9" },
			{ KeyCode.KeypadPeriod, "Keypad/KeypadPeriod" },
			{ KeyCode.KeypadDivide, "Keypad/KeypadDivide" },
			{ KeyCode.KeypadMultiply, "Keypad/KeypadMultiply" },
			{ KeyCode.KeypadMinus, "Keypad/KeypadMinus" },
			{ KeyCode.KeypadPlus, "Keypad/KeypadPlus" },
			{ KeyCode.KeypadEnter, "Keypad/KeypadEnter" },
			{ KeyCode.KeypadEquals, "Keypad/KeypadEquals" },
			{ KeyCode.UpArrow, "Arrows/UpArrow" },
			{ KeyCode.DownArrow, "Arrows/DownArrow" },
			{ KeyCode.RightArrow, "Arrows/RightArrow" },
			{ KeyCode.LeftArrow, "Arrows/LeftArrow" },
			{ KeyCode.Insert, "Keyboard/Not Used/Insert" },
			{ KeyCode.Home, "Keyboard/Not Used/Home" },
			{ KeyCode.End, "Keyboard/Not Used/End" },
			{ KeyCode.PageUp, "Keyboard/Not Used/PageUp" },
			{ KeyCode.PageDown, "Keyboard/Not Used/PageDown" },
			{ KeyCode.F1, "FButtons/F1" },
			{ KeyCode.F2, "FButtons/F2" },
			{ KeyCode.F3, "FButtons/F3" },
			{ KeyCode.F4, "FButtons/F4" },
			{ KeyCode.F5, "FButtons/F5" },
			{ KeyCode.F6, "FButtons/F6" },
			{ KeyCode.F7, "FButtons/F7" },
			{ KeyCode.F8, "FButtons/F8" },
			{ KeyCode.F9, "FButtons/F9" },
			{ KeyCode.F10, "FButtons/F10" },
			{ KeyCode.F11, "FButtons/F11" },
			{ KeyCode.F12, "FButtons/F12" },
			{ KeyCode.F13, "FButtons/F13" },
			{ KeyCode.F14, "FButtons/F14" },
			{ KeyCode.F15, "FButtons/F15" },
			{ KeyCode.Numlock, "Keypad/Numlock" },
			{ KeyCode.CapsLock, "Keyboard/Not Used/CapsLock" },
			{ KeyCode.ScrollLock, "Keyboard/Not Used/ScrollLock" },
			{ KeyCode.RightShift, "Keyboard/Right/Shift" },
			{ KeyCode.LeftShift, "Keyboard/Left/Shift" },
			{ KeyCode.RightControl, "Keyboard/Right/Control" },
			{ KeyCode.LeftControl, "Keyboard/Left/Control" },
			{ KeyCode.RightAlt, "Keyboard/Right/Alt" },
			{ KeyCode.LeftAlt, "Keyboard/Left/Alt" },
			{ KeyCode.RightCommand, "Keyboard/Right/Command" },
			{ KeyCode.LeftCommand, "Keyboard/Left/Command" },
			{ KeyCode.LeftWindows, "Keyboard/Left/Windows" },
			{ KeyCode.RightWindows, "Keyboard/Right/Windows" },
			{ KeyCode.AltGr, "Keyboard/Not Used/AltGr" },
			{ KeyCode.Help, "Keyboard/Not Used/Help" },
			{ KeyCode.Print, "Keyboard/Not Used/Print" },
			{ KeyCode.SysReq, "Keyboard/Not Used/SysReq" },
			{ KeyCode.Break, "Keyboard/Not Used/Break" },
			{ KeyCode.Menu, "Keyboard/Not Used/Menu" },
			{ KeyCode.Mouse0, "Mouse/Left (Primary 0)" },
			{ KeyCode.Mouse1, "Mouse/Right (Secondary 1)" },
			{ KeyCode.Mouse2, "Mouse/Middle (2)" },
			{ KeyCode.Mouse3, "Mouse/Mouse3" },
			{ KeyCode.Mouse4, "Mouse/Mouse4" },
			{ KeyCode.Mouse5, "Mouse/Mouse5" },
			{ KeyCode.Mouse6, "Mouse/Mouse6" },
			{ KeyCode.JoystickButton0, "Joystick/Joystick/Button0" },
			{ KeyCode.JoystickButton1, "Joystick/Joystick/Button1" },
			{ KeyCode.JoystickButton2, "Joystick/Joystick/Button2" },
			{ KeyCode.JoystickButton3, "Joystick/Joystick/Button3" },
			{ KeyCode.JoystickButton4, "Joystick/Joystick/Button4" },
			{ KeyCode.JoystickButton5, "Joystick/Joystick/Button5" },
			{ KeyCode.JoystickButton6, "Joystick/Joystick/Button6" },
			{ KeyCode.JoystickButton7, "Joystick/Joystick/Button7" },
			{ KeyCode.JoystickButton8, "Joystick/Joystick/Button8" },
			{ KeyCode.JoystickButton9, "Joystick/Joystick/Button9" },
			{ KeyCode.JoystickButton10, "Joystick/Joystick/Button10" },
			{ KeyCode.JoystickButton11, "Joystick/Joystick/Button11" },
			{ KeyCode.JoystickButton12, "Joystick/Joystick/Button12" },
			{ KeyCode.JoystickButton13, "Joystick/Joystick/Button13" },
			{ KeyCode.JoystickButton14, "Joystick/Joystick/Button14" },
			{ KeyCode.JoystickButton15, "Joystick/Joystick/Button15" },
			{ KeyCode.JoystickButton16, "Joystick/Joystick/Button16" },
			{ KeyCode.JoystickButton17, "Joystick/Joystick/Button17" },
			{ KeyCode.JoystickButton18, "Joystick/Joystick/Button18" },
			{ KeyCode.JoystickButton19, "Joystick/Joystick/Button19" },
			{ KeyCode.Joystick1Button0, "Joystick/Joystick1/Button0" },
			{ KeyCode.Joystick1Button1, "Joystick/Joystick1/Button1" },
			{ KeyCode.Joystick1Button2, "Joystick/Joystick1/Button2" },
			{ KeyCode.Joystick1Button3, "Joystick/Joystick1/Button3" },
			{ KeyCode.Joystick1Button4, "Joystick/Joystick1/Button4" },
			{ KeyCode.Joystick1Button5, "Joystick/Joystick1/Button5" },
			{ KeyCode.Joystick1Button6, "Joystick/Joystick1/Button6" },
			{ KeyCode.Joystick1Button7, "Joystick/Joystick1/Button7" },
			{ KeyCode.Joystick1Button8, "Joystick/Joystick1/Button8" },
			{ KeyCode.Joystick1Button9, "Joystick/Joystick1/Button9" },
			{ KeyCode.Joystick1Button10, "Joystick/Joystick1/Button10" },
			{ KeyCode.Joystick1Button11, "Joystick/Joystick1/Button11" },
			{ KeyCode.Joystick1Button12, "Joystick/Joystick1/Button12" },
			{ KeyCode.Joystick1Button13, "Joystick/Joystick1/Button13" },
			{ KeyCode.Joystick1Button14, "Joystick/Joystick1/Button14" },
			{ KeyCode.Joystick1Button15, "Joystick/Joystick1/Button15" },
			{ KeyCode.Joystick1Button16, "Joystick/Joystick1/Button16" },
			{ KeyCode.Joystick1Button17, "Joystick/Joystick1/Button17" },
			{ KeyCode.Joystick1Button18, "Joystick/Joystick1/Button18" },
			{ KeyCode.Joystick1Button19, "Joystick/Joystick1/Button19" },
			{ KeyCode.Joystick2Button0, "Joystick/Joystick2/Button0" },
			{ KeyCode.Joystick2Button1, "Joystick/Joystick2/Button1" },
			{ KeyCode.Joystick2Button2, "Joystick/Joystick2/Button2" },
			{ KeyCode.Joystick2Button3, "Joystick/Joystick2/Button3" },
			{ KeyCode.Joystick2Button4, "Joystick/Joystick2/Button4" },
			{ KeyCode.Joystick2Button5, "Joystick/Joystick2/Button5" },
			{ KeyCode.Joystick2Button6, "Joystick/Joystick2/Button6" },
			{ KeyCode.Joystick2Button7, "Joystick/Joystick2/Button7" },
			{ KeyCode.Joystick2Button8, "Joystick/Joystick2/Button8" },
			{ KeyCode.Joystick2Button9, "Joystick/Joystick2/Button9" },
			{ KeyCode.Joystick2Button10, "Joystick/Joystick2/Button10" },
			{ KeyCode.Joystick2Button11, "Joystick/Joystick2/Button11" },
			{ KeyCode.Joystick2Button12, "Joystick/Joystick2/Button12" },
			{ KeyCode.Joystick2Button13, "Joystick/Joystick2/Button13" },
			{ KeyCode.Joystick2Button14, "Joystick/Joystick2/Button14" },
			{ KeyCode.Joystick2Button15, "Joystick/Joystick2/Button15" },
			{ KeyCode.Joystick2Button16, "Joystick/Joystick2/Button16" },
			{ KeyCode.Joystick2Button17, "Joystick/Joystick2/Button17" },
			{ KeyCode.Joystick2Button18, "Joystick/Joystick2/Button18" },
			{ KeyCode.Joystick2Button19, "Joystick/Joystick2/Button19" },
			{ KeyCode.Joystick3Button0, "Joystick/Joystick3/Button0" },
			{ KeyCode.Joystick3Button1, "Joystick/Joystick3/Button1" },
			{ KeyCode.Joystick3Button2, "Joystick/Joystick3/Button2" },
			{ KeyCode.Joystick3Button3, "Joystick/Joystick3/Button3" },
			{ KeyCode.Joystick3Button4, "Joystick/Joystick3/Button4" },
			{ KeyCode.Joystick3Button5, "Joystick/Joystick3/Button5" },
			{ KeyCode.Joystick3Button6, "Joystick/Joystick3/Button6" },
			{ KeyCode.Joystick3Button7, "Joystick/Joystick3/Button7" },
			{ KeyCode.Joystick3Button8, "Joystick/Joystick3/Button8" },
			{ KeyCode.Joystick3Button9, "Joystick/Joystick3/Button9" },
			{ KeyCode.Joystick3Button10, "Joystick/Joystick3/Button10" },
			{ KeyCode.Joystick3Button11, "Joystick/Joystick3/Button11" },
			{ KeyCode.Joystick3Button12, "Joystick/Joystick3/Button12" },
			{ KeyCode.Joystick3Button13, "Joystick/Joystick3/Button13" },
			{ KeyCode.Joystick3Button14, "Joystick/Joystick3/Button14" },
			{ KeyCode.Joystick3Button15, "Joystick/Joystick3/Button15" },
			{ KeyCode.Joystick3Button16, "Joystick/Joystick3/Button16" },
			{ KeyCode.Joystick3Button17, "Joystick/Joystick3/Button17" },
			{ KeyCode.Joystick3Button18, "Joystick/Joystick3/Button18" },
			{ KeyCode.Joystick3Button19, "Joystick/Joystick3/Button19" },
			{ KeyCode.Joystick4Button0, "Joystick/Joystick4/Button0" },
			{ KeyCode.Joystick4Button1, "Joystick/Joystick4/Button1" },
			{ KeyCode.Joystick4Button2, "Joystick/Joystick4/Button2" },
			{ KeyCode.Joystick4Button3, "Joystick/Joystick4/Button3" },
			{ KeyCode.Joystick4Button4, "Joystick/Joystick4/Button4" },
			{ KeyCode.Joystick4Button5, "Joystick/Joystick4/Button5" },
			{ KeyCode.Joystick4Button6, "Joystick/Joystick4/Button6" },
			{ KeyCode.Joystick4Button7, "Joystick/Joystick4/Button7" },
			{ KeyCode.Joystick4Button8, "Joystick/Joystick4/Button8" },
			{ KeyCode.Joystick4Button9, "Joystick/Joystick4/Button9" },
			{ KeyCode.Joystick4Button10, "Joystick/Joystick4/Button10" },
			{ KeyCode.Joystick4Button11, "Joystick/Joystick4/Button11" },
			{ KeyCode.Joystick4Button12, "Joystick/Joystick4/Button12" },
			{ KeyCode.Joystick4Button13, "Joystick/Joystick4/Button13" },
			{ KeyCode.Joystick4Button14, "Joystick/Joystick4/Button14" },
			{ KeyCode.Joystick4Button15, "Joystick/Joystick4/Button15" },
			{ KeyCode.Joystick4Button16, "Joystick/Joystick4/Button16" },
			{ KeyCode.Joystick4Button17, "Joystick/Joystick4/Button17" },
			{ KeyCode.Joystick4Button18, "Joystick/Joystick4/Button18" },
			{ KeyCode.Joystick4Button19, "Joystick/Joystick4/Button19" },
			{ KeyCode.Joystick5Button0, "Joystick/Joystick5/Button0" },
			{ KeyCode.Joystick5Button1, "Joystick/Joystick5/Button1" },
			{ KeyCode.Joystick5Button2, "Joystick/Joystick5/Button2" },
			{ KeyCode.Joystick5Button3, "Joystick/Joystick5/Button3" },
			{ KeyCode.Joystick5Button4, "Joystick/Joystick5/Button4" },
			{ KeyCode.Joystick5Button5, "Joystick/Joystick5/Button5" },
			{ KeyCode.Joystick5Button6, "Joystick/Joystick5/Button6" },
			{ KeyCode.Joystick5Button7, "Joystick/Joystick5/Button7" },
			{ KeyCode.Joystick5Button8, "Joystick/Joystick5/Button8" },
			{ KeyCode.Joystick5Button9, "Joystick/Joystick5/Button9" },
			{ KeyCode.Joystick5Button10, "Joystick/Joystick5/Button10" },
			{ KeyCode.Joystick5Button11, "Joystick/Joystick5/Button11" },
			{ KeyCode.Joystick5Button12, "Joystick/Joystick5/Button12" },
			{ KeyCode.Joystick5Button13, "Joystick/Joystick5/Button13" },
			{ KeyCode.Joystick5Button14, "Joystick/Joystick5/Button14" },
			{ KeyCode.Joystick5Button15, "Joystick/Joystick5/Button15" },
			{ KeyCode.Joystick5Button16, "Joystick/Joystick5/Button16" },
			{ KeyCode.Joystick5Button17, "Joystick/Joystick5/Button17" },
			{ KeyCode.Joystick5Button18, "Joystick/Joystick5/Button18" },
			{ KeyCode.Joystick5Button19, "Joystick/Joystick5/Button19" },
			{ KeyCode.Joystick6Button0, "Joystick/Joystick6/Button0" },
			{ KeyCode.Joystick6Button1, "Joystick/Joystick6/Button1" },
			{ KeyCode.Joystick6Button2, "Joystick/Joystick6/Button2" },
			{ KeyCode.Joystick6Button3, "Joystick/Joystick6/Button3" },
			{ KeyCode.Joystick6Button4, "Joystick/Joystick6/Button4" },
			{ KeyCode.Joystick6Button5, "Joystick/Joystick6/Button5" },
			{ KeyCode.Joystick6Button6, "Joystick/Joystick6/Button6" },
			{ KeyCode.Joystick6Button7, "Joystick/Joystick6/Button7" },
			{ KeyCode.Joystick6Button8, "Joystick/Joystick6/Button8" },
			{ KeyCode.Joystick6Button9, "Joystick/Joystick6/Button9" },
			{ KeyCode.Joystick6Button10, "Joystick/Joystick6/Button10" },
			{ KeyCode.Joystick6Button11, "Joystick/Joystick6/Button11" },
			{ KeyCode.Joystick6Button12, "Joystick/Joystick6/Button12" },
			{ KeyCode.Joystick6Button13, "Joystick/Joystick6/Button13" },
			{ KeyCode.Joystick6Button14, "Joystick/Joystick6/Button14" },
			{ KeyCode.Joystick6Button15, "Joystick/Joystick6/Button15" },
			{ KeyCode.Joystick6Button16, "Joystick/Joystick6/Button16" },
			{ KeyCode.Joystick6Button17, "Joystick/Joystick6/Button17" },
			{ KeyCode.Joystick6Button18, "Joystick/Joystick6/Button18" },
			{ KeyCode.Joystick6Button19, "Joystick/Joystick6/Button19" },
			{ KeyCode.Joystick7Button0, "Joystick/Joystick7/Button0" },
			{ KeyCode.Joystick7Button1, "Joystick/Joystick7/Button1" },
			{ KeyCode.Joystick7Button2, "Joystick/Joystick7/Button2" },
			{ KeyCode.Joystick7Button3, "Joystick/Joystick7/Button3" },
			{ KeyCode.Joystick7Button4, "Joystick/Joystick7/Button4" },
			{ KeyCode.Joystick7Button5, "Joystick/Joystick7/Button5" },
			{ KeyCode.Joystick7Button6, "Joystick/Joystick7/Button6" },
			{ KeyCode.Joystick7Button7, "Joystick/Joystick7/Button7" },
			{ KeyCode.Joystick7Button8, "Joystick/Joystick7/Button8" },
			{ KeyCode.Joystick7Button9, "Joystick/Joystick7/Button9" },
			{ KeyCode.Joystick7Button10, "Joystick/Joystick7/Button10" },
			{ KeyCode.Joystick7Button11, "Joystick/Joystick7/Button11" },
			{ KeyCode.Joystick7Button12, "Joystick/Joystick7/Button12" },
			{ KeyCode.Joystick7Button13, "Joystick/Joystick7/Button13" },
			{ KeyCode.Joystick7Button14, "Joystick/Joystick7/Button14" },
			{ KeyCode.Joystick7Button15, "Joystick/Joystick7/Button15" },
			{ KeyCode.Joystick7Button16, "Joystick/Joystick7/Button16" },
			{ KeyCode.Joystick7Button17, "Joystick/Joystick7/Button17" },
			{ KeyCode.Joystick7Button18, "Joystick/Joystick7/Button18" },
			{ KeyCode.Joystick7Button19, "Joystick/Joystick7/Button19" },
			{ KeyCode.Joystick8Button0, "Joystick/Joystick8/Button0" },
			{ KeyCode.Joystick8Button1, "Joystick/Joystick8/Button1" },
			{ KeyCode.Joystick8Button2, "Joystick/Joystick8/Button2" },
			{ KeyCode.Joystick8Button3, "Joystick/Joystick8/Button3" },
			{ KeyCode.Joystick8Button4, "Joystick/Joystick8/Button4" },
			{ KeyCode.Joystick8Button5, "Joystick/Joystick8/Button5" },
			{ KeyCode.Joystick8Button6, "Joystick/Joystick8/Button6" },
			{ KeyCode.Joystick8Button7, "Joystick/Joystick8/Button7" },
			{ KeyCode.Joystick8Button8, "Joystick/Joystick8/Button8" },
			{ KeyCode.Joystick8Button9, "Joystick/Joystick8/Button9" },
			{ KeyCode.Joystick8Button10, "Joystick/Joystick8/Button10" },
			{ KeyCode.Joystick8Button11, "Joystick/Joystick8/Button11" },
			{ KeyCode.Joystick8Button12, "Joystick/Joystick8/Button12" },
			{ KeyCode.Joystick8Button13, "Joystick/Joystick8/Button13" },
			{ KeyCode.Joystick8Button14, "Joystick/Joystick8/Button14" },
			{ KeyCode.Joystick8Button15, "Joystick/Joystick8/Button15" },
			{ KeyCode.Joystick8Button16, "Joystick/Joystick8/Button16" },
			{ KeyCode.Joystick8Button17, "Joystick/Joystick8/Button17" },
			{ KeyCode.Joystick8Button18, "Joystick/Joystick8/Button18" },
			{ KeyCode.Joystick8Button19, "Joystick/Joystick8/Button19" }
		};*/

		static Dictionary<string, KeyCode> pathToKeyCode = new Dictionary<string, KeyCode> () {
			{ "None", KeyCode.None },
			{ "Keyboard/Not Used/Backspace", KeyCode.Backspace },
			{ "Keyboard/Not Used/Delete",KeyCode.Delete },
			{ "Keyboard/Tab", KeyCode.Tab },
			{ "Keyboard/Not Used/Clear", KeyCode.Clear },
			{ "Keyboard/Not Used/Return", KeyCode.Return },
			{ "Keyboard/Not Used/Pause", KeyCode.Pause },
			{ "Keyboard/Escape", KeyCode.Escape },
			{ "Keyboard/Space", KeyCode.Space },
			{ "Keyboard/Not Used/Exclaim", KeyCode.Exclaim },
			{ "Keyboard/Not Used/DoubleQuote", KeyCode.DoubleQuote },
			{ "Keyboard/Not Used/Hash", KeyCode.Hash },
			{ "Keyboard/Not Used/Dollar", KeyCode.Dollar },
			{ "Keyboard/Not Used/Ampersand", KeyCode.Ampersand },
			{ "Keyboard/Not Used/Quote", KeyCode.Quote },
			{ "Keyboard/Left/Paren", KeyCode.LeftParen },
			{ "Keyboard/Right/Paren", KeyCode.RightParen },
			{ "Keyboard/Not Used/Asterisk", KeyCode.Asterisk },
			{ "Keyboard/Not Used/Plus", KeyCode.Plus },
			{ "Keyboard/Not Used/Comma", KeyCode.Comma },
			{ "Keyboard/Not Used/Minus", KeyCode.Minus },
			{ "Keyboard/Not Used/Period", KeyCode.Period },
			{ "Keyboard/Not Used/Slash", KeyCode.Slash },
			{ "Numbers/Alpha0", KeyCode.Alpha0 },
			{ "Numbers/Alpha1", KeyCode.Alpha1 },
			{ "Numbers/Alpha2", KeyCode.Alpha2 },
			{ "Numbers/Alpha3", KeyCode.Alpha3 },
			{ "Numbers/Alpha4", KeyCode.Alpha4 },
			{ "Numbers/Alpha5", KeyCode.Alpha5 },
			{ "Numbers/Alpha6", KeyCode.Alpha6 },
			{ "Numbers/Alpha7", KeyCode.Alpha7 },
			{ "Numbers/Alpha8", KeyCode.Alpha8 },
			{ "Numbers/Alpha9", KeyCode.Alpha9 },
			{ "Keyboard/Not Used/Colon", KeyCode.Colon },
			{ "Keyboard/Not Used/Semicolon", KeyCode.Semicolon },
			{ "Keyboard/Not Used/Less", KeyCode.Less },
			{ "Keyboard/Not Used/Equals", KeyCode.Equals },
			{ "Keyboard/Not Used/Greater", KeyCode.Greater },
			{ "Keyboard/Not Used/Question", KeyCode.Question },
			{ "Keyboard/Not Used/At", KeyCode.At },
			{ "Keyboard/Left/Bracket", KeyCode.LeftBracket },
			{ "Keyboard/Not Used/Backslash", KeyCode.Backslash },
			{ "Keyboard/Right/Bracket", KeyCode.RightBracket },
			{ "Keyboard/Not Used/Caret", KeyCode.Caret },
			{ "Keyboard/Not Used/Underscore", KeyCode.Underscore },
			{ "Keyboard/Not Used/BackQuote", KeyCode.BackQuote },
			{ "Alphabet/A", KeyCode.A },
			{ "Alphabet/B", KeyCode.B },
			{ "Alphabet/C", KeyCode.C },
			{ "Alphabet/D", KeyCode.D },
			{ "Alphabet/E", KeyCode.E },
			{ "Alphabet/F", KeyCode.F },
			{ "Alphabet/G", KeyCode.G },
			{ "Alphabet/H", KeyCode.H },
			{ "Alphabet/I", KeyCode.I },
			{ "Alphabet/J", KeyCode.J },
			{ "Alphabet/K", KeyCode.K },
			{ "Alphabet/L", KeyCode.L },
			{ "Alphabet/M", KeyCode.M },
			{ "Alphabet/N", KeyCode.N },
			{ "Alphabet/O", KeyCode.O },
			{ "Alphabet/P", KeyCode.P },
			{ "Alphabet/Q", KeyCode.Q },
			{ "Alphabet/R", KeyCode.R },
			{ "Alphabet/S", KeyCode.S },
			{ "Alphabet/T", KeyCode.T },
			{ "Alphabet/U", KeyCode.U },
			{ "Alphabet/V", KeyCode.V },
			{ "Alphabet/W", KeyCode.W },
			{ "Alphabet/X", KeyCode.X },
			{ "Alphabet/Y", KeyCode.Y },
			{ "Alphabet/Z", KeyCode.Z },
			{ "Keypad/Keypad0", KeyCode.Keypad0 },
			{ "Keypad/Keypad1", KeyCode.Keypad1 },
			{ "Keypad/Keypad2", KeyCode.Keypad2 },
			{ "Keypad/Keypad3", KeyCode.Keypad3 },
			{ "Keypad/Keypad4", KeyCode.Keypad4 },
			{ "Keypad/Keypad5", KeyCode.Keypad5 },
			{ "Keypad/Keypad6", KeyCode.Keypad6 },
			{ "Keypad/Keypad7", KeyCode.Keypad7 },
			{ "Keypad/Keypad8", KeyCode.Keypad8 },
			{ "Keypad/Keypad9", KeyCode.Keypad9 },
			{ "Keypad/KeypadPeriod", KeyCode.KeypadPeriod },
			{ "Keypad/KeypadDivide", KeyCode.KeypadDivide },
			{ "Keypad/KeypadMultiply", KeyCode.KeypadMultiply },
			{ "Keypad/KeypadMinus", KeyCode.KeypadMinus },
			{ "Keypad/KeypadPlus", KeyCode.KeypadPlus },
			{ "Keypad/KeypadEnter", KeyCode.KeypadEnter },
			{ "Keypad/KeypadEquals", KeyCode.KeypadEquals },
			{ "Arrows/UpArrow", KeyCode.UpArrow },
			{ "Arrows/DownArrow", KeyCode.DownArrow },
			{ "Arrows/RightArrow", KeyCode.RightArrow },
			{ "Arrows/LeftArrow", KeyCode.LeftArrow },
			{ "Keyboard/Not Used/Insert", KeyCode.Insert },
			{ "Keyboard/Not Used/Home", KeyCode.Home },
			{ "Keyboard/Not Used/End", KeyCode.End },
			{ "Keyboard/Not Used/PageUp", KeyCode.PageUp },
			{ "Keyboard/Not Used/PageDown", KeyCode.PageDown },
			{ "FButtons/F1", KeyCode.F1 },
			{ "FButtons/F2", KeyCode.F2 },
			{ "FButtons/F3", KeyCode.F3 },
			{ "FButtons/F4", KeyCode.F4 },
			{ "FButtons/F5", KeyCode.F5 },
			{ "FButtons/F6", KeyCode.F6 },
			{ "FButtons/F7", KeyCode.F7 },
			{ "FButtons/F8", KeyCode.F8 },
			{ "FButtons/F9", KeyCode.F9 },
			{ "FButtons/F10", KeyCode.F10 },
			{ "FButtons/F11", KeyCode.F11 },
			{ "FButtons/F12", KeyCode.F12 },
			{ "FButtons/F13", KeyCode.F13 },
			{ "FButtons/F14", KeyCode.F14 },
			{ "FButtons/F15", KeyCode.F15 },
			{ "Keypad/Numlock", KeyCode.Numlock },
			{ "Keyboard/Not Used/CapsLock", KeyCode.CapsLock },
			{ "Keyboard/Not Used/ScrollLock", KeyCode.ScrollLock },
			{ "Keyboard/Right/Shift", KeyCode.RightShift },
			{ "Keyboard/Left/Shift", KeyCode.LeftShift },
			{ "Keyboard/Right/Control", KeyCode.RightControl },
			{ "Keyboard/Left/Control", KeyCode.LeftControl },
			{ "Keyboard/Right/Alt", KeyCode.RightAlt },
			{ "Keyboard/Left/Alt", KeyCode.LeftAlt },
			{ "Keyboard/Right/Command", KeyCode.RightCommand },
			{ "Keyboard/Left/Command", KeyCode.LeftCommand },
			{ "Keyboard/Left/Windows", KeyCode.LeftWindows },
			{ "Keyboard/Right/Windows", KeyCode.RightWindows },
			{ "Keyboard/Not Used/AltGr", KeyCode.AltGr },
			{ "Keyboard/Not Used/Help", KeyCode.Help },
			{ "Keyboard/Not Used/Print", KeyCode.Print },
			{ "Keyboard/Not Used/SysReq", KeyCode.SysReq },
			{ "Keyboard/Not Used/Break", KeyCode.Break },
			{ "Keyboard/Not Used/Menu", KeyCode.Menu },
			{ "Mouse/Left (Primary 0)", KeyCode.Mouse0 },
			{ "Mouse/Right (Secondary 1)", KeyCode.Mouse1 },
			{ "Mouse/Middle (2)", KeyCode.Mouse2 },
			{ "Mouse/Mouse3", KeyCode.Mouse3 },
			{ "Mouse/Mouse4", KeyCode.Mouse4 },
			{ "Mouse/Mouse5", KeyCode.Mouse5 },
			{ "Mouse/Mouse6", KeyCode.Mouse6 },
			{ "Joystick/Joystick/Button0", KeyCode.JoystickButton0 },
			{ "Joystick/Joystick/Button1", KeyCode.JoystickButton1 },
			{ "Joystick/Joystick/Button2", KeyCode.JoystickButton2 },
			{ "Joystick/Joystick/Button3", KeyCode.JoystickButton3 },
			{ "Joystick/Joystick/Button4", KeyCode.JoystickButton4 },
			{ "Joystick/Joystick/Button5", KeyCode.JoystickButton5 },
			{ "Joystick/Joystick/Button6", KeyCode.JoystickButton6 },
			{ "Joystick/Joystick/Button7", KeyCode.JoystickButton7 },
			{ "Joystick/Joystick/Button8", KeyCode.JoystickButton8 },
			{ "Joystick/Joystick/Button9", KeyCode.JoystickButton9 },
			{ "Joystick/Joystick/Button10", KeyCode.JoystickButton10 },
			{ "Joystick/Joystick/Button11", KeyCode.JoystickButton11 },
			{ "Joystick/Joystick/Button12", KeyCode.JoystickButton12 },
			{ "Joystick/Joystick/Button13", KeyCode.JoystickButton13 },
			{ "Joystick/Joystick/Button14", KeyCode.JoystickButton14 },
			{ "Joystick/Joystick/Button15", KeyCode.JoystickButton15 },
			{ "Joystick/Joystick/Button16", KeyCode.JoystickButton16 },
			{ "Joystick/Joystick/Button17", KeyCode.JoystickButton17 },
			{ "Joystick/Joystick/Button18", KeyCode.JoystickButton18 },
			{ "Joystick/Joystick/Button19", KeyCode.JoystickButton19 },
			{ "Joystick/Joystick1/Button0", KeyCode.Joystick1Button0 },
			{ "Joystick/Joystick1/Button1", KeyCode.Joystick1Button1 },
			{ "Joystick/Joystick1/Button2", KeyCode.Joystick1Button2 },
			{ "Joystick/Joystick1/Button3", KeyCode.Joystick1Button3 },
			{ "Joystick/Joystick1/Button4", KeyCode.Joystick1Button4 },
			{ "Joystick/Joystick1/Button5", KeyCode.Joystick1Button5 },
			{ "Joystick/Joystick1/Button6", KeyCode.Joystick1Button6 },
			{ "Joystick/Joystick1/Button7", KeyCode.Joystick1Button7 },
			{ "Joystick/Joystick1/Button8", KeyCode.Joystick1Button8 },
			{ "Joystick/Joystick1/Button9", KeyCode.Joystick1Button9 },
			{ "Joystick/Joystick1/Button10", KeyCode.Joystick1Button10 },
			{ "Joystick/Joystick1/Button11", KeyCode.Joystick1Button11 },
			{ "Joystick/Joystick1/Button12", KeyCode.Joystick1Button12 },
			{ "Joystick/Joystick1/Button13", KeyCode.Joystick1Button13 },
			{ "Joystick/Joystick1/Button14", KeyCode.Joystick1Button14 },
			{ "Joystick/Joystick1/Button15", KeyCode.Joystick1Button15 },
			{ "Joystick/Joystick1/Button16", KeyCode.Joystick1Button16 },
			{ "Joystick/Joystick1/Button17", KeyCode.Joystick1Button17 },
			{ "Joystick/Joystick1/Button18", KeyCode.Joystick1Button18 },
			{ "Joystick/Joystick1/Button19", KeyCode.Joystick1Button19 },
			{ "Joystick/Joystick2/Button0", KeyCode.Joystick2Button0 },
			{ "Joystick/Joystick2/Button1", KeyCode.Joystick2Button1 },
			{ "Joystick/Joystick2/Button2", KeyCode.Joystick2Button2 },
			{ "Joystick/Joystick2/Button3", KeyCode.Joystick2Button3 },
			{ "Joystick/Joystick2/Button4", KeyCode.Joystick2Button4 },
			{ "Joystick/Joystick2/Button5", KeyCode.Joystick2Button5 },
			{ "Joystick/Joystick2/Button6", KeyCode.Joystick2Button6 },
			{ "Joystick/Joystick2/Button7", KeyCode.Joystick2Button7 },
			{ "Joystick/Joystick2/Button8", KeyCode.Joystick2Button8 },
			{ "Joystick/Joystick2/Button9", KeyCode.Joystick2Button9 },
			{ "Joystick/Joystick2/Button10", KeyCode.Joystick2Button10 },
			{ "Joystick/Joystick2/Button11", KeyCode.Joystick2Button11 },
			{ "Joystick/Joystick2/Button12", KeyCode.Joystick2Button12 },
			{ "Joystick/Joystick2/Button13", KeyCode.Joystick2Button13 },
			{ "Joystick/Joystick2/Button14", KeyCode.Joystick2Button14 },
			{ "Joystick/Joystick2/Button15", KeyCode.Joystick2Button15 },
			{ "Joystick/Joystick2/Button16", KeyCode.Joystick2Button16 },
			{ "Joystick/Joystick2/Button17", KeyCode.Joystick2Button17 },
			{ "Joystick/Joystick2/Button18", KeyCode.Joystick2Button18 },
			{ "Joystick/Joystick2/Button19", KeyCode.Joystick2Button19 },
			{ "Joystick/Joystick3/Button0", KeyCode.Joystick3Button0 },
			{ "Joystick/Joystick3/Button1", KeyCode.Joystick3Button1 },
			{ "Joystick/Joystick3/Button2", KeyCode.Joystick3Button2 },
			{ "Joystick/Joystick3/Button3", KeyCode.Joystick3Button3 },
			{ "Joystick/Joystick3/Button4", KeyCode.Joystick3Button4 },
			{ "Joystick/Joystick3/Button5", KeyCode.Joystick3Button5 },
			{ "Joystick/Joystick3/Button6", KeyCode.Joystick3Button6 },
			{ "Joystick/Joystick3/Button7", KeyCode.Joystick3Button7 },
			{ "Joystick/Joystick3/Button8", KeyCode.Joystick3Button8 },
			{ "Joystick/Joystick3/Button9", KeyCode.Joystick3Button9 },
			{ "Joystick/Joystick3/Button10", KeyCode.Joystick3Button10 },
			{ "Joystick/Joystick3/Button11", KeyCode.Joystick3Button11 },
			{ "Joystick/Joystick3/Button12", KeyCode.Joystick3Button12 },
			{ "Joystick/Joystick3/Button13", KeyCode.Joystick3Button13 },
			{ "Joystick/Joystick3/Button14", KeyCode.Joystick3Button14 },
			{ "Joystick/Joystick3/Button15", KeyCode.Joystick3Button15 },
			{ "Joystick/Joystick3/Button16", KeyCode.Joystick3Button16 },
			{ "Joystick/Joystick3/Button17", KeyCode.Joystick3Button17 },
			{ "Joystick/Joystick3/Button18", KeyCode.Joystick3Button18 },
			{ "Joystick/Joystick3/Button19", KeyCode.Joystick3Button19 },
			{ "Joystick/Joystick4/Button0", KeyCode.Joystick4Button0 },
			{ "Joystick/Joystick4/Button1", KeyCode.Joystick4Button1 },
			{ "Joystick/Joystick4/Button2", KeyCode.Joystick4Button2 },
			{ "Joystick/Joystick4/Button3", KeyCode.Joystick4Button3 },
			{ "Joystick/Joystick4/Button4", KeyCode.Joystick4Button4 },
			{ "Joystick/Joystick4/Button5", KeyCode.Joystick4Button5 },
			{ "Joystick/Joystick4/Button6", KeyCode.Joystick4Button6 },
			{ "Joystick/Joystick4/Button7", KeyCode.Joystick4Button7 },
			{ "Joystick/Joystick4/Button8", KeyCode.Joystick4Button8 },
			{ "Joystick/Joystick4/Button9", KeyCode.Joystick4Button9 },
			{ "Joystick/Joystick4/Button10", KeyCode.Joystick4Button10 },
			{ "Joystick/Joystick4/Button11", KeyCode.Joystick4Button11 },
			{ "Joystick/Joystick4/Button12", KeyCode.Joystick4Button12 },
			{ "Joystick/Joystick4/Button13", KeyCode.Joystick4Button13 },
			{ "Joystick/Joystick4/Button14", KeyCode.Joystick4Button14 },
			{ "Joystick/Joystick4/Button15", KeyCode.Joystick4Button15 },
			{ "Joystick/Joystick4/Button16", KeyCode.Joystick4Button16 },
			{ "Joystick/Joystick4/Button17", KeyCode.Joystick4Button17 },
			{ "Joystick/Joystick4/Button18", KeyCode.Joystick4Button18 },
			{ "Joystick/Joystick4/Button19", KeyCode.Joystick4Button19 },
			{ "Joystick/Joystick5/Button0", KeyCode.Joystick5Button0 },
			{ "Joystick/Joystick5/Button1", KeyCode.Joystick5Button1 },
			{ "Joystick/Joystick5/Button2", KeyCode.Joystick5Button2 },
			{ "Joystick/Joystick5/Button3", KeyCode.Joystick5Button3 },
			{ "Joystick/Joystick5/Button4", KeyCode.Joystick5Button4 },
			{ "Joystick/Joystick5/Button5", KeyCode.Joystick5Button5 },
			{ "Joystick/Joystick5/Button6", KeyCode.Joystick5Button6 },
			{ "Joystick/Joystick5/Button7", KeyCode.Joystick5Button7 },
			{ "Joystick/Joystick5/Button8", KeyCode.Joystick5Button8 },
			{ "Joystick/Joystick5/Button9", KeyCode.Joystick5Button9 },
			{ "Joystick/Joystick5/Button10", KeyCode.Joystick5Button10 },
			{ "Joystick/Joystick5/Button11", KeyCode.Joystick5Button11 },
			{ "Joystick/Joystick5/Button12", KeyCode.Joystick5Button12 },
			{ "Joystick/Joystick5/Button13", KeyCode.Joystick5Button13 },
			{ "Joystick/Joystick5/Button14", KeyCode.Joystick5Button14 },
			{ "Joystick/Joystick5/Button15", KeyCode.Joystick5Button15 },
			{ "Joystick/Joystick5/Button16", KeyCode.Joystick5Button16 },
			{ "Joystick/Joystick5/Button17", KeyCode.Joystick5Button17 },
			{ "Joystick/Joystick5/Button18", KeyCode.Joystick5Button18 },
			{ "Joystick/Joystick5/Button19", KeyCode.Joystick5Button19 },
			{ "Joystick/Joystick6/Button0", KeyCode.Joystick6Button0 },
			{ "Joystick/Joystick6/Button1", KeyCode.Joystick6Button1 },
			{ "Joystick/Joystick6/Button2", KeyCode.Joystick6Button2 },
			{ "Joystick/Joystick6/Button3", KeyCode.Joystick6Button3 },
			{ "Joystick/Joystick6/Button4", KeyCode.Joystick6Button4 },
			{ "Joystick/Joystick6/Button5", KeyCode.Joystick6Button5 },
			{ "Joystick/Joystick6/Button6", KeyCode.Joystick6Button6 },
			{ "Joystick/Joystick6/Button7", KeyCode.Joystick6Button7 },
			{ "Joystick/Joystick6/Button8", KeyCode.Joystick6Button8 },
			{ "Joystick/Joystick6/Button9", KeyCode.Joystick6Button9 },
			{ "Joystick/Joystick6/Button10", KeyCode.Joystick6Button10 },
			{ "Joystick/Joystick6/Button11", KeyCode.Joystick6Button11 },
			{ "Joystick/Joystick6/Button12", KeyCode.Joystick6Button12 },
			{ "Joystick/Joystick6/Button13", KeyCode.Joystick6Button13 },
			{ "Joystick/Joystick6/Button14", KeyCode.Joystick6Button14 },
			{ "Joystick/Joystick6/Button15", KeyCode.Joystick6Button15 },
			{ "Joystick/Joystick6/Button16", KeyCode.Joystick6Button16 },
			{ "Joystick/Joystick6/Button17", KeyCode.Joystick6Button17 },
			{ "Joystick/Joystick6/Button18", KeyCode.Joystick6Button18 },
			{ "Joystick/Joystick6/Button19", KeyCode.Joystick6Button19 },
			{ "Joystick/Joystick7/Button0", KeyCode.Joystick7Button0 },
			{ "Joystick/Joystick7/Button1", KeyCode.Joystick7Button1 },
			{ "Joystick/Joystick7/Button2", KeyCode.Joystick7Button2 },
			{ "Joystick/Joystick7/Button3", KeyCode.Joystick7Button3 },
			{ "Joystick/Joystick7/Button4", KeyCode.Joystick7Button4 },
			{ "Joystick/Joystick7/Button5", KeyCode.Joystick7Button5 },
			{ "Joystick/Joystick7/Button6", KeyCode.Joystick7Button6 },
			{ "Joystick/Joystick7/Button7", KeyCode.Joystick7Button7 },
			{ "Joystick/Joystick7/Button8", KeyCode.Joystick7Button8 },
			{ "Joystick/Joystick7/Button9", KeyCode.Joystick7Button9 },
			{ "Joystick/Joystick7/Button10", KeyCode.Joystick7Button10 },
			{ "Joystick/Joystick7/Button11", KeyCode.Joystick7Button11 },
			{ "Joystick/Joystick7/Button12", KeyCode.Joystick7Button12 },
			{ "Joystick/Joystick7/Button13", KeyCode.Joystick7Button13 },
			{ "Joystick/Joystick7/Button14", KeyCode.Joystick7Button14 },
			{ "Joystick/Joystick7/Button15", KeyCode.Joystick7Button15 },
			{ "Joystick/Joystick7/Button16", KeyCode.Joystick7Button16 },
			{ "Joystick/Joystick7/Button17", KeyCode.Joystick7Button17 },
			{ "Joystick/Joystick7/Button18", KeyCode.Joystick7Button18 },
			{ "Joystick/Joystick7/Button19", KeyCode.Joystick7Button19 },
			{ "Joystick/Joystick8/Button0", KeyCode.Joystick8Button0 },
			{ "Joystick/Joystick8/Button1", KeyCode.Joystick8Button1 },
			{ "Joystick/Joystick8/Button2", KeyCode.Joystick8Button2 },
			{ "Joystick/Joystick8/Button3", KeyCode.Joystick8Button3 },
			{ "Joystick/Joystick8/Button4", KeyCode.Joystick8Button4 },
			{ "Joystick/Joystick8/Button5", KeyCode.Joystick8Button5 },
			{ "Joystick/Joystick8/Button6", KeyCode.Joystick8Button6 },
			{ "Joystick/Joystick8/Button7", KeyCode.Joystick8Button7 },
			{ "Joystick/Joystick8/Button8", KeyCode.Joystick8Button8 },
			{ "Joystick/Joystick8/Button9", KeyCode.Joystick8Button9 },
			{ "Joystick/Joystick8/Button10", KeyCode.Joystick8Button10 },
			{ "Joystick/Joystick8/Button11", KeyCode.Joystick8Button11 },
			{ "Joystick/Joystick8/Button12", KeyCode.Joystick8Button12 },
			{ "Joystick/Joystick8/Button13", KeyCode.Joystick8Button13 },
			{ "Joystick/Joystick8/Button14", KeyCode.Joystick8Button14 },
			{ "Joystick/Joystick8/Button15", KeyCode.Joystick8Button15 },
			{ "Joystick/Joystick8/Button16", KeyCode.Joystick8Button16 },
			{ "Joystick/Joystick8/Button17", KeyCode.Joystick8Button17 },
			{ "Joystick/Joystick8/Button18", KeyCode.Joystick8Button18 },
			{ "Joystick/Joystick8/Button19", KeyCode.Joystick8Button19 }
		};

		#endregion

		static string[] searchList;

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight (property);
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			if (searchList == null) {
				searchList = new string[pathToKeyCode.Count];
				var index = 0;
				foreach (var key in pathToKeyCode.Keys) {
					searchList [index] = key;
					index++;
				}
			}
			var i = 0;
			for (int u = 0; u < searchList.Length; u++) {
				var tKeyCode = pathToKeyCode [searchList [u]];
				if ((int)tKeyCode == property.intValue) {
					i = u;
				}
			}
			var newIndex = EditorGUI.Popup (position, label.text, i, searchList);
			var keyCode = pathToKeyCode [searchList [newIndex]];

			var output = (int)(KeyCode)EditorGUI.EnumPopup (new Rect (0, 0, 0, 0), keyCode);
			property.intValue = output;
		}

		private void Print ()
		{
			var result = "";
			foreach (var key in pathToKeyCode) {
				result += "{" + string.Format ("KeyCode.{0}, \"{1}\"", key.Value, key.Key) + "},\n";
			}
			Debug.Log (result);
		}
	}
}