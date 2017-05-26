using System;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	/// <summary>
	/// This is an invalid script.
	/// This should fail.
	/// </summary>
	[TestFixture]
	public class Fixture_00
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD PATHS.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\Fixture_00";
			string SourceXmlFilePath = FixtureBaseDir + "\\SourceXmlFile.xml";
			string MappingScriptFilePath = FixtureBaseDir + "\\XmlConsumerScriptFile.txt";

			//
			// LOAD THE SOURCE DOCUMENT.
			//
			XmlDocument SourceXmlDocument = new XmlDocument();
			SourceXmlDocument.Load(SourceXmlFilePath);

			//
			// CREATE THE TARGET OBJECT.
			//

			//
			// CREATE THE INTERPRETER.
			//
			_CreatedInterpreter = new XmlConsumerInterpreter();
			_CreatedInterpreter.Init();
			_CreatedInterpreter.SourceXmlDocument = SourceXmlDocument;
			_CreatedInterpreter.MappingScriptFilePath = MappingScriptFilePath;
			_CreatedInterpreter.TargetObject = new TargetObject();
		}


		[Test]
		[ExpectedException(typeof(XmlConsumerParseException))]
		public void TestParse ()
		{
			_CreatedInterpreter.Execute();
		}

		
		XmlConsumerInterpreter _CreatedInterpreter;
	}
}
