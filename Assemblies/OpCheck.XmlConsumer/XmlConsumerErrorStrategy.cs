using Antlr4.Runtime;

namespace OpCheck.XmlConsumer
{
	/// <summary>
	/// Reports all errors.  No recovery.
	/// </summary>
	public class XmlConsumerErrorStrategy : DefaultErrorStrategy
	{
		public override void Recover (Parser P, RecognitionException E)
		{
			//
			// DO NOT RECOVER.
			//
			throw new XmlConsumerParseException(E);
		}


		public override IToken RecoverInline (Parser P)
		{
			//
			// DO NOT RECOVER.
			//
			throw new XmlConsumerParseException(new InputMismatchException(P));
		}


		public override void Sync (Parser recognizer)
		{
			//
			// DO NOT ATTEMPT TO RECOVER FROM PROBLEMS IN SUBRULES.
			//
		}
	}
}
