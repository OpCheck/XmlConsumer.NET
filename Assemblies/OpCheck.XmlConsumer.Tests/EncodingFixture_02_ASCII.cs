using System;
using System.IO;
using System.Text;
using System.Xml;

using MbUnit.Framework;

namespace OpCheck.XmlConsumer.Tests
{
	[TestFixture]
	public class EncodingFixture_02_ASCII
	{
		[TestFixtureSetUp]
		public void SetUp ()
		{
			//
			// BUILD PATHS.
			//
			string FixtureBaseDir = FixtureConfig.BaseDir + "\\EncodingFixture_02_ASCII";
			string SourceXmlFilePath = FixtureBaseDir + "\\SourceXmlFile.xml";
			string MappingScriptFilePath = FixtureBaseDir + "\\XmlConsumerScriptFile.txt";
		
			//
			// LOAD THE SOURCE DOCUMENT AS AN ARRAY OF BYTES.
			//
			byte[] SourceXmlBinaryData;
			
			using (FileStream XmlFileFileStream = new FileStream(SourceXmlFilePath, FileMode.Open, FileAccess.Read))
			{
				using (MemoryStream TargetStream = new MemoryStream())
				{
					byte[] Buffer = new byte[1024];
					int BytesRead;
					
					while((BytesRead = XmlFileFileStream.Read(Buffer, 0, Buffer.Length)) > 0)
						TargetStream.Write(Buffer, 0, BytesRead);
						
					SourceXmlBinaryData = TargetStream.ToArray();
				}
			}
			
			_Building = new Building();
		
			//
			// CREATE THE INTERPRETER AND EXECUTE THE SCRIPT.
			//
			_CreatedInterpreter = new XmlConsumerInterpreter();
			_CreatedInterpreter.Init();
			_CreatedInterpreter.SourceXmlBinaryData = SourceXmlBinaryData;
			_CreatedInterpreter.MappingScriptFilePath = MappingScriptFilePath;
			_CreatedInterpreter.TargetObject = _Building;
			_CreatedInterpreter.Execute();
		}


		[Test]
		public void TestBuilding ()
		{
			Assert.IsNotNull(_Building.Name);
			Assert.AreNotEqual(_Building.Name, "昨夜のコンサートは最高でした。");
		}


		private XmlConsumerInterpreter _CreatedInterpreter;
		private Building _Building;
	}
}
