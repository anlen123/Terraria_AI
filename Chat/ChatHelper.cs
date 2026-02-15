using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.UI.Chat;
using Terraria.Localization;
using Terraria.Net;

namespace Terraria.Chat
{
	// Token: 0x020005B3 RID: 1459
	public static class ChatHelper
	{
		// Token: 0x060039B4 RID: 14772 RVA: 0x00652A28 File Offset: 0x00650C28
		public static void DisplayMessageOnClient(NetworkText text, Color color, int playerId)
		{
			if (Main.dedServ)
			{
				NetPacket packet = NetTextModule.SerializeServerMessage(text, color, byte.MaxValue);
				NetManager.Instance.SendToClient(packet, playerId);
				return;
			}
			ChatHelper.DisplayMessage(text, color, byte.MaxValue);
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x00652A62 File Offset: 0x00650C62
		public static void SendChatMessageToClient(NetworkText text, Color color, int playerId)
		{
			ChatHelper.SendChatMessageToClientAs(byte.MaxValue, text, color, playerId);
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x00652A74 File Offset: 0x00650C74
		public static void SendChatMessageToClientAs(byte messageAuthor, NetworkText text, Color color, int playerId)
		{
			if (Main.dedServ)
			{
				NetPacket packet = NetTextModule.SerializeServerMessage(text, color, messageAuthor);
				NetManager.Instance.SendToClient(packet, playerId);
			}
			if (playerId == Main.myPlayer)
			{
				ChatHelper.DisplayMessage(text, color, messageAuthor);
			}
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x00652AAD File Offset: 0x00650CAD
		public static void BroadcastChatMessage(NetworkText text, Color color, int excludedPlayer = -1)
		{
			ChatHelper.BroadcastChatMessageAs(byte.MaxValue, text, color, excludedPlayer);
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x00652ABC File Offset: 0x00650CBC
		public static void BroadcastChatMessageAs(byte messageAuthor, NetworkText text, Color color, int excludedPlayer = -1)
		{
			if (Main.dedServ)
			{
				NetPacket packet = NetTextModule.SerializeServerMessage(text, color, messageAuthor);
				NetManager.Instance.Broadcast(packet, new NetManager.BroadcastCondition(ChatHelper.OnlySendToPlayersWhoAreLoggedIn), excludedPlayer);
				return;
			}
			if (excludedPlayer != Main.myPlayer)
			{
				ChatHelper.DisplayMessage(text, color, messageAuthor);
			}
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x00652B02 File Offset: 0x00650D02
		public static bool OnlySendToPlayersWhoAreLoggedIn(int clientIndex)
		{
			return Netplay.Clients[clientIndex].State == 10;
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x00652B14 File Offset: 0x00650D14
		public static void SendChatMessageFromClient(ChatMessage message)
		{
			if (!message.IsConsumed)
			{
				NetPacket packet = NetTextModule.SerializeClientMessage(message);
				NetManager.Instance.SendToServer(packet);
			}
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x00652B3C File Offset: 0x00650D3C
		public static void DisplayMessage(NetworkText text, Color color, byte messageAuthor)
		{
			string text2 = text.ToString();
			if (messageAuthor < 255)
			{
				Main.player[(int)messageAuthor].chatOverhead.NewMessage(text2, Main.PlayerOverheadChatMessageDisplayTime);
				Main.player[(int)messageAuthor].chatOverhead.color = color;
				text2 = NameTagHandler.GenerateTag(Main.player[(int)messageAuthor].name) + " " + text2;
			}
			if (ChatHelper.ShouldCacheMessage())
			{
				ChatHelper.CacheMessage(text2, color);
				return;
			}
			Main.NewTextMultiline(text2, false, color, -1);
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x00652BB6 File Offset: 0x00650DB6
		private static void CacheMessage(string message, Color color)
		{
			ChatHelper._cachedMessages.Add(new Tuple<string, Color>(message, color));
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x00652BCC File Offset: 0x00650DCC
		public static void ShowCachedMessages()
		{
			List<Tuple<string, Color>> cachedMessages = ChatHelper._cachedMessages;
			lock (cachedMessages)
			{
				foreach (Tuple<string, Color> tuple in ChatHelper._cachedMessages)
				{
					Main.NewTextMultiline(tuple.Item1, false, tuple.Item2, -1);
				}
			}
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x00652C50 File Offset: 0x00650E50
		public static void ClearDelayedMessagesCache()
		{
			ChatHelper._cachedMessages.Clear();
		}

		// Token: 0x060039BF RID: 14783 RVA: 0x00652C5C File Offset: 0x00650E5C
		private static bool ShouldCacheMessage()
		{
			return Main.netMode == 1 && Main.gameMenu;
		}

		// Token: 0x04005D87 RID: 23943
		private static List<Tuple<string, Color>> _cachedMessages = new List<Tuple<string, Color>>();
	}
}
