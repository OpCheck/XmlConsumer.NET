using System;
using System.Collections;

namespace OpCheck.XmlConsumer
{
	public class ContextInfo
	{
		/// <summary>
		/// The context object.
		/// </summary>
		public object Object;
		

		/// <summary>
		/// How the system is treating the context object.
		/// </summary>
		public MappingStrategy MappingStrategy;
	}
}
