using UnityEngine;
using UnityEditor;
using BevTree;

namespace BevTreeEditor
{
	[CustomEditor(typeof(BTAsset))]
	public class BTAssetInspector : Editor
	{
		private BTAsset m_asset;

		private void OnEnable()
		{
			m_asset = target as BTAsset;
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField("Title");
			m_asset.title = EditorGUILayout.TextArea(m_asset.title, EditorStyles.textField);
			EditorGUILayout.LabelField("Comment");
			m_asset.description = EditorGUILayout.TextArea(m_asset.description, EditorStyles.textField);
			
			if(GUILayout.Button("Open In Editor", GUILayout.Height(24.0f)))
			{
				BehaviourTreeEditor.Open(target as BTAsset);
			}

		}
	}
}