
using BevTree.Serialization;


namespace BevTree
{

	public abstract class Decorator : BehaviourNode
	{
		[BTProperty("Child")]
		[BTHideInInspector]
		protected BehaviourNode m_child;


		public Decorator()
			: base()
		{

		}


		public Decorator(BehaviourNode node)
			: base()
		{
			m_child = node;
		}


		public void SetChild(BehaviourNode node)
		{
			m_child = node;
		}


		public BehaviourNode GetChild()
		{
			return m_child;
		}

		protected override void OnInit()
		{
			if (m_child != null)
			{
				m_child._init();
			}
		}

		public override void OnBeforeSerialize(BTAsset btAsset)
		{
			base.OnBeforeSerialize(btAsset);

			if (m_child != null)
			{
				m_child.OnBeforeSerialize(btAsset);
			}
		}

		public override void OnAfterDeserialize(BTAsset btAsset)
		{
			base.OnAfterDeserialize(btAsset);

			if (m_child != null)
			{
				m_child.OnAfterDeserialize(btAsset);
			}
		}


	}

}

