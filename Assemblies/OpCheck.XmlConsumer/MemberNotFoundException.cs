using System;

namespace OpCheck.XmlConsumer
{
	public class MemberNotFoundException : Exception
	{
		public MemberNotFoundException () : base()
		{
		}


		public override string Message
		{
			get
			{
				return String.Format("No member with name '{0}' was found on target type '{1}'.", MemberName, TargetObjectType);
			}
		}
	
		public string MemberName;
		public Type TargetObjectType;
	}
}
