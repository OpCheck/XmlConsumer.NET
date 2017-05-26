using System;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	[TestFixture]
	public class NullMappingFixture_02
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\NullMappingFixture_02";
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
			_Building.Name = "Curtis Center";

			//
			// CREATE THE INTERPRETER AND EXECUTE THE SCRIPT.
			//
			XmlConsumerInterpreter CreatedInterpreter = new XmlConsumerInterpreter();
			CreatedInterpreter.Init();
			CreatedInterpreter.SourceXmlDocument = SourceXmlDocument;
			CreatedInterpreter.MappingScriptFilePath = ScriptFilePath;
			CreatedInterpreter.TargetObject = _Building;
			CreatedInterpreter.Execute();
		}


		[Test]
		public void TestBuilding ()
		{
			Assert.IsNull(_Building.Name);
		}

		
		private Building _Building;
	}
}
