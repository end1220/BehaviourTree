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


	public class ConstraintHelpBoxAttribute : Attribute
	{
		public readonly string Reference;

		public ConstraintHelpBoxAttribute(string reference)
		{
			Reference = reference;
		}
	}


}
