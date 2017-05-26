using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace OpCheck.XmlConsumer.MappingCommands
{
	/// <summary>
	/// Executes the mapping strategy where both the context object and target member are treated as arrays.
	/// To be clear, the target member is a member of the current element of the array - it is not a member of the array object itself.
	/// </summary>
	public class ArraysMappingCommand : MappingCommand
	{
		public override void Execute ()
		{
			//
			// WE ALREADY KNOW THAT THE CONTEXT OBJECT IS AN ARRAY.
			// CREATE THE OBJECT REFLECTOR AND GET THE CONTEXT MEMBER ELEMENT TYPE.
			//
			//ObjectReflector ContextReflector = new ObjectReflector();
			//ContextReflector.TargetObject = _ContextObject;
			//ContextReflector.Init();
			//Type ContextMemberElementType = ContextReflector.GetArrayElementType();

			foreach (object ElementObject in (object[])_ContextObject)
			{
				//
				// GET THE ARRAY MEMBER ELEMENT TYPE.
				//
				ObjectReflector ElementReflector = new ObjectReflector();
				ElementReflector.TargetObject = ElementObject;
				ElementReflector.Init();
				Type ArrayMemberElementType = ElementReflector.GetArrayMemberElementTypeByName(_MemberIdentifier);

				//
				// CREATE THE EXPRESSION EVALUATOR.
				//
				DataExpressionEvaluator CreatedEvaluator = DataExpressionEvaluatorFactory.CreateEvaluator(_OptionsTuple.ExpressionType);
				CreatedEvaluator.SourceNodes = _SourceNodes;
				CreatedEvaluator.Expression = _Expression;
				CreatedEvaluator.NamespaceManager = _NamespaceManager;
				string[] ResultStrings = CreatedEvaluator.EvaluateForArray();
					
				//
				// GET THE AUTO PARSE DELEGATE FOR THE ARRAY MEMBER ELEMENT TYPE.
				//
				AutoParseDelegate CurrentParseMethod = _AutoParseMethods[ArrayMemberElementType];
				
				//
				// CREATE AND BUILD THE LIST OF RESULT OBJECTS.
				// THIS IS THE FINAL RESULT OF THE MAPPING STATEMENT THAT WE ASSIGN TO THE CONTEXT OBJECT.
				//
				ArrayList ResultObjectList = new ArrayList();

				foreach (string CurrentResultString in ResultStrings)
				{
					ResultObjectList.Add(CurrentParseMethod(CurrentResultString));
				}
				
				object ResultArrayObject = ResultObjectList.ToArray(ArrayMemberElementType);
			
				//
				// SET THE RESULT ON THE TARGET OBJECT.
				//
				ObjectLoader ElementObjectLoader = new ObjectLoader();
				ElementObjectLoader.TargetObject = ElementObject;
				ElementObjectLoader.Init();
				ElementObjectLoader.SetMemberValue(_MemberIdentifier, ResultArrayObject);
			}
		}
	}
}
