

using BevTree.Serialization;

namespace BevTree
{
	[AddNodeMenu("Action/Util/RunBehaviourIndex")]
	public class RunBehaviourIndex : Action
	{
		public int SubTreeIndex = -1;

		private BehaviourTree m_behaviourTree;

		[BTIgnore]
		public BehaviourTree BehaviourTree
		{
			get
			{
				return m_behaviourTree;
			}
		}


		public override void OnBeforeSerialize(BTAsset btAsset)
		{
			base.OnBeforeSerialize(btAsset);
		}

		public override void OnAfterDeserialize(BTAsset btAsset)
		{
			base.OnAfterDeserialize(btAsset);
		}

		protected override void OnInit(BTAsset asset)
		{
			if(SubTreeIndex >= 0)
			{
				if (asset is RootTreeAsset)
				{
					RootTreeAsset rootAsset = asset as RootTreeAsset;
					m_behaviourTree = rootAsset.CreateRuntimeSubTree(SubTreeIndex);
					if (m_behaviourTree != null)
					{
						m_behaviourTree.Root._init(asset);
					}
				}
			}
		}

		protected override void OnOpen(Context context)
		{
			if (m_behaviourTree != null)
			{
				m_behaviourTree.Root._open(context);
			}
		}

		protected override void OnClose(Context context)
		{
			if(m_behaviourTree != null)
			{
				m_behaviourTree.Root._close(context);
			}
		}

		protected override void OnEnter(Context context)
		{
			if (m_behaviourTree != null)
			{
				m_behaviourTree.Root._enter(context);
			}
		}

		protected override void OnExit(Context context)
		{
			if (m_behaviourTree != null)
			{
				m_behaviourTree.Root._exit(context);
			}
		}

		protected override RunningStatus OnTick(Context context)
		{
			if(m_behaviourTree != null)
			{
				return m_behaviourTree.Root._tick(context);
			}
			else
			{
				return RunningStatus.Failure;
			}
		}
	}
}