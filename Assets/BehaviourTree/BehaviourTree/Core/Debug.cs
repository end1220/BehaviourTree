

using UnityEngine;
using UnityEditor;



namespace BevTree
{

	public enum Breakpoint
	{
		None = 1 << 1,
		OnOpen = 1 << 2,
		OnClose = 1 << 3,
		OnSuccess = 1 << 4,
		OnFailure = 1 << 5,
		//OnEnter = 1 << 6,
		//OnExit = 1 << 7
	}


	// for editor debug
	public static class BTDebugHelper
	{
		public static Context DebugContext = null;

		public static BehaviourTree CurrentDebugRootTree = null;

		public static bool BreakPointEnabled { get; set; }

#if UNITY_EDITOR

		/// <summary>
		/// check if current select scene object is BevComponent. if yes then open the bev tree as debug mode.
		/// </summary>
		public static bool CheckDebugOpen(string treeGuid, out BehaviourTree btInstance)
		{
			btInstance = null;

			if (!EditorApplication.isPlaying)
				return false;

			if (Selection.activeObject == null || (Selection.activeObject as GameObject) == null)
				return false;

			BevComponent agent = (Selection.activeObject as GameObject).GetComponent<BevComponent>();
			if (agent == null)
				return false;

			btInstance = TrySelectedObjectDebugging(treeGuid);

			return btInstance != null;
		}


		public static BehaviourTree TrySelectedObjectDebugging(string treeUid)
		{
			if (Selection.activeObject == null || (Selection.activeObject as GameObject) == null)
				return null;

			BevComponent bcom = (Selection.activeObject as GameObject).GetComponent<BevComponent>();
			if (bcom == null)
				return null;

			BehaviourTree tree;
			Context cntx;
			bcom.FindAttachedBevTree(treeUid, out tree, out cntx);
			// find a tree instance, so set current debug data.
			if (tree != null)
			{
				CurrentDebugRootTree = tree;
				DebugContext = cntx;
			}

			return tree;
		}
#endif

	}


}