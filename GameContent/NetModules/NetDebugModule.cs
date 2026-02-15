using System;
using System.IO;
using Terraria.Net;
using Terraria.Testing.ChatCommands;
using Terraria.UI.Chat;

namespace Terraria.GameContent.NetModules
{
	// Token: 0x020002E5 RID: 741
	public class NetDebugModule : NetModule
	{
		// Token: 0x06002628 RID: 9768 RVA: 0x0055D414 File Offset: 0x0055B614
		public static NetPacket Serialize(DebugMessage message)
		{
			NetPacket result = NetModule.CreatePacket<NetDebugModule>(65530);
			message.Serialize(result.Writer);
			return result;
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x0055D43C File Offset: 0x0055B63C
		public override bool Deserialize(BinaryReader reader, int senderPlayerId)
		{
			DebugMessage message = DebugMessage.Deserialize((byte)senderPlayerId, reader);
			ChatManager.DebugCommands.Process(message);
			return true;
		}
	}
}
