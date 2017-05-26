using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpCheck.XmlConsumer
{
	public class XPathDataExpressionEvaluator : DataExpressionEvaluator
	{
		public override string Evaluate ()
		{
			//
			// SPECIAL CASE FOR XPATH: AN EMPTY STRING EXPRESSION RETURNS AN EMPTY STRING.
			//
			if (_Expression.Equals(String.Empty))
				return "";
		
			//
			// START THE RESULTS.
			//
			StringBuilder ResultsBuilder = new StringBuilder();
		
			//
			// APPLY THE XPATH EXPRESSION TO THE SOURCE NODES.
			//
			MultiNodeExpressionEvaluator CreatedEvaluator = new MultiNodeExpressionEvaluator();
			CreatedEvaluator.Expression = _Expression;
			CreatedEvaluator.NamespaceManager = _NamespaceManager;
			XmlNode[] ResultNodes = CreatedEvaluator.Evaluate(_SourceNodes);
			
			foreach (XmlNode CurrentNode in ResultNodes)
			{
				ResultsBuilder.AppendFormat("{0}", XmlNodeValueExtractor.ExtractValue(CurrentNode));
			}
			
			return ResultsBuilder.ToString();
		}


		public override string[] EvaluateForArray ()
		{
			//
			// APPLY THE XPATH EXPRESSION TO THE SOURCE NODES.
			//
			MultiNodeExpressionEvaluator CreatedEvaluator = new MultiNodeExpressionEvaluator();
			CreatedEvaluator.Expression = _Expression;
			CreatedEvaluator.NamespaceManager = _NamespaceManager;
			XmlNode[] ResultNodes = CreatedEvaluator.Evaluate(_SourceNodes);
			
			//
			// CREATE THE LIST OF RESULTS.
			//
			List<string> ResultStringList = new List<string>();
			
			foreach (XmlNode CurrentNode in ResultNodes)
			{
				if (_Expression.Equals(String.Empty))
					ResultStringList.Add(String.Empty);
				else
					ResultStringList.Add(XmlNodeValueExtractor.ExtractValue(CurrentNode));
			}
			
			return ResultStringList.ToArray();
		}
	}
}
