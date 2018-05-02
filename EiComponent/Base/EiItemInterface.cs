using System;

namespace Eitrum
{
	public interface EiItemInterface
	{
		int Id { get; }

		string Name { get; }

		string Description{ get; }

		int Amount{ get; }

		float Durability { get; }
	}
}

