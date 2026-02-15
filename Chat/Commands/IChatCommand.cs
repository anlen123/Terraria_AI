using System;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005B7 RID: 1463
	public interface IChatCommand
	{
		// Token: 0x060039D2 RID: 14802
		void ProcessIncomingMessage(string text, byte clientId);

		// Token: 0x060039D3 RID: 14803
		void ProcessOutgoingMessage(ChatMessage message);
	}
}
