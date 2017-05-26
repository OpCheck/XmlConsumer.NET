using System;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	[TestFixture]
	public class ChildObjectFixture_01_Address
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\ChildObjectFixture_01_Address";
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

			//
			// CREATE THE INTERPRETER AND EXECUTE THE SCRIPT.
			//
			XmlConsumerInterpreter CreatedInterpreter = new XmlConsumerInterpreter();
			CreatedInterpreter.Init();
			CreatedInterpreter.SourceXmlDocument = SourceXmlDocument;
			CreatedInterpreter.MappingScriptFilePath = ScriptFilePath;
			CreatedInterpreter.TargetObject = _Building;
			CreatedInterpreter.Execute();
			
			_Address = _Building.Address;
		}


		[Test]
		public void TestBuilding ()
		{
			Assert.IsNotNull(_Building.Address);
		}


		[Test]
		public void TestAddress ()
		{
			Assert.IsNotNull(_Address);

			Assert.IsNotNull(_Address.Line1);
			Assert.AreEqual(_Address.Line1, "510 Walnut Street");

			Assert.IsNotNull(_Address.City);
			Assert.AreEqual(_Address.City, "Philadelphia");
			
			Assert.IsNotNull(_Address.State);
			Assert.AreEqual(_Address.State, "PA");
			
			Assert.IsNotNull(_Address.ZipCode);
			Assert.AreEqual(_Address.ZipCode, "19135");
		}

		
		private Building _Building;
		private Address _Address;
	}
}
