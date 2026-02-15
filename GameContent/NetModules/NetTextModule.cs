using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.Net;
using Terraria.UI.Chat;

namespace Terraria.GameContent.NetModules
{
	// Token: 0x020002E6 RID: 742
	public class NetTextModule : NetModule
	{
		// Token: 0x0600262B RID: 9771 RVA: 0x0055D460 File Offset: 0x0055B660
		public static NetPacket SerializeClientMessage(ChatMessage message)
		{
			NetPacket result = NetModule.CreatePacket<NetTextModule>(65530);
			message.Serialize(result.Writer);
			return result;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x0055D486 File Offset: 0x0055B686
		public static NetPacket SerializeServerMessage(NetworkText text, Color color)
		{
			return NetTextModule.SerializeServerMessage(text, color, byte.MaxValue);
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x0055D494 File Offset: 0x0055B694
		public static NetPacket SerializeServerMessage(NetworkText text, Color color, byte authorId)
		{
			NetPacket result = NetModule.CreatePacket<NetTextModule>(65530);
			result.Writer.Write(authorId);
			text.Serialize(result.Writer);
			result.Writer.WriteRGB(color);
			return result;
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x0055D4D4 File Offset: 0x0055B6D4
		private bool DeserializeAsClient(BinaryReader reader, int senderPlayerId)
		{
			byte messageAuthor = reader.ReadByte();
			NetworkText text = NetworkText.Deserialize(reader);
			Color color = reader.ReadRGB();
			ChatHelper.DisplayMessage(text, color, messageAuthor);
			return true;
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x0055D500 File Offset: 0x0055B700
		private bool DeserializeAsServer(BinaryReader reader, int senderPlayerId)
		{
			ChatMessage message = ChatMessage.Deserialize(reader);
			ChatManager.Commands.ProcessIncomingMessage(message, senderPlayerId);
			return true;
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x0055D521 File Offset: 0x0055B721
		public override bool Deserialize(BinaryReader reader, int senderPlayerId)
		{
			if (Main.dedServ)
			{
				return this.DeserializeAsServer(reader, senderPlayerId);
			}
			return this.DeserializeAsClient(reader, senderPlayerId);
		}
	}
}
