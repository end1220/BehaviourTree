using BevTree;
using System;
using UnityEditor;
using UnityEngine;


namespace BevTreeEditor
{
	public static class BTEditorStyle
	{
		private static GUISkin m_editorSkin;
		private static Texture m_arrowUp;
		private static Texture m_arrowDown;
		private static Texture m_breakpoint;
		private static Texture m_optionsIcon;
		private static Texture m_treeInfoIcon;
		private static Texture m_warningIcon;

		// icon of nodes
		private static Texture m_defaultNodeIcon;

		private static Texture m_selectorIcon;
		private static Texture m_sequenceIcon;
		private static Texture m_parallelIcon;
		private static Texture m_randomIcon;
		private static Texture m_randomSelectorIcon;
		private static Texture m_weightedRandomIcon;

		private static Texture m_decoratorIcon;
		private static Texture m_rootIcon;
		private static Texture m_nodeGroupIcon;
		private static Texture m_invertIcon;
		private static Texture m_repeatIcon;

		private static Texture m_actionIcon;
		private static Texture m_failIcon;
		private static Texture m_succeedIcon;
		private static Texture m_runBevTreeIcon;
		private static Texture m_waitIcon;
		//

		private static BTGraphNodeStyle m_compositeStyle;
		private static BTGraphNodeStyle m_decoratorStyle;
		private static BTGraphNodeStyle m_actionStyle;
		private static BTGraphNodeStyle m_nodeGroupStyle;

		private static GUIStyle m_headerLabel;
		private static GUIStyle m_boldLabel;
		private static GUIStyle m_editorFooter;
		private static GUIStyle m_selectionBoxStyle;
		private static GUIStyle m_multilineTextAreaStyle;
		private static GUIStyle m_helpBoxStyle;
		private static GUIStyle m_listHeaderStyle;
		private static GUIStyle m_listBackgroundStyle;
		private static GUIStyle m_listButtonStyle;
		private static GUIStyle m_listDragHandleStyle;
		private static GUIStyle m_arrowUpButtonStyle;
		private static GUIStyle m_arrowDownButtonStyle;
		private static GUIStyle m_breadcrumbLeftStyle;
		private static GUIStyle m_breadcrumbLeftActiveStyle;
		private static GUIStyle m_breadcrumbMidStyle;
		private static GUIStyle m_breadcrumbMidActiveStyle;
		private static GUIStyle m_separatorStyle;
		private static GUIStyle m_regionBackground;

		// tree or node label style
		private static GUIStyle m_nodeTitleLabel;
		private static GUIStyle m_nodeCommentLabel;
		private static GUIStyle m_nodeConstraintLabel;
		private static GUIStyle m_treeCommentLabel;


		public static Texture ArrowUp
		{
			get
			{
				return m_arrowUp;
			}
		}

		public static Texture ArrowDown
		{
			get
			{
				return m_arrowDown;
			}
		}

		public static Texture Breakpoint
		{
			get
			{
				return m_breakpoint;
			}
		}


		public static Texture OptionsIcon
		{
			get
			{
				return m_optionsIcon;
			}
		}

		public static Texture TreeInfoIcon
		{
			get
			{
				return m_treeInfoIcon;
			}
		}

		public static Texture WarningIcon
		{
			get
			{
				return m_warningIcon;
			}
		}

		public static GUIStyle HeaderLabel
		{
			get
			{
				return m_headerLabel;
			}
		}

		public static GUIStyle BoldLabel
		{
			get
			{
				return m_boldLabel;
			}
		}

		public static GUIStyle SelectionBox
		{
			get
			{
				return m_selectionBoxStyle;
			}
		}

		public static GUIStyle MultilineTextArea
		{
			get
			{
				return m_multilineTextAreaStyle;
			}
		}

		public static GUIStyle HelpBox
		{
			get
			{
				return m_helpBoxStyle;
			}
		}

