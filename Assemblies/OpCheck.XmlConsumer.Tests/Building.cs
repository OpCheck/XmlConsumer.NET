using System;
using System.Collections.Generic;

namespace OpCheck.XmlConsumer.Tests
{
	/// <summary>
	/// A building is our top-level parent object that contains all other objects.
	/// </summary>
	public class Building
	{
		public string Name;
		
		public Int32 StreetNumber;
		
		public Int64 PropertyId;
		
		public Single Latitude;
		
		public Double ListPrice;
		
		public bool IsResidential;
		
		public byte[] DeedDocument;
		
		//
		// SIMPLE CHILD OBJECTS.
		//
		public Address Address;
		
		public Tenant Tenant;
	
		//
		// AN ARRAY OF INTEGERS.
		//
		public int[] Floors;
		
		//
		// AN ARRAY OF OBJECTS WITH FIELDS.
		//
		public Office[] Offices;
		
		//
		// A LIST OF TENANTS.
		//
		public List<Tenant> TenantList;
		
		//
		// A FLOOR NUMBER TO NAME MAP.
		//
		public Dictionary<int, string> FloorNumberToNameMap;
	}
}
