using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

using Antlr4.Runtime.Tree;

using OpCheck.XmlConsumer.MappingCommands;
using OpCheck.XmlConsumer.AutoCreators;

namespace OpCheck.XmlConsumer
{
	public class XmlConsumerVisitor : XmlConsumerBaseVisitor<object>
	{
		/// <summary>
		/// Resets the listener.
		/// We always call this method before executing it.
		/// </summary>
		public void Init ()
		{
			_ContextObjectStack = new Stack<object>();
			_OptionsTupleStack = new Stack<OptionsTuple>();
		}


		public override object VisitXmlConsumerScript (XmlConsumerParser.XmlConsumerScriptContext CurrentContext)
		{
			//
			// CREATE AND PUSH THE INITIAL OPTIONS TUPLE ONTO THE STACK.
			//
			OptionsTuple CreatedOptionsTuple = new OptionsTuple();
			CreatedOptionsTuple.ExpressionType = _ExpressionType;
			CreatedOptionsTuple.AutoParseMode = _AutoParseMode;
			CreatedOptionsTuple.SourceNodes = _SourceNodes;
			CreatedOptionsTuple.AutoCreateMode = _AutoCreateMode;
			CreatedOptionsTuple.NullObjectTreatmentMode = _NullObjectTreatmentMode;
			CreatedOptionsTuple.Collectivity = _Collectivity;
			_OptionsTupleStack.Push(CreatedOptionsTuple);
		
			//
			// PUSH THE INITIAL CONTEXT OBJECT ONTO THE STACK.
			//
			_ContextObjectStack.Push(_ContextObject);
			
			//
			// PROCESS CHILDREN.
			//
			base.VisitXmlConsumerScript(CurrentContext);
			
			/*
			if (CurrentContext.optionsTuple() != null)
				VisitOptionsTuple(CurrentContext.optionsTuple());
			
			return VisitObjectMappingBlock(CurrentContext.objectMappingBlock());
			*/
			return null;
		}


		public override object VisitOptionsTuple (XmlConsumerParser.OptionsTupleContext CurrentContext)
		{
			//
			// GET THE CURRENT OPTIONS TUPLE.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();
			
			//
			// COPY THE CURRENT OPTIONS TUPLE.
			//
			OptionsTuple CopiedOptionsTuple = new OptionsTuple();
			CopiedOptionsTuple.ExpressionType = CurrentOptionsTuple.ExpressionType;
			CopiedOptionsTuple.AutoParseMode = CurrentOptionsTuple.AutoParseMode;
			CopiedOptionsTuple.SourceNodes = CurrentOptionsTuple.SourceNodes;
			CopiedOptionsTuple.AutoCreateMode = CurrentOptionsTuple.AutoCreateMode;
			CopiedOptionsTuple.NullObjectTreatmentMode = CurrentOptionsTuple.NullObjectTreatmentMode;
			CopiedOptionsTuple.Collectivity = CurrentOptionsTuple.Collectivity;

			//
			// PUSH THE NEW, COPIED OPTIONS TUPLE TO THE STACK.
			//
			_OptionsTupleStack.Push(CopiedOptionsTuple);
			
			//
			//
			//
			return base.VisitOptionsTuple(CurrentContext);
		}


		public override object VisitExpressionTypeOption (XmlConsumerParser.ExpressionTypeOptionContext CurrentOptionContext)
		{
			//
			// GET THE EXPRESSION TYPE.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentOptionContext.STRING_LITERAL();
			string ExpressionTypeCode = CreatedReader.GetString();
			
			//
			// PARSE THE EXPRESSION TYPE.
			//
			XmlConsumerExpressionType CurrentExpressionType = (XmlConsumerExpressionType)Enum.Parse(typeof(XmlConsumerExpressionType), ExpressionTypeCode);
			
			//
			// SET THE EXPRESSION TYPE.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();
			CurrentOptionsTuple.ExpressionType = CurrentExpressionType;

			return null;
		}


