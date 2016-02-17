using System.Linq;

namespace MaxManager.Web.Lan.Serialization
{
	public class ByteWriter
	{
		private byte[] _buffer = new byte[0];

		public void Write(byte data, int position)
		{
			EnsureBufferSize(position);
			_buffer[position] = (byte)(_buffer[position] | data);
		}

		private void EnsureBufferSize(int position)
		{
			if (position < _buffer.Length)
				return;

			var bytes = new byte[position + 1];
			_buffer.CopyTo(bytes, 0);
			_buffer = bytes;
		}

		public byte[] ToBytes()
		{
			return _buffer.ToArray();
		}
	}
}