using UnityEngine;
using UnityEditor;
using BevTree;

namespace BevTreeEditor
{
	public class BehaviourTreeEditor : EditorWindow
	{
		[SerializeField]
		private Texture m_gridTexture;
		[SerializeField]
		private BTAsset m_btAsset;
		[SerializeField]
		private BTNavigationHistory m_navigationHistory;

		private BTEditorGrid m_grid;
		private BTEditorGraph m_graph;
		private BTEditorCanvas m_canvas;
		private BTEditorHotKeyHandler m_hotkeyHandler;
		private bool m_isDisposed;

		public BTNavigationHistory NavigationHistory
		{
			get { return m_navigationHistory; }
		}

		

		private void OnEnable()
		{
			if(m_gridTexture == null)
			{
				m_gridTexture = Resources.Load<Texture>("BevTree/EditorGUI/background");
			}
			
			if(m_graph == null)
			{
				m_graph = BTEditorGraph.Create();
			}
			if(m_canvas == null)
			{
				m_canvas = new BTEditorCanvas();
				BTEditorCanvas.Current = m_canvas;
			}
			if (m_hotkeyHandler == null)
			{
				m_hotkeyHandler = new BTEditorHotKeyHandler(m_graph);
			}
			if(m_grid == null)
			{
				m_grid = new BTEditorGrid(m_gridTexture);
			}
			if(m_navigationHistory == null)
			{
				m_navigationHistory = new BTNavigationHistory();
			}

			ReloadBehaviourTree();
			m_isDisposed = false;
			m_canvas.OnRepaint += OnRepaint;
			EditorApplication.playmodeStateChanged += HandlePlayModeChanged;

			// debugging
			Selection.selectionChanged = delegate { SetupBTDebugging(); };
		}

		private void OnDisable()
		{
			Dispose();
		}

		private void OnDestroy()
		{
			Dispose();
		}

		private void Dispose()
		{
			if(!m_isDisposed)
			{
				if(m_graph != null)
				{
					BTEditorGraph.DestroyImmediate(m_graph);
					m_graph = null;
				}
				if(m_btAsset != null)
				{
					SaveBehaviourTree();
					m_btAsset.Dispose();
				}

				EditorApplication.playmodeStateChanged -= HandlePlayModeChanged;
				m_isDisposed = true;
			}
		}

		private void SetBTAsset(BTAsset asset, bool clearNavigationHistory)
		{
			if(asset != null && (clearNavigationHistory || asset != m_btAsset))
			{
				if(m_btAsset != null)
				{
					SaveBehaviourTree();
					m_btAsset.Dispose();
					m_btAsset = null;
				}

				BehaviourTree behaviourTree = asset.GetEditModeTree();
				if(behaviourTree != null)
				{
					m_btAsset = asset;
					m_graph.SetBehaviourTree(asset, behaviourTree);
					m_canvas.Area = m_btAsset.CanvasArea;
					
					if (Mathf.Approximately(m_btAsset.CanvasPosition.x, 0) && Mathf.Approximately(m_btAsset.CanvasPosition.y, 0))
						m_canvas.CenterOnPosition(behaviourTree.Root.Position, position.size);
					else
						m_canvas.Position = m_btAsset.CanvasPosition;

					m_canvas.IsDebuging = false;

					if(clearNavigationHistory)
					{
						m_navigationHistory.Clear();
					}

					m_navigationHistory.Push(m_btAsset, null);
				}
				else
				{
					CrashEditor("Failed to deserialize behaviour tree!\n\nThis can happen when you rename a behaviour node class, when you change the namespace or when you delete a behaviour node script.\n\nTry to enable text serialization and manually edit the asset file to fix the behaviour tree.");
				}
			}
		}

		private void SetBTAssetDebug(BTAsset asset, BehaviourTree btInstance, bool clearNavigationHistory)
		{
			if(asset != null && btInstance != null && (clearNavigationHistory || asset != m_btAsset || !m_canvas.IsDebuging))
			{
				m_btAsset = asset;
				m_graph.SetBehaviourTree(asset, btInstance);
				m_canvas.Area = m_btAsset.CanvasArea;

				if (Mathf.Approximately(m_btAsset.CanvasPosition.x, 0) && Mathf.Approximately(m_btAsset.CanvasPosition.y, 0))
					m_canvas.CenterOnPosition(btInstance.Root.Position, position.size);
				else
					m_canvas.Position = m_btAsset.CanvasPosition;

				m_canvas.IsDebuging = true;

				if(clearNavigationHistory)
				{
					m_navigationHistory.Clear();
				}

				m_navigationHistory.Push(m_btAsset, btInstance);
			}
		}

