using System;

namespace MaxManager.Web.Lan.Serialization
{
	public class MaxSerializationAttribute : Attribute
	{
		public MaxSerializationAttribute()
		{
			TrueValues = new[] { 9 };
		}

		public MaxSerializationAttribute(params int[] trueValues)
		{
			TrueValues = trueValues;
		}

		public int BytePos { get; set; }

		public int BitPos { get; set; }

		public int BitSpan { get; set; }

		public int ByteSpan { get; set; }

		public Type ReturnType { get; set; }

		public int[] TrueValues { get; set; }
	}
}