using System;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	[TestFixture]
	public class Fixture_11
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\Fixture_11";
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
			_TargetObject = new TargetChildObject();
		
			//
			// CREATE THE INTERPRETER AND EXECUTE THE SCRIPT.
			//
			_CreatedInterpreter = new XmlConsumerInterpreter();
			_CreatedInterpreter.Init();
			_CreatedInterpreter.SourceXmlDocument = SourceXmlDocument;
			_CreatedInterpreter.MappingScriptFilePath = ScriptFilePath;
			_CreatedInterpreter.TargetObject = _TargetObject;
			_CreatedInterpreter.Execute();
		}


		[Test]
		public void TestTargetObject ()
		{
			Assert.AreEqual(_TargetObject.YearsOld, 33);
			Assert.AreEqual(_TargetObject.BirthDateTime, DateTime.Parse("1975-08-15T00:00:00"));
		}

		
		private XmlConsumerInterpreter _CreatedInterpreter;
		private TargetChildObject _TargetObject;
	}
}
