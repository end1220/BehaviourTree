
using System.Collections;
using System.Collections.Generic;
using BevTree.Serialization;


namespace BevTree
{
	using NodeStack = Stack<BehaviourNode>;


	public enum RunningStatus
	{
		None,
		Running,
		Success,
		Failure
	}


	public class BehaviourTree
	{
		// asset uid
		public string guidString;

		// runtime uid
		[BTIgnore]
		public long guid;

		public string title;

		public string description;

		[BTProperty("Root")]
		public Root root;

		[BTIgnore]
		public Root Root
		{
			get
			{
				return root;
			}
		}

		[BTIgnore]
		public bool ReadOnly { get; set; }



		public BehaviourTree()
		{
			if (root == null)
			{
				root = new Root();
				root.guid = GuidGen.GenUniqueGUID();
				root.Position = new UnityEngine.Vector2(0, 0);
			}
		}


		public BehaviourTree(BehaviourTree inst)
		{
			guidString = inst.guidString;
			guid = GuidGen.GenUniqueGUID();
			title = inst.title;
			description = inst.description;
			root = inst.root;
			ReadOnly = inst.ReadOnly;
		}


		public BehaviourTree(long uid)
		{
			guid = uid;

			root = new Root();
			root.guid = GuidGen.GenUniqueGUID();
			root.Position = new UnityEngine.Vector2(0, 0);
		}


		public void Init()
		{
			if (root != null)
				root._init();
		}


		public RunningStatus Tick(Context context)
		{
			context.EnsureTreeEnvSetup(this);

			context._openNodes[guid].Clear();
			context._travelNodes[guid].Clear();

			RunningStatus ret = RunningStatus.Running;
			if (root != null)
				ret = root._tick(context);

			UpdateOpenNodes(context);

			return ret;
		}


		private void UpdateOpenNodes(Context context)
		{
			NodeStack openNodes = context._openNodes[guid];
			NodeStack tmpNodes = context._tempNodes[guid];
			NodeStack oldOpenNodes = context._oldOpenNodes[guid];
			
			while (openNodes.Count > 0 /*&& oldOpenNodes.Count > 0*/)
			{
				if (openNodes.Count > oldOpenNodes.Count)
				{
					tmpNodes.Push(openNodes.Pop());
				}
				else if (openNodes.Count < oldOpenNodes.Count)
				{
					oldOpenNodes.Pop()._close(context);
				}
				else if (openNodes.Peek().guid != oldOpenNodes.Peek().guid)
				{
					tmpNodes.Push(openNodes.Pop());
					oldOpenNodes.Pop()._close(context);
				}
				else
					break;
			}
			while (tmpNodes.Count > 0)
				oldOpenNodes.Push(tmpNodes.Pop());
		}


		public string Dump(Context context)
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder();

			string[] statusColors = { "grey", "yellow", "green", "red" };

			NodeStack nodeStack = new NodeStack();
			nodeStack.Push(root);
			while (nodeStack.Count > 0)
			{
				BehaviourNode node = nodeStack.Pop();
				int depth = 0;
				BehaviourNode tmpNode = node;
				while (tmpNode.parent != null)
				{
					tmpNode = tmpNode.parent;
					depth++;
				}
				while (depth-- > 0) builder.Append("    ");
				RunningStatus lastRet = (RunningStatus)context.blackboard.GetInt(this.guid, node.guid, "Status");
				string color = statusColors[(int)lastRet];
				if (!context._travelNodes[this.guid].Contains(node))
					color = statusColors[0];
				builder.Append(string.Format("<color={0}>{1}</color>\n", color, node.GetType().Name));
				if (node is Composite)
				{
					var childrenList = (node as Composite)._getChildren();
					for (int i = childrenList.Count - 1; i >= 0; --i)
					{
						nodeStack.Push(childrenList[i]);
					}
				}
				else if (node is Decorator)
				{
					nodeStack.Push((node as Decorator).GetChild());
				}
				
			}

			return builder.ToString();
		}

	}


	/// <summary>
	/// Runtime context data shared between different behavior trees
	/// </summary>
	public class Context
	{
		public object _actorCtroller;
		public object actorController { get { return _actorCtroller; } }

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


		public void SetAgent(object agent)
		{
			this._actorCtroller = agent;
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



	// for editor debug
	public static class BTDebugHelper
	{

		public static Context DebugContext = null;

		public static BehaviourTree CurrentDebugRootTree = null;

		private static Dictionary<string, BehaviourTree> debugTrees = new Dictionary<string, BehaviourTree>();

		// for breakpoint debugging
		public static bool BreakPointEnabled { get; set; }

		public static void AddHotTree(BehaviourTree tree)
		{
			if (tree != null && !debugTrees.ContainsKey(tree.guidString))
				debugTrees.Add(tree.guidString, tree);
		}

		public static void ClearHotTree()
		{
			debugTrees.Clear();
		}

		public static BehaviourTree FindTree(string uid)
		{
			BehaviourTree tree;
			debugTrees.TryGetValue(uid, out tree);
			return tree;
		}

	}


}