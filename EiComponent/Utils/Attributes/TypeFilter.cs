using System;
using UnityEngine;

namespace Eitrum
{
	public class TypeFilter : PropertyAttribute
	{
		public Type type;

		public TypeFilter (Type type)
		{
			this.type = type;
		}
	}

	public class TypeFilterScene : PropertyAttribute
	{

	}
}