
namespace BevTree
{
	[AddNodeMenu("Composite/OR")]
	public class OR : Composite
	{

		public OR()
		{

		}
		

		protected override RunningStatus OnTick(Context context)
		{
			for (int i = 0; i < m_children.Count; ++i)
			{
				RunningStatus ret = m_children[i]._tick(context);
				if (ret == RunningStatus.Success)
					return RunningStatus.Success;
			}

			return RunningStatus.Failure;
		}


	}

}

