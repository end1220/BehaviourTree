using UnityEngine;
using UnityEditor;
using BevTree;

namespace BevTreeEditor
{
	[CustomEditor(typeof(RootTreeAsset))]
	public class RootTreeAssetInspector : Editor
	{
		private RootTreeAsset m_asset;

		private void OnEnable()
		{
			m_asset = target as RootTreeAsset;
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField("Title");
			m_asset.title = EditorGUILayout.TextArea(m_asset.title, EditorStyles.textField);
			EditorGUILayout.LabelField("Comment");
			m_asset.description = EditorGUILayout.TextArea(m_asset.description, EditorStyles.textField);
			
// 			if(GUILayout.Button("Open In Editor", GUILayout.Height(24.0f)))
// 			{
// 				BehaviourTreeEditor.Open(target as BTAsset);
// 			}

			EditorGUIUtility.LookLikeInspector();
			SerializedProperty tps = serializedObject.FindProperty("indexedSubTrees");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(tps, true);
			if (EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();
			EditorGUIUtility.LookLikeControls();

		}
	}
}