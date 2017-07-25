

using System.Collections.Generic;
using UnityEngine;



namespace BevTree
{

	// for editor debug
	public static class BTDebugHelper
	{

		public static Context DebugContext = null;

		public static BehaviourTree CurrentDebugRootTree = null;

		private static Dictionary<string, BehaviourTree> debugTrees = new Dictionary<string, BehaviourTree>();

		// for breakpoint debugging
		public static bool BreakPointEnabled { get; set; }


		public static BehaviourTree FindTree(string uid)
		{
			BehaviourTree tree;
			debugTrees.TryGetValue(uid, out tree);
			return tree;
		}

#if UNITY_EDITOR

		public static BehaviourTree TrySelectedObjectDebugging(string treeUid)
		{
			if (UnityEditor.Selection.activeObject == null || (UnityEditor.Selection.activeObject as GameObject) == null)
				return null;

			Actor actor = (UnityEditor.Selection.activeObject as GameObject).GetComponent<Actor>();
			if (actor == null || actor.controller == null)
				return null;

			DebugContext = actor.controller.GetComponent<BevTreeComponent>().context;
			int count = actor.controller.GetComponent<BevTreeComponent>().GetTreeCount();
			debugTrees.Clear();
			for (int i = 0; i < count; ++i)
			{
				BehaviourTree t = actor.controller.GetComponent<BevTreeComponent>().GetTree(i);
				if (t != null && !debugTrees.ContainsKey(t.guidString))
					debugTrees.Add(t.guidString, t);
			}

			// If m_btAsset is one of the runtime trees of the actor, 
			// show the runtime tree in the editor.
			BehaviourTree tree = FindTree(treeUid);
			if (tree != null)
			{
				CurrentDebugRootTree = tree;
			}

			return tree;
		}
#endif

	}


}