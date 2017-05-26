namespace OpCheck.XmlConsumer
{
	/// <summary>
	/// Defines the list of available mapping algorithms.
	/// A mapping algorithm defines how an expression is applied to a member and/or its context object where it resides.
	/// </summary>
	public enum MappingStrategy
	{
		/// <summary>
		/// A single-value object contains another single value object.
		/// This is the simplest of the mapping strategies.
		/// </summary>
		SingleValueObjects,
		
		/// <summary>
		/// A single-value object contains an array member.
		/// </summary>
		SingleValueObjectContainsArray,
		
		/// <summary>
		/// An array contains a single value object.
		/// </summary>
		ArrayContainsSingleValueObject,
		
		
		Arrays,
		
		
		Collections,
		
		Dictionaries
	}
}
