using System;
using System.IO;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	// Token: 0x020002E0 RID: 736
	public class NetCreativeUnlocksPlayerReportModule : NetModule
	{
		// Token: 0x06002618 RID: 9752 RVA: 0x0055D028 File Offset: 0x0055B228
		public static NetPacket SerializeSacrificeRequest(int userId, int itemId, int amount)
		{
			NetPacket result = NetModule.CreatePacket<NetCreativeUnlocksPlayerReportModule>(65530);
			result.Writer.Write((byte)userId);
			result.Writer.Write((ushort)itemId);
			result.Writer.Write((ushort)amount);
			return result;
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x0055D06C File Offset: 0x0055B26C
		public override bool Deserialize(BinaryReader reader, int userId)
		{
			int num = (int)reader.ReadByte();
			int itemId = (int)reader.ReadUInt16();
			int amount = (int)reader.ReadUInt16();
			if (Main.dedServ)
			{
				NetManager.Instance.Broadcast(NetCreativeUnlocksPlayerReportModule.SerializeSacrificeRequest(userId, itemId, amount), userId);
				return true;
			}
			Player player = Main.player[num];
			if (Main.LocalPlayer.team > 0 && Main.LocalPlayer.team == player.team)
			{
				Main.LocalPlayerCreativeTracker.ItemSacrifices.RegisterItemSacrifice(itemId, amount, player.name);
			}
			return true;
		}
	}
}
