namespace OpCheck.XmlConsumer
{
	public class NullMappingCommand
	{
		public void Execute ()
		{
			//
			// THIS IS A NULL MAPPING STATEMENT.
			// JUST GET THE MEMBER AND ASSIGN IT A NULL VALUE.
			//
			ObjectLoader CreatedLoader = new ObjectLoader();
			CreatedLoader.TargetObject = _ContextObject;
			CreatedLoader.Init();
			CreatedLoader.SetMemberValue(_MemberIdentifier, null);
		}
		
	
		public string MemberIdentifier
		{
			set
			{
				_MemberIdentifier = value;
			}
		}


		public object ContextObject
		{
			set
			{
				_ContextObject = value;
			}
		}
		
		
		private string _MemberIdentifier;
		private object _ContextObject;
	}
}
