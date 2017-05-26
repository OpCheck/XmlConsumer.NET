using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace OpCheck.XmlConsumer.MappingCommands
{
	/// <summary>
	/// Executes the mapping strategy where both the context object and its member are treated as single value objects.
	/// </summary>
	public class SingleValueObjectsMappingCommand : MappingCommand
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
			Type TargetMemberType = CreatedLoader.GetMemberTypeByName(_MemberIdentifier);

			//
			// CREATE THE EXPRESSION EVALUATOR.
			//
			DataExpressionEvaluator CreatedEvaluator = DataExpressionEvaluatorFactory.CreateEvaluator(_OptionsTuple.ExpressionType);
			CreatedEvaluator.SourceNodes = _SourceNodes;
			CreatedEvaluator.Expression = _Expression;
			CreatedEvaluator.NamespaceManager = _NamespaceManager;
			string ResultString = CreatedEvaluator.Evaluate();
					
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
				if (_AutoParseMethods.ContainsKey(TargetMemberType))
				{
					//
					// PARSE THE RESULT STRING.
					//
					ResultObject = _AutoParseMethods[TargetMemberType](ResultString);
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
			// SET THE RESULT ON THE TARGET OBJECT.
			//
			CreatedLoader.SetMemberValue(_MemberIdentifier, ResultObject);
		}
	}
}
