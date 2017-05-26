using System;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	/// <summary>
	/// Tests child object assignment using overridden options.
	/// </summary>
	[TestFixture]
	public class Fixture_13
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\Fixture_13";
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
			TargetChildObject Subject = _TargetObject.ChildObject;
		
			Assert.IsNotNull(Subject.FirstName);
			Assert.AreEqual(Subject.FirstName, "Eric");

			Assert.IsNotNull(Subject.LastName);
			Assert.AreEqual(Subject.LastName, "Walker");

			Assert.AreEqual(Subject.BirthDateTime, DateTime.Parse("1985-01-25T00:00:00"));
			Assert.AreEqual(Subject.YearsOld, 31);
			
			Assert.IsNull(Subject.GrandChildObject);
		}

		
		private XmlConsumerInterpreter _CreatedInterpreter;
		private TargetObject _TargetObject;
	}
}
