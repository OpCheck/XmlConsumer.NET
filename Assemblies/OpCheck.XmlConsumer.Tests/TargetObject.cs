namespace OpCheck.XmlConsumer.Tests
{
	public class TargetObject
	{
		public string BillingCode1;


		public string BillingCode2
		{
			get
			{
				return _BillingCode2;
			}
		
			set
			{
				_BillingCode2 = value;
			}
		}

		
		public string GetBillingCode3 ()
		{
			return _BillingCode3;
		}

		
		public void SetBillingCode3 (string Arg)
		{
			_BillingCode3 = Arg;
		}
		
		
		public TargetChildObject ChildObject;

		
		private string _BillingCode2;
		private string _BillingCode3;
	}
}
