using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000A2 RID: 162
	public class WorldSeedOption_Drunk : AWorldGenerationOption
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001721 RID: 5921 RVA: 0x004DD0BC File Offset: 0x004DB2BC
		protected override string KeyName
		{
			get
			{
				return "Seed_Drunk";
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06001722 RID: 5922 RVA: 0x004DD0C3 File Offset: 0x004DB2C3
		public override string ServerConfigName
		{
			get
			{
				return "drunk";
			}
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x004DD0CA File Offset: 0x004DB2CA
		public WorldSeedOption_Drunk()
		{
			base.SpecialSeedNames = new string[0];
			base.SpecialSeedValues = new int[]
			{
				5162020
			};
		}
	}
}
