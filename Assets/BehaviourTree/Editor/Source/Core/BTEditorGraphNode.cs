using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using BevTree;

namespace BevTreeEditor
{
	public class BTEditorGraphNode : ScriptableObject
	{
		private const int DRAG_MOUSE_BUTTON = 0;
		private const int SELECT_MOUSE_BUTTON = 0;
		private const int CONTEXT_MOUSE_BUTTON = 1;
		private const float DOUBLE_CLICK_THRESHOLD = 0.2f;

		private List<BTEditorGraphNode> m_children;
		private BehaviourNode m_node;
		private BTEditorGraphNode m_parent;
		private BTEditorGraph m_graph;
		private Vector2 m_dragOffset;
		private float m_lastClickTime;
		private bool m_isSelected;
		private bool m_isDragging;
		private bool m_canBeginDragging;

		
		public BehaviourNode Node
		{
			get { return m_node; }
		}

		public BTEditorGraphNode Parent
		{
			get { return m_parent; }
		}

		public BTEditorGraph Graph
		{
			get { return m_graph; }
		}

		public int ChildCount
		{
			get { return m_children.Count; }
		}

		public bool IsRoot
		{
			get { return m_graph.IsRoot(this); }
		}

		public Vector2 NodePosition
		{
			get { return m_node.Position; }
			set
			{
				if(!IsRoot)
				{
					//Vector2 offset = value - m_node.Position;
					m_node.Position = value;
					//UpdateChildrenPosition(offset);
				}
			}
		}

		private bool CanUpdateChildren
		{
			get
			{
				return !(m_node is NodeGroup) || m_graph.IsRoot(this);
			}
		}

		private bool CanDrawChildren
		{
			get
			{
				return !(m_node is NodeGroup) || m_graph.IsRoot(this);
			}
		}
		
		private void OnCreated()
		{
			if(m_children == null)
			{
				m_children = new List<BTEditorGraphNode>();
			}
			
			m_isSelected = false;
			m_isDragging = false;
			m_canBeginDragging = false;
			m_dragOffset = Vector2.zero;
			m_lastClickTime = -1;

		}

		public void Update()
		{
			if(CanUpdateChildren)
				UpdateChildren();

			HandleEvents();
		}

		private void UpdateChildren()
		{
			for(int i = m_children.Count - 1; i >= 0; i--)
			{
				m_children[i].Update();
			}
		}

		private void HandleEvents()
		{
			Rect position = new Rect(NodePosition, BTEditorStyle.GetNodeSize(m_node));
			Vector2 mousePosition = BTEditorCanvas.Current.WindowSpaceToCanvasSpace(BTEditorCanvas.Current.Event.mousePosition);

			if(BTEditorCanvas.Current.Event.type == EventType.MouseDown && BTEditorCanvas.Current.Event.button == SELECT_MOUSE_BUTTON)
			{
				if(position.Contains(mousePosition))
				{
					if(!m_isSelected)
						m_graph.OnNodeSelect(this);

					if(m_lastClickTime > 0)
					{
						if(Time.realtimeSinceStartup <= m_lastClickTime + DOUBLE_CLICK_THRESHOLD)
						{
							OnDoubleClicked();
						}
					}
					
					m_lastClickTime = Time.realtimeSinceStartup;

					m_canBeginDragging = !IsRoot;
					BTEditorCanvas.Current.Event.Use();
				}
			}
			else if(BTEditorCanvas.Current.Event.type == EventType.MouseDown && BTEditorCanvas.Current.Event.button == CONTEXT_MOUSE_BUTTON)
			{
				if(position.Contains(mousePosition))
				{
					ShowContextMenu();
					BTEditorCanvas.Current.Event.Use();
				}
			}
			else if(BTEditorCanvas.Current.Event.type == EventType.MouseUp && BTEditorCanvas.Current.Event.button == SELECT_MOUSE_BUTTON)
			{
				if(m_isDragging)
				{
					m_graph.OnNodeEndDrag(this);
					BTEditorCanvas.Current.Event.Use();
				}
				m_canBeginDragging = false;
			}
			else if(BTEditorCanvas.Current.Event.type == EventType.MouseDrag && BTEditorCanvas.Current.Event.button == DRAG_MOUSE_BUTTON)
			{
				if(!m_graph.ReadOnly && !m_isDragging && m_canBeginDragging && position.Contains(mousePosition))
				{
					m_graph.OnNodeBeginDrag(this, mousePosition);
					BTEditorCanvas.Current.Event.Use();
				}
				else if(m_isDragging)
				{
					m_graph.OnNodeDrag(this, mousePosition);
					BTEditorCanvas.Current.Event.Use();
				}
			}
			else if(m_graph.SelectionBox.HasValue)
			{
				if(m_graph.SelectionBox.Value.Contains(position.center))
				{
					if(!m_isSelected)
					{
						m_graph.OnNodeSelect(this);
					}
				}
				else
				{
					if(m_isSelected)
					{
						m_graph.OnNodeDeselect(this);
					}
				}
			}
		}

