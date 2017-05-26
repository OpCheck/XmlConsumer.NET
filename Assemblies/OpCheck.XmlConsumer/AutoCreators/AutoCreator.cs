namespace OpCheck.XmlConsumer.AutoCreators
{
	public abstract class AutoCreator
	{
		/// <summary>
		/// Applies the auto creation logic and returns the target member object.
		/// </summary>
		public abstract object ApplyAutoCreateLogic ();
	
	
		public object TargetObject
		{
			set
			{
				_TargetObject = value;
			}
		}


		public string MemberIdentifier
		{
			set
			{
				_MemberIdentifier = value;
			}
		}


		public int MemberSize
		{
			set
			{
				_MemberSize = value;
			}
		}


		public Collectivity Collectivity
		{
			set
			{
				_Collectivity = value;
			}
		}
		
		
		protected object _TargetObject;
		protected string _MemberIdentifier;
		protected int _MemberSize;
		protected Collectivity _Collectivity;
	}
}
