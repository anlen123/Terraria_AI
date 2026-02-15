using System;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005C5 RID: 1477
	[ChatCommand("Say")]
	public class SayChatCommand : IChatCommand
	{
		// Token: 0x06003A09 RID: 14857 RVA: 0x006537A6 File Offset: 0x006519A6
		public void ProcessIncomingMessage(string text, byte clientId)
		{
			ChatHelper.BroadcastChatMessageAs(clientId, NetworkText.FromLiteral(text), Main.player[(int)clientId].ChatColor(), -1);
			if (Main.dedServ)
			{
				Console.WriteLine("<{0}> {1}", Main.player[(int)clientId].name, text);
			}
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}
	}
}
