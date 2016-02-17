using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MaxManager.Web.Lan.Commands;
using MaxManager.Web.Lan.Serialization.TypeSerializer;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Serialization
{
	public class MaxSerializer
	{
		private readonly List<ITypeSerializer> _typeSerializers;

		public MaxSerializer()
		{
			_typeSerializers = new List<ITypeSerializer>();
		}

		public T Deserialize<T>(byte[] state) where T : class
		{
			var type = typeof(T);
			var propertyInfos = type.GetProperties();

			var instance = Activator.CreateInstance<T>();

			foreach (var propertyInfo in propertyInfos)
			{
				var maxSerializationAttribute = propertyInfo.GetCustomAttribute<MaxSerializationAttribute>();
				if (maxSerializationAttribute == null)
					continue;

				var value = Deserialize(state, maxSerializationAttribute);
				propertyInfo.SetValue(instance, value);
			}

			return instance;
		}

		private object Deserialize(byte[] state, MaxSerializationAttribute maxSerializationAttribute)
		{
			var bytePos = maxSerializationAttribute.BytePos - 1;
			var bitPos = maxSerializationAttribute.BitPos;
			var returnType = maxSerializationAttribute.ReturnType;
			var bitSpan = maxSerializationAttribute.BitSpan;

			var value = state[bytePos];
			var value2 = value >> bitPos;
			var mask = (1 << bitSpan) - 1;

			var result = value2 & mask;

			if (returnType == typeof(string))
			{
				var charCount = bitSpan / 8;
				return Encoding.UTF8.GetString(state, bytePos, charCount);
			}

			if (returnType == typeof(MaxRfAddress))
			{
				var byteCount = bitSpan / 8;
				return new MaxRfAddress
				{
					HumanReadable = BitConverter.ToString(state, bytePos, byteCount),
					Bytes = state.Skip(bytePos).Take(byteCount).ToArray()
				};
			}

			if (returnType == typeof(bool))
			{
				foreach (var trueValue in maxSerializationAttribute.TrueValues)
				{
					if (trueValue == 9)
					{
						return result != 0;
					}

					if (result == trueValue)
					{
						return true;
					}
				}

				return false;
			}

			if (returnType == typeof(int))
			{
				return Convert.ToInt32(result);
			}

			if (returnType.GetTypeInfo().IsEnum)
			{
				var intValue = Convert.ToInt32(result);
				return Enum.ToObject(returnType, intValue);
			}

			return null;
		}

		public byte[] Serialize<T>(T maxCommand) where T : IMaxCommand
		{
			var type = typeof(T);
			var propertyInfos = type.GetProperties();

			var byteWriter = new ByteWriter();

			foreach (var propertyInfo in propertyInfos)
			{
				var maxSerializationAttribute = propertyInfo.GetCustomAttribute<MaxSerializationAttribute>();
				if (maxSerializationAttribute == null)
					continue;

				var value = propertyInfo.GetValue(maxCommand);
				Serialize(value, maxSerializationAttribute, byteWriter);
			}

			return byteWriter.ToBytes();
		}

		private void Serialize(object value, MaxSerializationAttribute maxSerializationAttribute, ByteWriter byteWriter)
		{
			var typeSerializer = _typeSerializers.SingleOrDefault(serializer => serializer.Accept(maxSerializationAttribute));
			if (typeSerializer == null)
				throw new Exception("No TypeSerializer for " + maxSerializationAttribute.ReturnType);
		}
	}
}