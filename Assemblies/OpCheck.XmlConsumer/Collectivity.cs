namespace OpCheck.XmlConsumer
{
	/// <summary>
	/// Describes the "collectivity" of a type - which is how the consumer recognizes and ultimately processes, uses, or treats an object. 
	/// </summary>
	public enum Collectivity
	{
		/// <summary>
		/// The system will examine the type and decide how to treat it.
		/// </summary>
		AutoDetect,
	
		/// <summary>
		/// The object should not be treated as a collection type at all - it is a single value object.
		/// </summary>
		SingleValueObject,
		
		/// <summary>
		/// The object is an array.
		/// </summary>
		Array,
		
		/// <summary>
		/// The object is a collection that implements the ICollection interface.
		/// </summary>
		Collection,
		
		/// <summary>
		/// The object is a dictionary that implements the IDictionary interface.
		/// </summary>
		Dictionary
	}
}
