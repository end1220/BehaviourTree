
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using BevTree;


namespace BevTreeEditor
{
	public static class BTNodeHelpBoxFactory
	{
		private static List<Tuple<Type, string>> m_nodeReferences;

		static BTNodeHelpBoxFactory()
		{
			Type nodeType = typeof(BehaviourNode);
			Assembly assembly = nodeType.Assembly;

			m_nodeReferences = new List<Tuple<Type, string>>();
			foreach(Type type in assembly.GetTypes().Where(t => t.IsSubclassOf(nodeType)))
			{
				object[] attributes = type.GetCustomAttributes(typeof(NodeHelpBoxAttribute), false);
				if(attributes.Length > 0)
				{
					NodeHelpBoxAttribute attribute = attributes[0] as NodeHelpBoxAttribute;
					m_nodeReferences.Add(new Tuple<Type, string>(type, attribute.Reference));
				}
			}
			foreach (Type type in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Constraint))))
			{
				object[] attributes = type.GetCustomAttributes(typeof(NodeHelpBoxAttribute), false);
				if (attributes.Length > 0)
				{
					NodeHelpBoxAttribute attribute = attributes[0] as NodeHelpBoxAttribute;
					m_nodeReferences.Add(new Tuple<Type, string>(type, attribute.Reference));
				}
			}
		}

		public static string GetHelpString(System.Object node)
		{
			foreach (var item in m_nodeReferences)
			{
				if (item.Item1.IsInstanceOfType(node))
				{
					return item.Item2;
				}
			}
			return "";
		}


	}

}