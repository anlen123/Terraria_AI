using System;

namespace Terraria.Chat
{
	// Token: 0x020005B5 RID: 1461
	public interface IChatProcessor
	{
		// Token: 0x060039CF RID: 14799
		void ProcessIncomingMessage(ChatMessage message, int clientId);

		// Token: 0x060039D0 RID: 14800
		ChatMessage CreateOutgoingMessage(string text);
	}
}
