
namespace BevTree
{
	[AddNodeMenu("Composite/Parallel")]
	public class Parallel : Composite
	{

		private bool m_failOnAny;
		private bool m_succeedOnAny;
		private bool m_failOnTie;

		public bool FailOnAny
		{
			get
			{
				return m_failOnAny;
			}
			set
			{
				m_failOnAny = value;
			}
		}

		public bool SucceedOnAny
		{
			get
			{
				return m_succeedOnAny;
			}
			set
			{
				m_succeedOnAny = value;
			}
		}

		public bool FailOnTie
		{
			get
			{
				return m_failOnTie;
			}
			set
			{
				m_failOnTie = value;
			}
		}


		public Parallel()
		{

		}


		public Parallel(params BehaviourNode[] nodes) :
			base(nodes)
		{
			m_failOnAny = true;
			m_succeedOnAny = false;
			m_failOnTie = true;
		}
		

		protected override RunningStatus OnTick(Context context)
		{
			/*for (int i = 0; i < m_children.Count; ++i)
			{
				RunningStatus ret = m_children[i]._tick(context);
				if (ret != RunningStatus.Running)
					return ret;
			}

			return RunningStatus.Running;*/
			RunningStatus status = RunningStatus.Success;
			int numberOfFailures = 0;
			int numberOfSuccesses = 0;
			int numberOfRunningChildren = 0;

			if (m_children.Count > 0)
			{
				for (int i = 0; i < m_children.Count; i++)
				{
					RunningStatus childStatus = (RunningStatus)context.blackboard.GetInt(context.tree.guid, m_children[i].guid, "Status");
					if (childStatus == RunningStatus.None || childStatus == RunningStatus.Running)
					{
						childStatus = m_children[i]._tick(context);
					}

					if (childStatus == RunningStatus.Success)
						numberOfSuccesses++;
					else if (childStatus == RunningStatus.Failure)
						numberOfFailures++;
					else if (childStatus == RunningStatus.Running)
						numberOfRunningChildren++;
				}

				if ((m_failOnAny && numberOfFailures > 0) || (m_succeedOnAny && numberOfSuccesses > 0))
				{
					if (m_failOnTie)
					{
						if (m_failOnAny && numberOfFailures > 0)
							status = RunningStatus.Failure;
						else if (m_succeedOnAny && numberOfSuccesses > 0)
							status = RunningStatus.Success;
					}
					else
					{
						if (m_succeedOnAny && numberOfSuccesses > 0)
							status = RunningStatus.Success;
						else if (m_failOnAny && numberOfFailures > 0)
							status = RunningStatus.Failure;
					}
				}
				else
				{
					if (numberOfSuccesses == m_children.Count)
						status = RunningStatus.Success;
					else if (numberOfFailures == m_children.Count)
						status = RunningStatus.Failure;
					else
						status = RunningStatus.Running;
				}
			}

			return status;
		}


	}

}

