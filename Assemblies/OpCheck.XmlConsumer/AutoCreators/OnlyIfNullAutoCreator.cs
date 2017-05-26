using System;

namespace OpCheck.XmlConsumer.AutoCreators
{
	public class OnlyIfNullAutoCreator : AutoCreator
	{
		public override object ApplyAutoCreateLogic ()
		{
			//
			// CREATE THE OBJECT LOADER AND GET THE TARGET MEMBER.
			//
			ObjectLoader CreatedLoader = new ObjectLoader();
			CreatedLoader.TargetObject = _TargetObject;
			CreatedLoader.Init();
			object TargetMember = CreatedLoader.GetMemberObjectByName(_MemberIdentifier);

			if (TargetMember != null)
				return TargetMember;

			//
			// THE TARGET MEMBER IS NULL SO WE NEED TO CREATE IT AND RETURN IT.
			//
			
			//
			// GET THE TYPE OF THE TARGET MEMBER.
			//
			ObjectReflector CreatedReflector = new ObjectReflector();
			CreatedReflector.TargetObject = _TargetObject;
			CreatedReflector.Init();
			Type TargetMemberType = CreatedReflector.GetMemberTypeByName(_MemberIdentifier);
			
			//
			// CREATE AN INSTANCE OF THE TARGET MEMBER.
			//
			ObjectCreator CreatedCreator = new ObjectCreator();
			CreatedCreator.TargetType = TargetMemberType;
			CreatedCreator.Size = _MemberSize;
			CreatedCreator.Collectivity = _Collectivity;
			object CreatedTargetMember = CreatedCreator.CreateObject();
			
			//
			// SET THE CREATE TARGET MEMBER.
			//
			CreatedLoader.SetMemberValue(_MemberIdentifier, CreatedTargetMember);
			
			//
			// RETURN THE CREATED TARGET MEMBER.
			//
			return CreatedTargetMember;
		}
	}
}