		public void Draw()
		{
			if(CanDrawChildren)
				DrawTransitions();

			DrawSelf();

			DrawComment();

			DrawConstraints();

			if(CanDrawChildren)
				DrawChildren();
		}

		private RunningStatus GetNodeStatus(BehaviourNode node)
		{
			RunningStatus status = RunningStatus.None;
			if (BTDebugHelper.DebugContext != null)
			{
				long treeId = BTDebugHelper.CurrentDebugRootTree.guid;
				if (BTDebugHelper.DebugContext._travelNodes.ContainsKey(treeId))
				{
					if (BTDebugHelper.DebugContext._travelNodes[treeId].Contains(node))
						status = (RunningStatus)BTDebugHelper.DebugContext.blackboard.GetInt(treeId, node.guid, "Status");
				}
			}

			return status;
		}

		private RunningStatus GetConstraintResult(BehaviourNode node, int index)
		{
			RunningStatus status = RunningStatus.None;
			if (BTDebugHelper.DebugContext != null)
			{
				long treeId = BTDebugHelper.CurrentDebugRootTree.guid;
				if (BTDebugHelper.DebugContext._travelNodes.ContainsKey(treeId))
				{
					if (BTDebugHelper.DebugContext._travelNodes[treeId].Contains(node))
					{
						int bits = BTDebugHelper.DebugContext.blackboard.GetInt(treeId, node.guid, "Constraints");
						int v = (bits >> index) & 1;
						status = v == 1 ? RunningStatus.Success : RunningStatus.Failure;
					}
				}
			}
			return status;
		}

		private void DrawTransitions()
		{
			Vector2 nodeSize = BTEditorStyle.GetNodeSize(m_node);
			Rect position = new Rect(NodePosition + BTEditorCanvas.Current.Position, nodeSize);
			BTEditorTreeLayout treeLayout = BTEditorStyle.TreeLayout;

			foreach(var child in m_children)
			{
				Vector2 childNodeSize = BTEditorStyle.GetNodeSize(child.Node);
				Rect childPosition = new Rect(child.Node.Position + BTEditorCanvas.Current.Position, childNodeSize);
				RunningStatus childStatus = BTEditorCanvas.Current.IsDebuging ? GetNodeStatus(child.Node) : RunningStatus.None;
				Color color = BTEditorStyle.GetTransitionColor(childStatus);
				Vector2 nodeCenter = position.center;
				Vector2 childCenter = childPosition.center;

				if(treeLayout == BTEditorTreeLayout.Vertical)
				{
					if(Mathf.Approximately(nodeCenter.y, childCenter.y) || Mathf.Approximately(nodeCenter.x, childCenter.x))
					{
						BTEditorUtils.DrawLine(nodeCenter, childCenter, color);
					}
					else
					{
						BTEditorUtils.DrawLine(nodeCenter, nodeCenter + Vector2.up * (childCenter.y - nodeCenter.y) / 2, color);

						BTEditorUtils.DrawLine(nodeCenter + Vector2.up * (childCenter.y - nodeCenter.y) / 2,
											   childCenter + Vector2.up * (nodeCenter.y - childCenter.y) / 2, color);

						BTEditorUtils.DrawLine(childCenter, childCenter + Vector2.up * (nodeCenter.y - childCenter.y) / 2, color);
					}
				}
				else if(treeLayout == BTEditorTreeLayout.Horizontal)
				{
					//BTEditorUtils.DrawBezier(nodeCenter, childCenter, color);
					Vector2 nodeRight = new Vector2(position.center.x + nodeSize.x / 2, position.center.y);
					Vector2 childLeft = new Vector2(childPosition.center.x - childNodeSize.x / 2, childPosition.center.y);
					BTEditorUtils.DrawBezier(nodeRight, childLeft, color);
				}
				else
				{
					BTEditorUtils.DrawLine(nodeCenter, childCenter, color);
				}
			}
		}

