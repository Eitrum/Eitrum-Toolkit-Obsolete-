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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RandomElement<T> (this T[] array, EiRandom random)
		{
			return random._Element (array);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RandomElement<T> (this IList<T> list)
		{
			return EiRandom.Element (list);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RandomElement<T> (this IList<T> list, EiRandom random)
		{
			return random._Element (list);
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

		#region Suffle

		public static void Suffle<T> (this T[] array)
		{
			int amount = array.Length * 3;
			int count = array.Length;
			T tempValue;
			for (int i = 0; i < amount; i++) {
				var index1 = EiRandom.Range (count);
				var index2 = EiRandom.Range (count);
				tempValue = array [index1];
				array [index1] = array [index2];
				array [index2] = tempValue;
			}
		}

		public static void Suffle<T> (this T[] array, int amount)
		{
			int count = array.Length;
			T tempValue;
			for (int i = 0; i < amount; i++) {
				var index1 = EiRandom.Range (count);
				var index2 = EiRandom.Range (count);
				tempValue = array [index1];
				array [index1] = array [index2];
				array [index2] = tempValue;
			}
		}

		public static void Suffle<T> (this T[] array, EiRandom random)
		{
			int amount = array.Length * 3;
			int count = array.Length;
			T tempValue;
			for (int i = 0; i < amount; i++) {
				var index1 = random._Range (count);
				var index2 = random._Range (count);
				tempValue = array [index1];
				array [index1] = array [index2];
				array [index2] = tempValue;
			}
		}

		public static void Suffle<T> (this T[] array, EiRandom random, int amount)
		{
			int count = array.Length;
			T tempValue;
			for (int i = 0; i < amount; i++) {
				var index1 = random._Range (count);
				var index2 = random._Range (count);
				tempValue = array [index1];
				array [index1] = array [index2];
				array [index2] = tempValue;
			}
		}

		#endregion

		#region List Dequeue/Enqueue

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static T Dequeue<T> (this IList<T> list)
		{
			if (list.Count == 0)
				return default(T);
			var item = list [0];
			list.RemoveAt (0);
			return item;
		}

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static void Enqueue<T> (this IList<T> list, T item)
		{
			list.Add (item);
		}

		#endregion
	}
}