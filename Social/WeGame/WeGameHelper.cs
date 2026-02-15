using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Terraria.Social.WeGame
{
	// Token: 0x0200012E RID: 302
	public class WeGameHelper
	{
		// Token: 0x06001C0B RID: 7179
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern void OutputDebugString(string message);

		// Token: 0x06001C0C RID: 7180 RVA: 0x004FC701 File Offset: 0x004FA901
		public static void WriteDebugString(string format, params object[] args)
		{
			"[WeGame] - " + format;
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x004FC710 File Offset: 0x004FA910
		public static string Serialize<T>(T data)
		{
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				dataContractJsonSerializer.WriteObject(memoryStream, data);
				memoryStream.Position = 0L;
				using (StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x004FC790 File Offset: 0x004FA990
		public static void UnSerialize<T>(string str, out T data)
		{
			using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(str)))
			{
				DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
				data = (T)((object)dataContractJsonSerializer.ReadObject(memoryStream));
			}
		}
	}
}
