using System;
using System.IO;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	// Token: 0x020002DE RID: 734
	public class NetBestiaryModule : NetModule
	{
		// Token: 0x06002610 RID: 9744 RVA: 0x0055CE60 File Offset: 0x0055B060
		public static NetPacket SerializeKillCount(int npcNetId, int killcount)
		{
			NetPacket result = NetModule.CreatePacket<NetBestiaryModule>(65530);
			result.Writer.Write(0);
			result.Writer.Write((short)npcNetId);
			result.Writer.Write7BitEncodedInt(killcount);
			return result;
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x0055CEA4 File Offset: 0x0055B0A4
		public static NetPacket SerializeSight(int npcNetId)
		{
			NetPacket result = NetModule.CreatePacket<NetBestiaryModule>(65530);
			result.Writer.Write(1);
			result.Writer.Write((short)npcNetId);
			return result;
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x0055CED8 File Offset: 0x0055B0D8
		public static NetPacket SerializeChat(int npcNetId)
		{
			NetPacket result = NetModule.CreatePacket<NetBestiaryModule>(65530);
			result.Writer.Write(2);
			result.Writer.Write((short)npcNetId);
			return result;
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x0055CF0C File Offset: 0x0055B10C
		public override bool Deserialize(BinaryReader reader, int userId)
		{
			if (Main.dedServ)
			{
				return false;
			}
			switch (reader.ReadByte())
			{
			case 0:
			{
				short key = reader.ReadInt16();
				string bestiaryCreditId = ContentSamples.NpcsByNetId[(int)key].GetBestiaryCreditId();
				int killCount = reader.Read7BitEncodedInt();
				Main.BestiaryTracker.Kills.SetKillCountDirectly(bestiaryCreditId, killCount);
				break;
			}
			case 1:
			{
				short key2 = reader.ReadInt16();
				string bestiaryCreditId2 = ContentSamples.NpcsByNetId[(int)key2].GetBestiaryCreditId();
				Main.BestiaryTracker.Sights.SetWasSeenDirectly(bestiaryCreditId2);
				break;
			}
			case 2:
			{
				short key3 = reader.ReadInt16();
				string bestiaryCreditId3 = ContentSamples.NpcsByNetId[(int)key3].GetBestiaryCreditId();
				Main.BestiaryTracker.Chats.SetWasChatWithDirectly(bestiaryCreditId3);
				break;
			}
			}
			return true;
		}

		// Token: 0x02000825 RID: 2085
		private enum BestiaryUnlockType : byte
		{
			// Token: 0x0400721F RID: 29215
			Kill,
			// Token: 0x04007220 RID: 29216
			Sight,
			// Token: 0x04007221 RID: 29217
			Chat
		}
	}
}
