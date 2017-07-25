

using System.Collections.Generic;
using UnityEngine;



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
		public static BehaviourTree TrySelectedObjectDebugging(string treeUid)
		{
			if (UnityEditor.Selection.activeObject == null || (UnityEditor.Selection.activeObject as GameObject) == null)
				return null;

			BevComponent bcom = (UnityEditor.Selection.activeObject as GameObject).GetComponent<BevComponent>();
			if (bcom == null)
				return null;

			bcom.FindAttachedBevTree(treeUid, out CurrentDebugRootTree, out DebugContext);

			return CurrentDebugRootTree;
		}
#endif

	}


}