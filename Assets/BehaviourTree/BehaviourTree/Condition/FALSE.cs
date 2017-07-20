
namespace BevTree
{
	[AddConstraintMenu("False")]
	[ConstraintHelpBox("Always False.")]
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

