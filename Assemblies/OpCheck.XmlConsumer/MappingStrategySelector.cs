using System;

namespace OpCheck.XmlConsumer
{
	public class MappingStrategySelector
	{
		public MappingStrategy SelectStrategy ()
		{
			//
			// GET THE COLLECTIVITY VALUE FOR THE CONTEXT OBJECT AND MEMBER.
			//
			Collectivity ContextObjectCollectivity = GetDefaultCollectivityForType(_ContextObjectType);
			Collectivity MemberCollectivity = GetDefaultCollectivityForType(_MemberType);
			
			//
			// DETERMINE THE MAPPING STRATEGY TO USE.
			//
			if (ContextObjectCollectivity == Collectivity.SingleValueObject)
			{
				if (MemberCollectivity == Collectivity.SingleValueObject)
					return MappingStrategy.SingleValueObjects;
				
				return MappingStrategy.SingleValueObjectContainsArray;
			}
			
			if (ContextObjectCollectivity == Collectivity.Array)
			{
				if (MemberCollectivity == Collectivity.SingleValueObject)
					return MappingStrategy.ArrayContainsSingleValueObject;
					
				return MappingStrategy.Arrays;
			}
			
			throw new Exception("Could not select a mapping strategy for the specified types.");
		}
		
		
		public Collectivity GetDefaultCollectivityForType (Type CurrentType)
		{
			if (CurrentType.IsArray)
				return Collectivity.Array;

			return Collectivity.SingleValueObject;
		}
		
		
		public Type ContextObjectType
		{
			set
			{
				_ContextObjectType = value;
			}
		}


		public Type MemberType
		{
			set
			{
				_MemberType = value;
			}
		}
		
	
		private Type _ContextObjectType;
		private Type _MemberType;
	}
}
