
using System.Collections.Generic;


namespace BevTree
{

	public class BevBlackboard
	{
		private Dictionary<long, Dictionary<string, int>> m_treeIntDic;
		private Dictionary<long, Dictionary<string, long>> m_treeLongDic;
		private Dictionary<long, Dictionary<string, float>> m_treeFloatDic;
		private Dictionary<long, Dictionary<string, bool>> m_treeBoolDic;
		private Dictionary<long, Dictionary<string, string>> m_treeStringDic;

		private Dictionary<long, Dictionary<long, Dictionary<string, int>>> m_nodeIntDic;
		private Dictionary<long, Dictionary<long, Dictionary<string, long>>> m_nodeLongDic;
		private Dictionary<long, Dictionary<long, Dictionary<string, float>>> m_nodeFloatDic;
		private Dictionary<long, Dictionary<long, Dictionary<string, bool>>> m_nodeBoolDic;
		private Dictionary<long, Dictionary<long, Dictionary<string, string>>> m_nodeStringDic;

		public BevBlackboard()
		{
			m_treeIntDic = new Dictionary<long, Dictionary<string, int>>();
			m_treeLongDic = new Dictionary<long, Dictionary<string, long>>();
			m_treeFloatDic = new Dictionary<long, Dictionary<string, float>>();
			m_treeBoolDic = new Dictionary<long, Dictionary<string, bool>>();
			m_treeStringDic = new Dictionary<long, Dictionary<string, string>>();
			m_nodeIntDic = new Dictionary<long, Dictionary<long, Dictionary<string, int>>>();
			m_nodeLongDic = new Dictionary<long, Dictionary<long, Dictionary<string, long>>>();
			m_nodeFloatDic = new Dictionary<long, Dictionary<long, Dictionary<string, float>>>();
			m_nodeBoolDic = new Dictionary<long, Dictionary<long, Dictionary<string, bool>>>();
			m_nodeStringDic = new Dictionary<long, Dictionary<long, Dictionary<string, string>>>();

		}

		public int GetInt(long treeId, string key)
		{
			int i = 0;
			Dictionary<string, int> dic;
			m_treeIntDic.TryGetValue(treeId, out dic);
			if (dic == null)
				return i;
			else
				dic.TryGetValue(key, out i);
			return i;
		}

		public void SetInt(long treeId, string key, int value)
		{
			_setScopeIntValue(ref m_treeIntDic, treeId, key, value);
		}

		public int GetInt(long treeId, long nodeId, string key)
		{
			int i = 0;
			Dictionary<long, Dictionary<string, int>> nodeDic;
			m_nodeIntDic.TryGetValue(treeId, out nodeDic);
			if (nodeDic != null)
			{
				Dictionary<string, int> dic;
				nodeDic.TryGetValue(nodeId, out dic);
				if (dic != null)
				{
					dic.TryGetValue(key, out i);
				}
			}
			return i;
		}

		public void SetInt(long treeId, long nodeId, string key, int value)
		{
			Dictionary<long, Dictionary<string, int>> sdic;
			m_nodeIntDic.TryGetValue(treeId, out sdic);
			if (sdic == null)
			{
				sdic = new Dictionary<long, Dictionary<string, int>>();
				m_nodeIntDic.Add(treeId, sdic);
			}
			_setScopeIntValue(ref sdic, nodeId, key, value);
		}

		public long GetLong(long treeId, string key)
		{
			long i = 0;
			Dictionary<string, long> dic;
			m_treeLongDic.TryGetValue(treeId, out dic);
			if (dic == null)
				return i;
			else
				dic.TryGetValue(key, out i);
			return i;
		}

		public void SetLong(long treeId, string key, long value)
		{
			_setScopeLongValue(ref m_treeLongDic, treeId, key, value);
		}

		public long GetLong(long treeId, long nodeId, string key)
		{
			long i = 0;
			Dictionary<long, Dictionary<string, long>> nodeDic;
			m_nodeLongDic.TryGetValue(treeId, out nodeDic);
			if (nodeDic != null)
			{
				Dictionary<string, long> dic;
				nodeDic.TryGetValue(nodeId, out dic);
				if (dic != null)
				{
					dic.TryGetValue(key, out i);
				}
			}
			return i;
		}

		public void SetLong(long treeId, long nodeId, string key, long value)
		{
			Dictionary<long, Dictionary<string, long>> sdic;
			m_nodeLongDic.TryGetValue(treeId, out sdic);
			if (sdic == null)
			{
				sdic = new Dictionary<long, Dictionary<string, long>>();
				m_nodeLongDic.Add(treeId, sdic);
			}
			_setScopeLongValue(ref sdic, nodeId, key, value);
		}

		public float GetFloat(long treeId, string key)
		{
			int i = 0;
			Dictionary<string, int> dic;
			m_treeIntDic.TryGetValue(treeId, out dic);
			if (dic == null)
				return i;
			else
				dic.TryGetValue(key, out i);
			return i;
		}

		public void SetFloat(long treeId, string key, float value)
		{
			_setScopeFloatValue(ref m_treeFloatDic, treeId, key, value);
		}

		public float GetFloat(long treeId, long nodeId, string key)
		{
			float i = 0;
			Dictionary<long, Dictionary<string, float>> nodeDic;
			m_nodeFloatDic.TryGetValue(treeId, out nodeDic);
			if (nodeDic != null)
			{
				Dictionary<string, float> dic;
				nodeDic.TryGetValue(nodeId, out dic);
				if (dic != null)
				{
					dic.TryGetValue(key, out i);
				}
			}
			return i;
		}

