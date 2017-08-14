using System;

namespace BevTree
{
	public class ObsoleteNodeAttribute : Attribute
	{
		public readonly string tip;

		public ObsoleteNodeAttribute(string tip)
		{
			this.tip = "This node is obsolete. " + tip;
		}
	}


}