		private void DrawSelf()
		{
			string label = m_node.Title;
			BTGraphNodeStyle nodeStyle = BTEditorStyle.GetNodeStyle(m_node);
			Vector2 nodeSize = BTEditorStyle.GetNodeSize(m_node);
			Rect position = new Rect(NodePosition + BTEditorCanvas.Current.Position, nodeSize);
			RunningStatus status = BTEditorCanvas.Current.IsDebuging ? GetNodeStatus(m_node) : RunningStatus.None;

			GUI.Box(position, "", nodeStyle.GetStyle(status, m_isSelected));

			int iconSize = 32;
			int iconOffsetY = 7;
			Rect iconPos = new Rect(position.x + (nodeSize.x - iconSize) / 2, position.y + (nodeSize.y - iconSize) / 2 - iconOffsetY, iconSize, iconSize);
			GUI.DrawTexture(iconPos, BTEditorStyle.GetNodeIcon(m_node));

			Rect titlePos = new Rect(position);
			titlePos.y = titlePos.y - 5;
			EditorGUI.LabelField(titlePos, label, BTEditorStyle.NodeTitleLabel);

			// show index of composite's children.
			if (Parent != null && Parent.Node is Composite)
			{
				Composite compNode = Parent.Node as Composite;
				int index = compNode.GetIndex(m_node);
				Rect nodeLeftPos = new Rect(position.x + 2, position.center.y - 8, 20, 16);
				EditorGUI.LabelField(nodeLeftPos, index.ToString(), EditorStyles.label);
			}

			if(m_node.Breakpoint != Breakpoint.None)
			{
				Rect imgPosition;
				if(m_node is NodeGroup)
				{
					imgPosition = new Rect(position.x + 2, position.y + 2, 12, 12);
				}
				else
				{
					imgPosition = new Rect(position.x + 2, position.y + 2, 12, 12);
				}
				
				GUI.DrawTexture(imgPosition, BTEditorStyle.Breakpoint);
			}
		}

		private void DrawChildren()
		{
			for(int i = 0; i < m_children.Count; i++)
			{
				m_children[i].Draw();
			}
		}

		private void DrawComment()
		{
			Vector2 nodeSize = BTEditorStyle.NodeCommentLabel.CalcSize(new GUIContent(m_node.Comment));
			Rect position = new Rect(NodePosition + BTEditorCanvas.Current.Position, nodeSize);
			position.y -= nodeSize.y + 3;
			EditorGUI.LabelField(position, string.Format("<color=green>{0}</color>", m_node.Comment), BTEditorStyle.NodeCommentLabel);
		}