		public override object VisitBindingOption (XmlConsumerParser.BindingOptionContext CurrentContext)
		{
			//
			// GET THE SOURCE NODES.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();
		
			//
			// GET THE XPATH EXPRESSION THAT WE NEED TO EXECUTE AGAINST THE SOURCE DOCUMENT.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentContext.STRING_LITERAL();
			string XPathExpression = CreatedReader.GetString();
			
			//
			// EXECUTE THE XPATH EXPRESSION AGAINST THE CURRENT SOURCE NODES.
			//
			MultiNodeExpressionEvaluator CreatedEvaluator = new MultiNodeExpressionEvaluator();
			CreatedEvaluator.NamespaceManager = _NamespaceManager;
			CreatedEvaluator.Expression = XPathExpression;
			CurrentOptionsTuple.SourceNodes = CreatedEvaluator.Evaluate(CurrentOptionsTuple.SourceNodes);

			return null;
		}


		public override object VisitBindingsOption (XmlConsumerParser.BindingsOptionContext CurrentContext)
		{
			//
			// GET THE SOURCE NODES.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();

			//
			// GET THE LIST OF XPATH EXPRESSIONS THAT WE NEED TO EXECUTE AGAINST THE SOURCE DOCUMENT.
			//
			foreach (ITerminalNode CurrentTerminalNode in CurrentContext.stringLiteralsList().STRING_LITERAL())
			{
				//
				// CREATE THE TERMINAL NODE READER.
				//
				TerminalNodeReader CreatedReader = new TerminalNodeReader();
				CreatedReader.TerminalNode = CurrentTerminalNode;
				string CurrentBindingExpression = CreatedReader.GetString();
				
				//
				// PROCESS THE CURRENT BINDING EXPRESSION.
				//
				MultiNodeExpressionEvaluator CreatedEvaluator = new MultiNodeExpressionEvaluator();
				CreatedEvaluator.NamespaceManager = _NamespaceManager;
				CreatedEvaluator.Expression = CurrentBindingExpression;
				CurrentOptionsTuple.SourceNodes = CreatedEvaluator.Evaluate(CurrentOptionsTuple.SourceNodes);
			}

			return null;
		}


		public override object VisitPrefixOption (XmlConsumerParser.PrefixOptionContext CurrentContext)
		{
			//
			// CREATE THE TERMINAL NODE READER.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
		
			//
			// GET THE NAMESPACE PREFIX.
			//
			CreatedReader.TerminalNode = CurrentContext.STRING_LITERAL(0);
			string Prefix = CreatedReader.GetString();
			
			//
			// GET THE FULL NAMESPACE.
			//
			CreatedReader.TerminalNode = CurrentContext.STRING_LITERAL(1);
			string Namespace = CreatedReader.GetString();
			
			//
			// ADD THE PREFIX TO THE NAMESPACE MANAGER.
			//
			_NamespaceManager.AddNamespace(Prefix, Namespace);

			return null;
		}


		public override object VisitAutoParseModeOption (XmlConsumerParser.AutoParseModeOptionContext CurrentContext)
		{
			//
			// CREATE THE TERMINAL NODE READER.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentContext.STRING_LITERAL();
			string AutoParseModeString = CreatedReader.GetString();
			
			//
			// PARSE THE AUTO PARSE MODE STRING.
			//
			AutoParseMode CurrentMode = (AutoParseMode)Enum.Parse(typeof(AutoParseMode), AutoParseModeString);
			
			//
			// PUSH THE AUTO PARSE MODE TO THE STACK.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();
			CurrentOptionsTuple.AutoParseMode = CurrentMode;

			return null;
		}


