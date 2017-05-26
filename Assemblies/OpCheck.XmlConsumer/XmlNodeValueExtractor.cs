using System;
using System.Collections.Generic;
using System.Xml;

namespace OpCheck.XmlConsumer
{
	public class XmlNodeValueExtractor
	{	
		static XmlNodeValueExtractor ()
		{
			_ExtractValueMethods = new Dictionary<Type, ExtractValueDelegate>();
			_ExtractValueMethods[typeof(XmlElement)] = new ExtractValueDelegate(ExtractValueFromElement);
		}
	
	
		public static string ExtractValue (XmlNode TargetNode)
		{
			if (_ExtractValueMethods.ContainsKey(TargetNode.GetType()))
				return _ExtractValueMethods[TargetNode.GetType()](TargetNode);
				
			return TargetNode.Value;
		}
		
		
		public static string ExtractValueFromElement (XmlNode TargetNode)
		{
			return TargetNode.InnerText;
		}
		
		
		private static Dictionary<Type, ExtractValueDelegate> _ExtractValueMethods;
	}
}
