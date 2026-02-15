using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x0200009C RID: 156
	public class WorldSeedOption_NotTheBees : AWorldGenerationOption
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x004DCD91 File Offset: 0x004DAF91
		protected override string KeyName
		{
			get
			{
				return "Seed_NotTheBees";
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x004DCD98 File Offset: 0x004DAF98
		public override string ServerConfigName
		{
			get
			{
				return "notthebees";
			}
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x004DCD9F File Offset: 0x004DAF9F
		public WorldSeedOption_NotTheBees()
		{
			base.SpecialSeedNames = new string[]
			{
				"notthebees"
			};
			base.SpecialSeedValues = new int[0];
		}
	}
}
