using System;
using System.IO;
using Terraria.GameContent.Drawing;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	// Token: 0x020002E2 RID: 738
	public class NetParticlesModule : NetModule
	{
		// Token: 0x0600261E RID: 9758 RVA: 0x0055D178 File Offset: 0x0055B378
		public static NetPacket Serialize(ParticleOrchestraType particleType, ParticleOrchestraSettings settings)
		{
			NetPacket result = NetModule.CreatePacket<NetParticlesModule>(22);
			result.Writer.Write((byte)particleType);
			settings.Serialize(result.Writer);
			return result;
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x0055D1AC File Offset: 0x0055B3AC
		public override bool Deserialize(BinaryReader reader, int userId)
		{
			ParticleOrchestraType particleOrchestraType = (ParticleOrchestraType)reader.ReadByte();
			ParticleOrchestraSettings settings = default(ParticleOrchestraSettings);
			settings.DeserializeFrom(reader);
			if (Main.netMode == 2)
			{
				NetManager.Instance.Broadcast(NetParticlesModule.Serialize(particleOrchestraType, settings), userId);
			}
			else
			{
				ParticleOrchestrator.SpawnParticlesDirect(particleOrchestraType, settings);
			}
			return true;
		}
	}
}
