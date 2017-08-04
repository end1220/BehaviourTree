
namespace BevTree
{
	[AddNodeMenu("Decorator/Delay")]
	public class Delay : Decorator
	{
		public int millseconds = 0;


		public Delay()
		{

		}


		public Delay(BehaviourNode node) :
			base(node)
		{

		}


		protected override void OnOpen(Context context)
		{
			long beginTime = System.DateTime.Now.Ticks / 10000;
			context.blackboard.SetLong(context.tree.guid, this.guid, "beginTime", beginTime);
		}


		protected override RunningStatus OnTick(Context context)
		{
			long beginTime = context.blackboard.GetLong(context.tree.guid, this.guid, "beginTime");
			if (System.DateTime.Now.Ticks / 10000 > beginTime + millseconds)
			{
				return m_child._tick(context);
			}

			return RunningStatus.Running;
		}

	}

}

