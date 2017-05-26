using System;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests.XmlConsumerInterpreterFixtures
{
	[TestFixture]
	public class ArrayFixture_02_Floors
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD THE PATHS TO THE FILE.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\ArrayFixture_02_Floors";
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
			Assert.IsNotNull(_Building.Floors);
			Assert.AreEqual(_Building.Floors.Length, 2);
			
			Assert.AreEqual(_Building.Floors[0], 1);
			Assert.AreEqual(_Building.Floors[1], 2);
		}

		
		private Building _Building;
	}
}
