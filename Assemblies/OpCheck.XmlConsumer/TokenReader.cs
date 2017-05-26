using Antlr4.Runtime;

namespace OpCheck.XmlConsumer
{
	public class TokenReader
	{
		public string GetText ()
		{
			return _Token.Text;
		}

	
		public IToken Token
		{
			set
			{
				_Token = value;
			}
		}
		
		
		private IToken _Token;
	}
}