		public static GUIStyle ListHeader
		{
			get
			{
				return m_listHeaderStyle;
			}
		}

		public static GUIStyle ListBackground
		{
			get
			{
				return m_listBackgroundStyle;
			}
		}

		public static GUIStyle ListButton
		{
			get
			{
				return m_listButtonStyle;
			}
		}

		public static GUIStyle ListDragHandle
		{
			get
			{
				return m_listDragHandleStyle;
			}
		}

		public static GUIStyle ArrowUpButton
		{
			get
			{
				return m_arrowUpButtonStyle;
			}
		}

		public static GUIStyle ArrowDownButton
		{
			get
			{
				return m_arrowDownButtonStyle;
			}
		}

		public static GUIStyle EditorFooter
		{
			get
			{
				return m_editorFooter;
			}
		}

		public static GUIStyle BreadcrumbLeft
		{
			get
			{
				return m_breadcrumbLeftStyle;
			}
		}

		public static GUIStyle BreadcrumbLeftActive
		{
			get
			{
				return m_breadcrumbLeftActiveStyle;
			}
		}

		public static GUIStyle BreadcrumbMiddle
		{
			get
			{
				return m_breadcrumbMidStyle;
			}
		}

		public static GUIStyle BreadcrumbMiddleActive
		{
			get
			{
				return m_breadcrumbMidActiveStyle;
			}
		}

		public static GUIStyle SeparatorStyle
		{
			get
			{
				return m_separatorStyle;
			}
		}

		public static GUIStyle RegionBackground
		{
			get
			{
				return m_regionBackground;
			}
		}


		public static GUIStyle NodeTitleLabel { get { return m_nodeTitleLabel; } }

		public static GUIStyle NodeCommentLabel { get { return m_nodeCommentLabel; } }

		public static GUIStyle NodeConstraintLabel { get { return m_nodeConstraintLabel; } }

		public static GUIStyle TreeCommentLabel { get { return m_treeCommentLabel; } }


		public static BTEditorTreeLayout TreeLayout
		{
			get
			{
				return BTEditorTreeLayout.Horizontal;// (BTEditorTreeLayout)EditorPrefs.GetInt("BevTree.Editor.TreeLayout", (int)BTEditorTreeLayout.Vertical);
			}
			set
			{
				EditorPrefs.SetInt("BevTree.Editor.TreeLayout", (int)value);
			}
		}

		public static void EnsureStyle()
		{
			LoadEditorSkin();
			LoadTextures();
			CreateNodeStyles();
			CreateGUIStyles();
		}

		private static void LoadEditorSkin()
		{
			if(m_editorSkin == null)
			{
				m_editorSkin = Resources.Load<GUISkin>("BevTree/EditorGUI/editor_style");
			}
		}

		private static void LoadTextures()
		{
			if(m_arrowUp == null)
				m_arrowUp = Resources.Load<Texture>("BevTree/EditorGUI/arrow_2_up");

			if(m_arrowDown == null)
				m_arrowDown = Resources.Load<Texture>("BevTree/EditorGUI/arrow_2_down");

			if(m_breakpoint == null)
				m_breakpoint = Resources.Load<Texture>("BevTree/EditorGUI/breakpoint");

			if(m_optionsIcon == null)
				m_optionsIcon = Resources.Load<Texture>("BevTree/EditorGUI/options_icon");

			if (m_treeInfoIcon == null)
				m_treeInfoIcon = Resources.Load<Texture>("BevTree/EditorGUI/treeinfo_icon");

			if (m_warningIcon == null)
				m_warningIcon = Resources.Load<Texture>("BevTree/EditorGUI/warn");

			// icon of nodes
			if (m_defaultNodeIcon == null)
				m_defaultNodeIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/default");

			if (m_selectorIcon == null)
				m_selectorIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/selector");
			if (m_sequenceIcon == null)
				m_sequenceIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/sequence");
			if (m_parallelIcon == null)
				m_parallelIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/parallel");
			if (m_randomIcon == null)
				m_randomIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/random");
			if (m_randomSelectorIcon == null)
				m_randomSelectorIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/randomSelector");
			if (m_weightedRandomIcon == null)
				m_weightedRandomIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/weightedRandom");

			if (m_decoratorIcon == null)
				m_decoratorIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/decorator");
			if (m_rootIcon == null)
				m_rootIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/root");
			if (m_nodeGroupIcon == null)
				m_nodeGroupIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/decorator");
			if (m_invertIcon == null)
				m_invertIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/invert");
			if (m_repeatIcon == null)
				m_repeatIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/repeat");

			if (m_actionIcon == null)
				m_actionIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/action");

			if (m_failIcon == null)
				m_failIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/failure");

			if (m_succeedIcon == null)
				m_succeedIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/success");

			if (m_runBevTreeIcon == null)
				m_runBevTreeIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/runBevTree");

			if (m_waitIcon == null)
				m_waitIcon = Resources.Load<Texture>("BevTree/EditorGUI/Node/wait");
			
		}

