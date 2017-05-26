using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpCheck.XmlConsumer.MappingCommands
{
	public abstract class MappingCommand
	{
		public abstract void Execute ();


		public string MemberIdentifier
		{
			set
			{
				_MemberIdentifier = value;
			}
		}


		public object ContextObject
		{
			set
			{
				_ContextObject = value;
			}
		}


		public OptionsTuple OptionsTuple
		{
			set
			{
				_OptionsTuple = value;
			}
		}


		public string Expression
		{
			set
			{
				_Expression = value;
			}
		}


		public XmlNode[] SourceNodes
		{
			set
			{
				_SourceNodes = value;
			}
		}


		public XmlNamespaceManager NamespaceManager
		{
			set
			{
				_NamespaceManager = value;
			}
		}


		public AutoParseMode AutoParseMode
		{
			set
			{
				_AutoParseMode = value;
			}
		}


		public Dictionary<Type, AutoParseDelegate> AutoParseMethods
		{
			set
			{
				_AutoParseMethods = value;
			}
		}
		
		
		protected object _ContextObject;
		protected string _MemberIdentifier;
		protected OptionsTuple _OptionsTuple;
		protected string _Expression;
		protected XmlNode[] _SourceNodes;
		protected XmlNamespaceManager _NamespaceManager;
		protected AutoParseMode _AutoParseMode;
		protected Dictionary<Type, AutoParseDelegate> _AutoParseMethods;
	}
}
