
using UnityEngine;


namespace BevTree
{
	/// <summary>
	/// Root Behaviour Tree Asset
	/// </summary>
	[CreateAssetMenu(menuName = "Behaviour Tree/Root Tree", order = 1)]
	public class RootTreeAsset : BTAsset
	{
		// 此树作为根树时，这里保留着所有子树的RunBehaviourIndex节点引用的BTAsset。
		[SerializeField]
		private BTAsset[] indexedSubTrees;

		public BTAsset[] IndexedSubTrees { get { return indexedSubTrees; } }


		public BehaviourTree CreateRuntimeSubTree(int subTreeIndex)
		{
			if (subTreeIndex < 0 || subTreeIndex >= indexedSubTrees.Length)
				return null;

			if (indexedSubTrees[subTreeIndex] == null)
				return null;

			BehaviourTree tree = BTUtils.DeserializeTree(indexedSubTrees[subTreeIndex].SerializedData);
			if (tree == null)
				tree = new BehaviourTree();

			tree.Root.OnAfterDeserialize(indexedSubTrees[subTreeIndex]);
			tree.ReadOnly = true;
			return tree;
		}

	}
}