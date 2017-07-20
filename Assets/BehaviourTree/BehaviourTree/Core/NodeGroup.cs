
namespace BevTree
{
	//[AddNodeMenu("Utility/Node Group")]
	public class NodeGroup : Decorator
	{
		protected override RunningStatus OnTick(Context context)
		{
			if (m_child != null)
				return m_child._tick(context);
			return RunningStatus.Success;
		}

	}

}

