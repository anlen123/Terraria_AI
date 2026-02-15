using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000A0 RID: 160
	public class WorldSeedOption_Skyblock : AWorldGenerationOption
	{
		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06001717 RID: 5911 RVA: 0x004DCE69 File Offset: 0x004DB069
		protected override string KeyName
		{
			get
			{
				return "Seed_Skyblock";
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06001718 RID: 5912 RVA: 0x004DCE70 File Offset: 0x004DB070
		public override string ServerConfigName
		{
			get
			{
				return "skyblock";
			}
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x004DCE77 File Offset: 0x004DB077
		public WorldSeedOption_Skyblock()
		{
			base.SpecialSeedNames = new string[]
			{
				"skyblock"
			};
			base.SpecialSeedValues = new int[0];
		}
	}
}
