using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x0200009D RID: 157
	public class WorldSeedOption_NoTraps : AWorldGenerationOption
	{
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600170E RID: 5902 RVA: 0x004DCDC7 File Offset: 0x004DAFC7
		protected override string KeyName
		{
			get
			{
				return "Seed_NoTraps";
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x004DCDCE File Offset: 0x004DAFCE
		public override string ServerConfigName
		{
			get
			{
				return "notraps";
			}
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x004DCDD5 File Offset: 0x004DAFD5
		public WorldSeedOption_NoTraps()
		{
			base.SpecialSeedNames = new string[]
			{
				"notraps"
			};
			base.SpecialSeedValues = new int[0];
		}
	}
}