		private void DrawConstraints()
		{
			if (m_node.Constraints.Count == 0)
				return;

			Vector2 nodeSize = BTEditorStyle.GetNodeSize(m_node);
			Rect position = new Rect(NodePosition + BTEditorCanvas.Current.Position, nodeSize);

			Rect cnsFoldoutPos = new Rect(position.x - 15, position.y + position.height, 10, 10);
			m_node.IsConstraintsExpanded = EditorGUI.Foldout(cnsFoldoutPos, m_node.IsConstraintsExpanded, GUIContent.none);

			int constraintOffsetY = 3;

			if (!m_node.IsConstraintsExpanded)
			{
				Rect consLabelPos = new Rect(cnsFoldoutPos.x + 20, cnsFoldoutPos.y + constraintOffsetY, 100, 20);
				EditorGUI.LabelField(consLabelPos, "<color=white>Constraints</color>", BTEditorStyle.NodeConstraintLabel);
			}
			else
			{
				int bits = -1;
				if (BTDebugHelper.DebugContext != null && BTDebugHelper.CurrentDebugRootTree != null)
				{
					long treeId = BTDebugHelper.CurrentDebugRootTree.guid;
					if (BTDebugHelper.DebugContext._travelNodes.ContainsKey(treeId))
					{
						if (BTDebugHelper.DebugContext._travelNodes[treeId].Contains(m_node))
						{
							bits = BTDebugHelper.DebugContext.blackboard.GetInt(treeId, m_node.guid, "Constraints");
						}
					}
				}

				bool lastFailed = false;
				for (int i = 0; i < m_node.Constraints.Count; i++)
				{
					Constraint constraint = m_node.Constraints[i];

					Rect headerPos = position;
					headerPos.y += nodeSize.y + i * 16 + constraintOffsetY;
					Rect labelPos = new Rect(headerPos.x, headerPos.y, position.width + 50, 16);

					string str = "";
					if (bits != -1 && !lastFailed)
					{
						int v = (bits >> i) & 1;
						if (v == 1)
						{
							str = string.Format(constraint.InvertResult ? "<color=green>! {0}</color>" : "<color=green>{0}</color>", constraint.Title);
						}
						else
						{
							lastFailed = true;
							str = string.Format(constraint.InvertResult ? "<color=red>! {0}</color>" : "<color=red>{0}</color>", constraint.Title);
						}
					}
					else
					{
						str = string.Format(constraint.InvertResult ? "<color=white>! {0}</color>" : "<color=white>{0}</color>", constraint.Title);
					}
					EditorGUI.LabelField(labelPos, str, BTEditorStyle.NodeConstraintLabel);
				}
			}
		}

		public void OnSelected()
		{
			m_isSelected = true;
			Selection.activeObject = this;
			BTEditorCanvas.Current.Repaint();
		}

		public void OnDeselected()
		{
			m_isSelected = false;
			m_isDragging = false;
			if(Selection.activeObject == this)
			{
				Selection.activeObject = null;
			}
			BTEditorCanvas.Current.Repaint();
		}

		public void OnBeginDrag(Vector2 position)
		{
			m_dragOffset = position - NodePosition;
			m_isDragging = true;
		}

		public void OnDrag(Vector2 position)
		{
			Vector2 nodePos = position - m_dragOffset;
			if(BTEditorCanvas.Current.SnapToGrid)
			{
				float snapSize = BTEditorCanvas.Current.SnapSize;
				nodePos.x = (float)Math.Round(nodePos.x / snapSize) * snapSize;
				nodePos.y = (float)Math.Round(nodePos.y / snapSize) * snapSize;
			}

			NodePosition = nodePos;

			BTEditorCanvas.Current.RecalculateSize(NodePosition);
			BTEditorCanvas.Current.Repaint();
		}

		public void OnEndDrag()
		{
			m_isDragging = false;

			UpdateSiblingNodeIndex();
		}

		private void OnDoubleClicked()
		{
			if(m_node is RunBehaviour)
			{
				RunBehaviour rb = (RunBehaviour)m_node;
				if(rb.BehaviourTreeAsset != null)
				{
					if(BTEditorCanvas.Current.IsDebuging && rb.BehaviourTree != null)
					{
						BehaviourTreeEditor.OpenSubtreeDebug(rb.BehaviourTreeAsset, rb.BehaviourTree);
					}
					else
					{
						BehaviourTreeEditor.OpenSubtree(rb.BehaviourTreeAsset);
					}
				}
			}
			if (m_node is RunBehaviourIndex)
			{
				RunBehaviourIndex rb = (RunBehaviourIndex)m_node;
				if (rb.SubTreeIndex >= 0)
				{
					if (BTEditorCanvas.Current.IsDebuging && rb.BehaviourTree != null)
					{
						BehaviourTreeEditor.OpenIndexSubtreeDebug(rb.SubTreeIndex, rb.BehaviourTree);
					}
					else
					{
						BehaviourTreeEditor.OpenIndexSubtree(rb.SubTreeIndex);
					}
				}
			}
			else if(m_node is NodeGroup)
			{
				if(IsRoot)
					m_graph.OnPopNodeGroup();
				else
					m_graph.OnPushNodeGroup(this);
			}
		}

