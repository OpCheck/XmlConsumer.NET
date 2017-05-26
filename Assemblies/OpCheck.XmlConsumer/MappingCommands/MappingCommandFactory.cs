using System;

namespace OpCheck.XmlConsumer.MappingCommands
{
	public class MappingCommandFactory
	{
		public static MappingCommand CreateCommand (MappingStrategy CurrentStrategy)
		{
			return (MappingCommand)Activator.CreateInstance(null, String.Format("OpCheck.XmlConsumer.MappingCommands.{0}MappingCommand", CurrentStrategy)).Unwrap();
		}
	}
}
