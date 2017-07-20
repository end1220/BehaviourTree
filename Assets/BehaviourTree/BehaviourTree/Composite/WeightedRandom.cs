using UnityEngine;
using System;
using System.Collections;

namespace BevTree
{
	[AddNodeMenu("Composite/WeightedRandom")]
	public class WeightedRandom : Random
	{
		private float[] m_weights;


		protected override void OnInit()
		{
			m_weights = new float[m_children.Count];

			for (int i = 0; i < m_children.Count; i++)
			{
				m_children[i]._init();
				m_weights[i] = m_children[i].Weight;
			}
		}


		protected override void OnOpen(Context context)
		{
			int nodeIndex = ChooseRandomChild();
			context.blackboard.SetInt(context.tree.guid, this.guid, "randomIndex", nodeIndex);
		}

		private int ChooseRandomChild()
		{
			int index = -1;

			float rand = UnityEngine.Random.value;
			for(int i = 0; i < m_children.Count; i++)
			{
				if (rand < m_children[i].Weight)
				{
					index = i;
					break;
				}

				rand -= m_children[i].Weight;
			}

			return index;
		}

	}

}