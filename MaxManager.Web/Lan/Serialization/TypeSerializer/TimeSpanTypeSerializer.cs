using System;

namespace MaxManager.Web.Lan.Serialization.TypeSerializer
{
	public class TimeSpanTypeSerializer : ITypeSerializer
	{
		public bool Accept(Type type)
		{
			return type == typeof(TimeSpan) || type == typeof(TimeSpan?);
		}

		public object Deserialize(byte[] payload, Type targetType, MaxSerializationAttribute maxSerializationAttribute)
		{
			throw new NotImplementedException();
		}

		public void Serialize(object value, Type sourceType, MaxSerializationAttribute maxSerializationAttribute, ByteWriter byteWriter)
		{
			if (value == null)
				return;

			var timeSpan = sourceType == typeof(TimeSpan?) ? ((TimeSpan?)value).Value : (TimeSpan)value;

			var round = (int)Math.Round(timeSpan.TotalHours * 2);
			byteWriter.Write((byte)round, maxSerializationAttribute.BytePos);
		}
	}
}