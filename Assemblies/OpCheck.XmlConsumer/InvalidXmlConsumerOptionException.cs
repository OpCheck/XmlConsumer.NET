using System;

namespace OpCheck.XmlConsumer
{
	public class InvalidXmlConsumerOptionException : Exception
	{
		public InvalidXmlConsumerOptionException () : base ()
		{
		}


		public override string Message
		{
			get
			{
				return String.Format("Option: '{0}'. Invalid value '{1}'.", _OptionName, _OptionValue);
			}
		}


		public string OptionName
		{
			set
			{
				_OptionName = value; 
			}
		}


		public string OptionValue
		{
			set
			{
				_OptionValue = value; 
			}
		}

		
		private string _OptionName;
		private string _OptionValue;
	}
}
