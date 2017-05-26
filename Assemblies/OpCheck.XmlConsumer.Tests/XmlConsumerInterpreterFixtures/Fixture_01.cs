using System;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	[TestFixture]
	public class Fixture_01
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\Fixture_01";
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
			_TargetObject = new TargetObject();
		
			//
			// CREATE THE INTERPRETER AND EXECUTE THE SCRIPT.
			//
			XmlConsumerInterpreter CreatedInterpreter = new XmlConsumerInterpreter();
			CreatedInterpreter.Init();
			CreatedInterpreter.SourceXmlDocument = SourceXmlDocument;
			CreatedInterpreter.MappingScriptFilePath = ScriptFilePath;
			CreatedInterpreter.TargetObject = _TargetObject;
			CreatedInterpreter.Execute();
		}


		[Test]
		public void TestContextObject ()
		{
			Assert.IsNotNull(_TargetObject);
			Assert.IsNull(_TargetObject.BillingCode1);
		}
		
		
		private TargetObject _TargetObject;
	}
}
