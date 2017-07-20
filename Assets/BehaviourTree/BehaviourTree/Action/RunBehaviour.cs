

using BevTree.Serialization;

namespace BevTree
{
	[AddNodeMenu("Action/Util/RunBehaviour")]
	public class RunBehaviour : Action
	{
		[BTProperty("BehaviourTreeID")]
		[BTHideInInspector]
		private string m_behaviourTreeID;

		private BTAsset m_behaviourTreeAsset;
		private BehaviourTree m_behaviourTree;

		[BTIgnore]
		public BTAsset BehaviourTreeAsset
		{
			get { return m_behaviourTreeAsset; }
			set { m_behaviourTreeAsset = value; }
		}

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

			if(string.IsNullOrEmpty(m_behaviourTreeID))
			{
				m_behaviourTreeID = BTUtils.GenerateUniqueStringID();
			}

			btAsset.SetSubtreeAsset(m_behaviourTreeID, m_behaviourTreeAsset);
		}

		public override void OnAfterDeserialize(BTAsset btAsset)
		{
			base.OnAfterDeserialize(btAsset);

			m_behaviourTreeAsset = btAsset.GetSubtreeAsset(m_behaviourTreeID);
		}

		protected override void OnInit()
		{
			if(m_behaviourTreeAsset != null)
			{
				m_behaviourTree = m_behaviourTreeAsset.CreateRuntimeTree();
				if(m_behaviourTree != null)
				{
					m_behaviourTree.Root._init();
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