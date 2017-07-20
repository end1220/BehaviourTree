
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
			for (int i = 0; i < m_children.Count; ++i)
			{
				RunningStatus ret = m_children[i]._tick(context);
				if (ret != RunningStatus.Failure)
					return ret;
			}

			return RunningStatus.Running;
		}
	}

}

