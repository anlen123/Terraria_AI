using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005B9 RID: 1465
	[ChatCommand("BossDamage")]
	public class BossDamageCommand : IChatCommand
	{
		// Token: 0x060039D5 RID: 14805 RVA: 0x00652DD8 File Offset: 0x00650FD8
		public void ProcessIncomingMessage(string text, byte clientId)
		{
			foreach (NPCDamageTracker npcdamageTracker in NPCDamageTracker.RecentAttempts())
			{
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active)
					{
						ChatHelper.SendChatMessageToClient(npcdamageTracker.GetReport(Main.player[i]), BossDamageCommand.RESPONSE_COLOR, i);
					}
				}
			}
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}

		// Token: 0x04005D8C RID: 23948
		private static readonly Color RESPONSE_COLOR = new Color(50, 255, 130);
	}
}