		private void SetExistingNode(BehaviourNode node)
		{
			DestroyChildren();

			m_node = node;
			m_isSelected = false;

			if(node is Composite)
			{
				Composite composite = node as Composite;
				for(int i = 0; i < composite.ChildCount; i++)
				{
					BehaviourNode childNode = composite.GetChild(i);
					BTEditorGraphNode graphNode = BTEditorGraphNode.CreateExistingNode(this, childNode);
					m_children.Add(graphNode);
				}
			}
			else if(node is Decorator)
			{
				Decorator decorator = node as Decorator;
				BehaviourNode childNode = decorator.GetChild();
				if(childNode != null)
				{
					BTEditorGraphNode graphNode = BTEditorGraphNode.CreateExistingNode(this, childNode);
					m_children.Add(graphNode);
				}
			}
		}

		private void ShowContextMenu()
		{
			GenericMenu menu = BTContextMenuFactory.CreateNodeContextMenu(this);
			menu.DropDown(new Rect(BTEditorCanvas.Current.Event.mousePosition, Vector2.zero));
		}

		public BTEditorGraphNode OnCreateChild(Type type)
		{
			if(type != null)
			{
				BehaviourNode node = BTUtils.CreateNode(type);
				if(node != null)
				{
					Vector2 nodeSize = BTEditorStyle.GetNodeSize(m_node);
					Vector2 nodePos = NodePosition + nodeSize + new Vector2(50, 0);
					nodePos.x = Mathf.Max(nodePos.x, 0.0f);
					nodePos.y = Mathf.Max(nodePos.y, 0.0f);

					// force horizontal
					nodePos.y = NodePosition.y;

					node.Position = nodePos;

					return OnCreateChild(node);
				}
			}

			return null;
		}

		public BTEditorGraphNode OnCreateChild(BehaviourNode node)
		{
			if(node != null && ((m_node is Composite) || (m_node is Decorator)))
			{
				if(m_node is Composite)
				{
					Composite composite = m_node as Composite;
					composite.AddChild(node);
				}
				else if(m_node is Decorator)
				{
					Decorator decorator = m_node as Decorator;

					DestroyChildren();
					decorator.SetChild(node);
				}

				BTEditorGraphNode graphNode = BTEditorGraphNode.CreateExistingNode(this, node);
				m_children.Add(graphNode);

				BTEditorCanvas.Current.RecalculateSize(node.Position);
				return graphNode;
			}

			return null;
		}

		public BTEditorGraphNode OnInsertChild(int index, Type type)
		{
			if(type != null)
			{
				BehaviourNode node = BTUtils.CreateNode(type);
				if(node != null)
				{
					Vector2 nodeSize = BTEditorStyle.GetNodeSize(m_node);
					Vector2 nodePos = NodePosition + nodeSize + new Vector2(50, 0);
					nodePos.x = Mathf.Max(nodePos.x, 0.0f);
					nodePos.y = Mathf.Max(nodePos.y, 0.0f);

					// force horizontal
					nodePos.y = NodePosition.y;

					node.Position = nodePos;

					return OnInsertChild(index, node);
				}
			}

			return null;
		}

