
namespace BevTree
{
	[AddNodeMenu("Action/Util/Fail")]
	[NodeHelpBox("Always return Failure.")]
	public class Fail : Action
	{
		protected override RunningStatus OnTick(Context context)
		{
			return RunningStatus.Failure;
		}
	}

}

