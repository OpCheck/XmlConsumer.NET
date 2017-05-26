using System;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	[TestFixture]
	public class ComplexFixture_02
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\ComplexFixture_02";
			string SourceXmlFilePath = FixtureBaseDir + "\\SourceXmlFile.xml";
			string ScriptFilePath = FixtureBaseDir + "\\XmlConsumerScriptFile.txt";
		
			//
			// LOAD THE SOURCE DOCUMENT.
			//
			XmlDocument SourceXmlDocument = new XmlDocument();
			SourceXmlDocument.Load(SourceXmlFilePath);
		
			//
			// CREATE THE TARGET OBJECT.
			//
			_Building = new Building();
			_Building.DeedDocument = new byte[]{};

			//
			// CREATE THE INTERPRETER AND EXECUTE THE SCRIPT.
			//
			_CreatedInterpreter = new XmlConsumerInterpreter();
			_CreatedInterpreter.Init();
			_CreatedInterpreter.SourceXmlDocument = SourceXmlDocument;
			_CreatedInterpreter.MappingScriptFilePath = ScriptFilePath;
			_CreatedInterpreter.TargetObject = _Building;
			_CreatedInterpreter.Execute();
			
			//
			// GET A REFERENCE TO THE BUILDING'S CHILD OBJECTS.
			//
			_Address = _Building.Address;
			_Tenant = _Building.Tenant;
			_Offices = _Building.Offices;
		}


		[Test]
		public void TestBuilding ()
		{
			Assert.IsNotNull(_Building.Name);
			Assert.AreEqual(_Building.Name, "Curtis Center");
			
			Assert.AreEqual(_Building.StreetNumber, 510);
			Assert.AreEqual(_Building.PropertyId, Int64.Parse("998298347"));
			Assert.AreEqual(_Building.Latitude, Single.Parse("16.5"));
			Assert.AreEqual(_Building.ListPrice, Double.Parse("500345.2345"));
			Assert.IsFalse(_Building.IsResidential);
			Assert.IsNull(_Building.DeedDocument);
			
			//
			// CHECK FLOORS.
			//
			Assert.IsNotNull(_Building.Floors);
			Assert.AreEqual(_Building.Floors.Length, 2);
			Assert.AreEqual(_Building.Floors[0], 1);
			Assert.AreEqual(_Building.Floors[1], 2);
		}


		[Test]
		public void TestAddress ()
		{
			Assert.IsNotNull(_Address);
			Assert.AreEqual(_Address.Line1, "510 Walnut Street");
			Assert.AreEqual(_Address.City, "Philadelphia");
			Assert.AreEqual(_Address.State, "PA");
			Assert.AreEqual(_Address.ZipCode, "19135");
		}


		[Test]
		public void TestTenant ()
		{
			Assert.IsNotNull(_Tenant);
			Assert.AreEqual(_Tenant.CompanyName, "ABIM");
		}


		[Test]
		public void TestOffices ()
		{
			Assert.IsNotNull(_Offices);
			Assert.AreEqual(_Offices.Length, 2);
		}

		
		private XmlConsumerInterpreter _CreatedInterpreter;
		private Building _Building;
		private Address _Address;
		private Tenant _Tenant;
		private Office[] _Offices;
	}
}