		private void ReloadBehaviourTree()
		{
			if(m_btAsset != null)
			{
				BehaviourTree behaviourTree = m_btAsset.GetEditModeTree();
				if(behaviourTree != null)
				{
					m_graph.SetBehaviourTree(m_btAsset, behaviourTree);
					m_canvas.Area = m_btAsset.CanvasArea;

					if (Mathf.Approximately(m_btAsset.CanvasPosition.x, 0) && Mathf.Approximately(m_btAsset.CanvasPosition.y, 0))
						m_canvas.CenterOnPosition(behaviourTree.Root.Position, position.size);
					else
						m_canvas.Position = m_btAsset.CanvasPosition;

					m_canvas.IsDebuging = false;
				}
				else
				{
					CrashEditor("Failed to deserialize behaviour tree!\n\nThis can happen when you rename a behaviour node class, when you change the namespace or when you delete a behaviour node script.\n\nTry to enable text serialization and manually edit the asset file to fix the behaviour tree.");
				}
			}
		}

		private void CrashEditor(string message)
		{
			Close();
			EditorUtility.DisplayDialog("Error", message, "Close");
		}

		public void CreateNewBehaviourTree()
		{
			string path = EditorUtility.SaveFilePanelInProject("Create new behaviour tree", "behaviour_tree", "asset", "");
			if(!string.IsNullOrEmpty(path))
			{
				BTAsset asset = ScriptableObject.CreateInstance<BTAsset>();

				AssetDatabase.CreateAsset(asset, path);
				AssetDatabase.Refresh();

				SetBTAsset(AssetDatabase.LoadAssetAtPath<BTAsset>(path), true);
			}
		}

		public void OpenBehaviourTree()
		{
			string path = EditorUtility.OpenFilePanel("Open behaviour tree", "", "asset");
			if(!string.IsNullOrEmpty(path))
			{
				int index = path.IndexOf("Assets");
				if(index >= 0)
				{
					path = path.Substring(index);
					SetBTAsset(AssetDatabase.LoadAssetAtPath<BTAsset>(path), true);
				}
			}
		}

		public void SaveBehaviourTree()
		{
			if(m_btAsset != null)
			{
				m_btAsset.CanvasArea = m_canvas.Area;
				m_btAsset.CanvasPosition = m_canvas.Position;
				m_btAsset.Serialize();
				EditorUtility.SetDirty(m_btAsset);
			}
		}

		private void HandlePlayModeChanged()
		{
			if(!EditorApplication.isPlaying)
			{
				if(EditorApplication.isPlayingOrWillChangePlaymode)
				{
					SaveBehaviourTree();
				}
				else
				{
					m_navigationHistory.DiscardInstances();
					ReloadBehaviourTree();
				}
			}
		}

		private void OnGUI()
		{
			if(m_btAsset != null)
			{
				Rect navHistoryRect = new Rect(0.0f, 0.0f, position.width, 20.0f);
				Rect optionsRect = new Rect(position.width - 20.0f, 0.0f, 20.0f, 20.0f);
				Rect footerRect = new Rect(0.0f, position.height - 18.0f, position.width, 20.0f);
				Rect canvasRect = new Rect(0.0f, navHistoryRect.yMax, position.width, position.height - (footerRect.height + navHistoryRect.height));
				Rect debugRect = new Rect(optionsRect.x - 60, 0.0f, 60.0f, 20.0f);
				//Rect treeInfoRect = new Rect(debugRect.x - 30, 0.0f, 20.0f, 20.0f);
				
				BTEditorStyle.EnsureStyle();
				m_grid.DrawGUI(position.size);
				m_graph.DrawGUI(canvasRect);
				m_canvas.HandleEvents(canvasRect, position.size);
				m_hotkeyHandler.HandlerEvents();
				DrawNavigationHistory(navHistoryRect);
				DrawFooter(footerRect);
				DrawOptions(optionsRect);
				DrawDebug(debugRect);
				//DrawTreeInfo(treeInfoRect);

				if(m_canvas.IsDebuging)
				{
					OnRepaint();
				}
			}
		}

		private void DrawNavigationHistory(Rect screenRect)
		{
			EditorGUI.LabelField(screenRect, "", BTEditorStyle.EditorFooter);

			if(m_navigationHistory.Size > 0)
			{
				float left = screenRect.x;
				for(int i = 0; i < m_navigationHistory.Size; i++)
				{
					BTAsset asset = m_navigationHistory.GetAssetAt(i);
					GUIStyle style;
					Vector2 size;

					if(i > 0)
					{
						style = (i == m_navigationHistory.Size - 1) ? BTEditorStyle.BreadcrumbMiddleActive : BTEditorStyle.BreadcrumbMiddle;
						size = style.CalcSize(new GUIContent(asset.name));
					}
					else
					{
						style = (i == m_navigationHistory.Size - 1) ? BTEditorStyle.BreadcrumbLeftActive : BTEditorStyle.BreadcrumbLeft;
						size = style.CalcSize(new GUIContent(asset.name));
					}

					Rect position = new Rect(left, screenRect.y, size.x, screenRect.height);
					left += size.x;

					if(i < m_navigationHistory.Size - 1)
					{
						if(GUI.Button(position, asset.name, style))
						{
							GoBackInHistory(i);
							break;
						}
					}
					else
					{
						EditorGUI.LabelField(position, asset.name, style);
					}
				}
			}
		}

