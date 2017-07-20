
namespace BevTree
{
	[AddNodeMenu("Action/Util/Wait")]
	public class Wait : Action
	{
		private uint m_millseconds = 0;


		public Wait()
		{

		}


		public Wait(float seconds)
			: base()
		{
			if (seconds < 0)
				seconds = 0;
			m_millseconds = (uint)(1000 * seconds);
		}


		protected override void OnOpen(Context context)
		{
			long beginTime = System.DateTime.Now.Ticks / 10000;
			context.blackboard.SetLong(context.tree.guid, this.guid, "beginTime", beginTime);
		}


		protected override RunningStatus OnTick(Context context)
		{
			long beginTime = context.blackboard.GetLong(context.tree.guid, this.guid, "beginTime");
			if (System.DateTime.Now.Ticks / 10000 > beginTime + m_millseconds)
			{
				return RunningStatus.Success;
			}

			return RunningStatus.Running;
		}


	}

}

