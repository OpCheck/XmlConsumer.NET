using System;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	[TestFixture]
	public class ArrayFixture_01_Offices
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\ArrayFixture_01_Offices";
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
			_CreatedInterpreter = new XmlConsumerInterpreter();
			_CreatedInterpreter.Init();
			_CreatedInterpreter.SourceXmlDocument = SourceXmlDocument;
			_CreatedInterpreter.MappingScriptFilePath = ScriptFilePath;
			_CreatedInterpreter.TargetObject = _Building;
			_CreatedInterpreter.Execute();
			
			//
			// GET A REFERENCE TO THE BUILDING'S CHILD OBJECTS.
			//
			_Offices = _Building.Offices;
		}


		[Test]
		public void TestOffices ()
		{
			Assert.IsNotNull(_Offices);
			Assert.AreEqual(_Offices.Length, 2);
		}

		[Test]
		public void TestOffice_0 ()
		{
			Office CurrentOffice = _Offices[0];
			Assert.IsNotNull(CurrentOffice);
			Assert.AreEqual(CurrentOffice.SquareFeet, 1000);
		}

		[Test]
		public void TestOffice_1 ()
		{
			Office CurrentOffice = _Offices[1];
			Assert.IsNotNull(CurrentOffice);
			Assert.AreEqual(CurrentOffice.SquareFeet, 2000);
		}

		
		private XmlConsumerInterpreter _CreatedInterpreter;
		private Building _Building;
		private Office[] _Offices;
	}
}
