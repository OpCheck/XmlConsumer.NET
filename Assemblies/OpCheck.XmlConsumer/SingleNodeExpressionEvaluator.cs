using System;
using System.Collections.Generic;

using System.Xml;

namespace OpCheck.XmlConsumer
{
	/// <summary>
	/// Evaluates an XPath expression against a single XML node.
	/// </summary>
	public class SingleNodeExpressionEvaluator
	{
		public XmlNode[] Evaluate (XmlNode TargetNode)
		{
			//
			// CREATE THE LIST OF RESULT NODES.
			//
			List<XmlNode> ResultNodeList = new List<XmlNode>();
		
			//
			// EVALUATE THE XPATH EXPRESSION AGAINST THE TARGET NODE.
			//
			foreach (XmlNode ResultNode in TargetNode.SelectNodes(_Expression, _NamespaceManager))
			{
				ResultNodeList.Add(ResultNode);
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
