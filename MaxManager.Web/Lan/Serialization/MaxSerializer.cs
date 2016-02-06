using System;
using System.Reflection;

namespace MaxManager.Web.Lan.Serialization
{
	public class MaxSerializer
	{
		public T Deserialize<T>(byte[] state) where T : class
		{
			var type = typeof (T);
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
			var bytePos = maxSerializationAttribute.BytePos-1;
			var bitPos = maxSerializationAttribute.BitPos;
			var returnType = maxSerializationAttribute.ReturnType;
			var bitSpan = maxSerializationAttribute.BitSpan;

			var value = state[bytePos];
			var value2 = value >> bitPos;
			var mask = (1 << bitSpan) - 1;

			var result = value2 & mask;

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

			return null;
		}
	}
}