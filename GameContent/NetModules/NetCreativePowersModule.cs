using System;
using System.IO;
using Terraria.GameContent.Creative;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	// Token: 0x020002DF RID: 735
	public class NetCreativePowersModule : NetModule
	{
		// Token: 0x06002615 RID: 9749 RVA: 0x0055CFD0 File Offset: 0x0055B1D0
		public static NetPacket PreparePacket(ushort powerId, int specificInfoBytesInPacketCount)
		{
			NetPacket result = NetModule.CreatePacket<NetCreativePowersModule>(65530);
			result.Writer.Write(powerId);
			return result;
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x0055CFF8 File Offset: 0x0055B1F8
		public override bool Deserialize(BinaryReader reader, int userId)
		{
			ushort id = reader.ReadUInt16();
			ICreativePower creativePower;
			if (!CreativePowerManager.Instance.TryGetPower(id, out creativePower))
			{
				return false;
			}
			creativePower.DeserializeNetMessage(reader, userId);
			return true;
		}
	}
}
