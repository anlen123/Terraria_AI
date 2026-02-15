using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005BC RID: 1468
	[ChatCommand("AllDeath")]
	public class AllDeathCommand : IChatCommand
	{
		// Token: 0x060039E1 RID: 14817 RVA: 0x00652FBC File Offset: 0x006511BC
		public void ProcessIncomingMessage(string text, byte clientId)
		{
			foreach (Player player in from x in Main.player
			where x != null && x.active
			orderby x.numberOfDeathsPVE descending
			select x)
			{
				NetworkText text2 = NetworkText.FromKey("LegacyMultiplayer.23", new object[]
				{
					player.name,
					player.numberOfDeathsPVE
				});
				if (player.numberOfDeathsPVE == 1)
				{
					text2 = NetworkText.FromKey("LegacyMultiplayer.25", new object[]
					{
						player.name,
						player.numberOfDeathsPVE
					});
				}
				ChatHelper.BroadcastChatMessage(text2, AllDeathCommand.RESPONSE_COLOR, -1);
			}
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}

		// Token: 0x04005D8F RID: 23951
		private static readonly Color RESPONSE_COLOR = new Color(255, 25, 25);
	}
}
