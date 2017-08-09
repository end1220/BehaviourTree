using UnityEngine;
using System.Collections.Generic;

namespace BevTree
{
	/// <summary>
	/// Sub Behaviour Tree Asset
	/// </summary>
	[CreateAssetMenu(menuName = "Behaviour Tree/Sub Tree", order = 2)]
	public class BTAsset : ScriptableObject
	{
		[System.Serializable]
		private class AssetIDPair
		{
			public BTAsset asset;
			public string assetID;
		}

		[SerializeField]
		[HideInInspector]
		protected string m_serializedData;

		public string SerializedData { get { return m_serializedData; } }

		[SerializeField]
		private Rect m_canvasArea;

		[SerializeField]
		private Vector2 m_canvasPosition;

		[SerializeField]
		private List<AssetIDPair> m_subtrees;

		[SerializeField]
		private string treeUidString;

		public string TreeUidString { get { return treeUidString; } }

		public string title;

		public string description;

		// 记录的序列化前的文件名，当复制此文件时可根据文件名不一致而重新生成tree uid（uid一定不能相同）。
		protected string lastFileName;


#if UNITY_EDITOR
		private BehaviourTree m_editModeTree;
#endif

		public static Vector2 DEFAULT_CANVAS_SIZE
		{
			get { return new Vector2(1000, 1000); }
		}

		public Rect CanvasArea
		{
			get
			{
				return m_canvasArea;
			}
			set
			{
				m_canvasArea = value;
			}
		}

		public Vector2 CanvasPosition
		{
			get
			{
				return m_canvasPosition;
			}
			set
			{
				m_canvasPosition = value;
			}
		}

#if UNITY_EDITOR
		private void OnEnable()
		{
			if(Mathf.Approximately(m_canvasArea.width, 0) || Mathf.Approximately(m_canvasArea.height, 0))
			{
				m_canvasArea = new Rect(-DEFAULT_CANVAS_SIZE.x / 2, -DEFAULT_CANVAS_SIZE.y / 2, DEFAULT_CANVAS_SIZE.x, DEFAULT_CANVAS_SIZE.y);
			}
			if(m_subtrees == null)
			{
				m_subtrees = new List<AssetIDPair>();
			}
		}

		public virtual BehaviourTree GetEditModeTree()
		{
			string nm = this.name;
			if(m_editModeTree == null)
			{
				m_editModeTree = BTUtils.DeserializeTree(m_serializedData);
				if(m_editModeTree != null)
				{
					if (string.IsNullOrEmpty(m_editModeTree.guidString) || lastFileName != this.name)
						m_editModeTree.guidString = BTUtils.GenerateUniqueStringID();
					m_editModeTree.Root.OnAfterDeserialize(this);
					m_editModeTree.ReadOnly = false;
				}
			}

			return m_editModeTree;
		}

		public virtual void Serialize()
		{
			if(m_editModeTree != null)
			{
				treeUidString = m_editModeTree.guidString;
				m_editModeTree.title = title;
				m_editModeTree.description = description;

				lastFileName = this.name;

				m_editModeTree.Root.OnBeforeSerialize(this);

				string serializedData = BTUtils.SerializeTree(m_editModeTree);
				if(serializedData != null)
				{
					m_serializedData = serializedData;
				}
			}
		}

		public virtual void Dispose()
		{
			m_editModeTree = null;
		}
#endif

		public virtual BehaviourTree CreateRuntimeTree()
		{
			BehaviourTree tree = BTUtils.DeserializeTree(m_serializedData);
			if(tree == null)
				tree = new BehaviourTree();

			tree.Root.OnAfterDeserialize(this);
			tree.ReadOnly = true;
			return tree;
		}

		public void SetSubtreeAsset(string subtreeID, BTAsset subtreeAsset)
		{
			if(!string.IsNullOrEmpty(subtreeID))
			{
				AssetIDPair subtree = m_subtrees.Find(obj => obj.assetID == subtreeID);
				if(subtree != null)
				{
					subtree.asset = subtreeAsset;
				}
				else
				{
					subtree = new AssetIDPair();
					subtree.asset = subtreeAsset;
					subtree.assetID = subtreeID;
					m_subtrees.Add(subtree);
				}
			}
		}

		public BTAsset GetSubtreeAsset(string subtreeID)
		{
			AssetIDPair subtree = m_subtrees.Find(obj => obj.assetID == subtreeID);
			return subtree != null ? subtree.asset : null;
		}
	}
}