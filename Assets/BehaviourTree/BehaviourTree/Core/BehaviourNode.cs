
using System.Collections.Generic;
using UnityEngine;
using BevTree.Serialization;


namespace BevTree
{


	public abstract class BehaviourNode
	{
		[BTProperty("__UniqueID")]
		private long m_guid;

		[BTIgnore]
		public BehaviourNode parent;

		private Breakpoint m_breakpoint;

		private Vector2 m_position;
		private string m_comment;
		private float m_weight;


		[BTProperty("Constraints")]
		private List<Constraint> m_constraints;
		[BTHideInInspector]
		private bool m_isConstraintExpanded = true;
		[BTProperty("Services")]
		private List<Service> m_services;


		[BTHideInInspector]
		public Vector2 Position
		{
			get
			{
				return m_position;
			}
			set
			{
				m_position = value;
			}
		}

		[BTHideInInspector]
		public string Comment
		{
			get
			{
				return m_comment;
			}
			set
			{
				m_comment = value;
			}
		}

		[BTIgnore]
		public bool IsConstraintsExpanded
		{
			get
			{
				return m_isConstraintExpanded;
			}
			set
			{
				m_isConstraintExpanded = value;
			}
		}

		[BTHideInInspector]
		public float Weight
		{
			get
			{
				return m_weight;
			}
			set
			{
				m_weight = value;
			}
		}

		[BTIgnore]
		public virtual string Title
		{
			get { return GetType().Name; }
		}

		[BTHideInInspector]
		public Breakpoint Breakpoint
		{
			get
			{
				return m_breakpoint;
			}
			set
			{
				m_breakpoint = value;
			}
		}

		[BTIgnore]
		public long guid
		{
			get { return m_guid; }
			set { m_guid = value; }
		}


#if UNITY_EDITOR
		/// <summary>
		/// This node's list of Constraints. For use only in the editor code. DO NOT USE IN RUNTIME CODE!!!
		/// </summary>
		[BTIgnore]
		public List<Constraint> Constraints
		{
			get { return m_constraints; }
		}

		/// <summary>
		/// This node's list of Services. For use only in the editor code. DO NOT USE IN RUNTIME CODE!!!
		/// </summary>
		[BTIgnore]
		public List<Service> Services
		{
			get { return m_services; }
		}
#endif


		public BehaviourNode()
		{
			//m_guid = GuidGen.GenUniqueGUID();
			m_breakpoint = Breakpoint.None;
			m_constraints = new List<Constraint>();
			m_services = new List<Service>();
		}


		protected virtual void OnInit() { }

		protected virtual void OnOpen(Context context) { }

		protected virtual void OnClose(Context context) { }

		protected virtual void OnEnter(Context context) { }

		protected virtual void OnExit(Context context) { }

		protected abstract RunningStatus OnTick(Context context);


		public void _init()
		{
			OnInit();
		}


		public void _enter(Context context)
		{
			OnEnter(context);

/*#if UNITY_EDITOR
			if (BTDebugHelper.BreakPointEnabled && m_breakpoint.Has(Breakpoint.OnEnter))
				Debug.Break();
#endif*/
		}


		public void _exit(Context context)
		{
			OnExit(context);

/*#if UNITY_EDITOR
			if (BTDebugHelper.BreakPointEnabled && m_breakpoint.Has(Breakpoint.OnExit))
				Debug.Break();
#endif*/
		}


		public void _open(Context context)
		{
			context.blackboard.SetBool(context.tree.guid, guid, "isOpen", true);
			OnOpen(context);

#if UNITY_EDITOR
			if (BTDebugHelper.BreakPointEnabled && m_breakpoint.Has(Breakpoint.OnOpen))
				Debug.Break();
#endif
		}


		public void _close(Context context)
		{
			context.blackboard.SetBool(context.tree.guid, guid, "isOpen", false);
			OnClose(context);

#if UNITY_EDITOR
			if (BTDebugHelper.BreakPointEnabled && m_breakpoint.Has(Breakpoint.OnClose))
				Debug.Break();
#endif
		}


		public RunningStatus _tick(Context context)
		{
			context._travelNodes[context.tree.guid].Push(this);

			RunningStatus ret = RunningStatus.Running;

			bool constraintsTrue = CheckConstraints(context);
			if (!constraintsTrue)
			{
				ret = RunningStatus.Failure;
			}
			else
			{
				if (!context.blackboard.GetBool(context.tree.guid, guid, "isOpen"))
				{
					_open(context);
				}

				context._openNodes[context.tree.guid].Push(this);

				_enter(context);

				ret = OnTick(context);

				_exit(context);

				if (ret != RunningStatus.Running)
				{
					context._openNodes[context.tree.guid].Pop();
					_close(context);

#if UNITY_EDITOR
					if (BTDebugHelper.BreakPointEnabled)
					{
						if ((ret == RunningStatus.Success && m_breakpoint.Has(Breakpoint.OnSuccess))
							|| (ret == RunningStatus.Failure && m_breakpoint.Has(Breakpoint.OnFailure)))
							Debug.Break();
					}
#endif
				}
			}

			context.blackboard.SetInt(context.tree.guid, guid, "Status", (int)ret);

			return ret;
		}


		private bool CheckConstraints(Context context)
		{
			int bits = 0;
			bool ret = true;
			for (int i = 0; i < m_constraints.Count; ++i)
			{
				bool r = m_constraints[i].OnExecute(context);
				bits = bits | (r ? 1<<i : 0<<i);
				if (!r)
				{
					ret = false;
					break;
				}
			}
			context.blackboard.SetInt(context.tree.guid, this.guid, "Constraints", bits);
			return ret;
		}


		public virtual void OnBeforeSerialize(BTAsset btAsset)
		{
			guid = BTUtils.GenUniqueGUID();

			foreach (var constraint in m_constraints)
				constraint.OnBeforeSerialize(btAsset);
			foreach (var service in m_services)
				service.OnBeforeSerialize(btAsset);
		}


		public virtual void OnAfterDeserialize(BTAsset btAsset)
		{
			if (m_constraints == null)
				m_constraints = new List<Constraint>();
			if (m_services == null)
				m_services = new List<Service>();

			foreach (var constraint in m_constraints)
				constraint.OnAfterDeserialize(btAsset);
			foreach (var service in m_services)
				service.OnAfterDeserialize(btAsset);
		}


	}


}