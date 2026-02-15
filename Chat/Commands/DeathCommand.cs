using System;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005BA RID: 1466
	[ChatCommand("Death")]
	public class DeathCommand : IChatCommand
	{
		// Token: 0x060039D9 RID: 14809 RVA: 0x00652E6C File Offset: 0x0065106C
		public void ProcessIncomingMessage(string text, byte clientId)
		{
			NetworkText text2 = NetworkText.FromKey("LegacyMultiplayer.23", new object[]
			{
				Main.player[(int)clientId].name,
				Main.player[(int)clientId].numberOfDeathsPVE
			});
			if (Main.player[(int)clientId].numberOfDeathsPVE == 1)
			{
				text2 = NetworkText.FromKey("LegacyMultiplayer.25", new object[]
				{
					Main.player[(int)clientId].name,
					Main.player[(int)clientId].numberOfDeathsPVE
				});
			}
			ChatHelper.BroadcastChatMessage(text2, DeathCommand.RESPONSE_COLOR, -1);
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}

		// Token: 0x04005D8D RID: 23949
		private static readonly Color RESPONSE_COLOR = new Color(255, 25, 25);
	}
}
