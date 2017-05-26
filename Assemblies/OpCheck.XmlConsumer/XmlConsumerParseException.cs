using System;

namespace OpCheck.XmlConsumer
{
	public class XmlConsumerParseException : Exception
	{
		public XmlConsumerParseException (Exception E) : base("An XML Consumer parsing exception occurred.", E)
		{}
	}
}
