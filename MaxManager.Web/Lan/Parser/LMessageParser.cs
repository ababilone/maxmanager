using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.Lan.Serialization;

namespace MaxManager.Web.Lan.Parser
{
	public class LMessageParser : IMessageParser
	{
		private readonly List<ILSubMessageParser> _subMessageParsers;

		public LMessageParser()
		{
			var maxSerializer = new MaxSerializer();

			_subMessageParsers = new List<ILSubMessageParser>
			{
				new LEcoButtonSubMessageParser(),
				new LHeatingThermostatSubMessageParser(maxSerializer),
				new LWallThermostatSubMessageParser()
			};
		}

		public bool Accept(string payload)
		{
			return payload.StartsWith("L:");
		}

		public object Parse(string payload)
		{
			var dataAsString = payload.Substring(2);
			var data = Convert.FromBase64String(dataAsString);

			var addresses = new List<string>();
			var erroneousFrameTypes = new List<byte>();
			var stateInformations = new List<byte[]>();
			var states = new List<byte[]>();

			var memoryStream = new MemoryStream(data);
			while (memoryStream.Position < memoryStream.Length)
			{
				var size = memoryStream.ReadByte();
				var deviceState = new byte[size];
				memoryStream.Read(deviceState, 0, size);

				addresses.Add(BitConverter.ToString(deviceState, 0, 3));
				erroneousFrameTypes.Add(deviceState[3]);
				stateInformations.Add(new[] { deviceState[4] });
				states.Add(GetState(deviceState, 5));
			}

			var messages = new List<LMessage>();

			for (var index = 0; index < states.Count; index++)
			{
				var state = states[index];
				var address = addresses[index];

				var erroneousFrameType = erroneousFrameTypes[index];
				var stateInformation = stateInformations[index];

				var isValid = GetBit(stateInformation, 0, 4);
				var isInitialized = GetBit(stateInformation, 0, 1);
				var isRfError = GetBit(stateInformation, 0, 3);

				var lSubMessageParser = _subMessageParsers.SingleOrDefault(parser => parser.Accept(state));
				if (lSubMessageParser != null)
				{
					var lMessage = lSubMessageParser.Parse(state);
					lMessage.RfAddress = address;
					lMessage.RadioState = GetRadioState(isRfError, erroneousFrameType);
					lMessage.StateInfo = CreateDeviceStateInfo(isValid, isInitialized);

					messages.Add(lMessage);
				}
			}

			return new LMessages { Messages = messages };
		}

		private bool GetBit(byte[] value, int bytePosition, int bitPosition)
		{
			return (value[bytePosition] >> bitPosition & 0x1) != 0;
		}

		private static byte[] GetState(byte[] item, int offset)
		{
			var size = item.Length - offset;
			var state = new byte[size];
			for (var i = 0; i < size; i++)
			{
				state[i] = item[(offset + i)];
			}
			return state;
		}

		private MaxRadioState GetRadioState(bool isRfError, byte frameTypeCode)
		{
			if (!isRfError)
			{
				return MaxRadioState.Ok;
			}

			// Todo : handle frameTypeCode

			return MaxRadioState.Error;
		}

		private MaxStateInfo CreateDeviceStateInfo(bool isValid, bool isInitialized)
		{
			if (isValid && isInitialized)
			{
				return MaxStateInfo.Valid;
			}
			return !isInitialized ? MaxStateInfo.NotInitialized : MaxStateInfo.OutOfDate;
		}
	}
}