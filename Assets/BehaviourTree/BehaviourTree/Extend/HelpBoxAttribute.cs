using System;

namespace BevTree
{
	public class NodeHelpBoxAttribute : Attribute
	{
		public readonly string Reference;

		public NodeHelpBoxAttribute(string reference)
		{
			Reference = reference;
		}
	}

}
