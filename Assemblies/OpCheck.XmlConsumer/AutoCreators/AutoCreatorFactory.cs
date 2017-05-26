using System;

namespace OpCheck.XmlConsumer.AutoCreators
{
	/// <summary>
	/// Creates an instance of an auto creator using the specified auto create mode.
	/// The auto create mode is an option in the XML Consumer Language.
	/// </summary>
	public class AutoCreatorFactory
	{
		public static AutoCreator CreateAutoCreator (AutoCreateMode AutoCreateModeArg)
		{
			return (AutoCreator)Activator.CreateInstance(null, String.Format("OpCheck.XmlConsumer.AutoCreators.{0}AutoCreator", AutoCreateModeArg)).Unwrap();
		}
	}
}
