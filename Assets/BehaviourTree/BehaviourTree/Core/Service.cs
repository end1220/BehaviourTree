
using BevTree.Serialization;

namespace BevTree
{
	public abstract class Service
	{
		[BTProperty("IsExpanded")]
		[BTHideInInspector]
		private bool m_isExpanded = true;

		[BTIgnore]
		public virtual string Title
		{
			get { return GetType().Name; }
		}

		[BTIgnore]
		public bool IsExpanded
		{
			get
			{
				return m_isExpanded;
			}
			set
			{
				m_isExpanded = value;
			}
		}

		public virtual void OnBeforeSerialize(BTAsset btAsset) { }
		public virtual void OnAfterDeserialize(BTAsset btAsset) { }

		public virtual void OnOpen(Context context) { }
		public virtual void OnClose(Context context) { }
		public abstract void OnExecute(Context context);
	}


}