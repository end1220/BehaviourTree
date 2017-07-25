
using System.Collections.Generic;



namespace BevTree
{
	using NodeStack = Stack<BehaviourNode>;


	/// <summary>
	/// Runtime context data shared between different behavior trees
	/// </summary>
	public class Context
	{
		public ActorController _actorCtroller;
		public ActorController actorController { get { return _actorCtroller; } }

		private BehaviourTree _tree;
		public BehaviourTree tree { get { return _tree; } }

		private BevBlackboard _blackboard = new BevBlackboard();
		public BevBlackboard blackboard { get { return _blackboard; } }


		// internal members
		private HashSet<long> treeSet = new HashSet<long>();
		public Dictionary<long, NodeStack> _openNodes = new Dictionary<long, NodeStack>();
		public Dictionary<long, NodeStack> _tempNodes = new Dictionary<long, NodeStack>();
		public Dictionary<long, NodeStack> _oldOpenNodes = new Dictionary<long, NodeStack>();
		public Dictionary<long, NodeStack> _travelNodes = new Dictionary<long, NodeStack>();


		public void SetAgent(ActorController agent)
		{
			_actorCtroller = agent;
		}


		public void EnsureTreeEnvSetup(BehaviourTree tree)
		{
			this._tree = tree;
			if (!treeSet.Contains(tree.guid))
			{
				treeSet.Add(tree.guid);
				_openNodes.Add(tree.guid, new NodeStack());
				_tempNodes.Add(tree.guid, new NodeStack());
				_oldOpenNodes.Add(tree.guid, new NodeStack());
				_travelNodes.Add(tree.guid, new NodeStack());
			}
		}

	}

}