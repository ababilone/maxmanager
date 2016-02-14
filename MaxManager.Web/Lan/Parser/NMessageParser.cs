using System;
using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.Lan.Serialization;

namespace MaxManager.Web.Lan.Parser
{
	public class NMessageParser : IMessageParser
	{
		private MaxSerializer _maxSerializer;

		public NMessageParser()
		{
			_maxSerializer = new MaxSerializer();
		}

		public bool Accept(string payload)
		{
			return payload.StartsWith("N:");
		}

		public IMaxMessage Parse(string payload)
		{
			var data = Convert.FromBase64String(payload.Substring(2));
			return _maxSerializer.Deserialize<NMessage>(data);
		}
	}
}