using System;
using System.Text;
using BCrypt.Net;

namespace Terraria.Utilities
{
	// Token: 0x020000D2 RID: 210
	public static class Secrets
	{
		// Token: 0x06001826 RID: 6182 RVA: 0x004E0A10 File Offset: 0x004DEC10
		public static string ToSecret(string plainInput)
		{
			byte[] array = Encoding.UTF8.GetBytes(plainInput);
			array = new BCrypt().CryptRaw(array, Secrets._salt, 4);
			for (int i = 0; i < 1000; i++)
			{
				int num = i % array.Length;
				int num2 = (int)array[num] % array.Length;
				Utils.Swap<byte>(ref array[num], ref array[num2]);
			}
			array = new BCrypt().CryptRaw(array, Secrets._salt, 4);
			return Convert.ToBase64String(array);
		}

		// Token: 0x040012AD RID: 4781
		private static readonly byte[] _salt = Convert.FromBase64String("fT2JQQzNMJl2NRoMbo9RjA==");
	}
}
