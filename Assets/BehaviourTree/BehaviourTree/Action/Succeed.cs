
namespace BevTree
{
	[AddNodeMenu("Action/Util/Succeed")]
	[NodeHelpBox("Always return Success.")]
	public class Succeed : Action
	{
		protected override RunningStatus OnTick(Context context)
		{
			return RunningStatus.Success;
		}
	}

}