		public override object VisitAutoCreateModeOption (XmlConsumerParser.AutoCreateModeOptionContext CurrentContext)
		{
			//
			// CREATE THE TERMINAL NODE READER.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentContext.STRING_LITERAL();
			string AutoCreateModeString = CreatedReader.GetString();
			
			//
			// PARSE THE AUTO CREATE MODE STRING.
			//
			AutoCreateMode CurrentMode = (AutoCreateMode)Enum.Parse(typeof(AutoCreateMode), AutoCreateModeString);
			
			//
			// PUSH THE AUTO CREATE MODE TO THE STACK.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();
			CurrentOptionsTuple.AutoCreateMode = CurrentMode;

			return null;
		}


		public override object VisitNullObjectTreatmentModeOption (XmlConsumerParser.NullObjectTreatmentModeOptionContext CurrentContext)
		{
			//
			// CREATE THE TERMINAL NODE READER.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentContext.STRING_LITERAL();
			string NullObjectTreatmentModeString = CreatedReader.GetString();
			
			//
			// PARSE THE NULL OBJECT TREATMENT MODE STRING.
			//
			NullObjectTreatmentMode CurrentMode = (NullObjectTreatmentMode)Enum.Parse(typeof(NullObjectTreatmentMode), NullObjectTreatmentModeString);
			
			//
			// PUSH THE AUTO CREATE MODE TO THE STACK.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();
			CurrentOptionsTuple.NullObjectTreatmentMode = CurrentMode;

			return null;
		}


		public override object VisitCollectivityOption(XmlConsumerParser.CollectivityOptionContext CurrentContext)
		{
			//
			// CREATE THE TERMINAL NODE READER.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentContext.STRING_LITERAL();
			string CollectivityString = CreatedReader.GetString();
			
			//
			// PARSE THE NULL OBJECT TREATMENT MODE STRING.
			//
			Collectivity CurrentValue = (Collectivity)Enum.Parse(typeof(Collectivity), CollectivityString);
			
			//
			// PUSH THE AUTO CREATE MODE TO THE STACK.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();
			return CurrentOptionsTuple.Collectivity = CurrentValue;
		}


		/// <summary>
		/// Processes a null mapping statement with no options.
		/// </summary>
		public override object VisitNullMapping (XmlConsumerParser.NullMappingContext CurrentContext)
		{
			//
			// CREATE THE TERMINAL NODE READER AND GET THE MEMBER IDENTIFIER.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentContext.OBJECT_MEMBER_IDENTIFIER();
			string MemberIdentifier = CreatedReader.GetIdentifier();

			//
			// GET THE CONTEXT OBJECT.
			//
			object CurrentContextObject = _ContextObjectStack.Peek();

			//
			// EXECUTE THE NULL MAPPING COMMAND.
			//
			NullMappingCommand CurrentCommand = new NullMappingCommand();
			CurrentCommand.MemberIdentifier = MemberIdentifier;
			CurrentCommand.ContextObject = CurrentContextObject;
			CurrentCommand.Execute();

			return null;
		}