		private static void CreateNodeStyles()
		{
			if(m_compositeStyle == null)
			{
				m_compositeStyle = new BTGraphNodeStyle("flow node 1", "flow node 1 on",
														"flow node 6", "flow node 6 on",
														"flow node 4", "flow node 4 on",
														"flow node 3", "flow node 3 on");
			}

			if(m_decoratorStyle == null)
			{
				m_decoratorStyle = new BTGraphNodeStyle("flow node 1", "flow node 1 on",
														"flow node 6", "flow node 6 on",
														"flow node 4", "flow node 4 on",
														"flow node 3", "flow node 3 on");
			}

			if(m_actionStyle == null)
			{
				m_actionStyle = new BTGraphNodeStyle("flow node 0", "flow node 0 on",
													"flow node 6", "flow node 6 on",
													"flow node 4", "flow node 4 on",
													"flow node 3", "flow node 3 on");
			}

			if(m_nodeGroupStyle == null)
			{
				m_nodeGroupStyle = new BTGraphNodeStyle("flow node 1", "flow node 1 on",
														"flow node 6", "flow node 6 on",
														"flow node 4", "flow node 4 on",
														"flow node 3", "flow node 3 on");
					/*new BTGraphNodeStyle("flow node hex 1", "flow node hex 1 on",
													"flow node hex 6", "flow node hex 6 on",
													"flow node hex 4", "flow node hex 4 on",
													"flow node hex 3", "flow node hex 3 on");*/
			}
		}

