using System;
using System.IO;
using Terraria.GameContent.Creative;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	// Token: 0x020002E1 RID: 737
	public class NetCreativePowerPermissionsModule : NetModule
	{
		// Token: 0x0600261B RID: 9755 RVA: 0x0055D0EC File Offset: 0x0055B2EC
		public static NetPacket SerializeCurrentPowerPermissionLevel(ushort powerId, int level)
		{
			NetPacket result = NetModule.CreatePacket<NetCreativePowerPermissionsModule>(65530);
			result.Writer.Write(0);
			result.Writer.Write(powerId);
			result.Writer.Write((byte)level);
			return result;
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x0055D130 File Offset: 0x0055B330
		public override bool Deserialize(BinaryReader reader, int userId)
		{
			if (reader.ReadByte() == 0)
			{
				ushort id = reader.ReadUInt16();
				int currentPermissionLevel = (int)reader.ReadByte();
				if (Main.netMode == 2)
				{
					return false;
				}
				ICreativePower creativePower;
				if (!CreativePowerManager.Instance.TryGetPower(id, out creativePower))
				{
					return false;
				}
				creativePower.CurrentPermissionLevel = (PowerPermissionLevel)currentPermissionLevel;
			}
			return true;
		}

		// Token: 0x0400503F RID: 20543
		private const byte _setPermissionLevelId = 0;
	}
}
