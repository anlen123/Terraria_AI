using System;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005C0 RID: 1472
	[ChatCommand("Emote")]
	public class EmoteCommand : IChatCommand
	{
		// Token: 0x060039F2 RID: 14834 RVA: 0x0065342A File Offset: 0x0065162A
		public void ProcessIncomingMessage(string text, byte clientId)
		{
			if (text != "")
			{
				text = string.Format("*{0} {1}", Main.player[(int)clientId].name, text);
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), EmoteCommand.RESPONSE_COLOR, -1);
			}
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}

		// Token: 0x04005D93 RID: 23955
		private static readonly Color RESPONSE_COLOR = new Color(200, 100, 0);
	}
}
