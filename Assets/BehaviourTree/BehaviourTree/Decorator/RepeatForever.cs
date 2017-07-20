
namespace BevTree
{
	[AddNodeMenu("Decorator/RepeatForever")]
	public class RepeatForever : Decorator
	{

		public RepeatForever()
		{

		}


		public RepeatForever(BehaviourNode node)
			: base(node)
		{
			
		}


		protected override void OnOpen(Context context)
		{
			
		}


		protected override RunningStatus OnTick(Context context)
		{
			m_child._tick(context);

			return RunningStatus.Running;
		}


	}

}

