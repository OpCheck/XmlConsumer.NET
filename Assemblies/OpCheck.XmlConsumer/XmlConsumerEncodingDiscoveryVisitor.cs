using System;
using System.Text;

using Antlr4.Runtime.Tree;

namespace OpCheck.XmlConsumer
{
	/// <summary>
	/// This visitor determines which encoding we should use to parse binary string data.
	/// </summary>
	public class XmlConsumerEncodingDiscoveryVisitor : XmlConsumerBaseVisitor<Encoding>
	{
		public override Encoding VisitXmlConsumerExpressionFile (XmlConsumerParser.XmlConsumerExpressionFileContext CurrentContext)
		{
			return VisitXmlConsumerScript(CurrentContext.xmlConsumerScript());
		}


		public override Encoding VisitXmlConsumerScript (XmlConsumerParser.XmlConsumerScriptContext CurrentContext)
		{
			if (CurrentContext.optionsTuple() != null)
				return VisitOptionsTuple(CurrentContext.optionsTuple());
			
			//
			// NO OPTIONS TUPLE SPECIFIED SO NO ENCODING CAN BE FOUND.
			//
			return null;
		}


		public override Encoding VisitOptionsTuple (XmlConsumerParser.OptionsTupleContext CurrentContext)
		{
			return VisitOptionsList(CurrentContext.optionsList());
		}


		public override Encoding VisitOptionsList (XmlConsumerParser.OptionsListContext CurrentContext)
		{
			foreach (XmlConsumerParser.OptionContext CurrentOptionContext in CurrentContext.option())
			{
				if (CurrentOptionContext is XmlConsumerParser.EncodingOptionContext)
					return VisitEncodingOption((XmlConsumerParser.EncodingOptionContext)CurrentOptionContext);
			}
			
			return null;
		}


		public override Encoding VisitEncodingOption (XmlConsumerParser.EncodingOptionContext CurrentOptionContext)
		{
			//
			// GET THE ENCODING NAME.
			//
			TerminalNodeReader CreatedReader = new TerminalNodeReader();
			CreatedReader.TerminalNode = CurrentOptionContext.STRING_LITERAL();
			string EncodingName = CreatedReader.GetString();
			
			//
			// GET THE ENCODING.
			//
			Encoding CurrentEncoding = EncodingFactory.GetEncodingByName(EncodingName.ToLower());
			
			//
			// IF THE SPECIFIED ENCODING IS NOT SUPPORTED THIS IS AN ERROR.
			//
			if (CurrentEncoding == null)
			{
				InvalidXmlConsumerOptionException CreatedException = new InvalidXmlConsumerOptionException();
				CreatedException.OptionName = "Encoding";
				CreatedException.OptionValue = EncodingName;
				throw CreatedException;
			}
			
			return CurrentEncoding;
		}
	}
}
