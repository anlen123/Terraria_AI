using System;
using System.Diagnostics;
using Terraria.Net.Sockets;
using Terraria.Testing;

namespace Terraria.Net
{
	// Token: 0x02000165 RID: 357
	public class Ping
	{
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001D99 RID: 7577 RVA: 0x00500DA0 File Offset: 0x004FEFA0
		// (set) Token: 0x06001D9A RID: 7578 RVA: 0x00500DA7 File Offset: 0x004FEFA7
		public static int CurrentPing { get; private set; }

		// Token: 0x06001D9B RID: 7579 RVA: 0x00500DAF File Offset: 0x004FEFAF
		public static void Reset()
		{
			Ping.CurrentPing = 0;
			Ping._stopwatch.Restart();
			Ping._waitingForResponse = false;
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x00500DC8 File Offset: 0x004FEFC8
		public static void Update()
		{
			if (Ping._waitingForResponse)
			{
				Ping.CurrentPing = Math.Max(Ping.CurrentPing, (int)Ping._stopwatch.ElapsedMilliseconds);
				return;
			}
			if (Ping._stopwatch.ElapsedMilliseconds >= 250L)
			{
				NetMessage.SendData(154, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				Ping._waitingForResponse = true;
				Ping._stopwatch.Restart();
			}
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x00500E3C File Offset: 0x004FF03C
		internal static void PingRecieved()
		{
			Ping.CurrentPing = (int)Ping._stopwatch.ElapsedMilliseconds;
			Ping._waitingForResponse = false;
			if (DebugOptions.Shared_ServerPing > 0)
			{
				int num = (DebugOptions.Shared_ServerPing - Ping.CurrentPing) / 2;
				num /= 5;
				DebugNetworkStream.Latency = (uint)Utils.Clamp<long>((long)((ulong)DebugNetworkStream.Latency + (ulong)((long)num)), 0L, 5000L);
			}
		}

		// Token: 0x0400163E RID: 5694
		private static Stopwatch _stopwatch = new Stopwatch();

		// Token: 0x0400163F RID: 5695
		private static bool _waitingForResponse;
	}
}
