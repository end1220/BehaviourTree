
using System.Collections.Generic;
using BevTree.Serialization;


namespace BevTree
{

	public abstract class Composite : BehaviourNode
	{
		[BTProperty("Children")]
		[BTHideInInspector]
		protected List<BehaviourNode> m_children;


		[BTIgnore]
		public int ChildCount
		{
			get
			{
				return m_children.Count;
			}
		}


		public Composite()
		{
			m_children = new List<BehaviourNode>();
		}


		public Composite(params BehaviourNode[] nodes)
			: base()
		{
			m_children = new List<BehaviourNode>();
			AddChildren(nodes);
		}


		public override void OnBeforeSerialize(BTAsset btAsset)
		{
			base.OnBeforeSerialize(btAsset);

			for (int i = 0; i < m_children.Count; i++)
			{
				m_children[i].OnBeforeSerialize(btAsset);
			}
		}


		public override void OnAfterDeserialize(BTAsset btAsset)
		{
			base.OnAfterDeserialize(btAsset);

			for (int i = 0; i < m_children.Count; i++)
			{
				m_children[i].OnAfterDeserialize(btAsset);
			}
		}


		protected override void OnInit(BTAsset asset)
		{
			for (int i = 0; i < m_children.Count; i++)
			{
				m_children[i]._init(asset);
			}
		}


		public void AddChildren(params BehaviourNode[] nodes)
		{
			for (int i = 0; i < nodes.Length; ++i)
			{
				AddChild(nodes[i]);
			}
		}


		public void AddChild(BehaviourNode node)
		{
			if (!m_children.Contains(node))
			{
				node.parent = this;
				m_children.Add(node);
			}
		}


		public void InsertChild(int index, BehaviourNode child)
		{
			if (child != null)
			{
				m_children.Insert(index, child);
			}
		}


		public void RemoveChild(BehaviourNode child)
		{
			if (child != null)
			{
				m_children.Remove(child);
			}
		}


		public void RemoveChild(int index)
		{
			if (index >= 0 && index < m_children.Count)
			{
				m_children.RemoveAt(index);
			}
		}


		public void RemoveAllChildren()
		{
			m_children.Clear();
		}


		public List<BehaviourNode> _getChildren()
		{
			return m_children;
		}


		public BehaviourNode GetChild(int i)
		{
			return m_children[i];
		}


		public int GetIndex(BehaviourNode node)
		{
			for (int i = 0; i < m_children.Count; i++)
			{
				if (m_children[i] == node)
					return i;
			}
			return -1;
		}


		public void SortChildren(System.Comparison<BehaviourNode> comparison)
		{
			m_children.Sort(comparison);
		}

	}

}

