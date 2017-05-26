namespace OpCheck.XmlConsumer
{
	/// <summary>
	/// Describes what the language application does when it discovers that the next context object is null.
	/// </summary>
	public enum NullObjectTreatmentMode
	{
		/// <summary>
		/// Throw an exception.
		/// </summary>
		Throw,
		
		/// <summary>
		/// Skip the object - do not process the mapping for it.
		/// </summary>
		Skip,
		
		/// <summary>
		/// Fuggit - let's see what happens!
		/// </summary>
		Ignore
	}
}
