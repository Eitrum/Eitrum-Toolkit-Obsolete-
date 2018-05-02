using System;

namespace Eitrum
{
	public static class ArrayExtensions
	{
		public static T RandomElement<T> (this T[] array)
		{
			return EiRandom.Element (array);
		}

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
			if (index > 0 && index < array.Length) {
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
	}
}

