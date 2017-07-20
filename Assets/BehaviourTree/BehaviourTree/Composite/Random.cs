using UnityEngine;

namespace BevTree
{
	[AddNodeMenu("Composite/Random")]
	public class Random : Composite
	{

		protected override void OnOpen(Context context)
		{
			int randomIndex = RandomGen.RandInt(0, m_children.Count - 1);
			context.blackboard.SetInt(context.tree.guid, this.guid, "randomIndex", randomIndex);
		}


		protected override RunningStatus OnTick(Context context)
		{
			int randomIndex = context.blackboard.GetInt(context.tree.guid, this.guid, "randomIndex");
			if (randomIndex >= 0 && randomIndex < m_children.Count)
				return m_children[randomIndex]._tick(context);
			else
				return RunningStatus.Failure;
		}
	}

}