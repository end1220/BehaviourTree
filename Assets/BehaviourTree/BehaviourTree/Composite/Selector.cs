
namespace BevTree
{
	[AddNodeMenu("Composite/Selector")]
	public class Selector : Composite
	{

		public Selector()
		{

		}


		public Selector(params BehaviourNode[] nodes) :
			base(nodes)
		{

		}

		protected override RunningStatus OnTick(Context context)
		{
			RunningStatus status = RunningStatus.Success;
			for (int i = 0; i < m_children.Count; ++i)
			{
				status = m_children[i]._tick(context);
				if (status != RunningStatus.Failure)
					return status;
			}

			return status;
		}
	}

}

