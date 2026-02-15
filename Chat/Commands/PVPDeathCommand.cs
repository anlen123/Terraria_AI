using System;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005BB RID: 1467
	[ChatCommand("PVPDeath")]
	public class PVPDeathCommand : IChatCommand
	{
		// Token: 0x060039DD RID: 14813 RVA: 0x00652F14 File Offset: 0x00651114
		public void ProcessIncomingMessage(string text, byte clientId)
		{
			NetworkText text2 = NetworkText.FromKey("LegacyMultiplayer.24", new object[]
			{
				Main.player[(int)clientId].name,
				Main.player[(int)clientId].numberOfDeathsPVP
			});
			if (Main.player[(int)clientId].numberOfDeathsPVP == 1)
			{
				text2 = NetworkText.FromKey("LegacyMultiplayer.26", new object[]
				{
					Main.player[(int)clientId].name,
					Main.player[(int)clientId].numberOfDeathsPVP
				});
			}
			ChatHelper.BroadcastChatMessage(text2, PVPDeathCommand.RESPONSE_COLOR, -1);
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}

		// Token: 0x04005D8E RID: 23950
		private static readonly Color RESPONSE_COLOR = new Color(255, 25, 25);
	}
}