		public BTEditorGraphNode OnInsertChild(int index, BehaviourNode node)
		{
			if(node != null && ((m_node is Composite) || (m_node is Decorator)))
			{
				BTEditorGraphNode graphNode = null;

				if(m_node is Composite)
				{
					Composite composite = m_node as Composite;
					composite.InsertChild(index, node);

					graphNode = BTEditorGraphNode.CreateExistingNode(this, node);
					m_children.Insert(index, graphNode);
				}
				else if(m_node is Decorator)
				{
					Decorator decorator = m_node as Decorator;

					DestroyChildren();
					decorator.SetChild(node);

					graphNode = BTEditorGraphNode.CreateExistingNode(this, node);
					m_children.Add(graphNode);
				}

				BTEditorCanvas.Current.RecalculateSize(node.Position);
				return graphNode;
			}

			return null;
		}

		public void OnDelete()
		{
			if(m_parent != null)
			{
				m_parent.RemoveChild(this);
				BTEditorGraphNode.DestroyImmediate(this);
			}
		}

		public void OnDeleteChild(int index)
		{
			BTEditorGraphNode child = GetChild(index);
			if(child != null)
			{
				child.OnDelete();
			}
		}

		public int GetChildIndex(BTEditorGraphNode child)
		{
			return m_children.IndexOf(child);
		}

		public void ChangeChildIndex(int sourceIndex, int destinationIndex)
		{
			if(sourceIndex >= 0 && sourceIndex < ChildCount && destinationIndex >= 0 && destinationIndex < ChildCount)
			{
			}
		}

		public BTEditorGraphNode GetChild(int index)
		{
			if(index >= 0 && index < m_children.Count)
			{
				return m_children[index];
			}

			return null;
		}

		private void RemoveChild(BTEditorGraphNode child)
		{
			if(m_children.Remove(child))
			{
				if(m_node is Composite)
				{
					Composite composite = m_node as Composite;
					composite.RemoveChild(child.Node);
				}
				else if(m_node is Decorator)
				{
					Decorator decorator = m_node as Decorator;
					decorator.SetChild(null);
				}
			}
		}

		private void DestroyChildren()
		{
			for(int i = 0; i < m_children.Count; i++)
			{
				BTEditorGraphNode.DestroyImmediate(m_children[i]);
			}

			if(m_node is Composite)
			{
				((Composite)m_node).RemoveAllChildren();
			}
			else if(m_node is Decorator)
			{
				((Decorator)m_node).SetChild(null);
			}

			m_children.Clear();
		}

		private void OnDestroy()
		{
			m_graph.OnNodeDeselect(this);
			foreach(var child in m_children)
			{
				BTEditorGraphNode.DestroyImmediate(child);
			}
		}

		private static int SortSiblingCompare(BehaviourNode n1, BehaviourNode n2)
		{
			float y1 = n1.Position.y;
			float y2 = n2.Position.y;
			if (y1 > y2)
				return 1;
			else if (y1 < y2)
				return -1;
			else
				return 0;
		}

		private void UpdateSiblingNodeIndex()
		{
			if (Parent != null && Parent.Node is Composite)
			{
				Composite parentNode = Parent.Node as Composite;
				parentNode.SortChildren(SortSiblingCompare);
			}
		}

		private static BTEditorGraphNode CreateEmptyNode()
		{
			BTEditorGraphNode graphNode = ScriptableObject.CreateInstance<BTEditorGraphNode>();
			graphNode.OnCreated();
			graphNode.hideFlags = HideFlags.HideAndDontSave;

			return graphNode;
		}

		private static BTEditorGraphNode CreateExistingNode(BTEditorGraphNode parent, BehaviourNode node)
		{
			BTEditorGraphNode graphNode = BTEditorGraphNode.CreateEmptyNode();
			graphNode.m_parent = parent;
			graphNode.m_graph = parent.Graph;
			graphNode.SetExistingNode(node);

			return graphNode;
		}

		public static BTEditorGraphNode CreateRoot(BTEditorGraph graph, Root node)
		{
			if(graph != null && node != null)
			{
				BTEditorGraphNode graphNode = BTEditorGraphNode.CreateEmptyNode();
				graphNode.m_graph = graph;
				graphNode.m_parent = null;
				graphNode.SetExistingNode(node);

				return graphNode;
			}

			return null;
		}

	}
}