		/// <summary>
		/// Processes an expression mapping statement with no options.
		/// </summary>
		public override object VisitStringMapping (XmlConsumerParser.StringMappingContext CurrentContext)
		{
			//
			// GET THE CONTEXT OBJECT.
			//
			object CurrentContextObject = _ContextObjectStack.Peek();

			//
			// CREATE THE TERMINAL NODE READER AND GET THE MEMBER IDENTIFIER.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentContext.OBJECT_MEMBER_IDENTIFIER();
			string MemberIdentifier = CreatedReader.GetIdentifier();
	
			//
			// GET THE EXPRESSION.
			//
			CreatedReader.TerminalNode = CurrentContext.STRING_LITERAL();
			string Expression = CreatedReader.GetString();
					
			//
			// GET THE EXPRESSION TYPE SO WE KNOW HOW TO EVALUATE THE EXPRESSION.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();
			
			//
			// CHECK THE CONTEXT OBJECT TO SEE HOW WE NEED TO PROCESS THIS MAPPING.
			//
			
			//
			// FIRST WE NEED TO DETERMINE THE TARGET TYPE.
			// CREATE AND INITIALIZE THE OBJECT LOADER.
			// WE DO THIS BECAUSE WE NEED TO KNOW IF THE TARGET IS A NORMAL FIELD, AN ARRAY, OR A COLLECTION.
			//
			ObjectReflector CreatedReflector = new ObjectReflector();
			CreatedReflector.TargetObject = CurrentContextObject;
			CreatedReflector.Init();
			Type MemberType = CreatedReflector.GetMemberTypeByName(MemberIdentifier);
			
			//
			// DETERMINE THE MAPPING ALGORITHM THAT WE NEED TO USE.
			//
			MappingStrategySelector StrategySelector = new MappingStrategySelector();
			StrategySelector.ContextObjectType = CurrentContextObject.GetType();
			StrategySelector.MemberType = MemberType;
			MappingStrategy CurrentMappingStrategy = StrategySelector.SelectStrategy();
			
			//
			// CREATE AND EXECUTE THE MAPPING COMMAND.
			//
			MappingCommand CurrentCommand = MappingCommandFactory.CreateCommand(CurrentMappingStrategy);
			CurrentCommand.ContextObject = CurrentContextObject;
			CurrentCommand.MemberIdentifier = MemberIdentifier;
			CurrentCommand.OptionsTuple = CurrentOptionsTuple;
			CurrentCommand.Expression = Expression;
			CurrentCommand.SourceNodes = CurrentOptionsTuple.SourceNodes;
			CurrentCommand.NamespaceManager = _NamespaceManager;
			CurrentCommand.AutoParseMode = _AutoParseMode;
			CurrentCommand.AutoParseMethods = _AutoParseMethods;
			CurrentCommand.Execute();
			
			//
			// NO OPTIONS TUPLE PROVIDED FOR THIS PRODUCTION SO WE DO NOT POP IT FROM THE STACK.
			//

			return null;
		}


		public override object VisitNullMappingWithOptions (XmlConsumerParser.NullMappingWithOptionsContext CurrentContext)
		{
			//
			// PROCESS THE OPTIONS TUPLE FIRST.
			//
			VisitOptionsTuple(CurrentContext.optionsTuple());

			//
			// GET THE OPTIONS TUPLE.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();
		
			//
			// CREATE THE TERMINAL NODE READER AND GET THE MEMBER IDENTIFIER.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentContext.OBJECT_MEMBER_IDENTIFIER();
			string MemberIdentifier = CreatedReader.GetIdentifier();

			//
			// GET THE CONTEXT OBJECT.
			//
			object CurrentContextObject = _ContextObjectStack.Peek();

			//
			// EXECUTE THE NULL MAPPING COMMAND.
			//
			NullMappingCommand CurrentCommand = new NullMappingCommand();
			CurrentCommand.MemberIdentifier = MemberIdentifier;
			CurrentCommand.ContextObject = CurrentContextObject;
			CurrentCommand.Execute();

			//
			// WE ARE DONE WITH THIS STATEMENT SO POP THE OPTIONS TUPLE OFF THE STACK.
			//
			_OptionsTupleStack.Pop();

			return null;
		}


