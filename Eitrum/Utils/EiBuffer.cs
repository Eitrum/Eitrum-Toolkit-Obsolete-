//Copyright 2017 Sebastian Broström
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
//to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
//and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Text;
using System.Reflection;
using UnityEngine;

namespace Eitrum
{
	[Serializable]
	public class EiBuffer : EiCore
	{
		#region Variables

		byte[] bytes;
		int index = 0;
		int writtenIndex = 0;
		int length = 256;
		bool dynamicBuffer = false;
		int versionID = 0;

		#endregion

		#region Properties

		public int VersionID {
			get {
				return versionID;
			}
		}

		public int WrittenLength {
			get {
				return writtenIndex;
			}
		}

		public bool IsDynamicSize {
			get {
				return dynamicBuffer;
			}
		}

		public int Index {
			get {
				return index;
			}
		}

		#endregion

		#region Constructors

		public EiBuffer ()
		{
			bytes = new byte[length];
		}

		public EiBuffer (bool isDynamic)
		{
			bytes = new byte[length];
			SetDynamic (isDynamic);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EiNet.EiNetBuffer"/> class.
		/// Creats a buffer with the set size.
		/// </summary>
		/// <param name="size">Size.</param>
		public EiBuffer (int size)
		{
			this.length = Math.Max (size, 8);
			bytes = new byte[this.length];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EiNet.EiNetBuffer"/> class.
		/// Creats a buffer with the set size.
		/// Sets dynamic to dynamic value
		/// </summary>
		/// <param name="size">Size.</param>
		public EiBuffer (int size, bool dynamic)
		{
			this.length = Math.Max (size, 8);
			bytes = new byte[this.length];
			SetDynamic (dynamic);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EiNet.EiNetBuffer"/> class.
		/// Creats a buffer with the byte array.
		/// Index will be set to 0.
		/// </summary>
		public EiBuffer (byte[] bytes)
		{
			this.length = bytes.Length;
			this.bytes = bytes;
			index = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EiNet.EiNetBuffer"/> class.
		/// Creats a buffer with the size, identifier and method identifier given.
		/// Index will start at 16
		/// </summary>
		/// <param name="size">Size.</param>
		/// <param name="id">Identifier/Time.</param>
		/// <param name="methodId">Method identifier.</param>
		public EiBuffer (int size, long id, int methodId)
		{
			this.length = Math.Max (size, 8);
			bytes = new byte[this.length];
			Write (this.length);
			Write (id);
			Write (methodId);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eitrum.EiBuffer"/> class.
		/// Path to file you want to read from
		/// </summary>
		/// <param name="path">Path.</param>
		public EiBuffer (string path)
		{
			bytes = System.IO.File.ReadAllBytes (path);
			length = bytes.Length;
		}

		#endregion

		#region Helpers // 13 methods

		/// <summary>
		/// Sets the buffer to dynamic mode.
		/// This will cost a lot each time it writes over the normal size as the re-alloc of the byte array.
		/// </summary>
		/// <returns>The dynamic.</returns>
		public EiBuffer SetDynamic (bool value = true)
		{
			dynamicBuffer = value;
			return this;
		}

		public virtual EiBuffer ClearBuffer ()
		{
			bytes = new byte[length];
			ResetPointer ();
			return this;
		}

		public virtual EiBuffer ResetPointer ()
		{
			index = 0;
			writtenIndex = 0;
			return this;
		}

		public virtual EiBuffer SetPointer (int index)
		{
			this.index = index;
			return this;
		}

		public virtual EiBuffer Skip (int length)
		{
			index += length;
			return this;
		}

		public virtual EiBuffer FillData (byte[] buffer, int startIndex, int length)
		{
			Array.Copy (buffer, startIndex, bytes, 0, length);
			index = 0;
			return this;
		}

		public virtual int GetLength ()
		{
			return length;
		}

		public virtual bool CanRead (int length)
		{
			return bytes.Length > index + length;
		}

		/// <summary>
		/// Shifts the bytes from index with the length of bytes to amount of steps from index.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="length">Length.</param>
		/// <param name="amount">Amount.</param>
		public virtual void ShiftBytes (int index, int length, int amount)
		{
			if (amount == 0)
				return;
			if (amount > 0) {
				for (int i = index + length - 1; i >= 0; i--) {
					bytes [i + amount] = bytes [i];
				}
			} else {
				for (int i = 0; i < length; i++) {
					bytes [i + amount] = bytes [i];
				}
			}
		}

		public virtual EiBuffer Insert (int index, int length)
		{
			ShiftBytes (index, length - index, length);
			SetPointer (index);
			return this;
		}

		public byte[] GetArray ()
		{
			return bytes;
		}

		public byte[] GetBytes ()
		{
			return bytes;
		}

		public byte[] GetWrittenBuffer ()
		{
			byte[] array = new byte[writtenIndex];
			Array.Copy (bytes, 0, array, 0, writtenIndex);
			return array;
		}

		#endregion

		#region File Management // 8 methods

		public static bool FileExist (string path)
		{
			return System.IO.File.Exists (path);
		}

		public bool _FileExist (string path)
		{
			return System.IO.File.Exists (path);
		}

		public EiBuffer ReadFromFile (string path)
		{
			Add (System.IO.File.ReadAllBytes (path));
			return this;
		}

		public EiBuffer WriteToFile (string path)
		{
			System.IO.File.WriteAllBytes (path, bytes);
			return this;
		}

		public EiBuffer WriteToFile (string path, int index, int length)
		{
			this.index = index;
			System.IO.File.WriteAllBytes (path, ReadByteArray (length));
			return this;
		}

		public EiBuffer WriteToFile (string path, bool writtenBuffer)
		{
			if (writtenBuffer)
				System.IO.File.WriteAllBytes (path, GetWrittenBuffer ());
			else
				return WriteToFile (path);
			return this;
		}

		/// <summary>
		/// Writes whole object to file. Very Untested and should only be used with care.
		/// </summary>
		/// <param name="path">Path to file.</param>
		/// <param name="obj">Object to pass through.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void WriteToFile<T> (string path, T obj)
		{
			EiBuffer buffer = new EiBuffer (true);
			buffer.WriteReflection<T> (obj);
			buffer.WriteToFile (path);
		}

		/// <summary>
		/// Reads from file to object. Very Untested and should only be used with care.
		/// </summary>
		/// <param name="path">Path to file.</param>
		/// <param name="obj">Object to read to.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T ReadFromFile<T> (string path, T obj)
		{
			EiBuffer buffer = new EiBuffer (System.IO.File.ReadAllBytes (path));
			return (T)buffer.ReadReflection (obj);
		}

		#endregion

		#region Read // 24 methods

		public EiBuffer ReadVersionId ()
		{
			versionID = ReadInt ();
			return this;
		}

		public T Read<T> ()
		{
			var size = EiDataConversionExtension.GetSizeOfType (typeof(T));
			T value = bytes.To<T> (index);
			index += size;
			return value;
		}

		public byte ReadByte ()
		{
			var value = bytes [index];
			index++;
			return value;
		}

		public byte[] ReadByteArray (int length)
		{
			var value = new byte[length];
			Array.Copy (bytes, index, value, 0, length);
			index += length;
			return value;
		}

		public bool ReadBoolean ()
		{
			var value = bytes [index] != 0;
			index++;
			return value;
		}

		public char ReadChar ()
		{
			var value = (char)bytes [index];
			index++;
			return value;
		}

		public short ReadShort ()
		{
			var value = BitConverter.ToInt16 (bytes, index);
			index += 2;
			return value;
		}

		public ushort ReadUShort ()
		{
			var value = BitConverter.ToUInt16 (bytes, index);
			index += 2;
			return value;
		}

		public int ReadInt ()
		{
			var value = BitConverter.ToInt32 (bytes, index);
			index += 4;
			return value;
		}

		public uint ReadUInt ()
		{
			var value = BitConverter.ToUInt32 (bytes, index);
			index += 4;
			return value;
		}

		public float ReadFloat ()
		{
			var value = BitConverter.ToSingle (bytes, index);
			index += 4;
			return value;
		}

		public double ReadDouble ()
		{
			var value = BitConverter.ToDouble (bytes, index);
			index += 8;
			return value;
		}

		public long ReadLong ()
		{
			var value = BitConverter.ToInt64 (bytes, index);
			index += 8;
			return value;
		}

		public ulong ReadULong ()
		{
			var value = BitConverter.ToUInt64 (bytes, index);
			index += 8;
			return value;
		}

		public string ReadASCII ()
		{
			var length = ReadInt ();
			var value = Encoding.ASCII.GetString (ReadByteArray (length));
			return value;
		}

		public string ReadASCIINoHeader (int length)
		{
			var value = Encoding.ASCII.GetString (ReadByteArray (length));
			return value;
		}

		public Type ReadType ()
		{
			return Type.GetType (ReadASCII ());
		}

		public DateTime ReadDateTime ()
		{
			return new DateTime (ReadLong ());
		}

		public Vector2 ReadVector2 ()
		{
			return new Vector3 (ReadFloat (), ReadFloat ());
		}

		public Vector3 ReadVector3 ()
		{
			return new Vector3 (ReadFloat (), ReadFloat (), ReadFloat ());
		}

		public Quaternion ReadQuaternion ()
		{
			return new Quaternion (ReadFloat (), ReadFloat (), ReadFloat (), ReadFloat ());
		}

		public Color ReadColor ()
		{
			return new Color (ReadFloat (), ReadFloat (), ReadFloat (), ReadFloat ());
		}

		public Color32 ReadColor32 ()
		{
			return new Color32 (ReadByte (), ReadByte (), ReadByte (), ReadByte ());
		}

		public Texture2D ReadTexture2D ()
		{
			string name = ReadASCII ();
			Texture2D texture = new Texture2D (ReadInt (), ReadInt (), (TextureFormat)ReadInt (), false);
			var colors = new Color32[texture.width * texture.height];

			for (int i = 0; i < colors.Length; i++) {
				colors [i].r = ReadByte ();
				colors [i].g = ReadByte ();
				colors [i].b = ReadByte ();
				colors [i].a = ReadByte ();
			}
			texture.SetPixels32 (colors);
			texture.Apply ();

			return texture;
		}

		#endregion

		#region Write	// 23 methods

		public EiBuffer WriteVersionId (int id)
		{
			versionID = id;
			Write (versionID);
			return this;
		}

		public EiBuffer Write (byte value)
		{
			Add (value);
			return this;
		}

		public EiBuffer Write (byte[] array)
		{
			Add (array);
			return this;
		}

		public EiBuffer Write (bool value)
		{
			Add ((byte)(value ? 1 : 0));
			return this;
		}

		public EiBuffer Write (char value)
		{
			Add ((byte)value);
			return this;
		}

		public EiBuffer Write (short value)
		{
			var converted = BitConverter.GetBytes (value);
			Add (converted);
			return this;
		}

		public EiBuffer Write (ushort value)
		{
			var converted = BitConverter.GetBytes (value);
			Add (converted);
			return this;
		}

		public EiBuffer Write (int value)
		{
			var converted = BitConverter.GetBytes (value);
			Add (converted);
			return this;
		}

		public EiBuffer Write (uint value)
		{
			var converted = BitConverter.GetBytes (value);
			Add (converted);
			return this;
		}

		public EiBuffer Write (float value)
		{
			var converted = BitConverter.GetBytes (value);
			Add (converted);
			return this;
		}

		public EiBuffer Write (double value)
		{
			var converted = BitConverter.GetBytes (value);
			Add (converted);
			return this;
		}

		public EiBuffer Write (long value)
		{
			var converted = BitConverter.GetBytes (value);
			Add (converted);
			return this;
		}

		public EiBuffer Write (ulong value)
		{
			var converted = BitConverter.GetBytes (value);
			Add (converted);
			return this;
		}

		public EiBuffer WriteASCII (string value)
		{
			var buffer = Encoding.ASCII.GetBytes (value);
			var length = buffer.Length;
			Write (length);
			Write (buffer);
			return this;
		}

		public EiBuffer WriteStringNoHeader (string value)
		{
			var buffer = Encoding.ASCII.GetBytes (value);
			Write (buffer);
			return this;
		}

		public EiBuffer Write (Type value)
		{
			return WriteASCII (value.FullName);
		}

		public EiBuffer Write (DateTime value)
		{
			return Write (value.Ticks);
		}

		public EiBuffer Write (Vector2 value)
		{
			Write (value.x);
			Write (value.y);
			return this;
		}

		public EiBuffer Write (Vector3 value)
		{
			Write (value.x);
			Write (value.y);
			Write (value.z);
			return this;
		}

		public EiBuffer Write (Quaternion value)
		{
			Write (value.x);
			Write (value.y);
			Write (value.z);
			Write (value.w);
			return this;
		}

		public EiBuffer Write (Color color)
		{
			Write (color.r);
			Write (color.g);
			Write (color.b);
			Write (color.a);
			return this;
		}

		public EiBuffer Write (Color32 color)
		{
			Write (color.r);
			Write (color.g);
			Write (color.b);
			Write (color.a);
			return this;
		}

		public EiBuffer Write (Texture2D texture)
		{
			WriteASCII (texture.name);
			Write (texture.width);
			Write (texture.height);
			Write ((int)texture.format);
			var colors = texture.GetPixels32 ();
			for (int i = 0; i < colors.Length; i++) {
				Write (colors [i].r);
				Write (colors [i].g);
				Write (colors [i].b);
				Write (colors [i].a);
			}

			return this;
		}

		#endregion

		#region Peak // 23 methods

		/// <summary>
		/// Peak the specified amount of bytes. Use extension tool To[DataType]() to get value type.
		/// </summary>
		/// <param name="amount">Amount of bytes to be peaked.</param>
		public byte[] Peak (int amount)
		{
			byte[] bytes = new byte[amount];
			Array.Copy (this.bytes, index, bytes, 0, amount);
			return bytes;
		}

		public byte PeakByte ()
		{
			return bytes [index];
		}

		public byte[] PeakByteArray (int length)
		{
			var value = new byte[length];
			Array.Copy (bytes, index, value, 0, length);
			return value;
		}

		public bool PeakBoolean ()
		{
			return bytes [index] != 0;
		}

		public char PeakChar ()
		{
			return (char)bytes [index];
		}

		public short PeakShort ()
		{
			return BitConverter.ToInt16 (bytes, index);
		}

		public ushort PeakUShort ()
		{
			return BitConverter.ToUInt16 (bytes, index);
		}

		public int PeakInt ()
		{
			return BitConverter.ToInt32 (bytes, index);
		}

		public uint PeakUInt ()
		{
			return BitConverter.ToUInt32 (bytes, index);
		}

		public float PeakFloat ()
		{
			return BitConverter.ToSingle (bytes, index);
		}

		public double PeakDouble ()
		{
			return BitConverter.ToDouble (bytes, index);
		}

		public long PeakLong ()
		{
			return BitConverter.ToInt64 (bytes, index);
		}

		public ulong PeakULong ()
		{
			return BitConverter.ToUInt64 (bytes, index);
		}

		public string PeakASCII ()
		{
			var length = PeakInt ();
			var value = Encoding.ASCII.GetString (Skip (4).PeakByteArray (length));
			index -= 4;
			return value;
		}

		public string PeakASCIINoHeader (int length)
		{
			return Encoding.ASCII.GetString (ReadByteArray (length));
		}

		public Type PeakType ()
		{
			return Type.GetType (PeakASCII ());
		}

		public DateTime PeakDateTime ()
		{
			return new DateTime (PeakLong ());
		}

		public Vector2 PeakVector2 ()
		{
			var i = index;
			var value = ReadVector2 ();
			this.index = i;
			return value;
		}

		public Vector3 PeakVector3 ()
		{
			var i = index;
			var value = ReadVector3 ();
			this.index = i;
			return value;
		}

		public Quaternion PeakQuaternion ()
		{
			var i = index;
			var value = ReadQuaternion ();
			this.index = i;
			return value;
		}

		public Color PeakColor ()
		{
			var i = index;
			var value = ReadColor ();
			this.index = i;
			return value;
		}

		public Color32 PeakColor32 ()
		{
			var i = index;
			var value = ReadColor32 ();
			this.index = i;
			return value;
		}

		public Texture2D PeakTexture2D ()
		{
			var i = index;
			var value = ReadTexture2D ();
			this.index = i;
			return value;
		}

		#endregion

		#region Add

		void Add (byte value)
		{
			if (index + 1 > length) {
				if (dynamicBuffer) {
					if (length == 0)
						length = 8;
					var newBytes = new byte[length * 2];
					Array.Copy (bytes, 0, newBytes, 0, bytes.Length);
					length = newBytes.Length;
					bytes = newBytes;
					Add (value);
				} else {
					LogWarning (() => "Tying to write outside of the buffer size\n" + Environment.StackTrace);
				}
			} else {
				bytes [index] = value;
				index += 1;
			}
			if (index > writtenIndex)
				writtenIndex = index;
		}

		void Add (byte[] bytes)
		{
			if (index + bytes.Length > length) {
				if (dynamicBuffer) {
					if (length == 0)
						length = bytes.Length;
					var newBytes = new byte[length * 2];
					Array.Copy (this.bytes, 0, newBytes, 0, this.bytes.Length);
					length = newBytes.Length;
					this.bytes = newBytes;
					Add (bytes);
				} else {
					LogWarning (() => "Tying to write outside of the buffer size\n" + Environment.StackTrace);
				}
			} else {
				Array.Copy (bytes, 0, this.bytes, index, bytes.Length);
				index += bytes.Length;
			}
			if (index > writtenIndex)
				writtenIndex = index;
		}

		#endregion

		#region Reflection Tests

		/// <summary>
		/// Writes the whole class using reflection.
		/// Extreamly untested
		/// </summary>
		/// <returns>The reflection.</returns>
		/// <param name="item">Item.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public EiBuffer WriteReflection<T> (T item)
		{
			var type = item.GetType ();
			var fields = type.GetFields (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < fields.Length; i++) {
				var obj = fields [i].GetValue (item);
				var typeCode = Convert.GetTypeCode (obj);

				if (typeCode == TypeCode.Object) {
					if (obj.GetType ().IsArray) {
						var array = (Array)obj;
						Write (array.Length);
						for (int i2 = 0; i2 < array.Length; i2++) {
							WriteReflection (array.GetValue (i2));
						}
					} else
						WriteReflection (obj);
				} else {
					switch (typeCode) {
					case TypeCode.Boolean:
						Write ((bool)obj);
						break;
					case TypeCode.Byte:
						Write ((byte)obj);
						break;
					case TypeCode.Char:
						Write ((char)obj);
						break;
					case TypeCode.DateTime:
						Write ((DateTime)obj);
						break;
					case TypeCode.Double:
						Write ((double)obj);
						break;

					case TypeCode.Int16:
						Write ((Int16)obj);
						break;
					case TypeCode.Int32:
						Write ((Int32)obj);
						break;
					case TypeCode.Int64:
						Write ((Int64)obj);
						break;

					case TypeCode.UInt16:
						Write ((UInt16)obj);
						break;
					case TypeCode.UInt32:
						Write ((UInt32)obj);
						break;
					case TypeCode.UInt64:
						Write ((UInt64)obj);
						break;

					case TypeCode.SByte:
						Write ((SByte)obj);
						break;
					case TypeCode.Single:
						Write ((Single)obj);
						break;
					case TypeCode.String:
						WriteASCII ((string)obj);
						break;
					}
				}
			}
			return this;
		}

		/// <summary>
		/// Reads the byte array and fills whole object using reflection.
		/// Extreamly untested
		/// </summary>
		/// <returns>The reflection.</returns>
		/// <param name="o">O.</param>
		public object ReadReflection (object item)
		{
			var type = item.GetType ();
			var fields = type.GetFields (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < fields.Length; i++) {
				var obj = fields [i].GetValue (item);
				var typeCode = Convert.GetTypeCode (obj);

				if (typeCode == TypeCode.Object) {
					if (obj.GetType ().IsArray) {
						var elementType = obj.GetType ().GetElementType ();
						var array = Array.CreateInstance (elementType, ReadInt ());
						for (int i2 = 0; i2 < array.Length; i2++) {
							array.SetValue (ReadReflection (Activator.CreateInstance (elementType)), i2);
						}
						fields [i].SetValue (item, array);
					} else
						fields [i].SetValue (item, ReadReflection (obj));
				} else {
					switch (typeCode) {
					case TypeCode.Boolean:
						obj = ReadBoolean ();
						break;
					case TypeCode.Byte:
						obj = ReadByte ();
						break;
					case TypeCode.Char:
						obj = ReadChar ();
						break;
					case TypeCode.DateTime:
						obj = ReadDateTime ();
						break;
					case TypeCode.Double:
						obj = ReadDouble ();
						break;

					case TypeCode.Int16:
						obj = ReadShort ();
						break;
					case TypeCode.Int32:
						obj = ReadInt ();
						break;
					case TypeCode.Int64:
						obj = ReadLong ();
						break;
					case TypeCode.UInt16:
						obj = ReadUShort ();
						break;
					case TypeCode.UInt32:
						obj = ReadUInt ();
						break;
					case TypeCode.UInt64:
						obj = ReadULong ();
						break;
					case TypeCode.SByte:
						obj = ReadByte ();
						break;
					case TypeCode.Single:
						obj = ReadFloat ();
						break;
					case TypeCode.String:
						obj = ReadASCII ();
						break;
					}
					fields [i].SetValue (item, obj);
				}
			}

			return item;
		}

		#endregion
	}
}