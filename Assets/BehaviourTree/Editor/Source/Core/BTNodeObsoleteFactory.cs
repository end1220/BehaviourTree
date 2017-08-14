using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using BevTree;

namespace BevTreeEditor
{
	public static class BTNodeObsoleteFactory
	{
		private static List<Tuple<Type, string>> m_nodeReferences;


		static BTNodeObsoleteFactory()
		{
			Assembly assembly = typeof(BehaviourNode).Assembly;

			m_nodeReferences = new List<Tuple<Type, string>>();
			foreach(Type type in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(BehaviourNode))))
			{
				object[] attributes = type.GetCustomAttributes(typeof(ObsoleteNodeAttribute), false);
				if(attributes.Length > 0)
				{
					ObsoleteNodeAttribute attribute = attributes[0] as ObsoleteNodeAttribute;
					m_nodeReferences.Add(new Tuple<Type, string>(type, attribute.tip));
				}
			}
			foreach (Type type in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Constraint))))
			{
				object[] attributes = type.GetCustomAttributes(typeof(ObsoleteNodeAttribute), false);
				if (attributes.Length > 0)
				{
					ObsoleteNodeAttribute attribute = attributes[0] as ObsoleteNodeAttribute;
					m_nodeReferences.Add(new Tuple<Type, string>(type, attribute.tip));
				}
			}
		}


		public static bool IsObsolete(System.Object node)
		{
			foreach (var item in m_nodeReferences)
				if (item.Item1.IsInstanceOfType(node))
					return true;
			return false;
		}


		public static string GetTipString(System.Object node)
		{
			foreach (var item in m_nodeReferences)
				if (item.Item1.IsInstanceOfType(node))
					return item.Item2;
			return "";
		}


	}

}