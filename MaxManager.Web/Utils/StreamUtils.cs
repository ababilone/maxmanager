using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace MaxManager.Web.Utils
{
	public static class StreamUtils
	{
		public static async Task<string> ReadLine(this DataReader dataReader)
		{
			using (var memoryStream = new MemoryStream())
			{
				while (true)
				{
					var count = await dataReader.LoadAsync(1);
					if (count != 1)
						break;

					int readByte = dataReader.ReadByte();
					if (readByte == -1)
						break;

					var currentByte = (byte) readByte;

					if (currentByte == 13)
						continue;

					if (currentByte == 10)
						break;

					memoryStream.WriteByte(currentByte);
				}

				return memoryStream.Length == 0 ? null : Encoding.UTF8.GetString(memoryStream.ToArray());
			}
		}

		public static async Task<string> ReadLine(this IInputStream inputStream)
		{
			using (var dataReader = new DataReader(inputStream))
			{
				var line = await dataReader.ReadLine();
				dataReader.DetachStream();
				return line;
			}
		}

		public static async Task WriteLine(this IOutputStream outputStream, string line)
		{
			using (var dataWriter = new DataWriter(outputStream))
			{
				dataWriter.WriteString(line);
				await dataWriter.StoreAsync();
				dataWriter.DetachStream();
			}
		}
	}
}
