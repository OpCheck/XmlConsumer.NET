using System;
using System.Collections.Generic;
using System.Text;

namespace OpCheck.XmlConsumer
{
	public class EncodingFactory
	{
		static EncodingFactory ()
		{
			_EncodingMap = new Dictionary<string, Encoding>();
			_EncodingMap["ascii"] = Encoding.ASCII;
			_EncodingMap["utf8"] = Encoding.UTF8;
		}
		
		
		public static Encoding GetEncodingByName (string LowerCaseName)
		{
			if (!_EncodingMap.ContainsKey(LowerCaseName))
				return null;
				
			return _EncodingMap[LowerCaseName];
		}
		
		
		private static Dictionary<string, Encoding> _EncodingMap;
	}
}
