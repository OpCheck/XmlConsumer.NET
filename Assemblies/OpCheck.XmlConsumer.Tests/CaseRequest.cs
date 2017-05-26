using System;
using System.Collections.Generic;

namespace OpCheck.XmlConsumer.Tests
{
	public class CaseRequest
	{
		public CaseRequest ()
		{
			BillingCodes = new List<string>();
			AdditionalFields = new Dictionary<string, string>();
		}
	
	
		public int CaseRequestId;
		public DateTime RequestDateTime;
		public bool Active;
		
		public long ExternalRequestId;
		public double AccountBalance;
		
		public Address PrimaryAddress;
		
		public Subject PrimarySubject;
		
		//
		// A LIST OF BILLING CODES TO BE LOADED.
		//
		public List<string> BillingCodes;
		
		//
		// A DICTIONARY OF ADDITIONAL FIELDS TO BE LOADED.
		//
		public Dictionary<string, string> AdditionalFields;
	}
}
