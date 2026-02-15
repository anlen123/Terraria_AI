using System;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005C3 RID: 1475
	[ChatCommand("Party")]
	public class PartyChatCommand : IChatCommand
	{
		// Token: 0x06003A00 RID: 14848 RVA: 0x00653688 File Offset: 0x00651888
		public void ProcessIncomingMessage(string text, byte clientId)
		{
			int team = Main.player[(int)clientId].team;
			Color color = Main.teamColor[team];
			if (team == 0 || Main.netMode == 0)
			{
				this.SendNoTeamError(clientId);
				return;
			}
			if (text == "")
			{
				return;
			}
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].team == team)
				{
					ChatHelper.SendChatMessageToClientAs(clientId, NetworkText.FromLiteral(text), color, i);
				}
			}
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x006536FB File Offset: 0x006518FB
		private void SendNoTeamError(byte clientId)
		{
			ChatHelper.SendChatMessageToClient(Lang.mp[10].ToNetworkText(), PartyChatCommand.ERROR_COLOR, (int)clientId);
		}

		// Token: 0x04005D96 RID: 23958
		private static readonly Color ERROR_COLOR = new Color(255, 240, 20);
	}
}
