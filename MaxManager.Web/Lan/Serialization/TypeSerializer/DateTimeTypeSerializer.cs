using System;

namespace MaxManager.Web.Lan.Serialization.TypeSerializer
{
	public class DateTimeTypeSerializer : ITypeSerializer
	{
		public bool Accept(Type type)
		{
			return type == typeof(DateTime) || type == typeof(DateTime?);
		}

		public object Deserialize(byte[] payload, Type targetType, MaxSerializationAttribute maxSerializationAttribute)
		{
			throw new NotImplementedException();
		}

		public void Serialize(object value, Type sourceType, MaxSerializationAttribute maxSerializationAttribute, ByteWriter byteWriter)
		{
			if (value == null)
				return;

			var dateTime = sourceType == typeof(DateTime?) ? ((DateTime?)value).Value : (DateTime)value;

			var leftMonth = (dateTime.Month & 0xe) << 4;
			var rightMonth = (dateTime.Month & 0x1) << 6;
			var day = dateTime.Day & 0x1f;
			var year = (dateTime.Year - 2000) & 0x1f;

			var firstByte = (byte)(leftMonth | day);
			var secondByte = (byte)(rightMonth | year);

			byteWriter.Write(firstByte, maxSerializationAttribute.BytePos);
			byteWriter.Write(secondByte, maxSerializationAttribute.BytePos + 1);
		}
	}
}