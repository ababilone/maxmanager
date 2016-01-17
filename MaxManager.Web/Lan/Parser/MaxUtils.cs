using System.Linq;
using System.Text;

namespace MaxManager.Web.Lan.Parser
{
	public static class MaxUtils
	{
		public static string ExtractString(byte[] data, int start, int len)
		{
			return Encoding.UTF8.GetString(data.Skip(start).Take(len).ToArray());
		}

		public static string ExtractHex(byte[] data, int start, int len)
		{
			var sb = new StringBuilder(len * 2);
			for (var i = 0; i < len; i++)
			{
				sb.Append(string.Format("%02x", data[i] & 0xff));
			}
			return sb.ToString();
		}
	}
}