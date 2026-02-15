using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	// Token: 0x020002E3 RID: 739
	public class NetPingModule : NetModule
	{
		// Token: 0x06002621 RID: 9761 RVA: 0x0055D1F4 File Offset: 0x0055B3F4
		public static NetPacket Serialize(Vector2 position)
		{
			NetPacket result = NetModule.CreatePacket<NetPingModule>(65530);
			result.Writer.WriteVector2(position);
			return result;
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x0055D21C File Offset: 0x0055B41C
		public override bool Deserialize(BinaryReader reader, int userId)
		{
			Vector2 position = reader.ReadVector2();
			if (Main.dedServ)
			{
				NetManager.Instance.Broadcast(NetPingModule.Serialize(position), userId);
			}
			else
			{
				Main.Pings.Add(position);
			}
			return true;
		}
	}
}