		private static void CreateGUIStyles()
		{
			if(m_headerLabel == null)
			{
				m_headerLabel = new GUIStyle(EditorStyles.boldLabel);
				m_headerLabel.alignment = TextAnchor.UpperLeft;
				m_headerLabel.contentOffset = new Vector2(0, -2);
				m_headerLabel.fontSize = 15;
				m_headerLabel.fontStyle = FontStyle.Bold;
			}

			if(m_boldLabel == null)
			{
				m_boldLabel = new GUIStyle(EditorStyles.boldLabel);
			}

			if(m_editorFooter == null)
			{
				m_editorFooter = new GUIStyle("ProjectBrowserHeaderBgTop");
				m_editorFooter.alignment = TextAnchor.MiddleRight;
				m_editorFooter.contentOffset = new Vector2(-10, 0);
			}

			if(m_selectionBoxStyle == null)
			{
				m_selectionBoxStyle = m_editorSkin.FindStyle("selection_box");
				if(m_selectionBoxStyle == null)
				{
					m_selectionBoxStyle = m_editorSkin.box;
				}
			}

			if(m_multilineTextAreaStyle == null)
			{
				m_multilineTextAreaStyle = new GUIStyle(EditorStyles.textField);
				m_multilineTextAreaStyle.wordWrap = true;
			}

			if (m_helpBoxStyle == null)
			{
				m_helpBoxStyle = new GUIStyle(EditorStyles.helpBox);
				m_helpBoxStyle.wordWrap = true;
				m_helpBoxStyle.fontSize = 12;
			}

			if(m_listHeaderStyle == null)
			{
				m_listHeaderStyle = new GUIStyle(Array.Find<GUIStyle>(GUI.skin.customStyles, obj => obj.name == "RL Header"));
				m_listHeaderStyle.normal.textColor = Color.black;
				m_listHeaderStyle.alignment = TextAnchor.MiddleLeft;
				m_listHeaderStyle.contentOffset = new Vector2(10, 0);
				m_listHeaderStyle.fontSize = 11;
			}

			if(m_listBackgroundStyle == null)
			{
				m_listBackgroundStyle = new GUIStyle("RL Background");
			}

			if(m_listButtonStyle == null)
			{
				m_listButtonStyle = new GUIStyle(Array.Find<GUIStyle>(GUI.skin.customStyles, obj => obj.name == "RL FooterButton"));
				m_listButtonStyle.alignment = TextAnchor.MiddleCenter;
			}

			if(m_listDragHandleStyle == null)
			{
				m_listDragHandleStyle = new GUIStyle("RL DragHandle");
			}

			if(m_arrowUpButtonStyle == null)
			{
				m_arrowUpButtonStyle = m_editorSkin.FindStyle("arrow_up");
			}

			if(m_arrowDownButtonStyle == null)
			{
				m_arrowDownButtonStyle = m_editorSkin.FindStyle("arrow_down");
			}

			if(m_breadcrumbLeftStyle == null)
			{
				m_breadcrumbLeftStyle = new GUIStyle("GUIEditor.BreadcrumbLeft");
			}

			if(m_breadcrumbLeftActiveStyle == null)
			{
				m_breadcrumbLeftActiveStyle = new GUIStyle("GUIEditor.BreadcrumbLeft");
				m_breadcrumbLeftActiveStyle.normal.background = m_breadcrumbLeftActiveStyle.active.background;
			}

			if(m_breadcrumbMidStyle == null)
			{
				m_breadcrumbMidStyle = new GUIStyle("GUIEditor.BreadcrumbMid");
			}

			if(m_breadcrumbMidActiveStyle == null)
			{
				m_breadcrumbMidActiveStyle = new GUIStyle("GUIEditor.BreadcrumbMid");
				m_breadcrumbMidActiveStyle.normal.background = m_breadcrumbMidActiveStyle.active.background;
			}

			if(m_separatorStyle == null)
			{
				m_separatorStyle = new GUIStyle("sv_iconselector_sep");
			}

			if(m_regionBackground == null)
			{
				m_regionBackground = new GUIStyle("RegionBg");
				m_regionBackground.contentOffset = new Vector2(0, -3);
				m_regionBackground.alignment = TextAnchor.MiddleCenter;
			}

			if (m_nodeTitleLabel == null)
			{
				m_nodeTitleLabel = new GUIStyle(EditorStyles.boldLabel);
				m_nodeTitleLabel.alignment = TextAnchor.LowerCenter;
			}

			if (m_nodeCommentLabel == null)
			{
				m_nodeCommentLabel = new GUIStyle(EditorStyles.label);
				m_nodeCommentLabel.wordWrap = true;
				m_nodeCommentLabel.alignment = TextAnchor.LowerLeft;
				m_nodeCommentLabel.richText = true;
			}

			if (m_nodeConstraintLabel == null)
			{
				m_nodeConstraintLabel = new GUIStyle(EditorStyles.label);
				m_nodeConstraintLabel.wordWrap = true;
				m_nodeConstraintLabel.alignment = TextAnchor.UpperLeft;
				m_nodeConstraintLabel.richText = true;
			}

			if (m_treeCommentLabel == null)
			{
				m_treeCommentLabel = new GUIStyle(EditorStyles.boldLabel);
				m_treeCommentLabel.wordWrap = true;
				m_treeCommentLabel.fontSize = 20;
				m_treeCommentLabel.alignment = TextAnchor.UpperLeft;
				m_treeCommentLabel.richText = true;
			}

		}

