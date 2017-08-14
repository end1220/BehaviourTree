
namespace BevTree
{
	[AddConstraintMenu("False")]
	[NodeHelpBox("Always False.")]
	public class FALSE : Constraint
	{

		public FALSE()
		{

		}


		protected override bool Evaluate(Context context)
		{
			return false;
		}

	}

}

