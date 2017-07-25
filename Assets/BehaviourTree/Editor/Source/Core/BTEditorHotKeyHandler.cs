using UnityEngine;
using UnityEngine.Events;
using UnityEditor;


namespace BevTreeEditor
{
	/// <summary>
	/// "Copy", "Cut", "Paste", "Delete", "SoftDelete", "Duplicate", "FrameSelected", "FrameSelectedWithLock", "SelectAll", "Find" and "FocusProjectWindow".
	/// </summary>
	/// 
	public class BTEditorHotKeyHandler
	{
		private BTEditorGraph m_graph;

		private bool bCtrlHold = false;


		public BTEditorHotKeyHandler(BTEditorGraph graph)
		{
			m_graph = graph;
		}


		public void HandlerEvents()
		{
			Event evt = BTEditorCanvas.Current.Event;
			if (evt.type == EventType.ValidateCommand)
			{
				if (evt.commandName == "Save")
					OnSave();
				else if (evt.commandName == "Copy")
					OnCopyNode();
				else if (evt.commandName == "Cut")
					OnCutNode();
				else if (evt.commandName == "Paste")
					OnPasteNode();
				else if (evt.commandName == "Delete")
					OnDeleteNode();
				else if (evt.commandName == "SelectAll")
					OnSelectAll();
				else if (evt.commandName == "Duplicate")
					OnDuplicate();
				else if (evt.commandName == "UndoRedoPerformed")
					OnUndoRedoPerformed();
			}

			if (evt.type == EventType.KeyDown)
			{
				if (evt.keyCode == KeyCode.LeftControl)
					bCtrlHold = true;
			}
			else if (evt.type == EventType.KeyUp)
			{
				if (evt.keyCode == KeyCode.LeftControl)
					bCtrlHold = false;
				else if (evt.keyCode == KeyCode.Delete)
					OnDeleteNode();
			}
		}


		private void OnSave()
		{
			Debug.LogError("Save");
		}


		private void OnCopyNode()
		{
			BTEditorGraphNode targetNode = m_graph.GetLastSelectedNode();
			if (targetNode != null)
				m_graph.OnCopyNode(targetNode);
		}

	
		private void OnCutNode()
		{
			BTEditorGraphNode targetNode = m_graph.GetLastSelectedNode();
			if (targetNode != null)
			{
				m_graph.OnCopyNode(targetNode);
				m_graph.OnNodeDelete(targetNode);
			}
		}


		private void OnPasteNode()
		{
			BTEditorGraphNode targetNode = m_graph.GetLastSelectedNode();
			if (targetNode != null)
				m_graph.OnPasteNode(targetNode);
		}


		private void OnDeleteNode()
		{
			BTEditorGraphNode targetNode = m_graph.GetLastSelectedNode();
			if (targetNode != null)
				m_graph.OnNodeDelete(targetNode);
		}


		private void OnSelectAll()
		{

		}


		private void OnDuplicate()
		{

		}


		private void OnUndoRedoPerformed()
		{

		}


	}
}