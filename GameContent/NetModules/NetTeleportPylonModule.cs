using System;
using System.IO;
using Terraria.DataStructures;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	// Token: 0x020002E4 RID: 740
	public class NetTeleportPylonModule : NetModule
	{
		// Token: 0x06002624 RID: 9764 RVA: 0x0055D258 File Offset: 0x0055B458
		public static NetPacket SerializePylonWasAddedOrRemoved(TeleportPylonInfo info, NetTeleportPylonModule.SubPacketType packetType)
		{
			NetPacket result = NetModule.CreatePacket<NetTeleportPylonModule>(65530);
			result.Writer.Write((byte)packetType);
			result.Writer.Write(info.PositionInTiles.X);
			result.Writer.Write(info.PositionInTiles.Y);
			result.Writer.Write((byte)info.TypeOfPylon);
			return result;
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x0055D2C0 File Offset: 0x0055B4C0
		public static NetPacket SerializeUseRequest(TeleportPylonInfo info)
		{
			NetPacket result = NetModule.CreatePacket<NetTeleportPylonModule>(65530);
			result.Writer.Write(2);
			result.Writer.Write(info.PositionInTiles.X);
			result.Writer.Write(info.PositionInTiles.Y);
			result.Writer.Write((byte)info.TypeOfPylon);
			return result;
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x0055D328 File Offset: 0x0055B528
		public override bool Deserialize(BinaryReader reader, int userId)
		{
			switch (reader.ReadByte())
			{
			case 0:
			{
				if (Main.dedServ)
				{
					return false;
				}
				TeleportPylonInfo info = default(TeleportPylonInfo);
				info.PositionInTiles = new Point16(reader.ReadInt16(), reader.ReadInt16());
				info.TypeOfPylon = (TeleportPylonType)reader.ReadByte();
				Main.PylonSystem.AddForClient(info);
				break;
			}
			case 1:
			{
				if (Main.dedServ)
				{
					return false;
				}
				TeleportPylonInfo info2 = default(TeleportPylonInfo);
				info2.PositionInTiles = new Point16(reader.ReadInt16(), reader.ReadInt16());
				info2.TypeOfPylon = (TeleportPylonType)reader.ReadByte();
				Main.PylonSystem.RemoveForClient(info2);
				break;
			}
			case 2:
			{
				TeleportPylonInfo info3 = default(TeleportPylonInfo);
				info3.PositionInTiles = new Point16(reader.ReadInt16(), reader.ReadInt16());
				info3.TypeOfPylon = (TeleportPylonType)reader.ReadByte();
				Main.PylonSystem.HandleTeleportRequest(info3, userId);
				break;
			}
			}
			return true;
		}

		// Token: 0x02000826 RID: 2086
		public enum SubPacketType : byte
		{
			// Token: 0x04007223 RID: 29219
			PylonWasAdded,
			// Token: 0x04007224 RID: 29220
			PylonWasRemoved,
			// Token: 0x04007225 RID: 29221
			PlayerRequestsTeleport
		}
	}
}
