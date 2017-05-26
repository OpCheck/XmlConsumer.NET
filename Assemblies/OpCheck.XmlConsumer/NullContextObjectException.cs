using System;

namespace OpCheck.XmlConsumer
{
	public class NullContextObjectException : Exception
	{
		public override string Message
		{
			get
			{
				return String.Format("Null context object detected.  Member name: '{0}'.", _MemberIdentifier);
			}
		}
	
	
		public string MemberIdentifier
		{
			set
			{	
				_MemberIdentifier = value;
			}
		}
		
		
		private string _MemberIdentifier;
	}
}
