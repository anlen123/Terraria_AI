using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x0200009E RID: 158
	public class WorldSeedOption_ForTheWorthy : AWorldGenerationOption
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x004DCDFD File Offset: 0x004DAFFD
		protected override string KeyName
		{
			get
			{
				return "Seed_ForTheWorthy";
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x004DCE04 File Offset: 0x004DB004
		public override string ServerConfigName
		{
			get
			{
				return "fortheworthy";
			}
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x004DCE0B File Offset: 0x004DB00B
		public WorldSeedOption_ForTheWorthy()
		{
			base.SpecialSeedNames = new string[]
			{
				"fortheworthy"
			};
			base.SpecialSeedValues = new int[0];
		}
	}
}
