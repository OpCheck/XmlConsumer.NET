using Antlr4.Runtime.Tree;

namespace OpCheck.XmlConsumer
{
	/// <summary>
	/// Reads terminal nodes.
	/// </summary>
	public class TerminalNodeReader
	{
		public string GetIdentifier ()
		{
			return GetStringTerminal();
		}


		public string GetString ()
		{
			return StringTerminalReader.Unescape(StringTerminalReader.Unwrap(GetStringTerminal()));
		}
		
		
		public string GetStringTerminal ()
		{
			TokenReader CreatedReader = new TokenReader();
			CreatedReader.Token = _TerminalNode.Symbol;
			return CreatedReader.GetText();
		}
	
	
		public ITerminalNode TerminalNode
		{
			set
			{
				_TerminalNode = value;
			}
		}


		public ITerminalNode _TerminalNode;
	}
}
