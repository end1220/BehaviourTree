
using System.Collections.Generic;


namespace BevTree
{

	using NodeStack = Stack<BehaviourNode>;


	public interface IAgent
	{

	}


	/// <summary>
	/// Runtime context data shared among different behavior trees
	/// </summary>
	public class Context
	{
		public IAgent _agent;
		public IAgent agent { get { return _agent; } }

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


		public void SetAgent(IAgent agent)
		{
			_agent = agent;
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


	class GuidGen
	{
		public static long GenUniqueGUID()
		{
			byte[] buffer = System.Guid.NewGuid().ToByteArray();
			return System.BitConverter.ToInt64(buffer, 0);
		}
	}

	class RandomGen
	{
		private static System.Random rnd = new System.Random(666);

		/// <summary>
		/// get a random integer in [min, max].
		/// </summary>
		/// <param name="min">min value</param>
		/// <param name="max">max value</param>
		/// <returns></returns>
		public static int RandInt(int min, int max)
		{
			return rnd.Next(min, max + 1);
		}

		/// <summary>
		/// get a random float in [0, 1).
		/// </summary>
		/// <returns></returns>
		public static float RandFloat()
		{
			return (rnd.Next(0, int.MaxValue)) / (int.MaxValue + 1.0f);
		}

		/// <summary>
		/// get a random float in (-1, 1).
		/// </summary>
		/// <returns></returns>
		public static float RandClamp()
		{
			return RandFloat() - RandFloat();
		}
	}


	public abstract class BevComponent : UnityEngine.MonoBehaviour
	{
		public virtual void FindAttachedBevTree(string treeUid, out BehaviourTree tree, out Context context)
		{
			tree = null;
			context = null;
		}
	}

}
