using System;
using System.Collections;
using System.Reflection;

namespace OpCheck.XmlConsumer
{
	/// <summary>
	/// 
	/// </summary>
	public class ObjectCreator
	{
		public object CreateObject ()
		{
			if (_TargetType.IsArray)
				return CreateArrayObject();
				
			return CreateSingleValueObject();
		}
		
		
		public object CreateArrayObject ()
		{
			//
			// WE NEED TO CREATE AN ARRAY.
			//
			Type ElementType = _TargetType.GetElementType();
			
			//
			// GET THE ELEMENT TYPE CONSTRUCTOR.
			//
			ConstructorInfo ElementConstructor = ElementType.GetConstructor(new Type[0]{});
			
			//
			// CREATE THE LIST.
			//
			ArrayList CreatedList = new ArrayList();
			
			for (int i = 0; i < _Size; i++)
			{
				CreatedList.Add(ElementConstructor.Invoke(null));
			}
			
			return CreatedList.ToArray(ElementType);
		}
		
		
		public object CreateSingleValueObject ()
		{
			//
			// GET THE ELEMENT TYPE CONSTRUCTOR.
			//
			ConstructorInfo TargetConstructor = _TargetType.GetConstructor(new Type[0]{});
			
			return TargetConstructor.Invoke(null);
		}
	
	
		public Type TargetType
		{
			set
			{
				_TargetType = value;
			}
		}


		public int Size
		{
			set
			{
				_Size = value;
			}
		}


		public Collectivity Collectivity
		{
			set
			{
				_Collectivity = value;
			}
		}
		
		
		private Type _TargetType;
		private int _Size;
		private Collectivity _Collectivity;
	}
}
