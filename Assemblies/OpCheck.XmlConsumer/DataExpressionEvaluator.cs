using System.Text;
using System.Xml;

namespace OpCheck.XmlConsumer
{
	public abstract class DataExpressionEvaluator
	{
		/// <summary>
		/// Evalutes an expression for a single object.
		/// </summary>
		public abstract string Evaluate ();
		
		
		/// <summary>
		/// Evalutes an expression for an array or collection.
		/// </summary>
		public abstract string[] EvaluateForArray ();
	
	
		public XmlNode[] SourceNodes
		{
			set
			{
				_SourceNodes = value;
			}
		}


		public string Expression
		{
			set
			{
				_Expression = value;
			}
		}


		public XmlNamespaceManager NamespaceManager
		{
			set
			{
				_NamespaceManager = value;
			}
		}


		protected XmlNode[] _SourceNodes;
		protected string _Expression;
		protected XmlNamespaceManager _NamespaceManager;
	}
}
