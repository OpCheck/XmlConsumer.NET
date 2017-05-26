using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace OpCheck.XmlConsumer.MappingCommands
{
	/// <summary>
	/// Executes the mapping strategy where the context object is treated as an array and its member is treated as single value object.
	/// </summary>
	public class ArrayContainsSingleValueObjectMappingCommand : MappingCommand
	{
		public override void Execute ()
		{
			//
			// NOTE: THIS MAPPING COMMAND ASSUMES THAT THE CONTEXT OBJECT - AN ARRAY - IS NOT NULL.
			// THIS MEANS THAT SOMETHING ELSE MUST INSTANTIATE IT BEFORE THE MAPPING OCCURS.
			//
			
			//
			// WE ALREADY KNOW THAT THE CONTEXT OBJECT IS AN ARRAY.
			// CREATE THE OBJECT REFLECTOR AND GET THE CONTEXT MEMBER ELEMENT TYPE.
			//

			//
			// CREATE THE EXPRESSION EVALUATOR AND GET THE SET OF RESULT STRINGS.
			//
			DataExpressionEvaluator CreatedEvaluator = DataExpressionEvaluatorFactory.CreateEvaluator(_OptionsTuple.ExpressionType);
			CreatedEvaluator.SourceNodes = _SourceNodes;
			CreatedEvaluator.Expression = _Expression;
			CreatedEvaluator.NamespaceManager = _NamespaceManager;
			string[] ResultStrings = CreatedEvaluator.EvaluateForArray();
			
			//
			//
			//
			object[] ElementObjects = (object[])_ContextObject;
			
			for (int i = 0; i < ElementObjects.Length && i < ResultStrings.Length; i++)
			{
				//
				// GET THE CURRENT ELEMENT OBJECT AND ITS ASSOCIATED RESULT STRING.
				//
				object ElementObject = ElementObjects[i];
				string ResultString = ResultStrings[i];
			
				//
				// GET THE TARGET MEMBER TYPE.
				//
				ObjectReflector ElementReflector = new ObjectReflector();
				ElementReflector.TargetObject = ElementObject;
				ElementReflector.Init();
				Type ElementMemberType = ElementReflector.GetMemberTypeByName(_MemberIdentifier);
				
				//
				// CREATE THE RESULT OBJECT.
				// THIS IS THE FINAL RESULT OF THE MAPPING STATEMENT THAT WE ASSIGN TO THE CONTEXT OBJECT.
				//
				object ResultObject = ResultString;

				//
				// IF AUTO PARSE MODE IS ON THEN WE NEED TO PARSE THE RESULT.
				//
				if (_OptionsTuple.AutoParseMode == AutoParseMode.On)
				{
					//
					// GET THE AUTO PARSE DELEGATE.
					//
					if (_AutoParseMethods.ContainsKey(ElementMemberType))
					{
						//
						// PARSE THE RESULT STRING.
						//
						ResultObject = _AutoParseMethods[ElementMemberType](ResultString);
					}
					else
					{
						//
						// NO AUTO PARSE DELEGATE FOUND.
						//
						throw new Exception("Could not find auto parse method for target member type.");
					}
				}
				
				//
				// CREATE THE OBJECT LOADER AND SET THE RESULT ON THE TARGET ELEMENT.
				//
				ObjectLoader ElementObjectLoader = new ObjectLoader();
				ElementObjectLoader.TargetObject = ElementObject;
				ElementObjectLoader.Init();
				ElementObjectLoader.SetMemberValue(_MemberIdentifier, ResultObject);
			}
		}
	}
}
