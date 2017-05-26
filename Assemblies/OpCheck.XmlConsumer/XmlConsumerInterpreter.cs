using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace OpCheck.XmlConsumer
{
	/// <summary>
	/// The entry point to the XmlConsumer.NET Intepreter.
	/// </summary>
	public class XmlConsumerInterpreter
	{
		public XmlConsumerInterpreter ()
		{
			//
			// SET THE INTERPRETER DEFAULTS.
			//
			_Encoding = Encoding.UTF8;
			_ExpressionType = XmlConsumerExpressionType.XPath;
			_AutoParseMode = AutoParseMode.On;
			_AutoCreateMode = AutoCreateMode.OnlyIfNull;
			_NullObjectTreatmentMode = NullObjectTreatmentMode.Throw;
			_Collectivity = Collectivity.AutoDetect;
			_AllowMappingScriptEncodingOverride = true;

			//
			// BUILD THE AUTO PARSE METHODS DICTIONARY FOR THIS INTERPRETER.
			//
			_AutoParseMethods = new Dictionary<Type, AutoParseDelegate>();
			_AutoParseMethods.Add(typeof(String), new AutoParseDelegate(AutoParseMethods.ParseString));
			_AutoParseMethods.Add(typeof(Int32), new AutoParseDelegate(AutoParseMethods.ParseInt32));
			_AutoParseMethods.Add(typeof(Int64), new AutoParseDelegate(AutoParseMethods.ParseInt64));
			_AutoParseMethods.Add(typeof(Single), new AutoParseDelegate(AutoParseMethods.ParseSingle));
			_AutoParseMethods.Add(typeof(Double), new AutoParseDelegate(AutoParseMethods.ParseDouble));
			_AutoParseMethods.Add(typeof(Boolean), new AutoParseDelegate(AutoParseMethods.ParseBoolean));
			_AutoParseMethods.Add(typeof(Byte), new AutoParseDelegate(AutoParseMethods.ParseByte));
			_AutoParseMethods.Add(typeof(Byte[]), new AutoParseDelegate(AutoParseMethods.ParseByteArray));
			_AutoParseMethods.Add(typeof(DateTime), new AutoParseDelegate(AutoParseMethods.ParseDateTime));

			//
			// CREATE THE LISTENER OBJECT.
			//
			_Visitor = new XmlConsumerVisitor();
		}

	
		public void Init ()
		{
			_MappingScriptEncoding = null;

			_Visitor.Init();
		}
		
		
		/// <summary>
		/// After a source property and mapping script have been set the application calls this method.
		/// </summary>
		public object Execute ()
		{
			//
			// LOAD THE MAPPING SCRIPT INTO THE MAPPING SCRIPT STRING FIELD.
			//
			if (_MappingScriptString == null)
				LoadMappingScriptFile();
			
			ExecuteMappingScriptString();
			
			return _TargetObject;
		}
		
		
		/// <summary>
		/// Executes the mapping script in the specified file.
		/// </summary>
		/// <param name="ScriptFilePath">The full path to the script file.</param>
		public void LoadMappingScriptFile ()
		{
			//
			// READ THE ENTIRE FILE INTO MEMORY.
			//
			using (StreamReader ScriptFileReader = new StreamReader(_MappingScriptFilePath, Encoding.UTF8))
			{
				_MappingScriptString = ScriptFileReader.ReadToEnd();
			}
		}
		
		
		/// <summary>
		/// Executes the consumer script.
		/// </summary>
		public void ExecuteMappingScriptString ()
		{
			//
			// DECIDE WHICH SOURCE TO USE IN ORDER OF PREFERENCE:
			// 1. XML DOCUMENT
			// 2. XML STRING
			// 3. XML BINARY DATA.
			//
			if (_SourceXmlDocument != null)
			{
				//
				// THE SOURCE XML DOCUMENT HAS ALREADY BEEN LOADED AND PROVIDED TO THE INTERPRETER.
				// WE DON'T HAVE TO DO ANYTHING HERE - WE ARE GOOD TO GO.
				//
			}
			else if (_SourceXmlString != null)
			{
				//
				// PROMOTE THE SOURCE XML STRING INTO THE SOURCE XML DOCUMENT.
				//
				PromoteSourceXmlString();
			}
			else if (_SourceXmlBinaryData != null)
			{
				//
				// PROMOTE THE SOURCE BINARY DATA INTO THE SOURCE XML STRING.
				//
				PromoteSourceXmlBinaryData();
				
				//
				// PROMOTE THE SOURCE XML STRING INTO THE SOURCE XML DOCUMENT.
				//
				PromoteSourceXmlString();
			}

			//
			// EXECUTE THE MAPPING VISITOR.
			//
			ExecuteMappingVisitor();
		}
		
		
		public void PromoteSourceXmlBinaryData ()
		{
			//
			// XML BINARY DATA HAS BEEN PROVIDED.
			// WE NEED TO LOAD THIS INTO A STRING USING THE SPECIFIED ENCODING.
			// THE ISSUE IS AN ENCODING OPTION MAY BE SPECIFIED IN THE MAPPING SCRIPT FILE IN THE ROOT OPTIONS TUPLE.
			// SO - WHAT WE NEED TO DO IS PARSE THE MAPPING SCRIPT FOR THE SOLE PURPOSE OF GETTING THAT VALUE.
			// IN EFFECT - WE END UP PARSING THE MAPPING SCRIPT TWICE - ONCE FOR GETTING THE ENCODING AND THE SECOND TIME TO EXECUTE THE MAPPINGS.
			//
			if (_AllowMappingScriptEncodingOverride)
			{
				//
				// PROCESS THE ENCODING DISCOVERY VISITOR.
				// THIS YIELDS WHAT ENCODING THE SCRIPT WANTS TO USE - SO WE USE THIS INSTEAD.
				//
				_MappingScriptEncoding = ExecuteEncodingDiscoveryVisitor();
			}

			//
			// DETERMINE WHICH ENCODING TO USE - THE ENCODING SPECIFIED IN THE MAPPING SCRIPT OR THE DEFAULT INTERPRETER ENCODING.
			//
			Encoding EffectiveEncoding = _MappingScriptEncoding == null ? _Encoding : _MappingScriptEncoding;

			_SourceXmlString = EffectiveEncoding.GetString(_SourceXmlBinaryData);
			
			//
			// NULLIFY THE BINARY DATA SO IT CAN BE COLLECTED AND SO WE DON'T HAVE TO PROMOTE IT AGAIN.
			//
			_SourceXmlBinaryData = null;
		}
		
		
		public void PromoteSourceXmlString ()
		{
			//
			// AN XML STRING HAS BEEN PROVIDED.
			// LOAD THE STRING INTO THE SOURCE XML DOCUMENT.
			//
			_SourceXmlDocument = new XmlDocument();
			_SourceXmlDocument.LoadXml(_SourceXmlString);
			
			//
			// NULLIFY THE SOURCE XML STRING SO IT CAN BE GARBAGE COLLECTED.
			// ALSO, IF WE RUN THIS INTERPRETER AGAIN WE DON'T NEED TO RELOAD THE XML DOCUMENT.
			//
			_SourceXmlString = null;
		}
		
		
		public Encoding ExecuteEncodingDiscoveryVisitor ()
		{
			using (StringReader CreatedReader = new StringReader(_MappingScriptString))
			{
				//
				// CREATE A CHARACTER STREAM.
				//
				AntlrInputStream CreatedInputStream = new AntlrInputStream(CreatedReader);
				
				//
				// CREATE THE LEXER.
				//
				XmlConsumerLexer CreatedLexer = new XmlConsumerLexer(CreatedInputStream);
				
				//
				// CREATE THE TOKEN STREAM.
				//
				CommonTokenStream CreatedTokenStream = new CommonTokenStream(CreatedLexer);
				
				//
				// CREATE THE PARSER.
				//
				XmlConsumerParser CreatedParser = new XmlConsumerParser(CreatedTokenStream);
				CreatedParser.RemoveErrorListeners();
				CreatedParser.ErrorHandler = new XmlConsumerErrorStrategy();
				
				//
				// GET THE SUBTREE THAT WE WILL PARSE AGAINST.
				// IN THIS CASE, IT'S THE WHOLE EXPRESSION TREE THAT WE WILL BE EVALUATING.
				//
				IParseTree Tree = CreatedParser.xmlConsumerExpressionFile();

				//
				// CREATE AND CONFIGURE THE LISTENER.
				//
				XmlConsumerEncodingDiscoveryVisitor CreatedVisitor = new XmlConsumerEncodingDiscoveryVisitor();
				return CreatedVisitor.Visit(Tree);
			}
		}


		public void ExecuteMappingVisitor ()
		{
			using (StringReader CreatedReader = new StringReader(_MappingScriptString))
			{
				//
				// CREATE A CHARACTER STREAM.
				//
				AntlrInputStream CreatedInputStream = new AntlrInputStream(CreatedReader);
				
				//
				// CREATE THE LEXER.
				//
				XmlConsumerLexer CreatedLexer = new XmlConsumerLexer(CreatedInputStream);
				
				//
				// CREATE THE TOKEN STREAM.
				//
				CommonTokenStream CreatedTokenStream = new CommonTokenStream(CreatedLexer);
				
				//
				// CREATE THE PARSER.
				//
				XmlConsumerParser CreatedParser = new XmlConsumerParser(CreatedTokenStream);
				CreatedParser.RemoveErrorListeners();
				CreatedParser.ErrorHandler = new XmlConsumerErrorStrategy();
				
				//
				// GET THE SUBTREE THAT WE WILL PARSE AGAINST.
				// IN THIS CASE, IT'S THE WHOLE EXPRESSION TREE THAT WE WILL BE EVALUATING.
				//
				IParseTree Tree = CreatedParser.xmlConsumerExpressionFile();

				//
				// CREATE AND CONFIGURE THE LISTENER.
				//
				_Visitor.ExpressionType = _ExpressionType;
				_Visitor.ContextObject = _TargetObject;
				_Visitor.SourceNodes = new XmlNode[]{_SourceXmlDocument.DocumentElement};
				_Visitor.NamespaceManager = new XmlNamespaceManager(_SourceXmlDocument.NameTable);
				_Visitor.AutoParseMode = _AutoParseMode;
				_Visitor.AutoParseMethods = _AutoParseMethods;
				_Visitor.AutoCreateMode = _AutoCreateMode;
				_Visitor.NullObjectTreatmentMode = _NullObjectTreatmentMode;
				_Visitor.Collectivity = _Collectivity;
				_Visitor.Visit(Tree);
			}
		}
	
	
		/// <summary>
		/// Sets the target object that must be populated.
		/// </summary>
		public object TargetObject
		{
			set
			{
				_TargetObject = value;
			}
		}


		public Type TargetObjectType
		{
			set
			{
				_TargetObjectType = value;
			}
		}


		/// <summary>
		/// Sets the root-level expression type for the interpreter.
		/// </summary>
		public XmlConsumerExpressionType ExpressionType
		{
			get
			{
				return _ExpressionType;
			}
		
			set
			{
				_ExpressionType = value;
			}
		}


		/// <summary>
		/// Sets the root-level encoding type for the interpreter.
		/// </summary>
		public Encoding Encoding
		{
			get
			{
				return _Encoding;
			}

			set
			{
				_Encoding = value;
			}
		}
		
		
		/// <summary>
		/// Sets the source XML document to be used to execute the mapping script against.
		/// </summary>
		public XmlDocument SourceXmlDocument
		{
			set
			{
				_SourceXmlDocument = value;
			}
		}


		/// <summary>
		/// Sets the source XML string to be used to execute the mapping script against.
		/// </summary>
		public string SourceXmlString
		{
			set
			{
				_SourceXmlString = value;
			}
		}


		/// <summary>
		/// Sets the source XML binary to be used to execute the mapping script against.
		/// </summary>
		public byte[] SourceXmlBinaryData
		{
			set
			{
				_SourceXmlBinaryData = value;
			}
		}


		public string MappingScriptFilePath
		{
			set
			{
				_MappingScriptFilePath = value;
			}
		}


		public string MappingScriptString
		{
			set
			{
				_MappingScriptString = value;
			}
		}


		public AutoParseMode AutoParseMode
		{
			set
			{
				_AutoParseMode = value;
			}
		}


		public bool AllowMappingScriptEncodingOverride
		{
			set
			{
				_AllowMappingScriptEncodingOverride = value;
			}
		}
	
	
		//
		// INPUT FIELDS.
		//
		private object _TargetObject;
		private Type _TargetObjectType;
		
		private XmlConsumerExpressionType _ExpressionType;
		private Encoding _Encoding;
		private Encoding _MappingScriptEncoding;
		private bool _AllowMappingScriptEncodingOverride;
		
		private XmlDocument _SourceXmlDocument;
		private string _SourceXmlString;
		private byte[] _SourceXmlBinaryData;
		
		private string _MappingScriptFilePath;
		private string _MappingScriptString;
		
		private AutoParseMode _AutoParseMode;
		private AutoCreateMode _AutoCreateMode;
		private NullObjectTreatmentMode _NullObjectTreatmentMode;
		private Collectivity _Collectivity;
		
		//
		// OPERATIONAL FIELDS.
		//
		private XmlConsumerVisitor _Visitor;
		
		//
		// TO DO: PARSE METHODS CONVERT THE SOURCE STRING INTO ITS TARGET DATA TYPE IN THE .NET FRAMEWORK.
		//
		private Dictionary<Type, AutoParseDelegate> _AutoParseMethods;
	}
}
