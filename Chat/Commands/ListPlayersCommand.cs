using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005C2 RID: 1474
	[ChatCommand("Playing")]
	public class ListPlayersCommand : IChatCommand
	{
		// Token: 0x060039FC RID: 14844 RVA: 0x006535FC File Offset: 0x006517FC
		public void ProcessIncomingMessage(string text, byte clientId)
		{
			ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral(string.Join(", ", from player in Main.player
			where player.active
			select player.name)), ListPlayersCommand.RESPONSE_COLOR, (int)clientId);
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}

		// Token: 0x04005D95 RID: 23957
		private static readonly Color RESPONSE_COLOR = new Color(255, 240, 20);
	}
}
