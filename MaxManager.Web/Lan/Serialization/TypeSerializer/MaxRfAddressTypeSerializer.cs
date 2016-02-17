using System;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Serialization.TypeSerializer
{
	public class MaxRfAddressTypeSerializer : ITypeSerializer
	{
		public bool Accept(Type type)
		{
			return type == typeof(MaxRfAddress);
		}

		public object Deserialize(byte[] payload, Type targetType, MaxSerializationAttribute maxSerializationAttribute)
		{
			throw new NotImplementedException();
		}

		public void Serialize(object value, Type sourceType, MaxSerializationAttribute maxSerializationAttribute, ByteWriter byteWriter)
		{
			var maxRfAddress = value as MaxRfAddress;
			if (maxRfAddress != null)
			{
				for (var index = 0; index < maxRfAddress.Bytes.Length; index++)
				{
					var b = maxRfAddress.Bytes[index];
					byteWriter.Write(b, maxSerializationAttribute.BytePos + index);
				}
			}
		}
	}
}