using System;

namespace OpCheck.XmlConsumer
{
	public class TypeAnalyzer
	{
		public Type TargetType
		{
			set
			{
				_TargetType = value;
			}
		}


		public Type GetMemberTypeByName (string MemberName)
		{
			if (HasFieldWithName(MemberName))
				return GetFieldTypeByName(MemberName);
				
			if (HasSetterWithName(MemberName))
				return GetPropertyTypeByName(MemberName);
			
			//
			// AT THIS POINT WE COULD NOT FIND A MATCHING MEMBER.
			//
			MemberNotFoundException CreatedException = new MemberNotFoundException();
			CreatedException.MemberName = MemberName;
			CreatedException.TargetObjectType = _TargetType;
			throw CreatedException;
		}


		public Type GetFieldTypeByName (string MemberName)
		{
			return _TargetType.GetField(MemberName).FieldType;
		}


		public Type GetPropertyTypeByName (string MemberName)
		{
			return _TargetType.GetProperty(MemberName).PropertyType;
		}

	
		public bool HasFieldWithName (string Name)
		{
			return _TargetType.GetField(Name) != null;
		}


		public bool HasSetterWithName (string Name)
		{
			return HasPropertyWithName(Name) && _TargetType.GetProperty(Name).GetSetMethod() != null;
		}


		public bool HasGetterWithName (string Name)
		{
			return HasPropertyWithName(Name) && _TargetType.GetProperty(Name).GetGetMethod() != null;
		}
		
		
		public bool HasPropertyWithName (string Name)
		{
			return _TargetType.GetProperty(Name) != null;
		}
		
		
		private Type _TargetType;
	}
}