		/// <summary>
		/// Executes a string mapping command with options specified.
		/// The only difference here is this mapping command knows that it needs to pop the options tuple stack at the end.
		/// </summary>
		public override object VisitStringMappingWithOptions (XmlConsumerParser.StringMappingWithOptionsContext CurrentContext)
		{
			//
			// PROCESS THE OPTIONS TUPLE FIRST.
			//
			VisitOptionsTuple(CurrentContext.optionsTuple());

			//
			// GET THE OPTIONS TUPLE.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();
		
			//
			// CREATE THE TERMINAL NODE READER AND GET THE MEMBER IDENTIFIER.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentContext.OBJECT_MEMBER_IDENTIFIER();
			string MemberIdentifier = CreatedReader.GetIdentifier();

			//
			// GET THE CURRENT CONTEXT OBJECT.
			//
			object CurrentContextObject = _ContextObjectStack.Peek();

			//
			// GET THE EXPRESSION.
			//
			CreatedReader.TerminalNode = CurrentContext.STRING_LITERAL();
			string Expression = CreatedReader.GetString();
			
			//
			// CHECK THE CONTEXT OBJECT TO SEE HOW WE NEED TO PROCESS THIS MAPPING.
			//
			
			//
			// FIRST WE NEED TO DETERMINE THE TARGET TYPE.
			// CREATE AND INITIALIZE THE OBJECT LOADER.
			// WE DO THIS BECAUSE WE NEED TO KNOW IF THE TARGET IS A NORMAL FIELD, AN ARRAY, OR A COLLECTION.
			//
			ObjectLoader CreatedLoader = new ObjectLoader();
			CreatedLoader.TargetObject = CurrentContextObject;
			CreatedLoader.Init();
			Type MemberType = CreatedLoader.GetMemberTypeByName(MemberIdentifier);
			
			/*
			//
			// IF AUTO CREATE MODE IS ON, THEN CREATE AN INSTANCE OF THIS MEMBER.
			//
			AutoCreateMode CurrentMode = AutoCreateMode.Always;
			
			if (CurrentMode == AutoCreateMode.Always)
			{
				//
				// GET THE MEMBER TYPE.
				//
				
			}
			*/
			
			//
			// NOW DETERMINE THE MAPPING ALGORITHM THAT WE NEED TO USE.
			//
			MappingStrategySelector StrategySelector = new MappingStrategySelector();
			StrategySelector.ContextObjectType = CurrentContextObject.GetType();
			StrategySelector.MemberType = MemberType;
			MappingStrategy CurrentMappingStrategy = StrategySelector.SelectStrategy();
			
			//
			// CREATE AND EXECUTE THE MAPPING COMMAND.
			//
			MappingCommand CurrentCommand = MappingCommandFactory.CreateCommand(CurrentMappingStrategy);
			CurrentCommand.ContextObject = CurrentContextObject;
			CurrentCommand.MemberIdentifier = MemberIdentifier;
			CurrentCommand.OptionsTuple = CurrentOptionsTuple;
			CurrentCommand.Expression = Expression;
			CurrentCommand.SourceNodes = CurrentOptionsTuple.SourceNodes;
			CurrentCommand.NamespaceManager = _NamespaceManager;
			CurrentCommand.AutoParseMode = _AutoParseMode;
			CurrentCommand.AutoParseMethods = _AutoParseMethods;
			CurrentCommand.Execute();
			
			//
			// AN OPTIONS TUPLE WAS INCLUDED WITH THIS MAPPING STATEMENT - SO DISCARD THE OPTIONS SINCE WE ARE DONE.
			//
			_OptionsTupleStack.Pop();

			return null;
		}


		public override object VisitObjectMappingBlock (XmlConsumerParser.ObjectMappingBlockContext CurrentContext)
		{
			base.VisitObjectMappingBlock(CurrentContext);
			
			//
			// POP THE CONTEXT OBJECT OFF OF THE STACK.
			//
			_ContextObjectStack.Pop();

			return null;
		}


