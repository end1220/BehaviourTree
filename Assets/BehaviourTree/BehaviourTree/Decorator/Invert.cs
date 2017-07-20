
namespace BevTree
{
	[AddNodeMenu("Decorator/Invert")]
	public class Invert : Decorator
	{

		public Invert()
		{

		}


		public Invert(BehaviourNode node) :
			base(node)
		{

		}


		protected override RunningStatus OnTick(Context context)
		{
			RunningStatus ret = m_child._tick(context);
			if (ret == RunningStatus.Success)
				return RunningStatus.Failure;
			else if (ret == RunningStatus.Failure)
				return RunningStatus.Success;

			return RunningStatus.Running;
		}

	}

}

