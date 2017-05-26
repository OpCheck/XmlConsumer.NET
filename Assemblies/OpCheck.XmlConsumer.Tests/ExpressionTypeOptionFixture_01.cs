using System;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	[TestFixture]
	public class ExpressionTypeOptionFixture_01
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\ExpressionTypeOptionFixture_01";
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
		}


		[Test]
		public void TestBuilding ()
		{
			Assert.AreEqual(_Building.Name, "Constant Building Name");
		}

		
		private Building _Building;
	}
}
