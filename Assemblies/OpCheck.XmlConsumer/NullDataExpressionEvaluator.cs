namespace OpCheck.XmlConsumer
{
	public class NullDataExpressionEvaluator : DataExpressionEvaluator
	{
		public override string Evaluate ()
		{
			return null;
		}


		public override string[] EvaluateForArray ()
		{
			return null;
		}
	}
}
