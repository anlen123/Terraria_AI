using System;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005C4 RID: 1476
	[ChatCommand("Roll")]
	public class RollCommand : IChatCommand
	{
		// Token: 0x06003A05 RID: 14853 RVA: 0x00653730 File Offset: 0x00651930
		public void ProcessIncomingMessage(string text, byte clientId)
		{
			int num = Main.rand.Next(1, 101);
			ChatHelper.BroadcastChatMessage(NetworkText.FromFormattable("*{0} {1} {2}", new object[]
			{
				Main.player[(int)clientId].name,
				Lang.mp[9].ToNetworkText(),
				num
			}), RollCommand.RESPONSE_COLOR, -1);
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}

		// Token: 0x04005D97 RID: 23959
		private static readonly Color RESPONSE_COLOR = new Color(255, 240, 20);
	}
}
