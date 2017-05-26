using System;
using System.Collections.Generic;

using System.Xml;

namespace OpCheck.XmlConsumer
{
	/// <summary>
	/// Evaluates an XPath expression against a set of XML nodes.
	/// </summary>
	public class MultiNodeExpressionEvaluator
	{
		public XmlNode[] Evaluate (XmlNode[] SourceNodes)
		{
			//
			// CREATE THE RESULT NODE LIST.
			//
			List<XmlNode> ResultNodeList = new List<XmlNode>();
			
			//
			// CREATE THE SINGLE NODE EXPRESSION EVALUATOR.
			//
			SingleNodeExpressionEvaluator CreatedEvaluator = new SingleNodeExpressionEvaluator();
			CreatedEvaluator.NamespaceManager = _NamespaceManager;
			CreatedEvaluator.Expression = _Expression;
			
			//
			// APPLY THE XPATH EXPRESSION TO EACH
			//
			foreach (XmlNode CurrentSourceNode in SourceNodes)
			{
				ResultNodeList.AddRange(CreatedEvaluator.Evaluate(CurrentSourceNode));
			}
			
			return ResultNodeList.ToArray();
		}


		public string Expression
		{
			set
			{
				_Expression = value;
			}
		}


		public XmlNamespaceManager NamespaceManager
		{
			set
			{
				_NamespaceManager = value;
			}
		}


		private string _Expression;
		private XmlNamespaceManager _NamespaceManager;
	}
}
