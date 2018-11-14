using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Eitrum.Mathematics;

namespace Eitrum
{
	public static class ArrayExtensions
	{
		#region Random

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static T RandomElement<T> (this T[] array)
		{
			return EiRandom.Element (array);
		}

		#endregion

		#region Add / Remove

		public static T[] Add<T> (this T[] array, T element)
		{
			if (array == null)
				return array;
			T[] newArray = new  T [array.Length + 1];
			for (int i = 0; i < array.Length; i++)
				newArray [i] = array [i];
			newArray [array.Length] = element;
			return newArray;
		}

		public static T[] Remove<T> (this T[] array, T element)
		{
			if (array == null || array.Length == 0)
				return array;
			int elementAtIndex = -1;
			for (int i = 0; i < array.Length; i++) {
				if (array [i].Equals (element)) {
					elementAtIndex = i;
					break;
				}
			}
			if (elementAtIndex != -1) {
				T[] newArray = new T [array.Length - 1];
				for (int i = 0; i < newArray.Length; i++) {
					if (i >= elementAtIndex) {
						newArray [i] = array [i + 1];
					} else
						newArray [i] = array [i];
				}
				return newArray;
			}
			return array;
		}

		public static T[] Remove<T> (this T[] array, int index)
		{
			if (array == null || array.Length == 0)
				return array;
			if (index >= 0 && index < array.Length) {
				T[] newArray = new T [array.Length - 1];
				for (int i = 0; i < newArray.Length; i++) {
					if (i >= index) {
						newArray [i] = array [i + 1];
					} else
						newArray [i] = array [i];
				}
				return newArray;
			}
			return array;
		}

		#endregion

		#region List Dequeue/Enqueue

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static T Dequeue<T> (this List<T> list)
		{
			if (list.Count == 0)
				return default(T);
			var item = list [0];
			list.RemoveAt (0);
			return item;
		}

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static void Enqueue<T> (this List<T> list, T item)
		{
			list.Add (item);
		}

		#endregion
	}
}