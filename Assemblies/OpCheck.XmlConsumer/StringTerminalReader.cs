namespace OpCheck.XmlConsumer
{
	public class StringTerminalReader
	{
		public static string Unwrap (string TargetString)
		{
			return TargetString.Substring(1, TargetString.Length - 2);
		}


		public static string Unescape (string TargetString)
		{
			return TargetString.Replace("\\'", "'");
		}
	}
}
