namespace OpCheck.XmlConsumer.AutoCreators
{
	public class NeverAutoCreator : AutoCreator
	{
		public override object ApplyAutoCreateLogic ()
		{
			//
			// CREATE THE OBJECT LOADER, GET THE TARGET MEMBER, AND RETURN IT.
			// WE DON'T DO ANYTHING HERE.
			//
			ObjectLoader CreatedLoader = new ObjectLoader();
			CreatedLoader.TargetObject = _TargetObject;
			CreatedLoader.Init();
			return CreatedLoader.GetMemberObjectByName(_MemberIdentifier);
		}
	}
}
