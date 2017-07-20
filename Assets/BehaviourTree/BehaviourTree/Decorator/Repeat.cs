
namespace BevTree
{
	[AddNodeMenu("Decorator/Repeat")]
	public class Repeat : Decorator
	{
		private uint m_targetCount = 0;


		public Repeat()
		{

		}


		public Repeat(uint count, BehaviourNode node)
			: base(node)
		{
			m_targetCount = count;
		}


		protected override void OnOpen(Context context)
		{
			context.blackboard.SetInt(context.tree.guid, this.guid, "count", 0);
		}


		protected override RunningStatus OnTick(Context context)
		{
			int m_count = context.blackboard.GetInt(context.tree.guid, this.guid, "count");
			if (m_targetCount >= 0 && m_count >= m_targetCount)
				return RunningStatus.Success;

			RunningStatus ret = m_child._tick(context);
			if (m_targetCount >= 0 && ret == RunningStatus.Success)
			{
				m_count++;
				context.blackboard.SetInt(context.tree.guid, this.guid, "count", m_count);
				if (m_count >= m_targetCount)
					return RunningStatus.Success;
			}

			return RunningStatus.Running;
		}


	}

}

