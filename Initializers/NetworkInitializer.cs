using System;
using Terraria.GameContent;
using Terraria.GameContent.Items;
using Terraria.GameContent.NetModules;
using Terraria.Net;

namespace Terraria.Initializers
{
	// Token: 0x02000085 RID: 133
	public static class NetworkInitializer
	{
		// Token: 0x06001589 RID: 5513 RVA: 0x004CB598 File Offset: 0x004C9798
		public static void Load()
		{
			NetManager.Instance.Register<NetLiquidModule>();
			NetManager.Instance.Register<NetTextModule>();
			NetManager.Instance.Register<NetPingModule>();
			NetManager.Instance.Register<NetAmbienceModule>();
			NetManager.Instance.Register<NetBestiaryModule>();
			NetManager.Instance.Register<NetCreativePowersModule>();
			NetManager.Instance.Register<NetCreativeUnlocksPlayerReportModule>();
			NetManager.Instance.Register<NetTeleportPylonModule>();
			NetManager.Instance.Register<NetParticlesModule>();
			NetManager.Instance.Register<NetCreativePowerPermissionsModule>();
			NetManager.Instance.Register<BannerSystem.NetBannersModule>();
			NetManager.Instance.Register<CraftingRequests.NetCraftingRequestsModule>();
			NetManager.Instance.Register<TagEffectState.NetModule>();
			NetManager.Instance.Register<LeashedEntity.NetModule>();
			NetManager.Instance.Register<UnbreakableWallScan.NetModule>();
		}
	}
}