		public static BTGraphNodeStyle GetNodeStyle(BehaviourNode node)
		{
			if(node != null)
			{
				if(node is NodeGroup)
				{
					return m_nodeGroupStyle;
				}
				else if(node is Composite)
				{
					return m_compositeStyle;
				}
				else if(node is Decorator)
				{
					return m_decoratorStyle;
				}
				else if(node is BevTree.Action)
				{
					return m_actionStyle;
				}
			}

			return null;
		}

		public static Vector2 GetNodeSize(BehaviourNode node)
		{
			return GetNodeSize_Horz(node);
			/*string label = node.Title;

			if(node != null)
			{
				if(node is NodeGroup)
				{
					return m_nodeGroupStyle.GetSize(label, TreeLayout);
				}
				else if(node is Composite)
				{
					return m_compositeStyle.GetSize(label, TreeLayout);
				}
				else if(node is Decorator || node is NodeGroup)
				{
					return m_decoratorStyle.GetSize(label, TreeLayout);
				}
				else if(node is BevTree.Action)
				{
					return m_actionStyle.GetSize(label, TreeLayout);
				}
			}

			return new Vector2(180, 40);*/
		}

		private static Vector2 GetNodeSize_Horz(BehaviourNode node)
		{
			string label = node.Title;

			if (node != null)
			{
				if (node is NodeGroup)
				{
					return m_nodeGroupStyle.GetSize_Horz(label);
				}
				else if (node is Composite)
				{
					return m_compositeStyle.GetSize_Horz(label);
				}
				else if (node is Decorator || node is NodeGroup)
				{
					return m_decoratorStyle.GetSize_Horz(label);
				}
				else if (node is BevTree.Action)
				{
					return m_actionStyle.GetSize_Horz(label);
				}
			}

			return new Vector2(180, 40);
		}

		public static Color GetTransitionColor(RunningStatus status)
		{
			switch(status)
			{
			case RunningStatus.Failure:
				return Color.red;
			case RunningStatus.Running:
				return new Color32(229, 202, 76, 255);
			case RunningStatus.Success:
				return Color.green;
			}

			return Color.white;
		}


		public static Texture GetNodeIcon(BehaviourNode node)
		{
			Texture tex = m_defaultNodeIcon;

			if (node is Composite)
			{
				if (node is Selector)
					tex = m_selectorIcon;
				else if (node is Sequence)
					tex = m_sequenceIcon;
				else if (node is Parallel)
					tex = m_parallelIcon;
				else if (node is WeightedRandom)
					tex = m_weightedRandomIcon;
				else if (node is BevTree.Random)
					tex = m_randomIcon;
				/*else if (node is RandomSelector)
					tex = m_randomSelectorIcon;*/
				
			}
			else if (node is Decorator)
			{
				tex = m_decoratorIcon;
				if (node is Root)
					tex = m_rootIcon;
				else if (node is NodeGroup)
					tex = m_nodeGroupIcon;
				else if (node is Invert)
					tex = m_invertIcon;
				else if (node is Repeat || node is RepeatForever)
					tex = m_repeatIcon;
			}
			else if (node is BevTree.Action)
			{
				tex = m_actionIcon;
				if (node is Fail)
					tex = m_failIcon;
				else if (node is Succeed)
					tex = m_succeedIcon;
				else if (node is RunBehaviour || node is RunBehaviourIndex)
					tex = m_runBevTreeIcon;
				else if (node is Wait)
					tex = m_waitIcon;
			}
			
			return tex;
		}

	}
}
