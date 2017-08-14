
namespace BevTree
{
	[AddConstraintMenu("True")]
	[NodeHelpBox("Always True.")]
	public class TRUE : Constraint
	{

		public TRUE()
		{

		}


		protected override bool Evaluate(Context context)
		{
			return true;
		}

	}

}

