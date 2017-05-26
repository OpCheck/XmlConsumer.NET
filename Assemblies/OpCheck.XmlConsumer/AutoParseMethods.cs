using System;

namespace OpCheck.XmlConsumer
{
	/// <summary>
	/// Built-in auto parse methods.
	/// </summary>
	public class AutoParseMethods
	{
		public static object ParseString (string CurrentArgString)
		{
			return CurrentArgString;
		}

	
		public static object ParseInt32 (string CurrentArgString)
		{
			return Int32.Parse(CurrentArgString);
		}


		public static object ParseInt64 (string CurrentArgString)
		{
			return Int64.Parse(CurrentArgString);
		}


		public static object ParseSingle (string CurrentArgString)
		{
			return Single.Parse(CurrentArgString);
		}


		public static object ParseDouble (string CurrentArgString)
		{
			return Double.Parse(CurrentArgString);
		}


		public static object ParseBoolean (string CurrentArgString)
		{
			return Boolean.Parse(CurrentArgString);
		}


		public static object ParseByte (string CurrentArgString)
		{
			return Byte.Parse(CurrentArgString);
		}


		public static object ParseByteArray (string CurrentArgString)
		{
			return Convert.FromBase64String(CurrentArgString);
		}
		
		
		public static object ParseDateTime (string CurrentArgString)
		{
			return DateTime.Parse(CurrentArgString);
		}
	}
}