		public override object VisitObjectMapping (XmlConsumerParser.ObjectMappingContext CurrentContext)
		{
			//
			// GET THE MEMBER IDENTIFIER.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentContext.OBJECT_MEMBER_IDENTIFIER();
			string MemberIdentifier = CreatedReader.GetIdentifier();
			
			//
			// GET THE CURRENT CONTEXT OBJECT.
			//
			object CurrentContextObject = _ContextObjectStack.Peek();
			
			//
			// GET THE CURRENT OPTIONS TUPLE.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();
			
			//
			// GET THE AUTOCREATOR AND APPLY THE AUTO CREATE MODE TO THE TARGET MEMBER.
			// THE TARGET MEMBER IS CREATED ON THE CONTEXT OBJECT AND RETURNED.
			// DOES THIS WORK FOR STRUCTURE OBJECTS?
			//
			AutoCreator CreatedAutoCreator = AutoCreatorFactory.CreateAutoCreator(CurrentOptionsTuple.AutoCreateMode);
			CreatedAutoCreator.TargetObject = CurrentContextObject;
			CreatedAutoCreator.MemberIdentifier = MemberIdentifier;
			CreatedAutoCreator.MemberSize = CurrentOptionsTuple.SourceNodes.Length;
			CreatedAutoCreator.Collectivity = CurrentOptionsTuple.Collectivity;
			object TargetMember = CreatedAutoCreator.ApplyAutoCreateLogic();
			
			//
			// CHECK TO SEE IF THE TARGET MEMBER IS NULL.
			// IF SO, THEN WE NEED TO ACTIVATE THE NULL OBJECT TREATMENT LOGIC.
			//
			if (TargetMember == null)
			{
				NullObjectTreatmentMode CurrentTreatmentMode = CurrentOptionsTuple.NullObjectTreatmentMode;
				
				if (CurrentTreatmentMode == NullObjectTreatmentMode.Skip)
				{
					//
					// WE ARE NOT EXECUTING THIS STATEMENT SO WE NEED TO EXIT IT GRACEFULLY.
					//
					return null;
				}
				
				if (CurrentTreatmentMode == NullObjectTreatmentMode.Throw)
				{
					NullContextObjectException CreatedException = new NullContextObjectException();
					CreatedException.MemberIdentifier = MemberIdentifier;
					throw CreatedException;
				}
				
				//
				// THE 3RD TREATMENT MODE IS IGNORE.  IF WE GET HERE WE DON'T DO ANYTHING.
				//
			}
			
			//
			// THE TARGET MEMBER BECOMES THE NEW CONTEXT OBJECT.
			//
			_ContextObjectStack.Push(TargetMember);
			
			//
			// VISIT THE OBJECT MAPPING BLOCK.
			//
			return VisitObjectMappingBlock(CurrentContext.objectMappingBlock());
		}


		public override object VisitObjectMappingWithOptions (XmlConsumerParser.ObjectMappingWithOptionsContext CurrentContext)
		{
			//
			// CREATE THE TERMINAL NODE READER AND GET THE MEMBER IDENTIFIER.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentContext.OBJECT_MEMBER_IDENTIFIER();
			string MemberIdentifier = CreatedReader.GetIdentifier();

			//
			// GET THE CURRENT CONTEXT OBJECT.
			//
			object CurrentContextObject = _ContextObjectStack.Peek();
			
			//
			// PROCESS THE OPTIONS TUPLE FIRST.
			// THIS IS WHY WE NEED A VISITOR INSTEAD OF A LISTENER - SO WE CAN PROCESS THESE PARTS OF THE LANGUAGE OUT OF ORDER.
			//
			VisitOptionsTuple(CurrentContext.optionsTuple());
			
			//
			// GET THE CURRENT OPTIONS TUPLE.
			//
			OptionsTuple CurrentOptionsTuple = _OptionsTupleStack.Peek();
			
			//
			// GET THE AUTOCREATOR AND APPLY THE AUTO CREATE MODE TO THE TARGET MEMBER.
			// THE TARGET MEMBER IS CREATED ON THE CONTEXT OBJECT AND RETURNED.
			// DOES THIS WORK FOR STRUCTURE OBJECTS?
			//
			AutoCreator CreatedAutoCreator = AutoCreatorFactory.CreateAutoCreator(CurrentOptionsTuple.AutoCreateMode);
			CreatedAutoCreator.TargetObject = CurrentContextObject;
			CreatedAutoCreator.MemberIdentifier = MemberIdentifier;
			CreatedAutoCreator.MemberSize = CurrentOptionsTuple.SourceNodes.Length;
			CreatedAutoCreator.Collectivity = CurrentOptionsTuple.Collectivity;
			object TargetMember = CreatedAutoCreator.ApplyAutoCreateLogic();
			
			//
			// CHECK TO SEE IF THE TARGET MEMBER IS NULL.
			// IF SO, THEN WE NEED TO ACTIVATE THE NULL OBJECT TREATMENT LOGIC.
			//
			if (TargetMember == null)
			{
				NullObjectTreatmentMode CurrentTreatmentMode = CurrentOptionsTuple.NullObjectTreatmentMode;
				
				if (CurrentTreatmentMode == NullObjectTreatmentMode.Skip)
				{
					//
					// WE ARE NOT EXECUTING THIS STATEMENT SO WE NEED TO EXIT IT GRACEFULLY.
					//
					
					//
					// WE MUST POP THE OPTIONS TUPLE OFF THE STACK.
					//
					return _OptionsTupleStack.Pop();
				}
				
				if (CurrentTreatmentMode == NullObjectTreatmentMode.Throw)
				{
					NullContextObjectException CreatedException = new NullContextObjectException();
					CreatedException.MemberIdentifier = MemberIdentifier;
					throw CreatedException;
				}
				
				//
				// THE 3RD TREATMENT MODE IS IGNORE.  IF WE GET HERE WE DON'T DO ANYTHING.
				//
			}
			
			//
			// THE TARGET MEMBER BECOMES THE NEW CONTEXT OBJECT.
			//
			_ContextObjectStack.Push(TargetMember);
			
			//
			// NOW WE CAN APPLY THE OBJECT MAPPING BLOCK.
			//
			VisitObjectMappingBlock(CurrentContext.objectMappingBlock());
			
			//
			// THIS MAPPING STATEMENT IS COMPLETE.
			// WE MUST POP THE OPTIONS TUPLE OFF THE STACK.
			//
			return _OptionsTupleStack.Pop();
		}


