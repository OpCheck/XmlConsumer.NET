namespace OpCheck.XmlConsumer.AutoCreators
{
	/// <summary>
	/// Always creates a new instance of the member on the target object.
	/// </summary>
	public class AlwaysAutoCreator : AutoCreator
	{
		public override object ApplyAutoCreateLogic ()
		{
			//
			// CREATE AN INSTANCE OF THE TARGET MEMBER AND RETURN IT.
			//
			ObjectLoader CreatedLoader = new ObjectLoader();
			CreatedLoader.TargetObject = _TargetObject;
			CreatedLoader.Init();
			return CreatedLoader.CreateInstanceOfMemberByName(_MemberIdentifier);
		}
	}
}
