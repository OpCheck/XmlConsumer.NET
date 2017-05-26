using System;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	/// <summary>
	/// Tests the null mapping keyword.
	/// We set a field to null in this test using the script.
	/// </summary>
	[TestFixture]
	public class Fixture_14
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\Fixture_14";
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
			_TargetObject.BillingCode1 = "BillingCode1";
			_TargetObject.BillingCode2 = "BillingCode2";
		
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
			Assert.IsNull(_TargetObject.BillingCode1);
			Assert.IsNull(_TargetObject.BillingCode2);
		}

		
		private XmlConsumerInterpreter _CreatedInterpreter;
		private TargetObject _TargetObject;
	}
}