		/// <summary>
		/// Sets the context object that must be populated.
		/// </summary>
		public object ContextObject
		{
			set
			{
				_ContextObject = value;
			}
		}


		/// <summary>
		/// Sets the root-level expression type for the interpreter.
		/// </summary>
		public XmlConsumerExpressionType ExpressionType
		{
			set
			{
				_ExpressionType = value;
			}
		}


		public Encoding Encoding
		{
			set
			{
				_Encoding = value;
			}
		}
		
		
		public XmlNode[] SourceNodes
		{
			set
			{
				_SourceNodes = value;
			}
		}


		public XmlNamespaceManager NamespaceManager
		{
			set
			{
				_NamespaceManager = value;
			}
		}


		public AutoParseMode AutoParseMode
		{
			set
			{
				_AutoParseMode = value;
			}
		}


		public Dictionary<Type, AutoParseDelegate> AutoParseMethods
		{
			set
			{
				_AutoParseMethods = value;
			}
		}
		
		
		public AutoCreateMode AutoCreateMode
		{
			set
			{
				_AutoCreateMode = value;
			}
		}


		public NullObjectTreatmentMode NullObjectTreatmentMode
		{
			set
			{
				_NullObjectTreatmentMode = value;
			}
		}


		public Collectivity Collectivity
		{
			set
			{
				_Collectivity = value;
			}
		}


		//
		// INPUT FIELDS.
		//
		private object _ContextObject;
		private XmlConsumerExpressionType _ExpressionType;
		private Encoding _Encoding;
		private XmlNode[] _SourceNodes;
		private XmlNamespaceManager _NamespaceManager;
		private AutoParseMode _AutoParseMode;
		private Dictionary<Type, AutoParseDelegate> _AutoParseMethods;
		private AutoCreateMode _AutoCreateMode;
		private NullObjectTreatmentMode _NullObjectTreatmentMode;
		private Collectivity _Collectivity;
		
		//
		// OPERATIONAL FIELDS.
		//
		private Stack<object> _ContextObjectStack;
		private Stack<OptionsTuple> _OptionsTupleStack;
	}
}
