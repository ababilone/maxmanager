using System;

namespace MaxManager.Web.Lan.Serialization.TypeSerializer
{
	public interface ITypeSerializer
	{
		bool Accept(Type type);

		object Deserialize(byte[] payload, Type targetType, MaxSerializationAttribute maxSerializationAttribute);

		void Serialize(object value, Type sourceType, MaxSerializationAttribute maxSerializationAttribute, ByteWriter byteWriter);
	}
}