		private void GoBackInHistory(int positionInHistory)
		{
			BTAsset btAsset;
			BehaviourTree btInstance;

			m_navigationHistory.GetAt(positionInHistory, out btAsset, out btInstance);

			if(btAsset != null)
			{
				m_navigationHistory.Trim(positionInHistory);
				if(btInstance != null)
				{
					SetBTAssetDebug(btAsset, btInstance, false);
				}
				else
				{
					SetBTAsset(btAsset, false);
				}
			}
			else
			{
				m_navigationHistory.Clear();
			}
		}

		private void DrawFooter(Rect screenRect)
		{
			string behaviourTreePath = AssetDatabase.GetAssetPath(m_btAsset).Substring(7);
			EditorGUI.LabelField(screenRect, behaviourTreePath, BTEditorStyle.EditorFooter);
		}

		private void DrawOptions(Rect screenRect)
		{
			if(GUI.Button(screenRect, BTEditorStyle.OptionsIcon, EditorStyles.toolbarButton))
			{
				GenericMenu menu = BTContextMenuFactory.CreateBehaviourTreeEditorMenu(this);
				menu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
			}
		}

		private void DrawDebug(Rect screenRect)
		{
			BTDebugHelper.BreakPointEnabled = GUI.Toggle(screenRect, BTDebugHelper.BreakPointEnabled, "debug");
		}

		private void DrawTreeInfo(Rect screenRect)
		{
			if (GUI.Button(screenRect, BTEditorStyle.TreeInfoIcon, EditorStyles.toolbarButton))
			{
				//GenericMenu menu = BTContextMenuFactory.CreateBehaviourTreeEditorMenu(this);
				//menu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
			}
		}

		public void OnRepaint()
		{
			Repaint();
		}

		private static string TitleName()
		{
			return "BehaviourTree";
		}

		public static void Open(BTAsset behaviourTree)
		{
			BehaviourTreeEditor window = EditorWindow.GetWindow<BehaviourTreeEditor>(TitleName());
			window.SetBTAsset(behaviourTree, true);
		}

		public static void OpenDebug(BTAsset btAsset, BehaviourTree btInstance)
		{
			BehaviourTreeEditor window = EditorWindow.GetWindow<BehaviourTreeEditor>(TitleName());
			window.SetBTAssetDebug(btAsset, btInstance, true);
		}

		public static void OpenSubtree(BTAsset behaviourTree)
		{
			BehaviourTreeEditor window = EditorWindow.GetWindow<BehaviourTreeEditor>(TitleName());
			window.SetBTAsset(behaviourTree, false);
		}

		public static void OpenSubtreeDebug(BTAsset btAsset, BehaviourTree btInstance)
		{
			BehaviourTreeEditor window = EditorWindow.GetWindow<BehaviourTreeEditor>(TitleName());
			window.SetBTAssetDebug(btAsset, btInstance, false);
		}

		[UnityEditor.Callbacks.OnOpenAsset(0)]
		private static bool OnOpenBTAsset(int instanceID, int line)
		{
			BTAsset asset = EditorUtility.InstanceIDToObject(instanceID) as BTAsset;
			if(asset != null)
			{
				if (EditorApplication.isPlaying)
				{
					BehaviourTree tree = BTDebugHelper.FindTree(asset.TreeUidString);
					if (tree != null)
					{
						BTDebugHelper.CurrentDebugRootTree = tree;
						OpenDebug(asset, tree);
						return true;
					}
				}

				Open(asset);

				return true;
			}

			return false;
		}


		// Debugging behaviours of selected actor.
		private void SetupBTDebugging()
		{
			if (Selection.activeObject == null || (Selection.activeObject as GameObject) == null)
				return;

			/*Actor actor = (Selection.activeObject as GameObject).GetComponent<Actor>();
			if (actor == null)
				return;

			if (actor.controller == null)
				return;

			BTDebugHelper.DebugContext = actor.controller.GetComponent<BevTreeComponent>().context;

			BTDebugHelper.ClearHotTree();

			int count = actor.controller.GetComponent<BevTreeComponent>().GetTreeCount();
			for (int i = 0; i < count; ++i)
			{
				BehaviourTree t = actor.controller.GetComponent<BevTreeComponent>().GetTree(i);
				BTDebugHelper.AddHotTree(t);
			}

			// If m_btAsset is one of the runtime trees of the actor, 
			// show the runtime tree in the editor.
			BehaviourTree tree = BTDebugHelper.FindTree(m_btAsset.TreeUidString);
			if (tree != null)
			{
				BTDebugHelper.CurrentDebugRootTree = tree;
				OpenDebug(m_btAsset, tree);
			}*/

		}


	}
}