using System;
using Terraria.GameContent.UI;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005BF RID: 1471
	[ChatCommand("RPS")]
	public class RockPaperScissorsCommand : IChatCommand
	{
		// Token: 0x060039EF RID: 14831 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessIncomingMessage(string text, byte clientId)
		{
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x00653398 File Offset: 0x00651598
		public void ProcessOutgoingMessage(ChatMessage message)
		{
			if (Main.netMode != 2 && Main.LocalPlayer.dead)
			{
				message.Consume();
				return;
			}
			int num = Main.rand.NextFromList(new int[]
			{
				37,
				38,
				36
			});
			if (Main.netMode == 0)
			{
				EmoteBubble.NewBubble(num, new WorldUIAnchor(Main.LocalPlayer), 360);
				EmoteBubble.CheckForNPCsToReactToEmoteBubble(num, Main.LocalPlayer);
			}
			else
			{
				NetMessage.SendData(120, -1, -1, null, Main.myPlayer, (float)num, 0f, 0f, 0, 0, 0);
			}
			message.Consume();
		}
	}
}
