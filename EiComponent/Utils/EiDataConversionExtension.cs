using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum
{
	public static class EiDataConversionExtension
	{
		#region Primitive Data Type Size

		public static Dictionary<Type, int> PrimitiveTypeToSize = new Dictionary<Type, int> () {
			{ typeof(byte), 1 },
			{ typeof(char), 1 },
			{ typeof(short), 2 },
			{ typeof(int), 4 },
			{ typeof(float), 4 },
			{ typeof(double), 8 },
			{ typeof(long), 8 },
			{ typeof(bool), 1 },
			{ typeof(sbyte), 1 },
			{ typeof(uint), 4 },
			{ typeof(ulong), 8 },
		};

		public static int GetSizeOfType (Type type)
		{
			if (PrimitiveTypeToSize.ContainsKey (type))
				return PrimitiveTypeToSize [type];
			return 0;
		}

		#endregion

		#region Basic Data Types

		#region ToByte

		public static byte[] ToByte (this int data)
		{
			return BitConverter.GetBytes (data);
		}

		public static byte[] ToByte (this uint data)
		{
			return BitConverter.GetBytes (data);
		}

		public static byte[] ToByte (this double data)
		{
			return BitConverter.GetBytes (data);
		}

		public static byte[] ToByte (this float data)
		{
			return BitConverter.GetBytes (data);
		}

		public static byte[] ToByte (this short data)
		{
			return BitConverter.GetBytes (data);
		}

		public static byte[] ToByte (this ushort data)
		{
			return BitConverter.GetBytes (data);
		}

		public static byte[] ToByte (this byte data)
		{
			return new byte[]{ data };
		}

		public static byte[] ToByte (this long data)
		{
			return BitConverter.GetBytes (data);
		}

		public static byte[] ToByte (this ulong data)
		{
			return BitConverter.GetBytes (data);
		}

		public static byte[] ToByte (this char data)
		{
			return BitConverter.GetBytes (data);
		}

		public static byte[] ToByte (this bool data)
		{
			return BitConverter.GetBytes (data);
		}

		public static byte[] ToByte (this string data)
		{
			return System.Text.Encoding.ASCII.GetBytes (data);
		}

		#endregion

		#region FromByte


		public static int ToInt (this byte[] data, int startIndex = 0)
		{
			return BitConverter.ToInt32 (data, startIndex);
		}

		public static uint ToUInt (this byte[] data, int startIndex = 0)
		{
			return BitConverter.ToUInt32 (data, startIndex);
		}

		public static double ToDouble (this byte[] data, int startIndex = 0)
		{
			return BitConverter.ToDouble (data, startIndex);
		}

		public static float ToFloat (this byte[] data, int startIndex = 0)
		{
			return BitConverter.ToSingle (data, startIndex);
		}

		public static short ToShort (this byte[]  data, int startIndex = 0)
		{
			return BitConverter.ToInt16 (data, startIndex);
		}

		public static ushort ToUShort (this byte[] data, int startIndex = 0)
		{
			return BitConverter.ToUInt16 (data, startIndex);
		}

		public static byte ToByte (this byte[] data, int startIndex = 0)
		{
			return data [startIndex];
		}

		public static sbyte ToSByte (this byte[] data, int startIndex = 0)
		{
			return (sbyte)data [startIndex];
		}

		public static long ToLong (this byte[] data, int startIndex = 0)
		{
			return BitConverter.ToInt64 (data, startIndex);
		}

		public static ulong ToULong (this byte[] data, int startIndex = 0)
		{
			return BitConverter.ToUInt64 (data, startIndex);
		}

		public static char ToChar (this byte[] data, int startIndex = 0)
		{
			return BitConverter.ToChar (data, startIndex);
		}

		public static bool ToBool (this byte[] data, int startIndex = 0)
		{
			return BitConverter.ToBoolean (data, startIndex);
		}

		public static string ToString (this byte[] data, int startIndex = 0, int length = 0)
		{
			return System.Text.Encoding.ASCII.GetString (data, startIndex, length);
		}

		public static T To<T> (this byte[] data, int startIndex = 0)
		{
			var t = typeof(T);
			object obj = null;
			if (t == typeof(int))
				obj = data.ToInt (startIndex);
			else if (t == typeof(float))
				obj = data.ToFloat (startIndex);
			else if (t == typeof(double))
				obj = data.ToDouble (startIndex);
			else if (t == typeof(long))
				obj = data.ToLong (startIndex);
			else if (t == typeof(uint))
				obj = data.ToUInt (startIndex);
			else if (t == typeof(ulong))
				obj = data.ToULong (startIndex);
			else if (t == typeof(ushort))
				obj = data.ToUShort (startIndex);
			else if (t == typeof(short))
				obj = data.ToShort (startIndex);
			else if (t == typeof(bool))
				obj = data.ToBool (startIndex);
			else if (t == typeof(byte))
				obj = data.ToByte (startIndex);
			else if (t == typeof(sbyte))
				obj = data.ToSByte (startIndex);
			
			return (T)obj;
		}

		#endregion

		#endregion

		#region UnityDataTypes

		public static byte[] ToByte (this Vector3 data)
		{
			List<byte> bytes = new List<byte> ();
			bytes.AddRange (BitConverter.GetBytes (data.x));
			bytes.AddRange (BitConverter.GetBytes (data.y));
			bytes.AddRange (BitConverter.GetBytes (data.z));
			return bytes.ToArray ();
		}

		public static Vector3 ToVector3 (this byte[] data, int startIndex)
		{
			Vector3 v = new Vector3 (
				            BitConverter.ToSingle (data, startIndex + 0),
				            BitConverter.ToSingle (data, startIndex + 4),
				            BitConverter.ToSingle (data, startIndex + 8)
			            );
			return v;
		}

		public static byte[] ToByte (this Color data)
		{
			List<byte> bytes = new List<byte> ();
			bytes.AddRange (BitConverter.GetBytes (data.r));
			bytes.AddRange (BitConverter.GetBytes (data.g));
			bytes.AddRange (BitConverter.GetBytes (data.b));
			bytes.AddRange (BitConverter.GetBytes (data.a));
			return bytes.ToArray ();
		}

		public static Color ToColor (this byte[] data, int startIndex = 0)
		{
			Color col = new Color (
				            BitConverter.ToSingle (data, startIndex + 0),
				            BitConverter.ToSingle (data, startIndex + 4),
				            BitConverter.ToSingle (data, startIndex + 8), 
				            BitConverter.ToSingle (data, startIndex + 12));

			return col;
		}

		public static byte[] ToByte (this Texture2D data)
		{
			List<byte> bytes = new List<byte> ();
			bytes.AddRange (BitConverter.GetBytes (data.width));
			bytes.AddRange (BitConverter.GetBytes (data.height));
			bytes.AddRange (BitConverter.GetBytes ((int)data.format));
			bytes.AddRange (BitConverter.GetBytes (data.mipmapCount));
			var tempData = data.GetRawTextureData ();
			bytes.AddRange (BitConverter.GetBytes (tempData.Length));
			bytes.AddRange (tempData);
			return bytes.ToArray ();
		}

		public static Texture2D ToTexture2D (this byte[] data, int startIndex = 0)
		{
			Texture2D t2d = new Texture2D (
				                BitConverter.ToInt32 (data, startIndex),
				                BitConverter.ToInt32 (data, startIndex + 4),
				                (TextureFormat)BitConverter.ToInt32 (data, startIndex + 8),
				                BitConverter.ToInt32 (data, startIndex + 12) > 1
			                );
			var length = BitConverter.ToInt32 (data, startIndex + 16);
			var rawData = new byte[length];
			Array.Copy (data, startIndex + 20, rawData, 0, length);
			t2d.LoadRawTextureData (rawData);
			t2d.Apply ();
			return t2d;
		}

		#endregion

		#region Special Operations

		public static byte[] AddLength (this byte[] data)
		{
			List<byte> bytes = new List<byte> (BitConverter.GetBytes (data.Length));
			bytes.AddRange (data);

			return bytes.ToArray ();
		}

		public static int GetLength (this byte[] data, int startIndex = 0)
		{
			return BitConverter.ToInt32 (data, startIndex);
		}

		public static void AddBytesToTargetArray (this byte[] data, ref byte[] targetArray)
		{

			var tempData = new byte[data.Length + targetArray.Length];
			Array.Copy (targetArray, 0, tempData, 0, targetArray.Length);
			Array.Copy (data, 0, tempData, targetArray.Length, data.Length);

			targetArray = tempData;
		}

		public static byte[] CombineArray (this byte[] data, params byte[][] otherArrays)
		{
			foreach (byte[] array in otherArrays) {
				array.AddBytesToTargetArray (ref data);
			}
			return data;
		}

		#endregion
	}
}