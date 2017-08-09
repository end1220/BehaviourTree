using UnityEngine;
using UnityEditor;
using BevTree;

namespace BevTreeEditor
{
	[CustomNodeInspector(typeof(RunBehaviourIndex))]
	public class RunBehaviourIndexInspector : NodeInspector
	{
		protected override void DrawProperties()
		{
			RunBehaviourIndex target = (RunBehaviourIndex)Target;
			bool prevGUIState = GUI.enabled;

			target.SubTreeIndex = EditorGUILayout.IntField(target.SubTreeIndex);
			EditorGUILayout.Space();

			BTAsset btAsset = BehaviourTreeEditor.GetIndexedSubTreeAsset(target.SubTreeIndex);
			EditorGUILayout.ObjectField("Behaviour Tree", btAsset, typeof(BTAsset), false);
			EditorGUILayout.Space();

			if(BTEditorCanvas.Current.IsDebuging && btAsset != null && target.BehaviourTree != null)
			{
				GUI.enabled = true;
				if(GUILayout.Button("Preview", GUILayout.Height(24.0f)))
				{
					BehaviourTreeEditor.OpenSubtreeDebug(btAsset, target.BehaviourTree);
				}
			}
			else
			{
				GUI.enabled = btAsset != null;
				if(GUILayout.Button("Open", GUILayout.Height(24.0f)))
				{
					BehaviourTreeEditor.OpenSubtree(btAsset);
				}
			}
				
			GUI.enabled = prevGUIState;
		}
	}
}