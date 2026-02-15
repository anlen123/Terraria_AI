using System;
using System.IO;
using Terraria.GameContent.Ambience;
using Terraria.GameContent.Skies;
using Terraria.Graphics.Effects;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	// Token: 0x020002DD RID: 733
	public class NetAmbienceModule : NetModule
	{
		// Token: 0x0600260D RID: 9741 RVA: 0x0055CDA8 File Offset: 0x0055AFA8
		public static NetPacket SerializeSkyEntitySpawn(Player player, SkyEntityType type)
		{
			int value = Main.rand.Next();
			NetPacket result = NetModule.CreatePacket<NetAmbienceModule>(65530);
			result.Writer.Write((byte)player.whoAmI);
			result.Writer.Write(value);
			result.Writer.Write((byte)type);
			return result;
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x0055CDFC File Offset: 0x0055AFFC
		public override bool Deserialize(BinaryReader reader, int userId)
		{
			if (Main.dedServ)
			{
				return false;
			}
			byte playerId = reader.ReadByte();
			int seed = reader.ReadInt32();
			SkyEntityType type = (SkyEntityType)reader.ReadByte();
			if (Main.remixWorld)
			{
				return true;
			}
			Main.QueueMainThreadAction(delegate
			{
				((AmbientSky)SkyManager.Instance["Ambience"]).Spawn(Main.player[(int)playerId], type, seed);
			});
			return true;
		}
	}
}
