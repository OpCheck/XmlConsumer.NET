using System;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	/// <summary>
	/// Tests child object assignment using inherited options.
	/// </summary>
	[TestFixture]
	public class Fixture_12
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\Fixture_12";
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
			_TargetObject.ChildObject = new TargetChildObject();
		
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
		public void TestChildObject ()
		{
			Assert.IsNotNull(_TargetObject.ChildObject.FirstName);
			Assert.AreEqual(_TargetObject.ChildObject.FirstName, "Cory");
		}

		
		private XmlConsumerInterpreter _CreatedInterpreter;
		private TargetObject _TargetObject;
	}
}
