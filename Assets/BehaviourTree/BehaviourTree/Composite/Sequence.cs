
namespace BevTree
{
	[AddNodeMenu("Composite/Sequence")]
	public class Sequence : Composite
	{

		public Sequence()
		{

		}


		public Sequence(params BehaviourNode[] nodes) :
			base(nodes)
		{

		}


		protected override void OnOpen(Context context)
		{
			context.blackboard.SetInt(context.tree.guid, this.guid, "childIndex", 0);
		}


		protected override RunningStatus OnTick(Context context)
		{
			RunningStatus ret = RunningStatus.Success;

			int lastIndex = context.blackboard.GetInt(context.tree.guid, this.guid, "childIndex");
			int currentChildIndex = lastIndex;

			for (int i = currentChildIndex; i < m_children.Count; ++i)
			{
				ret = m_children[i]._tick(context);
				if (ret == RunningStatus.Success)
					currentChildIndex++;
				else
					break;
			}

			if (currentChildIndex != lastIndex)
				context.blackboard.SetInt(context.tree.guid, this.guid, "childIndex", currentChildIndex);

			return ret;
		}


	}

}

