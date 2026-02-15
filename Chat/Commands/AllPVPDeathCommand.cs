using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005BD RID: 1469
	[ChatCommand("AllPVPDeath")]
	public class AllPVPDeathCommand : IChatCommand
	{
		// Token: 0x060039E5 RID: 14821 RVA: 0x006530C8 File Offset: 0x006512C8
		public void ProcessIncomingMessage(string text, byte clientId)
		{
			foreach (Player player in from x in Main.player
			where x != null && x.active
			orderby x.numberOfDeathsPVP descending
			select x)
			{
				NetworkText text2 = NetworkText.FromKey("LegacyMultiplayer.24", new object[]
				{
					player.name,
					player.numberOfDeathsPVP
				});
				if (player.numberOfDeathsPVP == 1)
				{
					text2 = NetworkText.FromKey("LegacyMultiplayer.26", new object[]
					{
						player.name,
						player.numberOfDeathsPVP
					});
				}
				ChatHelper.BroadcastChatMessage(text2, AllPVPDeathCommand.RESPONSE_COLOR, -1);
			}
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}

		// Token: 0x04005D90 RID: 23952
		private static readonly Color RESPONSE_COLOR = new Color(255, 25, 25);
	}
}
