using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace OpCheck.XmlConsumer.MappingCommands
{
	/// <summary>
	/// Executes a mapping for the situation where a single value object contains an array of parsable objects.
	/// A parsable object is anything that has a pre-defined parse method such as an Int32 - but it also could be a complex object as long as it has a parse method associated with the type.
	/// </summary>
	public class SingleValueObjectContainsArrayMappingCommand : MappingCommand
	{
		public override void Execute ()
		{
			//
			// CREATE AND INITIALIZE THE OBJECT LOADER.
			// WE DO THIS BECAUSE WE CAN USE IT TO GET THE TYPE OF THE TARGET MEMBER THAT WE ARE GOING TO SET.
			//
			ObjectLoader CreatedLoader = new ObjectLoader();
			CreatedLoader.TargetObject = _ContextObject;
			CreatedLoader.Init();
			
			//
			// WE ALREADY KNOW THAT THIS MEMBER IS AN ARRAY.
			// GET THE ELEMENT TYPE FOR THE ARRAY.
			//
			Type TargetMemberElementType = CreatedLoader.GetMemberTypeByName(_MemberIdentifier).GetElementType();

			//
			// CREATE THE EXPRESSION EVALUATOR.
			//
			DataExpressionEvaluator CreatedEvaluator = DataExpressionEvaluatorFactory.CreateEvaluator(_OptionsTuple.ExpressionType);
			CreatedEvaluator.SourceNodes = _SourceNodes;
			CreatedEvaluator.Expression = _Expression;
			CreatedEvaluator.NamespaceManager = _NamespaceManager;
			string[] ResultStrings = CreatedEvaluator.EvaluateForArray();
					
			//
			// GET THE AUTO PARSE DELEGATE FOR THE TARGET ARRAY ELEMENT TYPE.
			//
			AutoParseDelegate CurrentParseMethod = _AutoParseMethods[TargetMemberElementType];
			
			//
			// CREATE AND BUILD THE LIST OF RESULT OBJECTS.
			// THIS IS THE FINAL RESULT OF THE MAPPING STATEMENT THAT WE ASSIGN TO THE CONTEXT OBJECT.
			//
			ArrayList ResultObjectList = new ArrayList();

			foreach (string CurrentResultString in ResultStrings)
			{
				ResultObjectList.Add(CurrentParseMethod(CurrentResultString));
			}
			
			object ResultObjects = ResultObjectList.ToArray(TargetMemberElementType);
			
			//
			// SET THE RESULT ON THE TARGET OBJECT.
			//
			CreatedLoader.SetMemberValue(_MemberIdentifier, ResultObjects);
		}
	}
}