		public void SetFloat(long treeId, long nodeId, string key, float value)
		{
			Dictionary<long, Dictionary<string, float>> sdic;
			if (m_nodeFloatDic.ContainsKey(treeId))
			{
				sdic = m_nodeFloatDic[treeId];
			}
			else
			{
				sdic = new Dictionary<long, Dictionary<string, float>>();
				m_nodeFloatDic.Add(treeId, sdic);
			}
			_setScopeFloatValue(ref sdic, nodeId, key, value);
		}

		public bool GetBool(long treeId, string key)
		{
			bool i = false;
			Dictionary<string, bool> dic;
			m_treeBoolDic.TryGetValue(treeId, out dic);
			if (dic == null)
				return i;
			else
				dic.TryGetValue(key, out i);
			return i;
		}

		public void SetBool(long treeId, string key, bool value)
		{
			_setScopeBoolValue(ref m_treeBoolDic, treeId, key, value);
		}

		public bool GetBool(long treeId, long nodeId, string key)
		{
			bool i = false;
			Dictionary<long, Dictionary<string, bool>> nodeDic;
			m_nodeBoolDic.TryGetValue(treeId, out nodeDic);
			if (nodeDic != null)
			{
				Dictionary<string, bool> dic;
				nodeDic.TryGetValue(nodeId, out dic);
				if (dic != null)
				{
					dic.TryGetValue(key, out i);
				}
			}
			return i;
		}

		public void SetBool(long treeId, long nodeId, string key, bool value)
		{
			Dictionary<long, Dictionary<string, bool>> sdic;
			if (m_nodeBoolDic.ContainsKey(treeId))
			{
				sdic = m_nodeBoolDic[treeId];
			}
			else
			{
				sdic = new Dictionary<long, Dictionary<string, bool>>();
				m_nodeBoolDic.Add(treeId, sdic);
			}
			_setScopeBoolValue(ref sdic, nodeId, key, value);
		}

		public string GetString(long treeId, string key)
		{
			string i = "";
			Dictionary<string, string> dic;
			m_treeStringDic.TryGetValue(treeId, out dic);
			if (dic == null)
				return i;
			else
				dic.TryGetValue(key, out i);
			return i;
		}

		public void SetString(long treeId, string key, string value)
		{
			_setScopeStringValue(ref m_treeStringDic, treeId, key, value);
		}

		public string GetString(long treeId, long nodeId, string key)
		{
			string i = "";
			Dictionary<long, Dictionary<string, string>> nodeDic;
			m_nodeStringDic.TryGetValue(treeId, out nodeDic);
			if (nodeDic != null)
			{
				Dictionary<string, string> dic;
				nodeDic.TryGetValue(nodeId, out dic);
				if (dic != null)
				{
					dic.TryGetValue(key, out i);
				}
			}
			return i;
		}

		public void SetString(long treeId, long nodeId, string key, string value)
		{
			Dictionary<long, Dictionary<string, string>> sdic;
			if (m_nodeStringDic.ContainsKey(treeId))
			{
				sdic = m_nodeStringDic[treeId];
			}
			else
			{
				sdic = new Dictionary<long, Dictionary<string, string>>();
				m_nodeStringDic.Add(treeId, sdic);
			}
			_setScopeStringValue(ref sdic, nodeId, key, value);
		}

		private void _setScopeIntValue(ref Dictionary<long, Dictionary<string, int>> sdic, long scope, string key, int value)
		{
			Dictionary<string, int> dic;
			if (sdic.ContainsKey(scope))
			{
				dic = sdic[scope];
			}
			else
			{
				dic = new Dictionary<string, int>();
				sdic.Add(scope, dic);
			}
			if (dic.ContainsKey(key))
				dic[key] = value;
			else
				dic.Add(key, value);
		}

		private void _setScopeLongValue(ref Dictionary<long, Dictionary<string, long>> sdic, long scope, string key, long value)
		{
			Dictionary<string, long> dic;
			if (sdic.ContainsKey(scope))
			{
				dic = sdic[scope];
			}
			else
			{
				dic = new Dictionary<string, long>();
				sdic.Add(scope, dic);
			}
			if (dic.ContainsKey(key))
				dic[key] = value;
			else
				dic.Add(key, value);
		}

		private void _setScopeFloatValue(ref Dictionary<long, Dictionary<string, float>> sdic, long scope, string key, float value)
		{
			Dictionary<string, float> dic;
			if (sdic.ContainsKey(scope))
			{
				dic = sdic[scope];
			}
			else
			{
				dic = new Dictionary<string, float>();
				sdic.Add(scope, dic);
			}
			if (dic.ContainsKey(key))
				dic[key] = value;
			else
				dic.Add(key, value);
		}

		private void _setScopeBoolValue(ref Dictionary<long, Dictionary<string, bool>> sdic, long scope, string key, bool value)
		{
			Dictionary<string, bool> dic;
			if (sdic.ContainsKey(scope))
			{
				dic = sdic[scope];
			}
			else
			{
				dic = new Dictionary<string, bool>();
				sdic.Add(scope, dic);
			}
			if (dic.ContainsKey(key))
				dic[key] = value;
			else
				dic.Add(key, value);
		}

		private void _setScopeStringValue(ref Dictionary<long, Dictionary<string, string>> sdic, long scope, string key, string value)
		{
			Dictionary<string, string> dic;
			if (sdic.ContainsKey(scope))
			{
				dic = sdic[scope];
			}
			else
			{
				dic = new Dictionary<string, string>();
				sdic.Add(scope, dic);
			}
			if (dic.ContainsKey(key))
				dic[key] = value;
			else
				dic.Add(key, value);
		}

	}


}