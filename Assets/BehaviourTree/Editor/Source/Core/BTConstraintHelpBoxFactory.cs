using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using BevTree;

namespace BevTreeEditor
{
	public static class BTConstraintHelpBoxFactory
	{
		private static List<Tuple<Type, string>> m_constraintReferences;

		static BTConstraintHelpBoxFactory()
		{
			Type constraintType = typeof(Constraint);
			Assembly assembly = constraintType.Assembly;

			m_constraintReferences = new List<Tuple<Type, string>>();
			foreach(Type type in assembly.GetTypes().Where(t => t.IsSubclassOf(constraintType)))
			{
				object[] attributes = type.GetCustomAttributes(typeof(ConstraintHelpBoxAttribute), false);
				if(attributes.Length > 0)
				{
					ConstraintHelpBoxAttribute attribute = attributes[0] as ConstraintHelpBoxAttribute;
					m_constraintReferences.Add(new Tuple<Type, string>(type, attribute.Reference));
				}
			}
		}

		public static string GetHelpString(Constraint constraint)
		{
			foreach (var item in m_constraintReferences)
			{
				if (item.Item1.IsInstanceOfType(constraint))
				{
					return item.Item2;
				}
			}
			return "";// constraint.GetType().Name;
		}


	}

}