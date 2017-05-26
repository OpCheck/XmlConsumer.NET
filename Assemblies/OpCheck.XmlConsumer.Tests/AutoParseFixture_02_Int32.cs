using System;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	[TestFixture]
	public class AutoParseFixture_02_Int32
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\AutoParseFixture_02_Int32";
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
		}


		[Test]
		public void TestBuilding ()
		{
			Assert.AreEqual(_Building.StreetNumber, Int32.Parse("510"));
		}

		
		private XmlConsumerInterpreter _CreatedInterpreter;
		private Building _Building;
		private Office[] _Offices;
	}
}
