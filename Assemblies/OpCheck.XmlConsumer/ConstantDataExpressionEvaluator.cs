namespace OpCheck.XmlConsumer
{
	public class ConstantDataExpressionEvaluator : DataExpressionEvaluator
	{
		public override string Evaluate ()
		{
			return _Expression;
		}


		public override string[] EvaluateForArray ()
		{
			return new string[]{_Expression};
		}
	}
}
