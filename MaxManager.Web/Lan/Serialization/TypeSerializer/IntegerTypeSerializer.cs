using System;
using System.Linq;
using System.Reflection;

namespace MaxManager.Web.Lan.Serialization.TypeSerializer
{
	public class IntegerTypeSerializer : ITypeSerializer
	{
		public bool Accept(Type type)
		{
			if (type == typeof (short) || type == typeof (int) || type == typeof (long))
				return true;

			return type.GetTypeInfo().IsEnum;
		}

		public object Deserialize(byte[] payload, Type targetType, MaxSerializationAttribute maxSerializationAttribute)
		{
			throw new NotImplementedException();
		}

		public void Serialize(object value, Type sourceType, MaxSerializationAttribute maxSerializationAttribute, ByteWriter byteWriter)
		{
			byte[] bytes;
			var baseType = sourceType.GetTypeInfo().IsEnum ? sourceType.GetTypeInfo().DeclaredFields.First().FieldType : sourceType;
			if (baseType == typeof(int))
			{
				var intValue = (int)value;
				bytes = BitConverter.GetBytes(intValue);
			}
			else if (baseType == typeof(long))
			{
				var longValue = (long)value;
				bytes = BitConverter.GetBytes(longValue);
			}
			else if (baseType == typeof(short))
			{
				var longValue = (short)value;
				bytes = BitConverter.GetBytes(longValue);
			}
			else
			{
				throw new Exception();
			}

			bytes = bytes.Take(maxSerializationAttribute.ByteSpan).Reverse().ToArray();

			for (var index = 0; index < bytes.Length && index < maxSerializationAttribute.ByteSpan; index++)
			{
				var b = bytes[index];

				var bitSpan = maxSerializationAttribute.ByteSpan > 1
					? maxSerializationAttribute.ByteSpan*8
					: maxSerializationAttribute.BitSpan;

				var mask = (1 << bitSpan) - 1;
				var value2 = (b & mask) << maxSerializationAttribute.BitPos;
				// 0x00 0x04 0x40 0x00 0x00 0x00
				byteWriter.Write((byte)value2, maxSerializationAttribute.BytePos + index);
			}
		}
	}
}