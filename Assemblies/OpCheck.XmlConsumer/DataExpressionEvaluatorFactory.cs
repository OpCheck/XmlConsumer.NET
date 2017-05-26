using System;

namespace OpCheck.XmlConsumer
{
	public class DataExpressionEvaluatorFactory
	{
		public static DataExpressionEvaluator CreateEvaluator (XmlConsumerExpressionType ExpressionType)
		{
			return (DataExpressionEvaluator)Activator.CreateInstance(null, String.Format("OpCheck.XmlConsumer.{0}DataExpressionEvaluator", ExpressionType)).Unwrap();
		}
	}
}
