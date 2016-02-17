using System;

namespace MaxManager.Web.Lan.Serialization.TypeSerializer
{
	public interface ITypeSerializer
	{
		bool Accept(MaxSerializationAttribute maxSerializationAttribute);

		object Deserialize(byte[] payload, Type targetType, MaxSerializationAttribute maxSerializationAttribute);

		void Serialize(object value, MaxSerializationAttribute maxSerializationAttribute, ByteWriter byteWriter);
	}
}