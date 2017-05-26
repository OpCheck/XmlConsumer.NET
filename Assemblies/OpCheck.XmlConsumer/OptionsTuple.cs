using System.Text;
using System.Xml;

namespace OpCheck.XmlConsumer
{
	public class OptionsTuple
	{
		public XmlConsumerExpressionType ExpressionType;
		public AutoParseMode AutoParseMode;
		public XmlNode[] SourceNodes;
		public AutoCreateMode AutoCreateMode;
		public NullObjectTreatmentMode NullObjectTreatmentMode;
		public Collectivity Collectivity;
	}
}
