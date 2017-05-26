using System;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	[TestFixture]
	public class Fixture_07
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\Fixture_07";
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
			Assert.IsNotNull(_TargetObject.BillingCode1);
			Assert.AreEqual(_TargetObject.BillingCode1, "770011");

			Assert.IsNotNull(_TargetObject.BillingCode2);
			Assert.AreEqual(_TargetObject.BillingCode2, "337744");
		}

		
		private XmlConsumerInterpreter _CreatedInterpreter;
		private TargetObject _TargetObject;
	}
}
